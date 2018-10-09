var TYPE_SMALL = "small";
var TYPE_MIDDLE = "middle";
var TYPE_LARGE = "large";

$(function () {

    $('.gridly').gridly({
        base: 180, // px 
        gutter: 15, // px
        columns: 6
    });

    $("#btnGridItemAdd").click(function () {
        // 数据初始化
        $("#grid_identityID").val("0");
        $("#applicationIcon").val("");
        $("#applicationName").val("");
        $("input:radio[name='applicationType'][value='small']").prop("checked", true);
        $("#applicationUrl").val("");
        // 显示遮罩
        ShowMaskWindow("gridlyItem");
    });
    $("#btnGridItemSave").click(function () {
        var dataList = GetGridDataList();
        if (dataList == null || dataList.length == 0) {
            ErrorAlert("至少设置一项应用！");
            return;
        }
        // 设置数据
        $("#gridItemList").val(JSON.stringify(dataList));
        // 提交数据
        $("#operaterForm").submit();
    });
    $("#btnItemAdd").click(function () {
        GridAddOrUpdateItem();
    });
    $("#btnItemCancel").click(function () {
        HideMaskWindow("gridlyItem");
    });
    $(document).on("click", ".gridly .edit", function (event) {
        GridEditItem(this, event);
    });
    $(document).on("click", ".gridly .delete", function (event) {
        GridDeleteItem(this, event);
    });
});
function GridDeleteItem(ele, event) {

    event.preventDefault();
    event.stopPropagation();

    AskConfirmToCallback("确定删除该应用？", function () {
        ele.closest('.brick').remove();
        $('.gridly').gridly('layout');
    });
}
function GridEditItem(ele, event) {

    event.preventDefault();
    event.stopPropagation();

    var identityID = $(ele).attr("data-id");
    $("#grid_identityID").val(identityID);

    var gridItem = $("#grid_" + identityID);
    if (gridItem.length == 0) return;

    $("#applicationIcon").val(gridItem.children(".icon").attr("data-icon"));
    $("#applicationName").val(gridItem.children(".link").html());
    $("input:radio[name='applicationType'][value='" + gridItem.attr("data-type") + "']").prop("checked", true);
    $("#applicationUrl").val(gridItem.children(".link").attr("href"));
    // 显示遮罩
    ShowMaskWindow("gridlyItem");
}
function GridAddOrUpdateItem() {

    var identityID = $("#grid_identityID").val();
    if (identityID == "" || identityID == "0") identityID = Guid();

    var applicationIcon = StringTrim($("#applicationIcon").val());
    var applicationName = StringTrim($("#applicationName").val());
    var applicationType = $("#gridlyItem input[name='applicationType']:checked").val();
    var applicationUrl = StringTrim($("#applicationUrl").val());

    var ele = $("#grid_" + identityID);
    if (ele.length > 0) {

        var prevApplicationType = ele.attr("data-type");
        if (prevApplicationType != applicationType) {
            ele.attr("data-type", applicationType);
            ele.removeClass(prevApplicationType);
            ele.addClass(applicationType);
        }

        var width = gridSizeOptions.widthSmall;
        var height = gridSizeOptions.height;
        if (applicationType == TYPE_MIDDLE) {
            width = gridSizeOptions.widthMiddle;
        } else if (applicationType == TYPE_LARGE) {
            width = gridSizeOptions.widthLarge;
        }

        ele.children(".link").html(applicationName);
        ele.children(".link").attr("href", applicationUrl);
        ele.children(".icon").attr("data-icon", applicationIcon);
        ele.children(".icon").attr("class", applicationIcon + " icon");

        // 必须使用此种方法设置 width 和 height 数据
        ele.data("width", width);
        ele.data("height", height);

        $('.gridly').gridly('layout');
    } else {

        var GridItemFormat = "<div id=\"grid_{identityID}\" data-type=\"{applicationType}\" class=\"brick {applicationType}\"><i data-icon=\"{applicationIcon}\" class=\"{applicationIcon} icon\"></i><a href=\"{applicationUrl}\" class=\"link\">{applicationName}</a><span id=\"grid_edit_{identityID}\" data-id=\"{identityID}\" class=\"glyphicon glyphicon-edit oper edit\"></span><span id=\"grid_delete_{identityID}\" class=\"glyphicon glyphicon-remove-sign oper delete\"></span></div>";
        var html = GridItemFormat.replace(/[{]identityID[}]/gi, identityID)
            .replace(/[{]applicationType[}]/gi, applicationType)
            .replace(/[{]applicationUrl[}]/gi, applicationUrl)
            .replace(/[{]applicationName[}]/gi, applicationName)
            .replace(/[{]applicationIcon[}]/gi, applicationIcon);
        $("#gridlyList").append(html);

        $('.gridly').gridly();
    }
    HideMaskWindow("gridlyItem");
}
/*
    假设容器宽 1200px 
    base：最小单元格宽度，建议每个格子是倍数，比如 base 值为 100px，则小格子为 100px，中格子为 200px，大格子为 300px。
    gutter：为格子之间的间隔
    columns：则是容器宽 / base 值，则为 12
*/

function GetGridDataList() {
    var gridItemList = [];
    var divItemList = $("div#gridlyList div.brick");
    // 获取题库数据
    if (divItemList != null && divItemList.length > 0) {
        // 遍历题库数据
        for (var i = 0; i < divItemList.length; i++) {
            // 获取题库选项数据
            gridItemList.push({
                "ApplicationIcon": $(divItemList[i]).children(".icon").attr("data-icon"),
                "ApplicationName": $(divItemList[i]).children(".link").html(),
                "ApplicationUrl": $(divItemList[i]).children(".link").attr("href"),
                "ApplicationType": $(divItemList[i]).attr("data-type"),
                "ApplicationX": $(divItemList[i]).css("left").replace("px", ""),
                "ApplicationY": $(divItemList[i]).css("top").replace("px", ""),
                "ApplicationWidth": $(divItemList[i]).css("width").replace("px", ""),
                "ApplicationHeight": $(divItemList[i]).css("height").replace("px", ""),
            });
        }
    }
    return gridItemList;
}