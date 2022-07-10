var eng = new Object();
eng["Home"] = "Home";
eng["Profile"] = "Profile";
eng["Search"] = "Search";
eng["Top collections"] = "Top collections";
eng["Latest items"] = "Latest items";
eng["Light"] = "Light";
eng["Dark"] = "Dark";
eng["Collections"] = "Collections";
eng["New"] = "New";
eng["Delete"] = "Delete";
eng["New collection"] = "New collection";
eng["Name"] = "Name";
eng["Describe your collection"] = "Describe your collection";
eng["Subject"] = "Subject";
eng["Collection image"] = "Collection image";
eng["Fields"] = "Fields";
eng["Create"] = "Create";
eng["Owner"] = "Owner";
eng["New item"] = "New item";
eng["Add"] = "Add";
eng["Close"] = "Close";
eng["Edit item"] = "Edit item";
eng["Edit"] = "Edit";
eng["Delete"] = "Delete";
eng["Registration"] = "Registration";
eng["Login"] = "Login";
eng["Email"] = "Email";
eng["Password"] = "Password";

function setEnLanguage() {
    setCookie("Lang", "en", 30);
    setViewData(eng);
    document.getElementById("engLangSelect").checked = true;
}