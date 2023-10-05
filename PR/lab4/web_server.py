from flask import Flask, render_template

app = Flask(__name__)

products = [
    {
        "name": "Fluent Python: Clear, Concise, and Effective Programming",
        "author": "Luciano Ramalho",
        "price": 39.95,
        "description": "Don't waste time bending Python to fit patterns you've learned in other languages. Python's simplicity lets you become productive quickly, but often this means you aren't using everything the language has to offer. With the updated edition of this hands-on guide, you'll learn how to write effective, modern Python 3 code by leveraging its best ideas. ",
    },
    {
        "name": "Introducing Python: Modern Computing in Simple Packages",
        "author": "Bill Lubanovic",
        "price": 27.49,
        "description": "Easy to understand and fun to read, this updated edition of Introducing Python is ideal for beginning programmers as well as those new to the language. Author Bill Lubanovic takes you from the basics to more involved and varied topics, mixing tutorials with cookbook-style code recipes to explain concepts in Python 3. End-of-chapter exercises help you practice what you’ve learned.",
    },
]


@app.route("/")
def home():
    return "Home Page"


@app.route("/about")
def about():
    return "About Us Page"


@app.route("/contacts")
def contacts():
    return "Contact Us Page"


@app.route("/product/<int:product_id>")
def product(product_id):
    if 0 <= product_id < len(products):
        return render_template("product.html", product=products[product_id])
    else:
        return "Product not found", 404


@app.route("/products")
def product_list():
    return render_template("product_list.html", products=products)


@app.errorhandler(404)
def not_found_error(error):
    return "Page not found", 404


if __name__ == "__main__":
    app.run()
