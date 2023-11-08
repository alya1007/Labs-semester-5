# Web Crawler with RabbitMQ

This is a web crawler that uses RabbitMQ, a message broker, to efficiently parse and collect information from web pages. It is designed to scrape product details from a specific website and store them in a structured format.

## Components

The project consists of several components and files:

1. **producer.py**: This script sends URLs to a RabbitMQ queue for further processing. It establishes a connection to the RabbitMQ server and publishes URLs to a queue named "urls_queue".

2. **url_parser.py**: This script is responsible for scraping URLs from web pages. It uses the `requests` library to fetch web pages and `BeautifulSoup` for HTML parsing. It iterates through pages and extracts URLs to be processed.

3. **item_parser.py**: This script parses detailed product information from a given URL. It extracts features and their values from product pages.

4. **consumer.py**: This script consumes URLs from the RabbitMQ queue, processes them using the `item_parser.py`, and writes the collected information to the file `items_info.txt`.

5. **items_info.txt**: This file contains the collected products information in a structured format.

6. **urls.txt**: This file contains the URLs to be processed.

## How It Works

1. The main entry point of the project is `url_parser.py`, which initiates the crawling process. The user specifies the starting URL and the maximum number of pages to parse. The script extracts URLs from the web pages and writes them to the file `urls.txt`.

2. `producer.py` sends the collected URLs to the RabbitMQ queue, allowing for parallel processing of multiple URLs by consumers.

3. `consumer.py` is designed to run multiple consumer threads (adjustable via `num_consumers`) that fetch URLs from the queue, parse the product details using `item_parser.py`, and append the information to an output file.

## Usage

1. Start a RabbitMQ server on the localhost or update the connection parameters in `producer.py` and `consumer.py` to connect to your RabbitMQ server.

2. Run `url_parser.py` to collect and send URLs to the RabbitMQ queue. Specify the number of pages to parse. If you will introduce a value which is not a number, the crawler will parse all pages of the website.

3. Run `consumer.py` to process the URLs and store product details in the `items_info.txt` file.

## Configuration

You can customize the crawling behavior by modifying the following parameters:

- `url`: The starting URL in `url_parser.py`.
- `num_consumers`: The number of consumer threads in `consumer.py`.
