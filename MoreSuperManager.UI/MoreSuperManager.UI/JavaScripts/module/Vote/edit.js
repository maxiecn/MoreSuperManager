var voteManager = null;

function MoveUpItem(id) {
    voteManager.UpOrDownVoteItem(id, false);
}
function MoveDownItem(id) {
    voteManager.UpOrDownVoteItem(id, true);
}
function ClearItemData() {
    $("#itemTitle").val("");
    $("#itemMaxCount").val("");
    $("#voteItemA").val("");
    $("#voteItemB").val("");
    $("#voteItemC").val("");
    $("#voteItemD").val("");
    $("#voteItemE").val("");
}
function InsertItem() {
    // 设置添加状态
    $("#voteItemID").val("");
    // 清空数据
    ClearItemData();
    // 显示遮罩
    ShowMaskWindow("voteItem");
}
function RemoveItem(id) {
    AskConfirmToCallback("确定删除该题库？", function () {
        voteManager.RemoveVoteItem(id);
    });
}
function EditItem(id) {
    var itemData = voteManager.GetVoteItemData(id);
    if (itemData == null) return;
    // 清空数据
    ClearItemData();
    // 设置编辑状态
    $("#voteItemID").val(id);
    // 设置数据
    $("#itemTitle").val(itemData.itemTitle);
    // 设置类别
    $("input:radio[name='itemType'][value='" + itemData.itemType + "']").prop("checked", true);
    $("#itemMaxCount").val(itemData.itemMaxCount);
    var itemDataList = itemData.itemContent.split(",");
    // 拆分选项列表
    if (itemDataList != null && itemDataList.length > 0) {
        // 遍历选项列表
        for (var i = 0; i < itemDataList.length; i++) {
            if (StringTrim(itemDataList[i]) != "") {
                $("#voteItem" + voteManager.VotePrefixList[i]).val(itemDataList[i]);
            }
        }
    }
    VoteItemTypeChange(itemData.itemType);
    ShowMaskWindow("voteItem");
}
function SaveVoteData() {
    var voteDataList = voteManager.GetVoteDataList();
    if (voteDataList == null || voteDataList.length == 0) {
        ErrorAlert("至少要有一道题库数据！");
        return;
    }
    // 设置数据
    $("#voteItemList").val(JSON.stringify(voteDataList));
    // 提交数据
    $("#operaterForm").submit();
}
function VoteItemTypeChange(itemType) {
    // 如果是复选框
    if (itemType == VOTE_CHECKBOX) {
        $("#itemMaxCount").show();
    } else {
        $("#itemMaxCount").hide();
    }
    if (itemType == VOTE_TEXT) {
        $("#voteItemContainer").hide();
    } else {
        $("#voteItemContainer").show();
    }
}

$(function () {

    voteManager = new VoteManager(2, voteJsonData, "voteContainer");

    $("#newVoteItem").click(function () {
        InsertItem();
    });
    $("#btnItemAdd").click(function () {
        var result = voteManager.InsertVoteItem();
        if (result) {
            HideMaskWindow("voteItem");
        }
    });
    $("#btnItemCancel").click(function () {
        HideMaskWindow("voteItem");
    });
    $("#btnSaveVote").click(function () {
        SaveVoteData();
    });
    $("input:radio[name='itemType']").change(function () {
        VoteItemTypeChange($(this).val());
    });
});