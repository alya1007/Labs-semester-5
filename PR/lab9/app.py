from flask import Flask, request, render_template

app = Flask(__name__)


@app.route("/")
def index():
    return render_template("index.html")


@app.route("/submit", methods=["POST"])
def submit():
    input_value = request.form["inputField"]
    print("Input value: ", input_value)
    return f"Input value: {input_value}"


if __name__ == "__main__":
    app.run(debug=True)
