var menuCheckBoxTemplate = "<label class=\"forbidden-text-select\" style=\"display:inline-block;margin-right:45px;font-weight:normal;margin-bottom:0px;\"><input type=\"checkbox\" name=\"menuActionList\" value=\"{value}\" />{name}</label>";

$(function () {

    $("#channelCode").change(function () {
        SetMenuListGetJsonData(menuJsonData, $(this).val(), "parentID");
        SetMenuListGetJsonData(moduleJsonData, $(this).val(), "belongModule");
        $("#belongModule").change();
    });

    $("#parentID").change(function () {
        var parentID = $(this).val();
        if (parentID == "0") {
            $("#menuIconContainer").show();
        } else {
            $("#menuIconContainer").hide();
        }
    });

    $("#belongModule").change(function () {
        var html = "";
        // 获取当前选中的 value 内容
        var menuCode = $(this).val();
        var channelCode = $("#channelCode").val();
        if (menuCode != "-1") {
            var itemList = actionTypeJsonData[menuCode + "-" + channelCode];
            if (itemList != null) {
                for (var key in itemList) {
                    html += menuCheckBoxTemplate.replace(/[{]value[}]/gi, key).replace(/[{]name[}]/gi, itemList[key]);
                }
            }
        }
        $("#menuActionList").html(html);
        if (html != "") {
            $("#menuActionContainer").show();
        } else {
            $("#menuActionContainer").hide();
        }
    });

    $("#btnSettingsFlowStepList").click(function () {
        ShowMaskWindow("settingsFlowStepList");
    });

    $("#btnSettingsFlowStepData").click(function () {
        var flowStepList = [];
        $('#settingsFlowStepList input[name="flowStepList"]:checked').each(function () {
            flowStepList.push($(this).val());
        });
        var query = "stepList=" + flowStepList.join(",") + "&";
        var url = $("#menuUrl").val();
        if (url.indexOf("stepList=") >= 0) {
            url = url + "&";
            url = url.replace(/stepList[=](.*?)[&]/gi, query);
        } else {
            if (url.indexOf("?") >= 0) {
                url += "&" + query;
            } else {
                url += "?" + query;
            }
        }
        url = url.substring(0, url.length - 1);
        $("#menuUrl").val(url);
        HideMaskWindow("settingsFlowStepList");
    });
    $("#btnCloseFlowStepData").click(function () {
        HideMaskWindow("settingsFlowStepList");
    });
});