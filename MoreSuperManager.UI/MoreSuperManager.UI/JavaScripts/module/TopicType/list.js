$(function () {
    $("#tableList tr").click(function () {
        var id = $(this).attr("id");
        if (id == null) return;
        ShowAndHideRow(this, id);
    });
    $("#channelCode").change(function () {
        $("#searchForm").submit();
    });
});