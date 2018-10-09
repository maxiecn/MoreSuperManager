$(function () {

    $("#channelCode").change(function () {
        SetMenuListGetJsonData(topicTypeJsonData, $(this).val(), "topicType");
        SetCheckBoxListGetJsonData(topicPositionTypeJsonData, $(this).val(), "menuActionList", "positionTypeList");
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

    CKEDITOR.replace("topicContent", {
        allowedContent: true,
        filebrowserUploadUrl: uploadFilePath,
        filebrowserImageUploadUrl: uploadImagePath,
        filebrowserHtml5videoUploadUrl: uploadVideoPath
    });

    $("#cover_delete").click(function () {
        UploadFileClearCallback("cover_item", "cover_url");
    });

    $("#btnSaveAndCheck").click(function () {
        // 设置为审核状态
        $("#topicStatus").val("2");
        // 提交表单数据
        $("#operaterForm").submit();
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