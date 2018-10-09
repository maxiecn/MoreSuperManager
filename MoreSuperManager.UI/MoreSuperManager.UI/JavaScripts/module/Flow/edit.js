var STEP_ADD = "0";
var STEP_EDIT = "1";

function FlowDesignManager(flowID, processData, saveCallback) {
    var _canvas = $("#flowdesign_canvas").Flowdesign({
        "processData": processData,
        "canvasMenus": {
            "cmAdd": function (t) {
                // 显示面板
                ShowAddStepWindow(null);
            },
            "cmSave": function (t) {
                // 触发保存点击事件
                $("#btnSaveFlow").click();
            },
            "cmRefresh": function (t) {
                location.reload();//_canvas.refresh();
            }
        }, "processMenus": {
            "pmAttribute": function (t) {
                // 显示面板数据
                ShowAddStepWindow(_canvas.getActiveInfo());
            },
            "pmDelete": function (t) {
                if (confirm("确定删除此节点？")) {
                    _canvas.delProcess(_canvas.getActiveId());
                }
            }
        },
        fnClick: function () {

        }, fnDbClick: function () {
            // 显示面板数据
            ShowAddStepWindow(_canvas.getActiveInfo());
        }
    });

    function ShowAddStepWindow(data) {
        var left = $("#jqContextMenu").css("left");
        var top = $("#jqContextMenu").css("top");
        // 显示遮罩面板
        ShowMaskWindow("addStep");
        // 重置角色选择状态
        $('input[name="roleList"]:checked').prop("checked", false);
        if (data != null) {
            // 更新
            $("input:radio[name='symbolList'][value='" + data.code + "']").prop("checked", true);
            $("#stepCode").val(data.id);
            $("#stepName").val(data.step_name);
            $("#stepAddrName").val(data.step_addr_name);
            $("#nextStep").val(data.next_step);

            var roleDataList = [];
            var roleListValue = data.role_list;
            if (roleListValue != null && roleListValue != "") {
                roleDataList = roleListValue.split(",");
            }
            if (roleDataList.length > 0) {
                for (var i = 0; i < roleDataList.length; i++) {
                    $("input:checkbox[name='roleList'][value='" + roleDataList[i] + "']").prop("checked", true);
                }
            }
            // 设置为编辑状态
            $("#addStepStatus").val(STEP_EDIT);
        } else {
            // 设置为添加状态
            $("#addStepStatus").val(STEP_ADD);
            // 设置为可写
            $("#stepCode").removeAttr("readonly");
        }
    }

    $("#btnSaveFlow").click(function () {
        // 获取连接信息
        var processInfo = _canvas.getProcessInfo();//连接信息
        // 保存回调
        if (saveCallback != null) saveCallback(processInfo);
    });

    $("#btnAddStep").click(function () {
        // 获取操作状态
        var operaterStatus = $("#addStepStatus").val();

        var stepCode = StringTrim($("#stepCode").val());
        if (stepCode == "") {
            ErrorAlert("步骤编号不能为空！");
            return;
        }
        // 如果是添加
        if (operaterStatus == STEP_ADD) {
            if (_canvas.existsProcess(stepCode)) {
                ErrorAlert("步骤【" + stepCode + "】已存在！");
                return;
            }
        }

        //节点内容格式化
        var StepNameFormat = "[{stepCode}]{stepName}{stepAddrName}";
        // 获取节点编号
        var symbolCode = $("input[name='symbolList']:checked").val();
        var stepName = StringTrim($("#stepName").val());
        var stepAddrName = StringTrim($("#stepAddrName").val());
        var nextStep = StringTrim($("#nextStep").val());
        var roleListValue = "";
        var roleList = $("input[name='roleList']:checked").each(function () {
            roleListValue += $(this).val() + ",";
        });
        // 初始化节点名称
        StepNameFormat = StepNameFormat.replace(/[{]stepCode[}]/gi, stepCode).replace(/[{]stepName[}]/gi, stepName).replace(/[{]stepAddrName[}]/gi, stepAddrName != "" ? "(" + stepAddrName + ")" : stepAddrName);
        // 获取面板坐标
        var left = $("#jqContextMenu").css("left");
        var top = $("#jqContextMenu").css("top");
        // 设置节点数据
        var stepInfo = { "id": stepCode, "icon": symbolCode, "step_name": stepName, "step_addr_name": stepAddrName, "next_step" : nextStep,"flow_id": flowID, "process_name": StepNameFormat, "role_list": roleListValue, "style": "left:{left};top:{top};".replace(/[{]left[}]/gi, left).replace(/[{]top[}]/gi, top) };
        // 添加节点名称
        if (operaterStatus == STEP_ADD) {
            _canvas.addProcess(stepInfo);
        } else {
            // 更新节点信息
            _canvas.updateProcess(stepCode, stepInfo);
        }
        // 隐藏遮罩面板
        HideMaskWindow("addStep");
    });
}

$(function () {

    // 获取流程编号
    var flowID = $("#identityID").val();
    new FlowDesignManager(flowID, processData, function (data) {
        // 重新整理成后台使用的 数据结构
        var stepList = [];
        if (data != null) {
            for (var i in data) {
                var dataItem = {
                    "FlowID": 0,
                    "StepCode": i,
                    "StepSymbol": data[i].symbol_code,
                    "StepName": data[i].step_name,
                    "StepAddrName": data[i].step_addr_name,
                    "NextStep": data[i].next_step,
                    "RoleList": data[i].role_list,
                    "StepList": data[i].process_to.join(","),
                    "PositionTop": data[i].top,
                    "PositionLeft": data[i].left,
                };
                stepList.push(dataItem);
            }
        }

        if (stepList == null || stepList.length == 0) {
            ErrorAlert("请绘制流程步骤！");
            return;
        }

        var vLeft = ($(window).width() - $("#addFlowInfo").width()) * 0.5;
        $("#flowStepList").val(JSON.stringify(stepList));
        if (flowID == "0") {
            // 显示保存面板
            ShowMaskWindow("addFlowInfo", {
                top: 100,
                left: vLeft
            });
        } else {
            // 提交表单
            $("#operaterForm").submit();
        }
    });
    $("#btnAddFlowInfo").click(function () {
        HideMaskWindow("addFlowInfo");
        // 提交表单
        $("#operaterForm").submit();
    });
    $("#btnCancelStep").click(function () {
        HideMaskWindow("addStep");
    });
});