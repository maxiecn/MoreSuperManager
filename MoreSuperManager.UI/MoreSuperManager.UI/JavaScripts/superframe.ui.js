// 成功提醒
function SuccessAlert(note, url, closeCallback) {
    parent.SuccessAlert(note, url, closeCallback);
}
function SuccessAlertToCallback(note, callback, closeCallback) {
    parent.SuccessAlertToCallback(note, callback, closeCallback);
}
function AskConfirmSuperToUrl(note, okUrl, cancelUrl, closeCallback) {
    parent.AskConfirmSuperToUrl(note, okUrl, cancelUrl, closeCallback);
}
function AskConfirmSuperToCallback(note, okCallback, cancelCallback, closeCallback) {
    parent.AskConfirmSuperToCallback(note, okCallback, cancelCallback, closeCallback);
}

// 警告提醒
function WarningAlert(note, url, closeCallback) {
    parent.WarningAlert(note, url, closeCallback);
}
function WarningAlertToCallback(note, callback, closeCallback) {
    parent.WarningAlertToCallback(note, callback, closeCallback);
}

// 错误提醒
function ErrorAlert(note, url, closeCallback) {
    parent.ErrorAlert(note, url, closeCallback);
}
function ErrorAlertToCallback(note, callback, closeCallback) {
    parent.ErrorAlertToCallback(note, callback, closeCallback);
}

// 询问提醒，如果成功，则跳转到相应 URL
function AskConfirmToUrl(note, url, closeCallback) {
    parent.AskConfirmToUrl(note, url, closeCallback);
}

function AskConfirmToIFrameUrl(note, url, closeCallback) {
    parent.AskConfirmToIFrameUrl(note, url, closeCallback);
}
// 父元素回调
function AskConfirmToIFrameReturnUrl(url) {
    location.href = url;
}

// 询问提醒，如果成功，则调用相应的 Callback
function AskConfirmToCallback(note, callback, closeCallback) {
    parent.AskConfirmToCallback(note, callback, closeCallback);
}

// 遮罩 WINFOW
var MASK_WINDOW = null;
var MASK_WINDOW_TOP = 0;
var MASK_WINDOW_LEFT = 0;

