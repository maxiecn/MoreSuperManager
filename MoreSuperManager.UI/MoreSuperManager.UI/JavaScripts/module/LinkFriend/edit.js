$(function () {

    $("#channelCode").change(function () {
        SetMenuListGetJsonData(linkFriendTypeJsonData, $(this).val(), "linkFriendType");
    });

    var regExp = new RegExp(uploadImageExt + "$", "i");
    var uploadImageExtErrorNote = "只能上传" + uploadImageExt.toLocaleUpperCase() + " 格式的图片！";
    var uploadImageMaxSizeErrorNote = "上传图片大小不能超过 " + GetFormatUploadSizeText(uploadImageMaxSize) + " M！";

    AsyncUploadFile("coverUpload", uploadCoverPath, regExp, Number(uploadImageMaxSize), uploadImageExtErrorNote, uploadImageMaxSizeErrorNote, function (data) {
        if (data.Error != "") {
            ErrorAlert(data.Error);
            return;
        }
        UploadFileSuccessCallback("cover_url", "cover_link", "cover_item", data);
    }, null, null);
    $("#cover_delete").click(function () {
        UploadFileClearCallback("cover_item", "cover_url");
    });
});
function UploadFileSuccessCallback(url, link, container, data) {
    $("#" + url).attr("value", data.Data);
    $("#" + link).attr("href", data.Data);
    $("#" + container).show();
}
function UploadFileClearCallback(container, url) {
    $("#" + url).attr("value", "");
    $("#" + container).hide();
}