from flask import Flask, render_template

app = Flask(__name__, template_folder='SitePages/GlobalPage', static_folder='SitePages/GlobalPage')

@app.route("/")
def index():
    return render_template("MainPage.html")

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=8087)
