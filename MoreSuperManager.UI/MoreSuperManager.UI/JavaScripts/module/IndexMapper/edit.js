$(function () {
    $("#indexType").change(function () {
        SetMenuListGetJsonData(indexJsonData, $(this).val(), "indexID");
        SetMenuList();
    });
    $("#channelCode").change(function () {
        SetMenuList();
    });
});

function SetMenuList() {

    // 获取当前选中的 value 内容
    var indexType = $("#indexType").val();
    var channelCode = $("#channelCode").val();

    SetMenuListGetJsonData(mapperJsonData, indexType + "-" + channelCode, "mapperID");
}