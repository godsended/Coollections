function requestLogin() {
    let email = document.getElementsByName("Email")[0].value;
    let password = document.getElementsByName("Password")[0].value;
    let errorField = document.getElementById("logError");
    let reqData = {Name: "NULL", Email: email, Password: password}
    $.ajax({
        type: "POST",
        url: host + "Auth/ProcessLogin",
        data: reqData,
        success: function (result) {
            console.log(result);
            console.log(reqData);
            if (result.code == 0)
                window.location.href = host + "Home";
            else errorField.innerText = result.message;
        },
        dataType: "json"
    });
}

function openRegistration() {
    window.location.href = host + "Auth/Registration";
}