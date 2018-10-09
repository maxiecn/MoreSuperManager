$(function () {

    $("#channelCode").change(function () {
        SetMenuListGetJsonData(noticeTypeJsonData, $(this).val(), "noticeType");
    });

    CKEDITOR.replace("noticeContent", {
        allowedContent: true,
        filebrowserUploadUrl: uploadFilePath,
        filebrowserImageUploadUrl: uploadImagePath,
        filebrowserHtml5videoUploadUrl: uploadVideoPath
    });

});