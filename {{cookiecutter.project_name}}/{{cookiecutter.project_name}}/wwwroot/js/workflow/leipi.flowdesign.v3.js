
(function ($) {
    var defaults = {
        processData: {},//步骤节点数据
        //processUrl:'',//步骤节点数据
        fnRepeat: function () { alert("步骤连接重复"); },
        fnClick: function () { alert("单击"); },
        fnDbClick: function () { alert("双击"); },
        canvasMenus: { "one": function (t) { alert('画面右键') } },
        processMenus: { "one": function (t) { alert('步骤右键') } },
        /*右键菜单样式*/
        menuStyle: {
            border: '1px solid #5a6377',
            minWidth: '150px',
            padding: '5px 0'
        },
        itemStyle: {
            fontFamily: 'verdana',
            color: '#333',
            border: '0',
            padding: '5px 40px 5px 20px'
        },
        itemHoverStyle: {
            border: '0',
            color: '#fff',
            backgroundColor: '#5a6377'
        },
        mtAfterDrop: function (params) {
            //alert('连接成功后调用');
            //alert("连接："+params.sourceId +" -> "+ params.targetId);
        },
        //这是连接线路的绘画样式
        connectorPaintStyle: {
            lineWidth: 5,
            strokeStyle: "#49afcd",
            joinstyle: "round"
        },
        //鼠标经过样式
        connectorHoverStyle: {
            lineWidth: 3,
            strokeStyle: "#da4f49"
        }

    };/*defaults end*/

    var initEndPoints = function () {
        $(".process-flag").each(function (i, e) {
            var p = $(e).parent();
            jsPlumb.makeSource($(e), {
                parent: p,
                anchor: "Continuous",
                endpoint: ["Dot", { radius: 1 }],
                connector: ["Flowchart", { stub: [5, 5] }],
                connectorStyle: defaults.connectorPaintStyle,
                hoverPaintStyle: defaults.connectorHoverStyle,
                dragOptions: {},
                maxConnections: -1
            });
        });
    }

    /*设置隐藏域保存关系信息*/
    var aConnections = [];
    var setConnections = function (conn, remove) {
        if (!remove) {
            aConnections.push(conn);
            conn.startStepId = $('#' + conn.sourceId).data("data").stepId;
            conn.stopStepId = $('#' + conn.targetId).data("data").stepId;
            $.each(defaults.processData.paths, function (i, item) {
                if (item.startStepId == conn.startStepId && item.stopStepId == conn.stopStepId)
                    conn.data = item;
            });
            if (conn.data == undefined) {
                conn.data = { nikename: "同意", condition: 1, expression: "" };
                conn.newline = true;
            }
        }
        else {
            var idx = -1;
            for (var i = 0; i < aConnections.length; i++) {
                if (aConnections[i] == conn) {
                    idx = i; break;
                }
            }
            if (idx != -1) aConnections.splice(idx, 1);
        }

        $('#leipi_process_info').html('');
        for (var j = 0; j < aConnections.length; j++) {
            var path = $("<input type='hidden' value=\"" + aConnections[j].startStepId + "," + aConnections[j].stopStepId + "\" >").data("data", aConnections[j].data);
            $('#leipi_process_info').append(path);
        }
        if (conn.newline) lineCondition(conn);
        jsPlumb.repaintEverything();//重画
    };

    /*Flowdesign 命名纯粹为了美观，而不是 formDesign */
    $.fn.Flowdesign = function (options) {
        var _canvas = $(this);
        //右键步骤的步骤号
        _canvas.append('<input type="hidden" id="leipi_active_id" value="0"/><input type="hidden" id="leipi_copy_id" value="0"/>');
        _canvas.append('<div id="leipi_process_info"></div>');

        /*配置*/
        $.each(options, function (i, val) {
            if (typeof val == 'object' && defaults[i])
                $.extend(defaults[i], val);
            else
                defaults[i] = val;
        });
        /*画布右键绑定*/
        var contextmenu = {
            bindings: defaults.canvasMenus,
            menuStyle: defaults.menuStyle,
            itemStyle: defaults.itemStyle,
            itemHoverStyle: defaults.itemHoverStyle
        }
        $(this).contextMenu('canvasMenu', contextmenu);

        jsPlumb.importDefaults({
            DragOptions: { cursor: 'pointer' },
            EndpointStyle: { fillStyle: '#225588' },
            Endpoint: ["Dot", { radius: 1 }],
            ConnectionOverlays: [
                ["Arrow", { location: 1 }],
                ["Label", {
                    location: 0.1,
                    id: "label",
                    cssClass: "aLabel"
                }]
            ],
            Anchor: 'chenyinxin',
            ConnectorZIndex: 5,
            HoverPaintStyle: defaults.connectorHoverStyle
        });

        //初始化原步骤
        var lastProcessId = 0;
        var processData = defaults.processData;

        $.each(processData.steps, function (i, item) {
            var nodeDiv = document.createElement('div');
            var badge = 'badge-inverse', icon = 'icon-star';
            if (item.stepStatus == 1 || item.stepStatus == 100)//第一步
            {
                badge = 'badge-info';
                icon = item.stepStatus == 1 ? "icon-play" : "icon-stop";
            }
            $(nodeDiv).attr("id", "window" + item.stepId)
                .data("data", item)
                .attr("style", item.style)
                .addClass("process-step btn btn-small")
                .html('<span class="process-flag badge ' + badge + '"><i class="' + icon + ' icon-white"></i></span>&nbsp; <span class="p_stepName">' + item.stepName + '</span>')
                .mousedown(function (e) {
                    if (e.which == 3) { //右键绑定
                        _canvas.find('#leipi_active_id').val(item.id);
                        contextmenu.bindings = defaults.processMenus
                        $(this).contextMenu('processMenu', contextmenu);
                    }
                });
            _canvas.append(nodeDiv);
            //索引变量
            lastProcessId = item.id;
        });

        var timeout = null;
        //点击或双击事件,这里进行了一个单击事件延迟，因为同时绑定了双击事件
        $(document).on('click', ".process-step", function () {
            //激活
            _canvas.find('#leipi_active_id').val($(this).data("data").stepId),
                clearTimeout(timeout);
            var obj = this;
            timeout = setTimeout(defaults.fnClick, 300);
        });
        $(document).on('dblclick', ".process-step", function () {
            clearTimeout(timeout);
            defaults.fnDbClick();
        });

        //使之可拖动
        jsPlumb.draggable(jsPlumb.getSelector(".process-step"));
        initEndPoints();

        //绑定添加连接操作。画线-input text值  拒绝重复连接
        jsPlumb.bind("jsPlumbConnection", function (info) {
            setConnections(info.connection);
        });
        //绑定删除connection事件
        jsPlumb.bind("jsPlumbConnectionDetached", function (info) {
            setConnections(info.connection, true);
        });
        //绑定删除确认操作
        jsPlumb.bind("dblclick", function (c) {
            lineCondition(c);

        });

        //连接操作
        jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
            dropOptions: { hoverClass: "hover", activeClass: "active" },
            anchor: "Continuous",
            maxConnections: -1,
            endpoint: ["Dot", { radius: 1 }],
            paintStyle: { fillStyle: "#ec912a", radius: 1 },
            hoverPaintStyle: this.connectorHoverStyle,
            beforeDrop: function (params) {
                if (params.sourceId == params.targetId) return false;/*不能链接自己*/
                var canlink = true;
                $('#leipi_process_info').find('input').each(function (i) {
                    var str = $('#' + params.sourceId).data("data").stepId + ',' + $('#' + params.targetId).data("data").stepId;
                    if (str == $(this).val()) { canlink = false; }
                });
                return canlink;
            }
        });

        var _canvas_design = function () {
            $.each(processData.paths, function (i, item) {
                jsPlumb.connect({
                    source: "window" + item.startStepId,
                    target: "window" + item.stopStepId
                });
            });
        }
        _canvas_design();

        //-----外部调用----------------------

        var Flowdesign = {
            addProcess: function (item) {
                //添加步骤
                var nodeDiv = document.createElement('div');
                $(nodeDiv).attr("id", "window" + item.stepId)
                    .data("data", item)
                    .attr("style", item.style)
                    .addClass("process-step btn btn-small")
                    .html('<span class="process-flag badge badge-inverse"><i class="icon-star icon-white"></i></span>&nbsp;<span class="p_stepName">' + item.stepName + '</span>')
                    .mousedown(function (e) {
                        if (e.which == 3) { //右键绑定
                            _canvas.find('#leipi_active_id').val(item.stepId);
                            contextmenu.bindings = defaults.processMenus
                            $(this).contextMenu('processMenu', contextmenu);
                        }
                    });
                _canvas.append(nodeDiv);
                //使之可拖动 和 连线
                jsPlumb.draggable(jsPlumb.getSelector(".process-step"));
                initEndPoints();
                //使可以连接线
                jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
                    dropOptions: { hoverClass: "hover", activeClass: "active" },
                    anchor: "Continuous",
                    maxConnections: -1,
                    endpoint: ["Dot", { radius: 1 }],
                    paintStyle: { fillStyle: "#ec912a", radius: 1 },
                    hoverPaintStyle: this.connectorHoverStyle,
                    beforeDrop: function (params) {
                        var j = 0;
                        $('#leipi_process_info').find('input').each(function (i) {
                            var str = $('#' + params.sourceId).data("data").stepId + ',' + $('#' + params.targetId).data("data").stepId;
                            if (str == $(this).val()) {
                                j++;
                                return;
                            }
                        })
                        return true;
                    }
                });
                return true;

            },
            delProcess: function (activeId) {
                if (activeId <= 0) return false;

                $("#window" + activeId).remove();
                return true;
            },
            getActiveId: function () {
                return _canvas.find("#leipi_active_id").val();
            },
            copy: function (active_id) {
                if (!active_id)
                    active_id = _canvas.find("#leipi_active_id").val();

                _canvas.find("#leipi_copy_id").val(active_id);
                return true;
            },
            paste: function () {
                return _canvas.find("#leipi_copy_id").val();
            },
            getProcessInfo: function () {
                var result = { steps: [], paths: [] };
                /*步骤列表*/
                _canvas.find("div.process-step").each(function (i) {
                    if ($(this).attr('id')) {
                        var step = $(this).data("data");
                        step.style = typeof ($(this).attr('style')) == "undefined" ? "" : $(this).attr('style');
                        for (var key in step) if (step[key] == null || step[key] == "null") step[key] = "";
                        result.steps.push(step);
                    }
                });
                /*连接列表*/
                $("#leipi_process_info input[type=hidden]").each(function (i) {
                    var processVal = $(this).val().split(",");
                    var path = $(this).data("data");
                    path.startStepId = processVal[0];
                    path.stopStepId = processVal[1];
                    for (var key in path) if (path[key] == null || path[key] == "null") path[key] = "";
                    result.paths.push(path);
                });

                return result;
            },
            clear: function () {
                jsPlumb.detachEveryConnection();
                jsPlumb.deleteEveryEndpoint();
                $('#leipi_process_info').html('');
                jsPlumb.repaintEverything();
            },
            refresh: function () {
                this.clear();
                _canvas_design();
            }
        };
        return Flowdesign;
    }
})(jQuery);