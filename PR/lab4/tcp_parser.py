import socket
import re
import json
from bs4 import BeautifulSoup


def send_request(host, port, request):
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client_socket.connect((host, port))
    client_socket.send(request.encode())

    response = b""
    receiving_body = False

    while True:
        data = client_socket.recv(4096)
        if not data:
            break

        if receiving_body:
            response += data
        else:
            response += data
            if b"\r\n\r\n" in response:
                receiving_body = True
                response = response.split(b"\r\n\r\n", 1)[1]

    client_socket.close()
    return response.decode()


def parse_product_page(page_content):
    product = {}
    soup = BeautifulSoup(page_content, "html.parser")

    title_tag = soup.find("title")
    if title_tag:
        product["name"] = title_tag.text

    author_tag = soup.find(string=re.compile(r"Author:"))
    if author_tag:
        product["author"] = author_tag.split("Author:")[1].strip()

    price_tag = soup.find(string=re.compile(r"Price: \$"))
    if price_tag:
        product["price"] = float(re.search(r"\$([0-9.]+)", price_tag).group(1))

    description_tag = soup.find(string=re.compile(r"Description:"))
    if description_tag:
        product["description"] = description_tag.split("Description:")[1].strip()

    return product


def main():
    host = "127.0.0.1"
    port = 5000

    routes = ["/", "/about", "/contacts"]

    page_data = {}

    for route in routes:
        request = f"GET {route} HTTP/1.1\r\nHost: localhost\r\n\r\n"
        page_content = send_request(host, port, request)
        page_data[route] = {"content": page_content}

    request_products = "GET /products HTTP/1.1\r\nHost: localhost\r\n\r\n"
    products_page_content = send_request(host, port, request_products)
    product_urls = re.findall(r'<a href="/product/(\d+)">', products_page_content)

    product_details = []

    for product_url in product_urls:
        request_product = (
            f"GET /product/{product_url} HTTP/1.1\r\nHost: localhost\r\n\r\n"
        )
        product_page_content = send_request(host, port, request_product)
        product = parse_product_page(product_page_content)
        product_details.append(product)

    print("Home Page Data:", json.dumps(page_data["/"], indent=4))
    print("About Page Data:", json.dumps(page_data["/about"], indent=4))
    print("Contacts Page Data:", json.dumps(page_data["/contacts"], indent=4))

    for product in product_details:
        print("Product Data:", json.dumps(product, indent=4))


if __name__ == "__main__":
    main()
