function initThemes () {
    if (getCookie("Theme") == null)
        setCookie("Theme", "Light", 30);
    switch (getCookie("Theme")) {
        case "Light":
            setLightTheme();
            break;
        case "Dark":
            setDarkTheme();
            break;
        default:
            setLightTheme();
            break;
    }
};

function switchToLightTheme() {
    setLightTheme();
    window.location.reload();
}

function switchToDarkTheme() {
    setDarkTheme();
    window.location.reload();
}

function setLightTheme() {
    setCookie("Theme", "Light", 30);
    createLink("https://cdn.jsdelivr.net/npm/bootswatch@5.1.3/dist/quartz/bootstrap.min.css")
    createLink("https://cdn.jsdelivr.net/npm/bootswatch@5.1.3/dist/quartz/_bootswatch.scss")
    document.getElementById("lightThemeSelect").checked = true;
}

function setDarkTheme() {
    setCookie("Theme", "Dark", 30);
    createLink("https://cdn.jsdelivr.net/npm/bootswatch@5.1.3/dist/vapor/bootstrap.min.css")
    createLink("https://cdn.jsdelivr.net/npm/bootswatch@5.1.3/dist/vapor/_bootswatch.scss")
    document.getElementById("darkThemeSelect").checked = true;
}

function createLink(href) {
    let link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = href;
    document.head.appendChild(link);
}