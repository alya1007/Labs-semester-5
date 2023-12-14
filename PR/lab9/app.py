from flask import Flask, request, render_template
from ftplib import FTP
import smtplib
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from io import BytesIO

app = Flask(__name__)


ftp_server = "138.68.98.108"
ftp_user = "yourusername"
ftp_password = "yourusername"


@app.route("/")
def index():
    return render_template("index.html")


@app.route("/submit", methods=["POST"])
def submit():
    recipient = request.form.get("emailAddress")
    subject = request.form.get("emailSubject")
    body = request.form.get("emailBody")
    attached_file = request.files["emailAttachment"]

    ftp = FTP(ftp_server)
    ftp.login(ftp_user, ftp_password)

    # Stream the file contents to FTP server
    with BytesIO() as file_stream:
        attached_file.save(file_stream)
        file_stream.seek(0)  # Reset stream position
        ftp.storbinary(f"STOR {attached_file.filename}", file_stream)

    # Construct email body with file URL
    file_url = f"ftp://{ftp_user}@{ftp_server}/{attached_file.filename}"
    body_with_url = f"{body}\n\nAttached File URL: {file_url}"

    # Send email
    send_email(recipient, subject, body_with_url)

    return "Email sent successfully!"


def send_email(recipient, subject, body):
    # Email configuration
    smtp_server = "smtp.gmail.com"
    smtp_port = 587
    sender_email = "alea.konj@gmail.com"
    sender_password = "mpcw lviq lyye nldv"

    # Setup the MIME
    message = MIMEMultipart()
    message["From"] = sender_email
    message["To"] = recipient
    message["Subject"] = subject

    # Attach body
    message.attach(MIMEText(body, "plain"))

    # Connect to SMTP server and send email
    with smtplib.SMTP(smtp_server, smtp_port) as server:
        server.starttls()
        server.login(sender_email, sender_password)
        server.send_message(message)


if __name__ == "__main__":
    app.run(debug=True)
