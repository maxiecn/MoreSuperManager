
$(function () {
    $("#tableList a[class='operater-edit-flow']").click(function (evt) {
        // 跳转到编辑页面
        EditFlowOperater($(this).attr("data-id"));
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    $("#tableList a[class='operater-auth']").click(function (evt) {
        // 跳转到编辑页面
        AuthFlowOperater($(this).attr("data-id"));
        // 阻止事件冒泡
        evt.stopPropagation();
    });
});

function EditFlowOperater(id) {
    location.href = $("#operaterEditFlowUrl").val().replace("-1", id);
}
function AuthFlowOperater(id) {
    location.href = $("#operaterAuthUrl").val().replace("-1", id);
}