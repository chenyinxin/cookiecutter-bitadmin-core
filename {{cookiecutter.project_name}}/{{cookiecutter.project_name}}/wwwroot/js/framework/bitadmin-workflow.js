/***********************
 * BitAdmin2.0框架文件
 * 流程组件功能
 ***********************/
var workflow;
(function (workflow) {    
    //修改状态 为 已阅
    var workFlowRead = (function () {
        function workFlowRead(taskUserId, State) {
            if (taskUserId == null || taskUserId == '' || State != 'ToRead') {
                return;
            }
            $.pageLoading("show");
            $.ajax({
                url: '../../Workflow/UpdateStateReadEnd',
                cache: false,
                type: "GET",
                data: {
                    taskUserId: taskUserId
                },
                success: function (result) {
                    $.pageLoading("hide");
                    if (result.code == 1) {
                        alert(result.msg);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(XMLHttpRequest.responseText);
                    alert('待阅处理失败，请联系管理员');
                    $.pageLoading("hide");
                }
            });
        }
        return workFlowRead;
    })();
    workflow.workFlowRead = workFlowRead;

    //权限控制
    var SetEnabled = (function () {
        function SetEnabled(taskUserId, State, LinkCode) {
            switch (State) {
                case 'Done':
                case 'ToRead':
                case 'ReadEnd':
                    SetEnabled.HideTagFunc();
                    SetEnabled.ShowFlowHistory();
                    break;
                case 'ToDo':
                    $.ajax({
                        url: '../../Workflow/IsEnableFlowTab',
                        cache: false,
                        //asycn:false,
                        type: "GET",
                        data: {
                            taskUserId: taskUserId,
                            LinkCode: LinkCode
                        },
                        success: function (result) {
                            if (result.code == 1) {
                                alert(result.msg);
                                return;
                            }
                            if (result.data) {
                                //SetEnabled.ShowTagFunc();
                                SetEnabled.ShowFlowHistory();
                                return
                            }
                            SetEnabled.HideTagFunc();
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            console.log(XMLHttpRequest.responseText);
                            alert('待阅处理失败，请联系管理员');
                        }
                    });
                    break;
                default:
                    SetEnabled.HideFlowHistory();
                    //SetEnabled.ShowTagFunc();
                    break;
            }
        }
        SetEnabled.HideTagFunc = function () {
            $("input,textarea,select").attr("disabled", "disabled");//把输入框变为不可用    
            $("button").hide();//隐藏全部按钮
            $("*[class*='notHide']").show();//查看等不需要隐藏的按钮 
            var _data = $("[data-control]");
            _data.find('input,span').css({
                "cursor": "", "background-color": "#eee"
            });
            _data.find('span').unbind();


        }
        SetEnabled.ShowTagFunc = function () {
            $("input,textarea,select").removeAttr("disabled");//把输入框移除不可用
            $("button").show();//移除隐藏全部按钮
        }
        SetEnabled.ShowFlowHistory = function () {
            $("#flowHistoryBlock").show();
        }
        SetEnabled.HideFlowHistory = function () {
            $("#flowHistoryBlock").hide();
        }
        return SetEnabled;
    })();
    workflow.SetEnabled = SetEnabled;

})(workflow || (workflow = {}));

//流程套件
$.fn.workflow = function (option) {
    var _wrapper = $(this);
    var _option = {
        mainCode: '',//流程Code
        ParentId: '',
        expression: [],//表达式
        condition: '1',//同意步骤为1，不同意为10
        BillsCode: '',//流程审批关联的业务主键
        btnAuths: [], //设置按钮权限:2为保存草稿按钮,3为暂存按钮
        Primary: null, //
        taskUserId: '',//待办Id    Flow_BillsRecordUser 主键
        Type: null,   //类型：button(按钮)|hisotry(历史)
        load: null,
        setExpression: null,
        flowSubmit: null,
    };
    $.extend(_option, option);
    _option.Type = _wrapper.attr("data-type") || _option.Type;
    _option.mainCode = _wrapper.attr("data-maincode") || _option.mainCode;

    //按钮点击回调事件
    _option._btnBeforeCallback;
    _option.btnBefore = function (param) {
        if ($.isFunction(param)) {
            _option._btnBeforeCallback = param;
        }
        return _option;
    }

    //保存回调事件
    _option._saveCallback;
    _option.save = function (param) {
        if ($.isFunction(param)) {
            _option._saveCallback = param;
        }
        return _option;
    }

    //关闭回调事件
    _option._closeCallback;
    _option.close = function (param) {
        if ($.isFunction(param)) {
            _option._closeCallback = param;
        }
        return _option;
    }

    //加载
    _option._loadCallback;
    _option.load = function (param) {
        if ($.isFunction(param)) {
            _option._loadCallback = param;
        }

        _wrapper.html('');
        var url = '';
        if (_option.Type == 'hisotry') {
            url = '../Workflow/FlowHistory.html';
            eval("workflowHisotry=_option;");
        }
        else {
            url = '../Workflow/FlowHandle.html';
            eval("workflowbtn=_option;");
        }
        _wrapper.load(url, null, function (response, status, xhr) {
            if (status != 'success') alert('Load Workflow组件失败，请联系管理员');
        });
        return _option;
    }
    return _option;
}

//分页
$.fn.pagination = function (option) {
    var _wrapper = $(this);
    var _ul = _wrapper.find('ul');
    var _option = {
        pageSize: 10,
        pageIndex: 1,
        totalContent: 0,
        loadCallback: null
    };
    $.extend(_option, option);

    var pagezNum = Math.ceil(_option.totalContent / _option.pageSize);

    //清除html脚本
    _ul.html('');
    //首页
    var _control = $('<li><a href="javascript:void(0);" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>');
    _control.find('a').on("click", function () {
        _option.pageIndex = 1;
        _option.loadCallback(_option.pageIndex, _option.pageSize);
    });
    _ul.append(_control);

    //每一页
    for (var i = 1; i <= pagezNum; i++) {
        var classNmae = '';
        if (_option.pageIndex == i)
            classNmae = 'class="active"';
        _control = $('<li ' + classNmae + '><a href="javascript:void(0);">' + i + '</a></li>');
        _control.find('a').data("Data", { pageIndex: i });
        _control.find('a').on("click", function () {
            var _data = $(this).data("Data");
            _option.pageIndex = _data.pageIndex;
            _option.loadCallback(_option.pageIndex, _option.pageSize);
        });

        _ul.append(_control);
    }

    //尾页
    _control = $('<li><a href="javascript:void(0);" aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>');
    _control.find('a').on("click", function () {
        _option.pageIndex = pagezNum;
        _option.loadCallback(_option.pageIndex, _option.pageSize);
    });
    _ul.append(_control);

    return _option;
}
