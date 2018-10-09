$(function () {
    $("#channelCode").change(function () {
        location.href = editUrl.replace("0", identityID).replace("-1", $(this).val());
    });
    $("#tableList tr").click(function () {
        var id = $(this).attr("id");
        if (id == null) return;
        ShowAndHideRow(this, id);
    });
    $("input[name='menuActionList']").click(function (evt) {
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    $("#tableList label").click(function (evt) {
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    $("input[name='identityIDList']").change(function (evt) {
        var id = $(this).attr("data-id");
        if (id == null) return;
        YesOrNotChecked(this, id);
        return false;
    });
});

function YesOrNotChecked(ele, pid, status) {
    // 如果数据不正确
    if (ele == null || pid == null || pid == "") return;

    var status = status;
    if (arguments.length <= 2) {
        // 获取当前选中状态
        status = $(ele).prop("checked");
        if (!status) {
            $(ele).prop("checked", false);
        } else {
            $(ele).prop("checked", true);
        }
    }
    var checkboxList = $("input[name='identityIDList'][data-pid='" + pid + "']");
    if (checkboxList != null && checkboxList.length > 0) {
        for (var i = 0; i < checkboxList.length; i++) {
            if (!status) {
                $(checkboxList[i]).prop("checked", false);
            } else {
                $(checkboxList[i]).prop("checked", true);
            }
            YesOrNotChecked(checkboxList[i], $(checkboxList[i]).attr("data-id"), status);
        }
    }
}