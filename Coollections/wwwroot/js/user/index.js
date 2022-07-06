function deleteCollection(id) {
    let reqData = {};
    sendPostRequestAndReload(reqData, host + "Collections/Delete/" + id.toString());
}