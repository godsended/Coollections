$(document).ready(function() {
    $(window).keydown(function(event){
        if(event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
});
function requestRegistration()
{
    let name = document.getElementsByName("Name")[0].value;
    let email = document.getElementsByName("Email")[0].value;
    let password = document.getElementsByName("Password")[0].value;
    let errorField = document.getElementById("regError");
    let reqData = {Name: name, Email: email, Password: password}
    $.ajax({
        type: "POST",
        url: host + "Auth/ProcessRegistration",
        data: reqData,
        success: function (result) {
            console.log(result);
            if (result.code == 0)
                window.location.href = host + "Home";
            else errorField.innerText = result.message;
        },
        dataType: "json"
    });
}

function openLogin()
{
    window.location.href = host + "Auth/Login";
}