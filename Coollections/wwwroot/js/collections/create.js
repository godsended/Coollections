function onImageUrlChanged() {
    let img = document.getElementById("inputImagePreview");
    let input = document.getElementById("imageInput");
    img.src = input.value;
    if (input.value != "")
        img.hidden = false;
    else
        img.hidden = true;
}

function addRecipient() {
    let element = document.createElement('div')
    element.class = "input-group mb-3";
    element.innerHTML = "<div class='input-group mb-3'>" +
        "<div class='input-group-prepend'>" +
        "<span class='input-group-text' id='basic-addon1'>" +
        "<select name='Type' class='form-select'>" +
        "<option>Number</option>" +
        "<option>Text</option>" +
        "<option>Multiline text</option>" +
        "<option>Checkbox</option>" +
        "<option>Date</option>" +
        "</select>" +
        "</span>" +
        "</div>" +
        "<input name='FieldName' type='text' class='form-control' placeholder='Type field name'" +
        "</div>"
    document.getElementById('recipientsParent').appendChild(element);
}

function removeRecipient() {
    var element = document.getElementById("recipientsParent");
    console.log(element.childNodes);
    console.log(element.childNodes.length);
    if (element.childNodes.length > 2) {
        element.removeChild(element.lastChild);
    }
    console.log(element.childNodes);
    console.log(element.childNodes.length);
}

function createCollection() {
    let name = document.getElementById("nameInput").value;
    let description = document.getElementById("descriptionInput").value;
    let subject = document.getElementById("subjectInput").value;
    let collectionImage = document.getElementById("imageInput").value;
    let fieldsTypes = [];
    let fields = [];
    for (let el of document.getElementsByName("Type")) {
        fieldsTypes.push(el.value);
    }
    for (let el of document.getElementsByName("FieldName")) {
        fields.push(el.value);
    }
    let reqData = {
        Name: name, Description: description, Subject: subject, ImageUrl: collectionImage,
        FieldsTypes: fieldsTypes, FieldsNames: fields
    };
    $.ajax({
        type: "POST",
        url: host + "Collections/Create",
        data: reqData,
        success: function (result) {
            console.log(result);
            if (result.isSuccess == true)
                window.location.href = host + "User/Index";
        }
    })
}