// 成功提醒
function SuccessAlert(note, url, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.success, {
        onOk: function () {
            if (url != null && url != "") location.href = url;
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}
function SuccessAlertToCallback(note, callback, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.success, {
        onOk: function () {
            if (callback != null) callback();
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}

function AskConfirmSuperToUrl(note, okUrl, cancelUrl, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.confirm, {
        onOk: function () {
            if (okUrl != null && okUrl != "") location.href = okUrl;
        },
        onCancel: function () {
            if (cancelUrl != null && cancelUrl != "") location.href = cancelUrl;
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}
function AskConfirmSuperToCallback(note, okCallback, cancelCallback, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.confirm, {
        onOk: function () {
            if (okCallback != null) okCallback();
        },
        onCancel: function () {
            if (cancelCallback != null) cancelCallback();
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}

// 警告提醒
function WarningAlert(note, url, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.info, {
        onOk: function () {
            if (url != null && url != "") location.href = url;
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}
function WarningAlertToCallback(note, callback, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.info, {
        onOk: function () {
            if (callback != null) callback();
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}

// 错误提醒
function ErrorAlert(note, url, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.error, {
        onOk: function () {
            if (url != null && url != "") location.href = url;
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}
function ErrorAlertToCallback(note, callback, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.error, {
        onOk: function () {
            if (callback != null) callback();
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}

// 询问提醒，如果成功，则跳转到相应 URL
function AskConfirmToUrl(note, url, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.confirm, {
        onOk: function () {
            if (url != null && url != "") location.href = url;
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}

function AskConfirmToIFrameUrl(note, url, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.confirm, {
        onOk: function () {
            if (url != null && url != "") {
                var activeFrameItem = GetActiveIFrameItem();
                if (activeFrameItem == null || activeFrameItem.length == 0) return;
                // 触发框架页面事件
                activeFrameItem[0].contentWindow.AskConfirmToIFrameReturnUrl(url);
            }
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}

function AskConfirmToCallback(note, callback, closeCallback) {
    window.wxc.xcConfirm(note, window.wxc.xcConfirm.typeEnum.confirm, {
        onOk: function () {
            if (callback != null) callback();
        },
        onClose: function () {
            if (closeCallback != null) closeCallback();
        }
    });
}

// 之前操作的菜单编号
var PREV_EXPAND_OR_CLOSE_MENU_ID = 0;
var PREV_TAB_ID = 0;
var TAB_CONTEXT_MENU_DATA = null;
var TAB_CONTEXT_MENU_DEFAULT = null;
var FRAME_WIDTH = 0;
var FRAME_HEIGHT = 0;
var SHOW_MASK_WINDOW_ID = null;
var SHOW_MASK_WINDOW_OPTIONS = null;
var TEMP_FRAME_INDEX = 0;

$(function () {

    // 初始化滚动区域宽度
    InitTabScrollRangeWidth();
    // 初始化 IFrame 宽高
    InitFrameWidthAndHeight();
    // 初始化首页
    AppendMenuItem(0);
    // 附加按钮事件
    $("#btnAttachToFrame").click(function () {

        var url = $("#txtAttachToFrame").val();
        if (url == "") return;

        AddMenuItem("TempFrame_" + TEMP_FRAME_INDEX, url);
        TEMP_FRAME_INDEX++;

        $("#txtAttachToFrame").val("");
    });
    // 
    $("#frameNavList [data-trunk]").click(function (evt) {
        // 获取菜单编号
        var menuID = $(this).attr("data-trunk");
        //
        var trunkItem = $("#tree_trunk_" + menuID);
        var nodeItem = $("#tree_node_" + menuID);
        var treeIconItem = $("#tree_icon_" + menuID);
        // 获取索引数据
        var layerIndex = trunkItem.attr("data-layer");
        if (layerIndex <= 1) {
            // 如果之前操作的菜单编号与当前操作的编号不同
            if (PREV_EXPAND_OR_CLOSE_MENU_ID != menuID) {
                // 展开或者折叠之前的菜单
                var prevTrunkItem = $("#tree_trunk_" + PREV_EXPAND_OR_CLOSE_MENU_ID);
                var prevLayerIndex = prevTrunkItem.attr("data-layer");
                if (prevLayerIndex <= 1) prevTrunkItem.removeClass("selected");
                // 隐藏元素
                $("#tree_node_" + PREV_EXPAND_OR_CLOSE_MENU_ID).hide();
                // 设置隐藏/显示箭头
                $("#tree_icon_" + PREV_EXPAND_OR_CLOSE_MENU_ID).removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-left");
                // 设置菜单编号
                PREV_EXPAND_OR_CLOSE_MENU_ID = menuID;
            } else {
                PREV_EXPAND_OR_CLOSE_MENU_ID = 0;
            }
        }
        // 判断当前状态是否隐藏
        if (nodeItem.is(":hidden")) {
            // 显示列表
            nodeItem.show();
            // 设置展开/折叠状态
            treeIconItem.removeClass("glyphicon-chevron-left").addClass("glyphicon-chevron-down");
            // 只有第一级才操作
            if (layerIndex <= 1) {
                trunkItem.addClass("selected");
            }
        } else {
            // 隐藏列表
            nodeItem.hide();
            // 设置展开/折叠状态
            treeIconItem.removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-left");
            // 只有第一级才操作
            if (layerIndex <= 1) {
                trunkItem.removeClass("selected");
            }
        }
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    $("#frameNavList [data-node]").click(function (evt) {
        // 追加选项卡菜单
        AppendMenuItem($(this).attr("data-node"), $(this).attr("data-title"), $(this).attr("data-url"));
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    $("#frameNavList [data-node]").mouseenter(function (evt) {
        // 如果有兄弟节点
        if ($(this).parent().next().length > 0) {
            $(this).css("padding-bottom", "8px");
            $(this).parent().next().children("a").css("padding-top", "7px");
        }
    });
    $("#frameNavList [data-node]").mouseleave(function (evt) {
        $(this).css("padding-bottom", "0px");
        $(this).parent().next().children("a").css("padding-top", "15px");
    });
    $("#tabContainer [data-button]").click(function (evt) {
        // 获取按钮类型
        var buttonType = $(this).attr("data-button");
        // 如果是向后
        if (buttonType == "backward") {
            // 重置当前 LEFT 位置
            $("#tabItemList").css("margin-left", "0px");
            // 默认选择第一个
            ResetTabItemListStatus(0);
        } else {
            // 设置选项卡位置
            SetTabItemListPosition();
            // 默认选择最后一个
            ResetTabItemListStatus(-1);
        }
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    $("#btnExpandOrCloseNav").click(function () {
        // 获取子元素内容
        var item = $(this).children("i");
        // 如果是展开状态
        if (item.hasClass("glyphicon-folder-open")) {
            // 移除样式
            item.removeClass("glyphicon-folder-open").addClass("glyphicon-folder-close");
            $("#frameContent").css("margin-left", "0px");
            $("#frameNav").hide();
        } else {
            item.removeClass("glyphicon-folder-close").addClass("glyphicon-folder-open");
            $("#frameNav").show();
            $("#frameContent").css("margin-left", $("#frameNav").outerWidth() + "px");
        }
        // 调用窗口大小
        $(window).resize();
    });
    // 选项卡右键菜单
    TAB_CONTEXT_MENU_DATA = [
        {
            "text": "重新加载",
            "func": function () {
                // 重置之前选中的选项卡编号
                $("#iframe_item_" + $(this).attr("data-id")).attr("src", $(this).attr("data-url"));
            }
        }, {
            "text": "关闭标签页",
            "func": function () {
                $("#tab_item_icon_" + $(this).attr("data-id")).click();
            }
        }, {
            "text": "关闭其他标签页",
            "func": function () {
                CloseTabItemList($(this).attr("data-id"));
            },
        }, {
            "text": "关闭所有标签页",
            "func": function () {
                CloseTabItemList();
            },
        }
    ];
    TAB_CONTEXT_MENU_DEFAULT = [
        {
            "text": "重新加载",
            "func": function () {
                $("#tab_item_" + $(this).attr("data-id")).click();
            }
        }
    ];
});

// 窗口大小改变
$(window).resize(function () {
    // 初始化 IFrame 宽高
    InitFrameWidthAndHeight();
    // 初始化选项卡滚动区域宽度
    InitTabScrollRangeWidth();
    // 设置选项卡位置
    var activeItem = $("#tabItemList li[class='active']");
    // 找到被选中的选项卡菜单
    if (activeItem != null && activeItem.length > 0) {
        // 获得总宽度
        var vLeft = parseInt($("#tabItemList").css("margin-left"));
        var width = $("#tabScrollRegion").outerWidth() - (GetTabItemListWidth() + vLeft);
        if (width >= (0 - vLeft)) {
            $("#tabItemList").css("margin-left", "0px");
        } else {
            $("#tabItemList").css("margin-left", (vLeft + width) + "px");
        }
        // 执行点击事件
        $("#tab_item_" + activeItem.attr("data-id")).click();
    } else {
        SetTabItemListPosition();
    }

    var maskElement = $("#ui-mask-background");
    if (maskElement != null && maskElement.length > 0 && !maskElement.is(':hidden')) {
        // 找到当前选项中的 IFRAME
        var activeFrameItem = GetActiveIFrameItem();
        if (activeFrameItem.length > 0) {
            // 调用 IFRAME 函数
            activeFrameItem[0].contentWindow.ShowMaskWindow(SHOW_MASK_WINDOW_ID, SHOW_MASK_WINDOW_OPTIONS);
        }
    }
});
// 初始化 IFrame 宽高
function InitFrameWidthAndHeight() {
    var vHeight = $(window).height() - $("#contentTop").outerHeight() - $("#tabContainer").outerHeight();
    var vWidth = $(window).width();
    if (!$("#frameNav").is(":hidden")) {
        vWidth -= $("#frameNav").outerWidth();
    }
    $("#frameContent").width(vWidth);

    FRAME_WIDTH = vWidth;
    FRAME_HEIGHT = vHeight;

    var frameList = $("#iframeContainer iframe");
    for (var i = 0; i < frameList.length; i++) {
        $(frameList[i]).width(vWidth).height(vHeight);
    }
}
// 初始化选项卡滚动区域宽度
function InitTabScrollRangeWidth() {
    var itemList = $("#tabContainer div[data-button]");

    var width = $("#frameContent").outerWidth();
    if (itemList != null && itemList.length > 0) {
        for (var i = 0; i < itemList.length; i++) {
            width -= $(itemList[i]).outerWidth();
        }
    }
    $("#tabScrollRegion").width(width);
}
function AddMenuItem(title, url) {
    var id = Guid();
    AppendMenuItem(id, title, url);
}
// 追加选项卡菜单
function AppendMenuItem(id, title, url) {
    // 只有非首页才进行下面操作
    if (id != 0) {
        // 检查是否已经添加过相应的选项卡
        var tabItem = $("#tab_item_" + id);
        // 如果已经添加过该选项卡
        if (tabItem != null && tabItem.length > 0) {
            // 做操作
            $("#tab_item_" + id).click();
            return;
        }
        // 重置选择的样式
        ResetTabItemListStatus(-2);
        // 创建 Iframe 
        $("#iframeContainer").append("<iframe id=\"iframe_item_" + id + "\" data-id=\"" + id  + "\" src=\"" + url + "\" class=\"frame-iframe\"></iframe>");
        // 追加选项卡数据
        $("#tabItemList").append("<li class=\"active\" data-id=\"" + id + "\" data-url=\"" + url + "\" id=\"tab_item_" + id + "\"><a>" + title + "<i class=\"glyphicon glyphicon-remove\" data-id=\"" + id + "\" id=\"tab_item_icon_" + id + "\"></i></a></li>");
        // 设置当前选项卡为选中状态
        SetTabItemListActiveByID(id);
    }
    // 绑定点击事件
    $("#tab_item_" + id).click(function (evt) {
        // 重置所有状态
        ResetTabItemListStatus(-2);
        // 设置选中状态
        SetTabItemListActiveByItem(this);
        // 选择左状态
        var left = parseInt($("#tabItemList").css("margin-left"));
        // 获取当前选项卡宽度
        var menuWidth = GetTabItemListWidth($(this).attr("data-id"));
        var vLeft = (0 - left) - (menuWidth - $(this).width());
        if (vLeft > 0) {
            $("#tabItemList").css("margin-left", (left + vLeft) + "px");
        } else {
            var width = $("#tabScrollRegion").width();
            if (menuWidth > (width - left)) {
                $("#tabItemList").css("margin-left", (width - menuWidth) + "px");
            }
        }
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    // 只有非首页才进行下面操作
    if (id != 0) {
        // 绑定右键菜单
        var menuData = [];
        $("#tab_item_" + id).smartMenu(menuData, {
            beforeShow: function () {
                $.smartMenu.remove();
                if ($(this).hasClass("active")) {
                    menuData[0] = TAB_CONTEXT_MENU_DATA;
                } else {
                    menuData[0] = TAB_CONTEXT_MENU_DEFAULT;
                }
            }
        });
        // 绑定删除事件
        $("#tab_item_icon_" + id).click(function (evt) {
            // 默认数据
            var activeItem = null;
            // 获取选项卡
            var tabItem = $("#tab_item_" + $(this).attr("data-id"));
            // 如果当前选项卡是选中状态
            if (tabItem.hasClass("active")) {
                // 找兄弟节点,如果存在兄弟节点，选中，
                var activeItem = tabItem.prev();
                // 如果没有前置兄弟节点，则找后置兄弟节点
                if (activeItem == null || activeItem.length == 0) {
                    activeItem = tabItem.next();
                }
            }
            // 移除选项卡和 IFrame
            RemoveTabAndIFrameItem(tabItem, $(this).attr("data-id"));
            // 如果有兄弟节点，设置兄弟节点选中
            if (activeItem != null && activeItem.length > 0) {
                activeItem.addClass("active");
                // 触发点击事件
                activeItem.click();
            }
            // 阻止事件冒泡
            evt.stopPropagation();
        });
        // 设置选项卡位置
        SetTabItemListPosition();
    }
}
// 0 表示默认选中第一个，-1 表示默认选中最后一个，如果 index 在范围之中，则选中
function ResetTabItemListStatus(index) {
    var itemList = $("#tabItemList li");
    for (var i = 0; i < itemList.length; i++) {
        $(itemList[i]).removeClass("active");
    }
    if (index == 0) {
        $(itemList[0]).addClass("active");
    } else if (index == -1) {
        $(itemList[itemList.length - 1]).addClass("active");
    } else {
        if (index >= 0 && index <= itemList.length - 1) {
            $(itemList[index]).addClass("active");
        }
    }
}
function ResetIframeItemListStatus(id) {
    var frameList = $("#iframeContainer iframe");
    for (var i = 0; i < frameList.length; i++) {
        // 除此之外全部隐藏
        if ($(frameList[i]).attr("data-id") != id) {
            $(frameList[i]).hide();
        } else {
            $(frameList[i]).show();
            $(frameList[i]).width(FRAME_WIDTH).height(FRAME_HEIGHT);
        }
    }
}
// 设置选项卡位置
function SetTabItemListPosition() {
    var width = $("#tabScrollRegion").width();
    // 获得宽度
    var itemWidth = GetTabItemListWidth();

    if (itemWidth > width) {
        $("#tabItemList").css("margin-left", (width - itemWidth) + "px");
    } else {
        $("#tabItemList").css("margin-left", "0px");
    }
}
// 获得当前菜单宽度
function GetTabItemListWidth(id) {
    var itemWidth = 0;
    var itemList = $("#tabItemList li");
    for (var i = 0; i < itemList.length; i++) {
        itemWidth += $(itemList[i]).outerWidth();
        if (id != null && id != "") {
            if ($(itemList[i]).attr("data-id") == id) return itemWidth;
        }
    }
    return itemWidth;
}
// 设置选项卡激活
function SetTabItemListActive(ele) {
    // 设置当前选项卡选中
    ele.addClass("active");

    var tabID = ele.attr("data-id");

    // 如果当前正处于打开状态，返回
    if (tabID == PREV_TAB_ID) return;
    PREV_TAB_ID = tabID;

    // 获取 URL 信息
    var url = ele.attr("data-url");
    if (url == null || url == "") return;
    // 显示当前 IFrame
    ResetIframeItemListStatus(tabID);
    // 设置 IFrame URL
    //$("#iframe_item_" + tabID).attr("src", url);
}
function SetTabItemListActiveByItem(item) {
    SetTabItemListActive($(item));
}
function SetTabItemListActiveByID(id) {
    SetTabItemListActive($("#tab_item_" + id));
}
function CloseTabItemList(id) {
    var itemList = $("#tabItemList li");
    for (var i = 0; i < itemList.length; i++) {
        // 获取选项卡编号
        var itemID = $(itemList[i]).attr("data-id");
        // 如果是关闭其他标签页，直接跳过
        if (itemID == "0" || itemID == id) continue;
        // 移除选项卡和 IFrame
        RemoveTabAndIFrameItem(itemList[i], itemID);
        // 设置位置
        $("#tabItemList").css("margin-left", "0px");
    }
    if (id == null || id == "") {
        // 如果关闭全部，则选择首页
        $("#tab_item_0").click();
    }
}
function RemoveTabAndIFrameItem(item, id) {
    // 移除绑定
    $(item).unbind();
    $("#tab_item_icon_" + id).unbind();

    $(item).remove();
    // 删除 Iframe
    $("#iframe_item_" + id).remove();
}
// 获取当前选中的 IFrame
function GetActiveIFrameItem() {
    var frameList = $("#iframeContainer iframe");
    for (var i = 0; i < frameList.length; i++) {
        if (!$(frameList[i]).is(":hidden")) return $(frameList[i]);
    }
}

// 显示遮罩层
function ShowMaskWindow(elementID, options, eleWidth, eleHeight, callback) {

    SHOW_MASK_WINDOW_ID = elementID;
    SHOW_MASK_WINDOW_OPTIONS = options;

    // 如果输入数据不正确
    if (elementID == null || elementID == "") return;

    var activeFrameItem = GetActiveIFrameItem();
    if (activeFrameItem == null || activeFrameItem.length == 0) return;

    activeFrameItem.css("z-index", "1000");

    // 如果未添加遮罩层
    if ($("#ui-mask-background").length == 0) {
        var maskHtmlText = "<div id=\"ui-mask-background\" style=\"width:100%;height:100%;position:fixed;top:0px;left:0px;display:block;z-index:999;filter: Alpha(Opacity=30);opacity: 0.3; background-color:#000000;\"></div>";
        $("body").append(maskHtmlText);
    } else {
        $("#ui-mask-background").show();
    }

    var top = ($(document).height() - eleHeight) * 0.5;
    var left = ($(document).width() - eleWidth) * 0.5;
    // 减去顶部
    top -= ($("#contentTop").outerHeight() + $("#tabContainer").outerHeight());
    if (!$("#frameNav").is(":hidden")) {
        // 减去左边
        left -= $("#frameNav").outerWidth();
    }

    if (options != null) {
        if (options.hasOwnProperty("top")) {
            top = options["top"];
        }
        if (options.hasOwnProperty("left")) {
            left = options["left"];
        }
        if (options.hasOwnProperty("offsetX")) {
            left -= Number(options["offsetX"]);
        }
        if (options.hasOwnProperty("offsetY")) {
            top -= Number(options["offsetY"]);
        }
    }
    if (callback != null) callback(left, top);
}

// 隐藏遮罩层
function HideMaskWindow(callback) {
    $("#ui-mask-background").hide();
    if (callback != null) callback(SHOW_MASK_WINDOW_ID);
}