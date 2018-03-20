
(function ($) {
    var defaults = {
        processData: {},//步骤节点数据
        //processUrl:'',//步骤节点数据
        fnRepeat: function () {
            alert("步骤连接重复");
        },
        fnClick: function () {
            alert("单击");
        },
        fnDbClick: function () {
            alert("双击");
        },
        canvasMenus: {
            "one": function (t) { alert('画面右键') }
        },
        processMenus: {
            "one": function (t) { alert('步骤右键') }
        },
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
            /*borderLeft:'5px solid #fff',*/
            padding: '5px 40px 5px 20px'
        },
        itemHoverStyle: {
            border: '0',
            /*borderLeft:'5px solid #49afcd',*/
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
        if (!remove) aConnections.push(conn);
        else {
            var idx = -1;
            for (var i = 0; i < aConnections.length; i++) {
                if (aConnections[i] == conn) {
                    idx = i; break;
                }
            }
            if (idx != -1) aConnections.splice(idx, 1);
        }
        if (aConnections.length > 0) {
            var s = "";
            for (var j = 0; j < aConnections.length; j++) {
                var from = $('#' + aConnections[j].sourceId).attr('process_id');
                var target = $('#' + aConnections[j].targetId).attr('process_id');
                var conditions = $('#' + aConnections[j].sourceId).attr('conditions');
                var condition = 1;
                var condition_d = "";
                var expression = "";
                if (typeof (conditions) != "undefined" && conditions != null) {
                    var cs = conditions.split(",");
                    for (var i = 0; i < cs.length; i++) {
                        var cOne = cs[i].split(":");
                        if (cOne != null && cOne.length > 1 && cOne[0] == target) {
                            condition = cOne[1];
                        }
                        if (cOne != null && cOne.length > 2 && cOne[0] == target) {
                            condition_d = cOne[2];
                        }
                        if (cOne != null && cOne.length > 3 && cOne[0] == target) {
                            expression = cOne[3];
                        }
                    }
                }
                s = s + "<input type='hidden' value=\"" + from + "," + target + "\" condition=" + condition + " condition_d='" + condition_d + "' expression='" + expression + "'>";

            }
            $('#leipi_process_info').html(s);
        } else {
            $('#leipi_process_info').html('');
        }
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
        //if (!$.support.leadingWhitespace) { //ie9以下，用VML画图
        //    jsPlumb.setRenderMode(jsPlumb.VML);
        //} else { //其他浏览器用SVG
        //    jsPlumb.setRenderMode(jsPlumb.SVG);
        //}


        //初始化原步骤
        var lastProcessId = 0;
        var processData = defaults.processData;
        if (processData.list) {
            $.each(processData.list, function (i, row) {
                var nodeDiv = document.createElement('div');
                var nodeId = "window" + row.id, badge = 'badge-inverse', icon = 'icon-star';
                if (row.StepStatus == 1)//第一步
                {
                    badge = 'badge-info';
                    icon = 'icon-play';
                }
                if (row.StepStatus == 100)//第一步
                {
                    badge = 'badge-info';
                    icon = 'icon-stop';
                }
                if (row.icon) {
                    icon = row.icon;
                }
                $(nodeDiv).attr("id", nodeId)
                .attr("style", row.style)
                .attr("process_to", row.process_to)
                .attr("conditions", row.conditions)
                .attr("process_id", row.id)
                .attr("stepName", row.stepName)
                .attr("auditNorm", row.auditNorm)
                .attr("auditId", row.auditId)
                .attr("auditNormRead", row.auditNormRead)
                .attr("auditIdRead", row.auditIdRead)
                .attr("stepStatus", row.stepStatus)
                .attr("description", row.description)
                .attr("relationStepKey", row.relationStepKey)
                .attr("auditors", row.auditors)
                .attr("mainId", row.mainId)
                .attr("openChoices", row.openChoices)
                .attr("power", row.power)
                .attr("runMode", row.runMode)
                .attr("function", row.function)
                .attr("joinMode", row.joinMode)
                .attr("circularize", row.circularize)
                .attr("examineMode", row.examineMode)
                .attr("percentage", row.percentage)
                .attr("linkCode", row.linkCode)
                .attr("showTabIndex", row.showTabIndex)
                .attr("smsTemplateToDo", row.smsTemplateToDo)
                .attr("smsTemplateRead", row.smsTemplateRead)
                .attr("reminderTimeout", row.reminderTimeout)
                .addClass("process-step btn btn-small")
                .html('<span class="process-flag badge ' + badge + '"><i class="' + icon + ' icon-white"></i></span>&nbsp; <span class="p_stepName">' + row.process_name + '</span>')
                .mousedown(function (e) {
                    if (e.which == 3) { //右键绑定
                        _canvas.find('#leipi_active_id').val(row.id);
                        contextmenu.bindings = defaults.processMenus
                        $(this).contextMenu('processMenu', contextmenu);
                    }
                });
                _canvas.append(nodeDiv);
                //索引变量
                lastProcessId = row.id;
            });//each
        }

        var timeout = null;
        //点击或双击事件,这里进行了一个单击事件延迟，因为同时绑定了双击事件
        $(document).on('click', ".process-step", function () {
            //激活
            _canvas.find('#leipi_active_id').val($(this).attr("process_id")),
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
            setConnections(info.connection)
        });
        //绑定删除connection事件
        jsPlumb.bind("jsPlumbConnectionDetached", function (info) {
            setConnections(info.connection, true);
        });
        //绑定删除确认操作
        jsPlumb.bind("dblclick", function (c) {
            //if (confirm("你确定取消连接吗?"))
            //    jsPlumb.detach(c);
            lineCondition(c);

        });
        //绑定删除确认操作
        //jsPlumb.bind("dblclick", function (c) {
        //    if (confirm("双击了"))
        //        jsPlumb.detach(c);
        //});

        // 双击取消连接
        //jsPlumb.bind("dblclick", function (conn, originalEvent) {
        //    //lineModal.modal({
        //    //    //remote: url
        //    //});
        //    alert(lineModal);
        //    //if (confirm("双击了"))
        //    //    jsPlumb.detach(c);
        //    lineModal.show();
        //});

        //连接成功回调函数
        function mtAfterDrop(params) {
            //console.log(params)
            defaults.mtAfterDrop({ sourceId: $("#" + params.sourceId).attr('process_id'), targetId: $("#" + params.targetId).attr('process_id') });

        }

        jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
            dropOptions: { hoverClass: "hover", activeClass: "active" },
            anchor: "Continuous",
            maxConnections: -1,
            endpoint: ["Dot", { radius: 1 }],
            paintStyle: { fillStyle: "#ec912a", radius: 1 },
            hoverPaintStyle: this.connectorHoverStyle,
            beforeDrop: function (params) {
                if (params.sourceId == params.targetId) return false;/*不能链接自己*/
                var j = 0;
                $('#leipi_process_info').find('input').each(function (i) {
                    var str = $('#' + params.sourceId).attr('process_id') + ',' + $('#' + params.targetId).attr('process_id');
                    if (str == $(this).val()) {
                        j++;
                        return;
                    }
                })
                //if (j > 0) {
                //    defaults.fnRepeat();
                //    return false;
                //} else {
                //    mtAfterDrop(params);
                //    return true;
                //}

                mtAfterDrop(params);
                return true;
            }
        });
        //reset  start
        var _canvas_design = function () {
            //连接关联的步骤
            $('.process-step').each(function (i) {
                var sourceId = $(this).attr('process_id');
                //var nodeId = "window"+id;
                var prcsto = $(this).attr('process_to');
                if (typeof (prcsto) != "undefined") {
                    var toArr = prcsto.split(",");
                    var processData = defaults.processData;
                    $.each(toArr, function (j, targetId) {

                        if (targetId != '' && targetId != 0) {
                            //检查 source 和 target是否存在
                            var is_source = false, is_target = false;
                            $.each(processData.list, function (i, row) {
                                if (row.id == sourceId) {
                                    is_source = true;
                                } else if (row.id == targetId) {
                                    is_target = true;
                                }
                                if (is_source && is_target)
                                    return true;
                            });

                            if (is_source && is_target) {
                                jsPlumb.connect({
                                    source: "window" + sourceId,
                                    target: "window" + targetId
                                    /* ,labelStyle : { cssClass:"component label" }
                                     ,label : id +" - "+ n*/
                                });
                                return;
                            }
                        }
                    })
                }
            });
        }//_canvas_design end reset 
        _canvas_design();

        //-----外部调用----------------------

        var Flowdesign = {

            addProcess: function (row) {

                if (row.id <= 0) {
                    return false;
                }
                var nodeDiv = document.createElement('div');
                var nodeId = "window" + row.id, badge = 'badge-inverse', icon = 'icon-star';

                if (row.icon) {
                    icon = row.icon;
                }
                $(nodeDiv).attr("id", nodeId)
                .attr("style", row.style)
                .attr("process_to", row.process_to)
                .attr("process_id", row.id)
                .attr("conditions", row.conditions)
                .attr("stepName", row.process_name)
                .attr("auditNorm", "Roles")
                .attr("auditId", "")
                .attr("auditNormRead", "Roles")
                .attr("auditIdRead", "")
                .attr("stepStatus", row.stepStatus)
                .attr("description", row.process_name)
                .attr("relationStepKey", row.relationStepKey)
                .attr("auditors", 1)
                .attr("mainId", row.mainId)
                .attr("openChoices", row.openChoices)
                .attr("power", row.power)
                .attr("runMode", row.runMode)
                .attr("function", row.function)
                .attr("joinMode", row.joinMode)
                .attr("circularize", row.circularize)
                .attr("examineMode", row.examineMode)
                .attr("percentage", row.percentage)
                .attr("linkCode", row.linkCode)
                .attr("showTabIndex", row.showTabIndex)
                .attr("smsTemplateToDo", row.smsTemplateToDo)
                .attr("smsTemplateRead", row.smsTemplateRead)
                .attr("reminderTimeout", row.reminderTimeout)
                .addClass("process-step btn btn-small")
                .html('<span class="process-flag badge ' + badge + '"><i class="' + icon + ' icon-white"></i></span>&nbsp;<span class="p_stepName">' + row.process_name + '</span>')
                .mousedown(function (e) {
                    if (e.which == 3) { //右键绑定
                        _canvas.find('#leipi_active_id').val(row.id);
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
                            var str = $('#' + params.sourceId).attr('process_id') + ',' + $('#' + params.targetId).attr('process_id');
                            if (str == $(this).val()) {
                                j++;
                                return;
                            }
                        })
                        //if (j > 0) {
                        //    defaults.fnRepeat();
                        //    return false;
                        //} else {
                        //    return true;
                        //}

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
                try {
                    /*连接关系*/
                    var aProcessData = {};
                    var retList = [];
                    $("#leipi_process_info input[type=hidden]").each(function (i) {
                        var processVal = $(this).val().split(",");
                        if (processVal.length == 2) {
                            if (!aProcessData[processVal[0]]) {
                                aProcessData[processVal[0]] = { "id": "", "style": "", "process_id": "", "process_to": "", "conditions": "", "stepName": "", "auditNorm": "", "auditId": "", "auditNormRead": "", "auditIdRead": "", "stepStatus": 10, "description": "", "relationStepKey": "", "auditors": 1, "mainId": "", "linkCode": "", "showTabIndex": "", "reminderTimeout": "", "smsTemplateToDo": "", "smsTemplateRead": "" };
                            }
                            //aProcessData[processVal[0]]["process_to"].push(processVal[1]);
                            if (aProcessData[processVal[0]]["process_to"] != "") aProcessData[processVal[0]]["process_to"] += ","
                            aProcessData[processVal[0]]["process_to"] += processVal[1];
                            if (aProcessData[processVal[0]]["conditions"] != "") aProcessData[processVal[0]]["conditions"] += ","
                            aProcessData[processVal[0]]["conditions"] += processVal[1] + ":" + $(this).attr("condition") + ":" + $(this).attr("condition_d") + ":" + $(this).attr("expression");
                        }
                    })
                    /*位置*/
                    _canvas.find("div.process-step").each(function (i) { //生成Json字符串，发送到服务器解析
                        if ($(this).attr('id')) {
                            var pId = $(this).attr('process_id');
                            //var pLeft = parseInt($(this).css('left'));
                            //var pTop = parseInt($(this).css('top'));
                            if (!aProcessData[pId]) {
                                aProcessData[pId] = { "id": "", "style": "", "process_name": "", "process_id": "", "process_to": "", "conditions": "", "stepName": "", "auditNorm": "", "auditId": "", "auditNormRead": "", "auditIdRead": "", "stepStatus": 10, "description": "", "relationStepKey": "", "auditors": 1, "mainId": "", "linkCode": "", "showTabIndex": "", "reminderTimeout": "", "smsTemplateToDo": "", "smsTemplateRead": "" };

                            }
                            //aProcessData[pId]["top"] = pTop;
                            //aProcessData[pId]["left"] = pLeft;
                            aProcessData[pId]["id"] = typeof ($(this).attr('process_id')) == "undefined" ? "" : $(this).attr('process_id');
                            aProcessData[pId]["style"] = typeof ($(this).attr('style')) == "undefined" ? "" : $(this).attr('style');
                            aProcessData[pId]["process_id"] = typeof ($(this).attr('process_id')) == "undefined" ? "" : $(this).attr('process_id');
                            aProcessData[pId]["stepName"] = typeof ($(this).attr('stepName')) == "undefined" ? "" : $(this).attr('stepName');
                            aProcessData[pId]["process_name"] = typeof ($(this).attr('stepName')) == "undefined" ? "" : $(this).attr('stepName');
                            aProcessData[pId]["auditNorm"] = typeof ($(this).attr('auditNorm')) == "undefined" ? "" : $(this).attr('auditNorm');
                            aProcessData[pId]["auditId"] = typeof ($(this).attr('auditId')) == "undefined" ? "" : $(this).attr('auditId');
                            aProcessData[pId]["auditNormRead"] = typeof ($(this).attr('auditNormRead')) == "undefined" ? "" : $(this).attr('auditNormRead');
                            aProcessData[pId]["auditIdRead"] = typeof ($(this).attr('auditIdRead')) == "undefined" ? "" : $(this).attr('auditIdRead');
                            aProcessData[pId]["stepStatus"] = typeof ($(this).attr('stepStatus')) == "undefined" ? 10 : $(this).attr('stepStatus');
                            aProcessData[pId]["description"] = typeof ($(this).attr('description')) == "undefined" ? "" : $(this).attr('description');
                            aProcessData[pId]["relationStepKey"] = typeof ($(this).attr('relationStepKey')) == "undefined" ? "" : $(this).attr('relationStepKey');
                            aProcessData[pId]["auditors"] = typeof ($(this).attr('auditors')) == "undefined" ? 1 : $(this).attr('auditors');
                            aProcessData[pId]["mainId"] = typeof ($(this).attr('mainId')) == "undefined" ? "" : $(this).attr('mainId');
                            aProcessData[pId]["openChoices"] = typeof ($(this).attr('OpenChoices')) == "undefined" ? "" : $(this).attr('openChoices');
                            aProcessData[pId]["power"] = typeof ($(this).attr('power')) == "undefined" ? "" : $(this).attr('power');
                            aProcessData[pId]["runMode"] = typeof ($(this).attr('runMode')) == "undefined" ? "" : $(this).attr('runMode');
                            aProcessData[pId]["function"] = typeof ($(this).attr('function')) == "undefined" ? "" : $(this).attr('function');
                            aProcessData[pId]["joinMode"] = typeof ($(this).attr('joinMode')) == "undefined" ? "" : $(this).attr('joinMode');
                            aProcessData[pId]["circularize"] = typeof ($(this).attr('circularize')) == "undefined" ? "" : $(this).attr('circularize');
                            aProcessData[pId]["examineMode"] = typeof ($(this).attr('power')) == "undefined" ? "" : $(this).attr('examineMode');
                            aProcessData[pId]["percentage"] = typeof ($(this).attr('power')) == "undefined" ? "" : $(this).attr('percentage');
                            aProcessData[pId]["linkCode"] = typeof ($(this).attr('linkCode')) == "undefined" ? "" : $(this).attr('linkCode');
                            aProcessData[pId]["showTabIndex"] = typeof ($(this).attr('showTabIndex')) == "undefined" ? "" : $(this).attr('showTabIndex');
                            aProcessData[pId]["reminderTimeout"] = typeof ($(this).attr('reminderTimeout')) == "undefined" ? "" : $(this).attr('reminderTimeout');
                            aProcessData[pId]["smsTemplateToDo"] = typeof ($(this).attr('smsTemplateToDo')) == "undefined" ? "" : $(this).attr('smsTemplateToDo');
                            aProcessData[pId]["smsTemplateRead"] = typeof ($(this).attr('smsTemplateRead')) == "undefined" ? "" : $(this).attr('smsTemplateRead');
                            retList.push(aProcessData[pId]);
                        }
                    })
                    //return JSON.stringify(aProcessData);
                    return retList;
                } catch (e) {
                    return '';
                }

            },
            clear: function () {
                try {

                    jsPlumb.detachEveryConnection();
                    jsPlumb.deleteEveryEndpoint();
                    $('#leipi_process_info').html('');
                    jsPlumb.repaintEverything();
                    return true;
                } catch (e) {
                    return false;
                }
            },
            refresh: function () {
                try {
                    //jsPlumb.reset();
                    this.clear();
                    _canvas_design();
                    return true;
                } catch (e) {
                    return false;
                }
            }
        };
        return Flowdesign;


    }
})(jQuery);