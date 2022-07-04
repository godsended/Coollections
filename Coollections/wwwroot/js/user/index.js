function deleteCollection(id) {
    let reqData = {};
    sendPostRequestAndReload(reqData, "https://localhost:7177/Collections/Delete/" + id.toString());
}