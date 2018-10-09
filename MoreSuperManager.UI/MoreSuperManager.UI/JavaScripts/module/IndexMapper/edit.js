$(function () {
    $("#indexType").change(function () {
        // 获取当前选中的 value 内容
        var indexType = $(this).val();
        SetMenuItemText(indexJsonData, indexType, "indexID");
        SetMenuItemText(indexMapperData, indexType, "mapperID");
    });
});
function SetMenuItemText(jsonData, indexType, element)
{
    var html = "";
    var itemList = jsonData[indexType];
    if (itemList != null) {
        for (var index = 0; index < itemList.length; index++) {
            html += "<option value={value}>{name}</option>".replace(/[{]value[}]/gi, itemList[index].key).replace(/[{]name[}]/gi, itemList[index].value);
        }
    }
    $("#" + element).html("");
}