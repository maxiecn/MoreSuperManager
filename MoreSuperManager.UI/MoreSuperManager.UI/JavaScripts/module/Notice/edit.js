$(function () {

    CKEDITOR.replace("noticeContent", {
        allowedContent: true,
        filebrowserUploadUrl: uploadFilePath,
        filebrowserImageUploadUrl: uploadImagePath,
        filebrowserHtml5videoUploadUrl: uploadVideoPath
    });

});