$(function () {
    $("input[name='identityIDList']").click(function (evt) {
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    $("#tableList a[class='operater-edit']").click(function (evt) {
        // 跳转到编辑页面
        EditOperater($(this).attr("data-id"));
        // 阻止事件冒泡
        evt.stopPropagation();
    });
    $("#tableList a[class='operater-delete']").click(function (evt) {
        // 删除提示
        DeleteAskConfirmToUrl($(this).attr("data-id"));
        // 阻止事件冒泡
        evt.stopPropagation();
    });

    $("#btnOperaterSubmit").click(function () {
        var operaterType = $("#operaterType").val();
        // 只有删除才提示
        ValidOperaterMore(operaterType == "1");
    });
    $("#toolbarAdd").click(function () {
        location.href = $("#operaterAddUrl").val().replace("-1", 0);
    });
    $("#toolbarDelete").click(function () {
        ValidOperaterMore(true);
    });

    $("#checkAllOrNoBottom").change(function () {
        CheckBoxCheckAllOrNot(this);
    });
    $("#checkAllOrNoTop").change(function () {
        CheckBoxCheckAllOrNot(this);
    });
});

// 窗口大小改变
$(window).resize(function () {
});
$(window).scroll(function () {
    if (MASK_WINDOW != null) {
        var top = MASK_WINDOW_TOP + $(window).scrollTop();
        var left = MASK_WINDOW_LEFT + $(window).scrollLeft();
        MASK_WINDOW.css("top", top);
        MASK_WINDOW.css("left", left);
    }
});

function ShowAndHideRow(ele, pid, status) {
    var itemStatus = !($(ele).attr("data-status") == "true");
    $(ele).attr("data-status", itemStatus);

    if (itemStatus) {
        $("#span_tree_" + pid).removeClass("tree-default-icon").addClass("tree-expand-icon");
    } else {
        $("#span_tree_" + pid).removeClass("tree-expand-icon").addClass("tree-default-icon");
    }
    if (arguments.length == 2) status = itemStatus;

    // 获取所有符合条件的 TR 元素
    var childItemList = $("tr[data-pid='" + pid + "']");
    // 遍历 TR 元素
    if (childItemList != null && childItemList.length > 0) {
        for (var i = 0; i < childItemList.length; i++) {
            if (arguments.length <= 2) {
                if (status) {
                    $(childItemList[i]).show();
                } else {
                    $(childItemList[i]).hide();
                }
            } else {
                if (!status) {
                    $(childItemList[i]).hide();
                } else {
                    if (itemStatus) {
                        $(childItemList[i]).show();
                    }
                }
            }
            ShowAndHideRow(childItemList[i], $(childItemList[i]).attr("id"), status);
        }
    }
}
function CheckBoxCheckAllOrNot(element) {
    var checked = $(element).prop("checked");
    if (checked) {
        $("input[name='identityIDList']").prop("checked", true);
    } else {
        $('input[name="identityIDList"]:checked').prop("checked", false);
    }
    $("#checkAllOrNoTop").prop("checked", checked);
    $("#checkAllOrNoBottom").prop("checked", checked);
}
function ValidOperaterMore(isDelete) {
    var identityIDList = [];
    $('input[name="identityIDList"]:checked').each(function () {
        identityIDList.push($(this).val());
    });
    if (identityIDList.length == 0) {
        WarningAlert("没有要操作的数据！");
        return;
    }
    // 如果是删除
    if (isDelete) {
        //  询问删除
        AskConfirmToCallback("确定删除这些数据？", function () {
            $("#operaterForm").submit();
        });
    } else {
        $("#operaterForm").submit();
    }
}
function EditOperater(id) {
    location.href = $("#operaterEditUrl").val().replace("-1", id);
}
function DeleteAskConfirmToUrl(id) {
    AskConfirmToIFrameUrl("确定删除该条数据？", $("#operaterDeleteUrl").val().replace("-1", id));
}
// 显示遮罩层
function ShowMaskWindow(elementID, options) {
    // 添加遮罩层
    if ($("#ui-mask-background").length == 0) {
        var maskHtmlText = "<div id=\"ui-mask-background\" style=\"width:100%;height:100%;position:fixed;top:0px;left:0px;display:block;z-index:999;filter: Alpha(Opacity=30);opacity: 0.3; background-color:#000000;\"></div>";
        $("body").append(maskHtmlText);
    } else {
        $("#ui-mask-background").show();
    }

    var ele = $("#" + elementID);
    MASK_WINDOW = ele;
    parent.ShowMaskWindow(elementID, options, ele.outerWidth(), ele.outerHeight(), function (left, top) {
        MASK_WINDOW_TOP = top;
        MASK_WINDOW_LEFT = left;
        // 设置左边距，上边距
        ele.css("position", "absolute").css("z-index", 1000).css("left", $(window).scrollLeft() + left).css("top", $(window).scrollTop() + top).show();
    });
}
// 隐藏遮罩层
function HideMaskWindow() {
    MASK_WINDOW = null;
    parent.HideMaskWindow(function (eleID) {
        $("#ui-mask-background").hide();
        $("#" + eleID).hide();
    });
}
// 显示或者隐藏 loading 层
function ShowOrHiddenLoadingMask(progress) {
    if (progress >= 0) {
        $("#upload_progress").html(progress);
        ShowMaskWindow("loadingMaskWindow", {
            autoHide: false,
            top: 100
        });
    } else {
        HideMaskWindow();
    }
}

/*
	上传文件
	id：input file ID 名称
	url：上传文件的服务器地址
	filter：上传文件格式验证，正则表达式，例：/(\.|\/)(gif|jpe?g|png)$/i
	maxFileSize：最大上传文件大小：例：100 * 1024 * 1024，只能最大上传 100 M 文件
	filterText：上传文件格式验证失败提示文本
	maxFileSizeText：超过上传文件大小验证提示文本
	successCallback：上传成功回调，返回 Json 数据
	progressCallback：上传中回调，返回上传进度
	failCallback：上传失败回调，返回错误字符串
*/
function AsyncUploadFile(id, url, filter, maxFileSize, filterText, maxFileSizeText, successCallback, progressCallback, failCallback) {
    $("#" + id).fileupload({
        url: url,
        dataType: 'json',
        acceptFileTypes: filter,
        maxFileSize: maxFileSize,
        maxNumberOfFiles: 1, //最多只能上传一个
        messages: {
            maxFileSize: maxFileSizeText,
            acceptFileTypes: filterText
        },
        done: function (e, data) {
            ShowOrHiddenLoadingMask(-1);
            if (successCallback != null) successCallback(data.result);
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            if (progressCallback != null) {
                progressCallback(progress);
            } else {
                ShowOrHiddenLoadingMask(progress);
            }
        },
        processfail: function (e, data) {
            var currentFile = data.files[data.index];
            if (data.files.error && currentFile.error) {
                ShowOrHiddenLoadingMask(-1);
                if (failCallback != null) {
                    failCallback(currentFile.error);
                } else {
                    ErrorAlert(currentFile.error);
                }
            }
        }
    });
}
// 表单信息验证
function FormSubmitValidate(rules, messages, formName, errorNoteName) {

    if (arguments.length <= 2) formName = "submitForm";
    if (arguments.length <= 3) errorNoteName = "errorNote";

    if (rules == null) rules = {};

    $("#" + formName).validate({
        rules: rules,
        messages: messages,
        showErrors: function (errorMap, errorList) {
            if (errorList.length != 0) {
                $("#" + errorNoteName).empty().html(errorList[0].message);
            }
        }
    });
}
function GetFormatUploadSizeText(size) {
    var data = Number(size) / (1024 * 1024);
    return data.toFixed(1);
}
function SetMenuListGetJsonData(jsonData, dataType, elementID) {
    var html = "";
    var itemList = jsonData[dataType];
    if (itemList != null) {
        for (var index = 0; index < itemList.length; index++) {
            html += "<option value=\"{value}\">{name}</option>".replace(/[{]value[}]/gi, itemList[index].key).replace(/[{]name[}]/gi, itemList[index].value);
        }
    }
    $("#" + elementID).html(html);
}
function SetCheckBoxListGetJsonData(jsonData, dataType, elementID, checkBoxName) {
    var html = "";
    var itemList = jsonData[dataType];
    if (itemList != null) {
        for (var index = 0; index < itemList.length; index++) {
            html += "<label class=\"form-checkbox-label\"><input type=\"checkbox\" name=\"{checkBoxName}\" value=\"{value}\">{name}</label>".replace(/[{]checkBoxName[}]/gi, checkBoxName).replace(/[{]value[}]/gi, itemList[index].key).replace(/[{]name[}]/gi, itemList[index].value);
        }
    }
    $("#" + elementID).html(html);
}