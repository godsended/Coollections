function sendPostRequestAndReload(reqData, url){
    $.ajax({
        type: "POST",
        url: url,
        data: reqData,
        success: function (result) {
            console.log(result);
            if (result.isSuccess == true)
                window.location.reload();
        }
    })
}