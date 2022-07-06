

function collapse(id){
    let elem = document.getElementById(id);
    if (elem.classList.contains("collapse"))
        elem.classList.remove("collapse");
    else
        elem.classList.add("collapse");
}

function like(item){
    let reqData = {}

    $.ajax({
        type: "POST",
        url: host + "Collections/Like/" + item.toString(),
        data: reqData,
        success: function (result) {
            console.log(result);
            console.log(reqData);
            if (result.isSuccess = true){
                updateLikedData();
            }
                
        },
        dataType: "json"
    });
}