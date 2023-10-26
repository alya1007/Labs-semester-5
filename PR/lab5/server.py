import json
import os
import socket
import threading
import base64

# Server configuration
HOST = "127.0.0.1"
PORT = 12345
MEDIA_PATH = "./server_media"


class Client:
    def __init__(self, socket, name, room):
        self.socket = socket
        self.name = name
        self.room = room


class Server:
    def __init__(self, host, port):
        self.HOST = host
        self.PORT = port
        self.MEDIA_PATH = "./server_media"
        self.server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.server_socket.bind((self.HOST, self.PORT))
        self.server_socket.listen()
        self.rooms = {}

    def serve_connections(self):
        print(f"Server is listening on {self.HOST}:{self.PORT}")
        while True:
            client_socket, client_address = self.server_socket.accept()
            try:
                print(f"Accepted connection from {client_address}")
                connect_message_raw = client_socket.recv(2048).decode("utf-8")
                connect_message_payload = json.loads(connect_message_raw)["payload"]

                room_name = connect_message_payload["room"]
                client_name = connect_message_payload["name"]

                client = Client(client_socket, client_name, room_name)
                self.register_client(client, client_address)
            except KeyboardInterrupt:
                print("Closing server")
                break
            except Exception:
                self.remove_client(client, client_address, room_name)

    def register_client(self, client, client_address):
        room_name = client.room
        if room_name not in self.rooms:
            self.rooms[room_name] = []
        self.rooms[room_name].append(client)

        acknowledge_message = {
            "type": "acknowledge",
            "payload": {"message": f"Welcome to {room_name}, {client.name}!"},
        }
        self.send_response(client, acknowledge_message)

        receive_thread = threading.Thread(
            target=self.handle_client, args=(client, client_address)
        )
        receive_thread.daemon = True
        receive_thread.start()

        connection_notification = {
            "type": "notification",
            "payload": {"message": f"{client.name} has joined the room"},
        }
        self.broadcast(connection_notification, room_name, client)

    def remove_client(self, client, client_address, room_name):
        self.rooms[room_name].remove(client)
        client.socket.close()
        print(f"Client {client_address} disconnected")

    def handle_client(self, client, client_address):
        while True:
            try:
                raw_message = client.socket.recv(40960).decode("utf-8")
                if raw_message.startswith('{"type": "upload"'):
                    while not raw_message.endswith("}"):
                        raw_message += client.socket.recv(40960).decode("utf-8")

                received_message = json.loads(raw_message)
                print(f"Received message from {client.name}:")
                print(raw_message)
                self.handle_message(client, received_message)
            except Exception:
                print(f"Error from client {client.name}, trying again...")

    def handle_message(self, client, message):
        message_type = message["type"]
        print(f"Received message from {client.name}")
        if message_type == "message":
            self.broadcast(
                {
                    "type": "message",
                    "payload": {
                        "sender": client.name,
                        "text": message["payload"]["text"],
                    },
                },
                client.room,
                client,
            )
        elif message_type == "upload":
            self.handle_upload(client, message["payload"])
        elif message_type == "download":
            self.handle_download(client, message["payload"])
        else:
            print(f"Unknown message type '{message_type}'")

    def handle_upload(self, client, payload):
        try:
            file_name = payload["file_name"]
            file_path = os.path.join(self.MEDIA_PATH, file_name)
            if not os.path.exists(os.path.dirname(file_path)):
                os.makedirs(os.path.dirname(file_path))
            base64_data = payload["data"]
            data = base64.b64decode(base64_data)
            with open(file_path, "wb") as file:
                file.write(data)
            self.send_response(
                client,
                {
                    "type": "acknowledge",
                    "payload": {"message": "File uploaded successfully"},
                },
            )
            self.broadcast(
                {
                    "type": "notification",
                    "payload": {"message": f"{client.name} uploaded {file_name}"},
                },
                client.room,
                client,
            )
        except Exception as e:
            self.send_response(
                client, {"type": "notification", "payload": {"message": f"Error: {e}"}}
            )

    def handle_download(self, client, payload):
        try:
            file_name = payload["file_name"]
            file_path = os.path.join(self.MEDIA_PATH, file_name)
            if not os.path.exists(file_path):
                self.send_response(
                    client,
                    {
                        "type": "notification",
                        "payload": {"message": f"File {file_name} does not exist"},
                    },
                )
                return
            with open(file_path, "rb") as file:
                data = file.read()
            base64_data = base64.b64encode(data).decode("utf-8")
            self.send_response(
                client,
                {
                    "type": "download",
                    "payload": {"file_name": file_name, "data": base64_data},
                },
            )
        except Exception as e:
            self.send_response(
                client, {"type": "notification", "payload": {"message": f"Error: {e}"}}
            )

    def send_response(self, client, message):
        print(f"Sending response to {client.name}")
        print(json.dumps(message))
        client.socket.send(json.dumps(message).encode("utf-8"))

    def broadcast(self, message, room_name, sender=None):
        print(f"Broadcasting message to {room_name}")
        print(json.dumps(message))
        encoded_message = json.dumps(message).encode("utf-8")
        for client in filter(lambda x: x != sender, self.rooms[room_name]):
            client.socket.send(encoded_message)


if __name__ == "__main__":
    server = Server("127.0.0.1", 12345)
    server.serve_connections()
