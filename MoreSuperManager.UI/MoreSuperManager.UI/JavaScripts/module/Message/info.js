
$(function () {

    CKEDITOR.replace("replyContent", {
        allowedContent: true,
        filebrowserUploadUrl: uploadFilePath,
        filebrowserImageUploadUrl: uploadImagePath,
        filebrowserHtml5videoUploadUrl: uploadVideoPath
    });

});