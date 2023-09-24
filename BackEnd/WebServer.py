from flask import Flask, render_template

app = Flask(__name__, template_folder='../FrontEnd/SitePages', static_folder='../FrontEnd/SitePages')

@app.route("/")
def index():
    # Здесь можно вернуть главную страницу или какой-то общий шаблон
    return render_template("MainPage/MainPage.html")

@app.route("/<path:page_name>/")
def render_page(page_name):
    template_path = f"{page_name}/Page.html"  # Формируем путь к шаблону страницы
    return render_template(template_path)

@app.route("/login")
def login():
    return render_template("LogInPage.html")

@app.route("/main")
def main():
    return render_template("MainPage.html")

@app.route("/signup")
def signup():
    return render_template("SignUpPage.html")

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=8087)
