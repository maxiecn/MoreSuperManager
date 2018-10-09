/*
 * 使用说明:
 * window.wxc.Pop(popHtml, [type], [options])
 * popHtml:html字符串
 * type:window.wxc.xcConfirm.typeEnum集合中的元素
 * options:扩展对象
 * 用法:
 * 1. window.wxc.xcConfirm("我是弹窗<span>lalala</span>");
 * 2. window.wxc.xcConfirm("成功","success");
 * 3. window.wxc.xcConfirm("请输入","input",{onOk:function(){}})
 * 4. window.wxc.xcConfirm("自定义",{title:"自定义"})
 */
(function ($) {
    window.wxc = window.wxc || {};
    window.wxc.xcConfirm = function (popHtml, type, options) {
        var btnType = window.wxc.xcConfirm.btnEnum;
        var eventType = window.wxc.xcConfirm.eventEnum;
        var popType = {
            info: { title: "信息", icon: "0 0", btn: btnType.ok },
            success: { title: "成功", icon: "0 -48px", btn: btnType.ok },
            error: { title: "错误", icon: "-48px -48px", btn: btnType.ok },
            confirm: { title: "提示", icon: "-48px 0", btn: btnType.okcancel },
            warning: { title: "警告", icon: "0 -96px", btn: btnType.okcancel },
            input: { title: "输入", icon: "", btn: btnType.ok },
            custom: { title: "", icon: "", btn: btnType.ok }
        };
        var itype = type ? type instanceof Object ? type : popType[type] || {} : {};//格式化输入的参数:弹窗类型
        var config = $.extend(true, {
            //属性
            title: "", //自定义的标题
            icon: "", //图标
            btn: btnType.ok, //按钮,默认单按钮
            //事件
            onOk: $.noop,//点击确定的按钮回调
            onCancel: $.noop,//点击取消的按钮回调
            onClose: $.noop//弹窗关闭的回调,返回触发事件
        }, itype, options);

        var $txt = $("<p>").html(popHtml);//弹窗文本dom
        var $tt = $("<span>").addClass("tt").text(config.title);//标题
        var icon = config.icon;
        var $icon = icon ? $("<div>").addClass("bigIcon").css("backgroundPosition", icon) : "";
        var btn = config.btn;//按钮组生成参数

        var popId = creatPopId();//弹窗索引

        var $box = $("<div>").addClass("xcConfirm");//弹窗插件容器
        var $layer = $("<div>").addClass("xc_layer");//遮罩层
        var $popBox = $("<div>").addClass("popBox");//弹窗盒子
        var $ttBox = $("<div>").addClass("ttBox");//弹窗顶部区域
        var $txtBox = $("<div>").addClass("txtBox");//弹窗内容主体区
        var $btnArea = $("<div>").addClass("btnArea");//按钮区域

        var $ok = $("<a>").addClass("sgBtn").addClass("ok").text("确定");//确定按钮
        var $cancel = $("<a>").addClass("sgBtn").addClass("cancel").text("取消");//取消按钮
        var $input = $("<input>").addClass("inputBox");//输入框
        var $clsBtn = $("<a>").addClass("clsBtn");//关闭按钮

        //建立按钮映射关系
        var btns = { ok: $ok, cancel: $cancel };

        init();

        function init() {
            //处理特殊类型input
            if (popType["input"] === itype) {
                $txt.append($input);
            }

            creatDom();
            bind();
        }

        function creatDom() {
            $popBox.append(
				$ttBox.append(
					$clsBtn
				).append(
					$tt
				)
			).append(
				$txtBox.append($icon).append($txt)
			).append(
				$btnArea.append(creatBtnGroup(btn))
			);
            $box.attr("id", popId).append($layer).append($popBox);
            $("body").append($box);
        }

        function bind() {
            //点击确认按钮
            $ok.click(doOk);

            //回车键触发确认按钮事件
            $(window).bind("keydown", function (e) {
                if (e.keyCode == 13) {
                    if ($("#" + popId).length == 1) {
                        doOk();
                    }
                }
            });

            //点击取消按钮
            $cancel.click(doCancel);

            //点击关闭按钮
            $clsBtn.click(doClose);
        }

        //确认按钮事件
        function doOk() {
            var $o = $(this);
            var v = $.trim($input.val());
            if ($input.is(":visible"))
                config.onOk(v);
            else
                config.onOk();
            $("#" + popId).remove();
            config.onClose(eventType.ok);
        }

        //取消按钮事件
        function doCancel() {
            var $o = $(this);
            config.onCancel();
            $("#" + popId).remove();
            config.onClose(eventType.cancel);
        }

        //关闭按钮事件
        function doClose() {
            $("#" + popId).remove();
            config.onClose(eventType.close);
            $(window).unbind("keydown");
        }

        //生成按钮组
        function creatBtnGroup(tp) {
            var $bgp = $("<div>").addClass("btnGroup");
            $.each(btns, function (i, n) {
                if (btnType[i] == (tp & btnType[i])) {
                    $bgp.append(n);
                }
            });
            return $bgp;
        }

        //重生popId,防止id重复
        function creatPopId() {
            var i = "pop_" + (new Date()).getTime() + parseInt(Math.random() * 100000);//弹窗索引
            if ($("#" + i).length > 0) {
                return creatPopId();
            } else {
                return i;
            }
        }
    };

    //按钮类型
    window.wxc.xcConfirm.btnEnum = {
        ok: parseInt("0001", 2), //确定按钮
        cancel: parseInt("0010", 2), //取消按钮
        okcancel: parseInt("0011", 2) //确定&&取消
    };

    //触发事件类型
    window.wxc.xcConfirm.eventEnum = {
        ok: 1,
        cancel: 2,
        close: 3
    };

    //弹窗类型
    window.wxc.xcConfirm.typeEnum = {
        info: "info",
        success: "success",
        error: "error",
        confirm: "confirm",
        warning: "warning",
        input: "input",
        custom: "custom"
    };

})(jQuery);




