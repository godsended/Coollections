function toggle(source) {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    for(var checkbox of checkboxes)
        checkbox.checked = source.checked;
}

function blockUsers() {
    sendPostRequestAndReload(buildReqData(), "https://localhost:7177/Admin/BlockUsers");
}

function unblockUsers() {
    sendPostRequestAndReload(buildReqData(), "https://localhost:7177/Admin/UnlockUsers");
}

function addAdmins() {
    sendPostRequestAndReload(buildReqData(), "https://localhost:7177/Admin/AddAdmins");
}

function removeAdmins() {
    sendPostRequestAndReload(buildReqData(), "https://localhost:7177/Admin/RemoveAdmins");
}

function deleteUsers() {
    sendPostRequestAndReload(buildReqData(), "https://localhost:7177/Admin/DeleteUsers");
}

function getSelectedIds(){
    let ids = [];
    let elements = document.getElementsByClassName('admin-checks');
    for(let el of elements) {
        if(el.checked == true)
            ids.push(el.value);
    }
    return ids;
}

function buildReqData(){
    let ids = getSelectedIds();
    let reqData = {
        Ids: ids
    };
    return reqData;
}