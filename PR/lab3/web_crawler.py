import requests
from bs4 import BeautifulSoup

from urllib.parse import urljoin

import math


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


def main():
    url = "https://m.999.md/ro/list/real-estate/apartments-and-rooms"
    max_pages = 2
    current_page = 1
    last_page = False
    hrefs = []

    parse(url, max_pages, current_page, last_page, hrefs)

    for href in hrefs:
        print(href)

    print(len(hrefs))


if __name__ == "__main__":
    main()
