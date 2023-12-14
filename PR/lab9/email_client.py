import smtplib

sender_email = "alea.konj@gmail.com"
rec_email = "alea.konj@gmail.com"
# password = input(str("Please enter your password : "))
password = "mpcw lviq lyye nldv"
message = "Hey, this was sent using python"

server = smtplib.SMTP("smtp.gmail.com", 587)
server.starttls()
server.login(sender_email, password)
print("Login success")
server.sendmail(sender_email, rec_email, message)
print("Email has been sent to ", rec_email)
