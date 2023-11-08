import requests
from bs4 import BeautifulSoup

from urllib.parse import urljoin

import math

from producer import send_urls_to_queue


def parse(url, max_pages, current_page, last_page, hrefs):
    response = requests.get(url)
    soup = BeautifulSoup(response.text, "html.parser")

    links = soup.find_all("a", class_="block-items__item__link js-item-ad")

    for link in links:
        if not link.get("href").startswith("/booster"):
            hrefs.append(urljoin(url, link.get("href")))

    nextBtn = soup.find("a", class_="block-nav__next")

    nextBtnClasses = nextBtn["class"]

    if "is-disabled" not in nextBtnClasses:
        last_page = False
    else:
        last_page = True

    nextBtnHref = urljoin("https://m.999.md", nextBtn.get("href"))

    if math.isnan(max_pages):
        max_pages = float("inf")

    if not last_page and current_page < max_pages:
        current_page += 1
        parse(nextBtnHref, max_pages, current_page, last_page, hrefs)

    return hrefs


def write_to_file(hrefs):
    with open("urls.txt", "w") as file:
        for href in hrefs:
            file.write(href + "\n")


def main():
    url = "https://m.999.md/ro/list/real-estate/apartments-and-rooms"
    max_pages = input("Enter number of pages to parse: ")
    try:
        max_pages = int(max_pages)
    except ValueError:
        max_pages = float("inf")

    current_page = 1
    last_page = False
    hrefs = []

    hrefs = parse(url, max_pages, current_page, last_page, hrefs)

    print(f"Number of urls: {len(hrefs)}")

    write_to_file(hrefs)

    send_urls_to_queue(hrefs)


if __name__ == "__main__":
    main()
