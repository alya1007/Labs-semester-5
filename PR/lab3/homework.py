import requests
from bs4 import BeautifulSoup


def parse(url):
    response = requests.get(url)
    soup = BeautifulSoup(response.text, "html.parser")

    featuresDict = {}

    featuresDiv = soup.find("div", class_="item-page__features")

    featuresKeys = featuresDiv.find_all(
        "dt", class_="item-page__features__feature__key"
    )
    featuresValues = featuresDiv.find_all(
        "dd", class_="item-page__features__feature__value"
    )

    for key, value in zip(featuresKeys, featuresValues):
        featuresDict[key.text.strip()] = value.text.strip()

    return featuresDict


def main():
    url = "https://m.999.md/ro/83926563"
    featuresDict = parse(url)

    print(featuresDict)


if __name__ == "__main__":
    main()
