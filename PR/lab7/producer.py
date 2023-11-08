def send_urls_to_queue(urls):
    import pika

    connection_params = pika.ConnectionParameters("localhost")
    connection = pika.BlockingConnection(connection_params)
    channel = connection.channel()

    channel.queue_declare(queue="urls_queue")
    urls = [url.strip() for url in urls]

    for url in urls:
        channel.basic_publish(exchange="", routing_key="urls_queue", body=url)

    connection.close()
