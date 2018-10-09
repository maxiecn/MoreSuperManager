
$(function () {
    $("#tableList a[class='operater-info']").click(function (evt) {
        // 跳转到编辑页面
        InfoOperater($(this).attr("data-id"));
        // 阻止事件冒泡
        evt.stopPropagation();
    });
});
function InfoOperater(id) {
    location.href = $("#operaterInfoUrl").val().replace("-1", id);
}