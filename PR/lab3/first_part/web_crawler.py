import requests
from bs4 import BeautifulSoup

from urllib.parse import urljoin


hrefs = []

# max_pages = float("NaN")
max_pages = 1


def parsePage(url, page_nr):
    page = requests.get(str(url + str(page_nr)))
    max = 0
    try:
        max = int(max_pages)
    except:
        max = float("inf")

    if page.status_code == 200 and (page_nr <= max):
        soup = BeautifulSoup(page.text, "html.parser")

        links = soup.find_all("a", class_="block-items__item__link")

        for link in links:
            href = link.get("href")
            if not href.startswith("/booster"):
                hrefs.append(urljoin(url, href))

        for href in hrefs:
            print(href, end="\n")
            print()

        page_nr += 1
        parsePage(url, page_nr)

    else:
        return hrefs


def main():
    url = "https://m.999.md/ro/list/real-estate/apartments-and-rooms?page="
    pages = 3

    for n in range(pages):
        parsePage(url, 1)

    print(len(hrefs))


if __name__ == "__main__":
    main()
