$(function () {

    $("#channelCode").change(function () {
        SetMenuListGetJsonData(topicTypeJsonData, $(this).val(), "parentID");
    });
});