(function (a) { var b = a(document).data("func", {}); a.smartMenu = a.noop; a.fn.smartMenu = function (f, c) { var i = a("body"), g = { name: "", offsetX: 2, offsetY: 2, textLimit: 8, beforeShow: a.noop, afterShow: a.noop }; var h = a.extend(g, c || {}); var e = function (k) { var m = k || f, j = k ? Math.random().toString() : h.name, o = "", n = "", l = "smart_menu_"; if (a.isArray(m) && m.length) { o = '<div id="smartMenu_' + j + '" class="' + l + 'box"><div class="' + l + 'body"><ul class="' + l + 'ul">'; a.each(m, function (q, p) { if (q) { o = o + '<li class="' + l + 'li_separate">&nbsp;</li>' } if (a.isArray(p)) { a.each(p, function (s, v) { var w = v.text, u = "", r = "", t = Math.random().toString().replace(".", ""); if (w) { if (w.length > h.textLimit) { w = w.slice(0, h.textLimit) + "…"; r = ' title="' + v.text + '"' } if (a.isArray(v.data) && v.data.length) { u = '<li class="' + l + 'li" data-hover="true">' + e(v.data) + '<a href="javascript:" class="' + l + 'a"' + r + ' data-key="' + t + '"><i class="' + l + 'triangle"></i>' + w + "</a></li>" } else { u = '<li class="' + l + 'li"><a href="javascript:" class="' + l + 'a"' + r + ' data-key="' + t + '">' + w + "</a></li>" } o += u; var x = b.data("func"); x[t] = v.func; b.data("func", x) } }) } }); o = o + "</ul></div></div>" } return o }, d = function () { var j = "#smartMenu_", l = "smart_menu_", k = a(j + h.name); if (!k.size()) { a("body").append(e()); a(j + h.name + " a").bind("click", function () { var m = a(this).attr("data-key"), n = b.data("func")[m]; if (a.isFunction(n)) { n.call(b.data("trigger")) } a.smartMenu.hide(); return false }); a(j + h.name + " li").each(function () { var m = a(this).attr("data-hover"), n = l + "li_hover"; a(this).hover(function () { var o = a(this).siblings("." + n); o.removeClass(n).children("." + l + "box").hide(); o.children("." + l + "a").removeClass(l + "a_hover"); if (m) { a(this).addClass(n).children("." + l + "box").show(); a(this).children("." + l + "a").addClass(l + "a_hover") } }) }); return a(j + h.name) } return k }; a(this).each(function () { this.oncontextmenu = function (l) { if (a.isFunction(h.beforeShow)) { h.beforeShow.call(this) } l = l || window.event; l.cancelBubble = true; if (l.stopPropagation) { l.stopPropagation() } a.smartMenu.hide(); var k = b.scrollTop(); var j = d(); if (j) { j.css({ display: "block", left: l.clientX + h.offsetX, top: l.clientY + k + h.offsetY }); b.data("target", j); b.data("trigger", this); if (a.isFunction(h.afterShow)) { h.afterShow.call(this) } return false } } }); if (!i.data("bind")) { i.bind("click", a.smartMenu.hide).data("bind", true) } }; a.extend(a.smartMenu, { hide: function () { var c = b.data("target"); if (c && c.css("display") === "block") { c.hide() } }, remove: function () { var c = b.data("target"); if (c) { c.remove() } } }) })(jQuery);


/*
    原生对象扩展，所有函数都大写
*/
function Guid() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
}
function StringTrim(str) {
    if (str == null) return "";
    return str.replace(/(^\s*)|(\s*$)/g, "");
}
function StringLeftTrim(str) {
    if (str == null) return "";
    return str.replace(/(^\s*)/g, "");
}
function StringRightTrim(str) {
    if (str == null) return "";
    return str.replace(/(\s*$)/g, "");
}

// 
function ArrayGetItemData(arr, propertyName, propertyValue) {
    // 如果数组为空，或者长度为 0，则返回
    if (arr.length == 0) return null;
    var len = arr.length;
    for (var i = 0; i < len; i++) {
        if (arr[i].hasOwnProperty(propertyName) && arr[i][propertyName] == propertyValue) return arr[i];
    }
    return null;
}
function ArrayGetItemIndex(arr, propertyName, propertyValue) {
    // 如果数组为空，或者长度为 0，则返回
    if (arr.length == 0) return -1;
    var len = arr.length;
    for (var i = 0; i < len; i++) {
        if (arr[i].hasOwnProperty(propertyName) && arr[i][propertyName] == propertyValue) return i;
    }
    return -1;
}
function ArraySetItemData(arr, data, propertyName, propertyValue) {
    var index = ArrayGetItemIndex(propertyName, propertyValue);
    if (index == -1) return;
    arr[index] = data;
}
// 交换元素
function ArraySwapItemData(arr, oneIndex, twoIndex) {
    arr[oneIndex] = arr.splice(twoIndex, 1, arr[oneIndex])[0];
}
function ArraySwapUpItemData(arr, index) {
    if (index == 0) {
        return;
    }
    ArraySwapItemData(arr, index, index - 1);
}
function ArraySwapDownItemData(arr, index) {
    if (index == arr.length - 1) {
        return;
    }
    ArraySwapItemData(arr, index, index + 1);
}