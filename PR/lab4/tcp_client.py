import socket


def tcp_client():
    host = "localhost"
    port = 5000

    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client_socket.connect((host, port))

    request = b"GET / HTTP/1.1\r\nHost: localhost\r\n\r\n"
    client_socket.send(request)

    response = client_socket.recv(4096)
    print(response.decode())

    client_socket.close()


if __name__ == "__main__":
    tcp_client()
