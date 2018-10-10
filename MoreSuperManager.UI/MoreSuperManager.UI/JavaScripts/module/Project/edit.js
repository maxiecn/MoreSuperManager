$(function () {

    $("#channelCode").change(function () {
        SetMenuListGetJsonData(projectTypeJsonData, $(this).val(), "projectType");
        SetMenuListGetJsonData(flowJsonData, $(this).val(), "flowID");
    });
});