var viewData;

function setViewData(data) {
    viewData = data;
    let elems = document.getElementsByClassName("coolang")
    for(let el of elems)
    {
        el.innerText = data[el.dataset.text];
    }
}

function initLocalization() {
    if (getCookie("Lang") == null)
        setCookie("Lang", "en", 30);
    switch (getCookie("Lang")) {
        case "en":
            setEnLanguage();
            break;
        case "ru":
            setRuLanguage();
            break;
        default:
            setEnLanguage();
            break;
    }
}