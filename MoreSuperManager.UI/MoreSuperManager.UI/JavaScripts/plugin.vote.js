var VOTE_RADIO = 1;
var VOTE_CHECKBOX = 2;
var VOTE_TEXT = 3;

function VoteManager(min, voteList, voteContainer) {
    // 最小选项数量
    this.Min = min;
    this.VoteContainer = voteContainer;
    this.VotePrefixList = ["A", "B", "C", "D", "E"];
    if (voteList != null && voteList.length > 0) {
        for (var i = 0; i < voteList.length; i++) {
            this.AppendVoteItem(voteList[i]);
        }
        this.RefreshVoteItem();
    }
}
// 初始化投票节点数据
VoteManager.prototype.InitVoteItemData = function () {

    var itemTitle = StringTrim($("#itemTitle").val());
    var itemType = $("#voteItem input[name='itemType']:checked").val();
    var voteItemA = StringTrim($("#voteItemA").val());
    var voteItemB = StringTrim($("#voteItemB").val());
    var voteItemC = StringTrim($("#voteItemC").val());
    var voteItemD = StringTrim($("#voteItemD").val());
    var voteItemE = StringTrim($("#voteItemE").val());
    var itemMaxCount = StringTrim($("#itemMaxCount").val())

    var itemList = [];
    if (voteItemA != "") itemList.push(voteItemA);
    if (voteItemB != "") itemList.push(voteItemB);
    if (voteItemC != "") itemList.push(voteItemC);
    if (voteItemD != "") itemList.push(voteItemD);
    if (voteItemE != "") itemList.push(voteItemE);

    var voteItemID = StringTrim($("#voteItemID").val());

    if (itemType != VOTE_CHECKBOX) itemMaxCount = "0";
    if (itemType == VOTE_TEXT) itemList = [];

    var voteItemData = {
        "id": voteItemID == "" ? Guid() : voteItemID,
        "itemType": itemType,
        "itemTitle": itemTitle,
        "itemMaxCount": itemMaxCount,
        "voteItemList": itemList,
        "isEdit": voteItemID == "" ? false : true
    };
    return voteItemData;
}
// 追加投票节点 HTML
VoteManager.prototype.AppendVoteItem = function (voteItemData) {
    // 如果数据不正确，则返回
    if (voteItemData == null) return;

    var html = "<div style=\"border:1px dashed #0CA3F2;padding:15px;margin-top:15px;\" id=\"vote_item_";
    html += voteItemData.id;
    html += "\" data-id=\"";
    html += voteItemData.id;
    html += "\" data-item data-item-title=\"";
    html += voteItemData.itemTitle;
    html += "\" data-item-type=\"";
    html += voteItemData.itemType;
    html += "\" data-item-list=\"";
    html += voteItemData.voteItemList.join(",");
    html += "\" data-max-count=\"";
    html += voteItemData.itemMaxCount;
    html += "\">";
    html += this.GetItemContentHtmlText(voteItemData);
    html += "</div>";
    // 设置内容
    $("#" + this.VoteContainer).append(html);
    // 绑定相关的鼠标事件
    var divItem = $("#vote_item_" + voteItemData.id);
    if (divItem != null && divItem.length > 0) {
        // 设置双击事件
        divItem.dblclick(function (evt) {
            // 双击进行编辑状态
            EditItem($(this).attr("data-id"));
            // 阻止事件冒泡
            evt.stopPropagation();
        });
    }
}
VoteManager.prototype.UpdateVoteItem = function (voteItemData) {
    // 获取 HTML
    var divItem = $("#vote_item_" + voteItemData.id);
    if (divItem == null || divItem.length == 0) return;
    // 设置属性
    divItem.attr("data-item-title", voteItemData.itemTitle);
    divItem.attr("data-item-type", voteItemData.itemType);
    divItem.attr("data-item-list", voteItemData.voteItemList.join(","));
    divItem.attr("data-max-count", voteItemData.itemMaxCount);
    // 设置子内容
    divItem.html(this.GetItemContentHtmlText(voteItemData));
}
VoteManager.prototype.GetItemContentHtmlText = function (voteItemData) {

    // 默认是单选
    var itemType = "radio";
    if (voteItemData.itemType == VOTE_CHECKBOX) itemType = "checkbox";

    var html = "";
    html += "<div style=\"overflow:hidden;clear:both;\"><div class=\"pull-left\">题库&nbsp;";
    html += "<i style=\"font-style:normal;\" class=\"vote_index\"></i>：";
    html += voteItemData.itemTitle;
    html += "</div><div class=\"pull-right\">";
    html += "<span onclick=\"EditItem('";
    html += voteItemData.id;
    html += "')\" style=\"display: inline-block;width: 29px;height: 29px;cursor:pointer;background: url(/Content/Static/vote-edit.png) center no-repeat;\"></span>";
    html += "<span onclick=\"MoveUpItem('";
    html += voteItemData.id;
    html += "')\" style=\"display: inline-block;width: 29px;height: 29px;cursor:pointer;background: url(/Content/Static/vote-up.png) center no-repeat;\"></span>";
    html += "<span onclick=\"MoveDownItem('";
    html += voteItemData.id;
    html += "')\" style=\"display: inline-block;width: 29px;height: 29px;cursor:pointer;background: url(/Content/Static/vote-down.png) center no-repeat;\"></span>";
    html += "<span onclick=\"RemoveItem('";
    html += voteItemData.id;
    html += "')\" style=\"display: inline-block;width: 29px;height: 29px;cursor:pointer;background: url(/Content/Static/vote-delete.png) center no-repeat;\"></span>";
    html += "</div></div><div class=\"";
    html += itemType;
    html += "-inline\" style=\"padding-top:0px;\">";

    if (voteItemData.itemType != VOTE_TEXT) {
        if (voteItemData.voteItemList != null && voteItemData.voteItemList.length > 0) {
            for (var i = 0; i < voteItemData.voteItemList.length; i++) {
                html += "<label style=\"display:inline-block;font-weight:normal;margin-bottom:0px;width:240px;text-align:left;\">";
                html += "<input type=\"";
                html += itemType;
                html += "\" name=\"";
                html += itemType;
                html += "_";
                html += voteItemData.id;
                html += "\" /><i style=\"font-style:normal;\">";
                html += this.VotePrefixList[i];
                html += "</i>、";
                html += voteItemData.voteItemList[i];
                html += "</label>";
            }
        }
    } else {
        html += "<textarea class=\"form-control\" style=\"height:50px;width:785px;margin-left:-20px;\"></textarea>";
    }
    html += "</div>";
    return html;
}
// 添加一个投票节点
VoteManager.prototype.InsertVoteItem = function () {
    try {
        var voteItemData = this.InitVoteItemData();
        // 验证最少选项是否正确
        if (voteItemData.itemType != VOTE_TEXT && voteItemData.voteItemList.length < this.Min) {
            ErrorAlert("至少需要 " + this.Min + " 个选项！");
            return;
        }
        if (voteItemData.itemType == VOTE_CHECKBOX && voteItemData.itemMaxCount == "") {
            //alert("多选题库需要设置最多选择项数目！");
            ErrorAlert("多选题库需要设置最多选择项数目！");
            return;
        }
        if (!voteItemData.isEdit) {
            // 追加元素
            this.AppendVoteItem(voteItemData);
        } else {
            this.UpdateVoteItem(voteItemData);
        }
        // 刷新索引数据
        this.RefreshVoteItem();
        return true;
    }
    catch (e) {
        return false;
    }
}
// 刷新节点索引
VoteManager.prototype.RefreshVoteItem = function () {
    var spanItemList = $(".vote_index");
    if (spanItemList != null && spanItemList.length > 0) {
        var length = spanItemList.length;
        for (var i = 0; i < length; i++) {
            $(spanItemList[i]).html(i + 1);
        }
    }
}
// 元素上移或者下移
VoteManager.prototype.UpOrDownVoteItem = function (id, isDown) {
    // 获取元素
    var element = $("#vote_item_" + id);
    // 如果数据不正确
    if (element == null || element.length == 0) return;
    // 如果是向上
    if (!isDown) {
        var prevElement = element.prev();
        // 如果没有前面兄弟元素
        if (prevElement == null || prevElement.length == 0) return;
        // 交换元素位置 
        element.insertBefore(prevElement);
    } else {
        // 如果下一个元素不存在
        var nextElement = element.next();
        // 如果没有后面兄弟元素
        if (nextElement == null || nextElement.length == 0) return;
        // 交换元素位置 
        nextElement.insertBefore(element);
    }
    this.RefreshVoteItem();
}
VoteManager.prototype.RemoveVoteItem = function (id) {
    // 移除元素
    $("#vote_item_" + id).remove();
    // 重新排列
    this.RefreshVoteItem();
}
VoteManager.prototype.GetVoteDataList = function () {

    var voteItemList = [];

    var divItemList = $("#voteContainer div[data-item]");
    // 获取题库数据
    if (divItemList != null && divItemList.length > 0) {
        // 遍历题库数据
        for (var i = 0; i < divItemList.length; i++) {
            // 获取题库选项数据
            voteItemList.push({
                "ItemID": $(divItemList[i]).attr("data-id"),
                "ItemTitle": $(divItemList[i]).attr("data-item-title"),
                "ItemType": $(divItemList[i]).attr("data-item-type"),
                "ItemContent": $(divItemList[i]).attr("data-item-list"),
                "ItemMaxCount": $(divItemList[i]).attr("data-max-count"),
            });
        }
    }
    return voteItemList;
}
VoteManager.prototype.GetVoteItemData = function (id) {
    var voteItem = $("#vote_item_" + id);
    if (voteItem == null || voteItem.length == 0) return null;
    return {
        "itemTitle": voteItem.attr("data-item-title"),
        "itemType": voteItem.attr("data-item-type"),
        "itemContent": voteItem.attr("data-item-list"),
        "itemMaxCount": voteItem.attr("data-max-count"),
    };
}