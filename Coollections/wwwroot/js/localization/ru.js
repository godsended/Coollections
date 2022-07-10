var ru = new Object();
ru["Home"] = "Домашняя страница";
ru["Profile"] = "Профиль";
ru["Search"] = "Поиск";
ru["Top collections"] = "Лучшие коллекции";
ru["Latest items"] = "Недавно добавленное";
ru["Light"] = "Светлая тема";
ru["Dark"] = "Ночная тема";
ru["Collections"] = "Коллекции";
ru["New"] = "Создать";
ru["Delete"] = "Удалить";
ru["New collection"] = "Новая коллекция";
ru["Name"] = "Имя";
ru["Describe your collection"] = "Опишите вашу коллекцию";
ru["Subject"] = "Тематика";
ru["Collection image"] = "Фон коллекции";
ru["Fields"] = "Поля";
ru["Create"] = "Создать";
ru["Owner"] = "Создатель";
ru["New item"] = "Новая запись";
ru["Add"] = "Добавить";
ru["Close"] = "Закрыть";
ru["Edit item"] = "Изменить запись";
ru["Edit"] = "Изменить";
ru["Delete"] = "Удалить";
ru["Registration"] = "Регистрация";
ru["Login"] = "Войти";
ru["Email"] = "Электронная почта";
ru["Password"] = "Пароль";


function setRuLanguage() {
    setCookie("Lang", "ru", 30);
    setViewData(ru);
    document.getElementById("ruLangSelect").checked = true;
}