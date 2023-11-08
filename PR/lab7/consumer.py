import pika
import threading
from item_parser import parse

# Number of consumers you want to run concurrently
num_consumers = 4  # Adjust this as needed


def write_descriptions_to_file(item_info: dict):
    with open("items_info.txt", "a", encoding="utf-8") as file:
        file.write(str(item_info) + "\n")


def on_message_callback(channel, method, properties, body):
    decoded_body = body.decode("utf-8")
    item_info: dict = parse(decoded_body)
    write_descriptions_to_file(item_info)


def consume():
    connection_params = pika.ConnectionParameters("localhost")
    connection = pika.BlockingConnection(connection_params)
    channel = connection.channel()

    channel.queue_declare(queue="urls_queue")

    channel.basic_consume(
        queue="urls_queue", auto_ack=True, on_message_callback=on_message_callback
    )

    channel.start_consuming()


def main():
    threads = []

    for _ in range(num_consumers):
        thread = threading.Thread(target=consume)
        threads.append(thread)
        thread.start()

    for thread in threads:
        thread.join()


if __name__ == "__main__":
    main()
