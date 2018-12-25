/***********************
 * BitAdmin2.0框架文件
 * 自定义控件
 ***********************/
$.fn.bitSelect = function (_changes) {
    var _wrapper = $(this);
    var _option = {
        text: "text",
        value: "value"
    };
    $.extend(_option, {
        name: _wrapper.attr("name"),
        url: _wrapper.attr("data-select"),
        text: _wrapper.attr("data-text"),
        value: _wrapper.attr("data-value")
    });

    if (_option.url != null && _option.url != "" && _option.url != undefined) {
        $.getJSON(_option.url, { type: _wrapper.attr("data-type") }, function (result) {
            _bind(_wrapper, result.data, true);
        });
    }
    var _bind = function (select, data, addblank) {
        $(select).empty();
        $(select).data("bind", data);
        if (addblank)
            $(select).append($("<option value=''>===请选择===</option>"));
        var actualval = _wrapper.attr("data-actualval");
        for (var i in data) {
            $(select).append($("<option value='" + data[i][_option.value] + "' " + (actualval + '' == data[i][_option.value] ? 'selected' : '') + ">" + data[i][_option.text] + "</option>"));
        }
    };
    _wrapper.change(function () {
        if ($.isFunction(_changes[_option.name])) {
            var data = $(_wrapper).data("bind")[_wrapper.get(0).selectedIndex - 1];
            _changes[_option.name](data,$(this));
        }
    });
    return _option;
}

$.fn.bitRadio = function (_changes) {
    var _wrapper = $(this);
    var _option = {
        text: "text",
        value: "value",
        inline: "true"
    };
    $.extend(_option, {
        name: _wrapper.attr("name"),
        url: _wrapper.attr("data-radio"),
        text: _wrapper.attr("data-text"),
        value: _wrapper.attr("data-value"),
        inline: _wrapper.attr("data-inline")
    });
    _option.parent = _wrapper.parent();

    if (_option.url != null && _option.url != "" && _option.url != undefined) {
        $.getJSON(_option.url, { type: _wrapper.attr("data-type") }, function (result) {
            var _radioControl = _wrapper.find('input[type="radio"]');
            var _label = $(_option.inline ? '<div class="custom-control custom-radio" style="display:inline-block;"></div>' : '<div class="custom-control custom-radio"></div>');
            var actualval = _wrapper.parents("form").attr("data-" + _option.name);

            _option.parent.empty();

            $.each(result.data, function (index, row) {
                var _radio = $('<input class="custom-control-input" type="radio" id="rdid' + _option.name + index + '" name="' + _option.name + '">').data("bind", row).val(row[_option.value]);
                var _labControl = _label.clone().append(_radio).append('<label class="custom-control-label" for="rdid' + _option.name + index + '">' + row[_option.text] + '</label>');
                //var _labControl = _label.clone();
                //var _radio = $('<input type="radio" name="' + _option.name + '">').data("bind", row).val(row[_option.value]);
                //_labControl.append(_radio);
                //_labControl.append(row[_option.text]);

                if (actualval == _radio.val()) {
                    _radio.prop("checked", "checked");
                }
                if (_option.inline == "false")
                    _option.parent.append($('<div class="radio"></div>').append(_labControl));
                else
                    _option.parent.append(_labControl);

                _radio.change(function (e) {
                    if ($.isFunction(_changes[_option.name]))
                        _changes[_option.name](row, $(this));
                });
            });
        });
    }    
    return _option;
}

$.fn.bitCheckbox = function (_changes) {
    var _wrapper = $(this);
    var _option = {
        text: "text",
        value: "value",
        inline: "true"
    };
    $.extend(_option, {
        name: _wrapper.attr("name"),
        url: _wrapper.attr("data-checkbox"),
        text: _wrapper.attr("data-text"),
        value: _wrapper.attr("data-value"),
        inline: _wrapper.attr("data-inline")
    });
    _option.parent = _wrapper.parent();

    if (_option.url != null && _option.url != "" && _option.url != undefined) {
        $.getJSON(_option.url, { type: _wrapper.attr("data-type") }, function (result) {
            var _radioControl = _wrapper.find('input[type="checkbox"]');
            var _label = $(_option.inline ? '<div class="custom-control custom-checkbox" style="display:inline-block;"></div>' : '<div class="custom-control custom-checkbox"></div>');
            var actualval = _wrapper.parents("form").attr("data-" + _option.name);

            _option.parent.empty();

            $.each(result.data, function (index, row) {
                var _checkbox = $('<input class="custom-control-input" type="checkbox" id="cbid' + _option.name + index+ '" name="' + _option.name + '">').data("bind", row).val(row[_option.value]);
                var _labControl = _label.clone().append(_checkbox).append('<label class="custom-control-label" for="cbid' + _option.name + index+ '">' + row[_option.text] + '</label>');

                if (actualval != '' && actualval != null && ('|' + actualval + '|').indexOf('|' + _checkbox.val()) > 0) {
                    _checkbox.prop("checked", "checked");
                }
                if (_option.inline == "false") 
                    _option.parent.append($('<div class="checkbox"></div>').append(_labControl));
                else 
                    _option.parent.append(_labControl);

                _checkbox.change(function (e) {
                    if ($.isFunction(_changes[_option.name]))
                        _changes[_option.name](row, $(this));
                });
            });
        });
    }

    return _option;
}

$.fn.bitAutoComplete = function (_changes) {
    var _wrapper = $(this);
    var _option = {
        text: "text"
    };
    $.extend(_option, {
        name: _wrapper.attr("name"),
        url: _wrapper.attr("data-autotext"),
        text: _wrapper.attr("data-text")
    });
    if (_option.url != null && _option.url != "" && _option.url != undefined) {
        $.getJSON(_option.url, { type: _wrapper.attr("data-type") }, function (result) {
            var availableTags = [];
            for (var i in result.data) {
                if (!$.isFunction(result.data[i]))
                    availableTags.push(result.data[i][_option.text]);
            };
            _wrapper.autocomplete({
                source: availableTags,
                change: function (event, data) {
                    $(_wrapper).data("bind", data);
                }
            });
        });
    }
    _wrapper.change(function () {
        if ($.isFunction(_changes[_option.name])) {
            var data = $(_wrapper)[0].value;
            _changes[_option.name](data, $(this));
        }
    });
    return _option;
}

$.fn.bitAutoComSelect = function (_changes) {
    var _wrapper = $(this);
    _wrapper.append('<div class="input-group-append"><span class="input-group-text"><i class="fas fa-angle-down"></i></span></div>');
    var _text = _wrapper.find("[type=text]");
    var _value = _wrapper.find("[type=hidden]");

    var _textName = _text.attr("name");
    var _valueName = _value.attr("name");
    var backVal;
    var _option = {
        text: "text",
        value: "value"
    };
    $.extend(_option, {
        url: _wrapper.attr("data-autoselect"),
        text: _wrapper.attr("data-text"),
        value: _wrapper.attr("data-value")
    });
    if (_option.url != null && _option.url != "" && _option.url != undefined) {
        $.getJSON(_option.url, { type: _wrapper.attr("data-type") }, function (result) {
            var availableTags = [];
            for (var i in result.data) {
                if (!$.isFunction(result.data[i]))
                    availableTags.push({ label: result.data[i][_option.text], value: result.data[i][_option.text], dataVal: result.data[i][_option.value] });
            };
            _text.autocomplete({
                delay: 0,
                minLength: 0,
                source: availableTags,
                select: function (event, ui) {
                    backVal = ui.item;
                },
                change: function (event, data) {
                    if (data.item == null) {
                        _value.val('');
                        backVal = "";
                        return;
                    }
                    _value.val(data.item.dataVal);
                }

            })
        });
    }
    _wrapper.change(function () {
        var exec = true;
        $.each(_wrapper.find("input"), function () {
            var name = $(this).attr("name");
            if ($.isFunction(_changes[name]) && exec) {
                _changes[name](backVal, $(this));
                exec = false;
            }
        });  
    });
   
    return _option;
}

$.fn.bitTree = function (option) {
    var _wrapper = $(this);
    var _option = {
        url: null,
        checkbox: false,
        text: "text",
        nodes: "children",
        expandIcon: 'fas fa-chevron-right',    //展开图标
        collapseIcon: 'fas fa-chevron-down',   //合并图标
        onNodeSelected: function (event, node) { },
        onNodeUnselected: function (event, node){},
        bind: null,
        treeview: null,
        loadCallback: function (result) { }
    };
    $.extend(_option, {
        url: _wrapper.attr("data-url"),
        checkbox: _wrapper.attr("data-checkbox"),
        text: _wrapper.attr("data-text"),
        nodes: _wrapper.attr("data-nodes")
    });
    $.extend(_option, option);

    var format = function (tree) {
        for (var i in tree) {
            tree[i]["text"] = tree[i][_option.text];
            tree[i]["nodes"] = tree[i][_option.nodes];
            format(tree[i].nodes);
        }
    }
    _option.load = function (callback) {
        if ($.isFunction(callback))
            _option.loadCallback = callback;

        if (callback == undefined) {
            $.ajax({
                url: _option.url,
                type: 'post',
                cache: false,
                async: true,
                data: { },
                success: function (result) {
                    format(result.data);
                    _option.treeview = _wrapper.treeview({
                        expandIcon: _option.expandIcon,
                        collapseIcon: _option.collapseIcon,
                        data: result.data,
                        showCheckbox: _option.checkbox,
                        onNodeSelected: _option.onNodeSelected,
                        onNodeUnselected: _option.onNodeUnselected
                    });
                    if ($.isFunction(_option.loadCallback))
                        _option.loadCallback(result);
                }
            });
        }
        return _option;
    };
    _option.select = function (callback) {
        if ($.isFunction(callback))
            _option.onNodeSelected = callback;
        return _option;
    };
    _option.unselect = function (callback) {
        if ($.isFunction(callback))
            _option.onNodeUnselected = callback;
        return _option;
    };
    return _option;
};

$.fn.picker = function (parent, _changes) {
    var _wrapper = $(this);
    var _text = _wrapper.find("[type=text]");
    var _value = _wrapper.find("[type=hidden]");

    var _textName = _text.attr("name");
    var _valueName = _value.attr("name");

    var _option = {
        url: _wrapper.attr("data-picker"),
        param: _wrapper.attr("data-param"),
        isShowRemove: _wrapper.attr("data-remove")
    };

    if (parent == undefined || parent == null || parent =="#undefined")
        parent = "";
    var _data = {
        text: parent + " [name=" + _textName + "]",
        value: parent + " [name=" + _valueName + "]"
    };
    var lock = false;
    var clickfn = function () {
        if (lock) return;

        lock = true;
        if (_option.param != undefined) {
            var val = {};
            var inParams = _option.param.split(",");
            for (var i in inParams) {
                if (typeof inParams[i] != 'string')
                    continue;
                var para = inParams[i].split(":");
                var key = para[0];
                var value = para[1];

                if (value != null && value.indexOf("&") >= 0) {
                    val[key] = $(wrapper + " [name=" + value.substr(1) + "]").val();
                } else if (value != null && value.indexOf("#") >= 0) {
                    val[key] = $("#" + value.substr(1)).val();
                } else {
                    val[key] = value;
                }
            }
            $.extend(_data, val);
        }
        //先清除,再绑定
        pickType = _option.url.replace(/\./g, "p").replace(/\//g, "p");
        $("[data-picker-type=" + pickType + "]").remove();
        var _picker = $('<div id="' + _textName + '" data-picker-type=' + pickType + '></div>');
        _picker.attr("data-param", JSON.stringify(_data));
        _picker.load(_option.url);
        $("body").append(_picker);  
        lock = false;
    };
    //清除绑定事件
    _text.unbind("click");
    _text.click(clickfn);

    var appendIco = $('<div class="input-group-append"></div>');
    $(this).append(appendIco);
    var icon = $('<span class="input-group-text"><i class="far fa-list-alt"></i></span>').click(clickfn);
    appendIco.append(icon);

    if (_option.isShowRemove != "false") {
        var remove = $('<span class="input-group-text" title="清空"><i class="fas fa-times"></i></span>').click(function () {
            _text.val("");
            _value.val("");
        });
        appendIco.append(remove);
    }
    _text.parent().children().css("cursor", "pointer");
    _text.attr("readonly", "readonly").css("background-color", "white").css("border-right", "0px");

    _text.change(function () {
        if ($.isFunction(_changes[_textName])) {
            _changes[_textName](_value.val(), _text.val());
        }
        else if ($.isFunction(_changes[_valueName])) {
            _changes[_valueName](_value.val(), _text.val());
        }
    });
    return _option;
};
$.fn.pickerModal = function () {
    var _wrapper = $(this);
    _wrapper.modal('hide');
    _wrapper.modal('show');
    var _option = {};
    var param = JSON.parse(_wrapper.parent().attr("data-param"));
    $.extend(_option, param);

    _wrapper.find("[action=save]").click(function () {
        var result = _savecallback();
        $(_option.value).val(result.value);
        $(_option.text).val(result.text);
        $(_option.text).change();
        if (param.callback != undefined && param.callback != "")
            eval(param.callback + "()");

        _wrapper.modal('hide');        
    });

    var _savecallback;
    _option.save = function (callback) {
        if ($.isFunction(callback))
            _savecallback = callback;
        return _option;
    }
    return _option;
}
$.fn.datePicker = function (_changes) {
    var _value = $(this).clone();

    var _option = {
        format: "YYYY-MM-DD hh:mm:ss",               //日期格式
        minDate: "1900-01-01 00:00:00",              //最小日期
        maxDate: "2099-12-31 23:59:59",              //最大日期
        festival: false,                             //是否显示农历节日
        zIndex: 2099,                                //弹出层的层级高度
        isShowCalendar: true,
        isShowRemove: true,
        choosefun: function (elem, datas) { }
    };
    $.extend(_option, {
        name: _value.attr("name"),
        format: _value.attr('data-format').replace("yyyy", "YYYY").replace("dd", "DD").replace("HH", "hh"),
        minDate: _value.attr('data-min'),
        maxDate: _value.attr('data-max'),
        parent: $(this).parent()
    });

    var _wrapper = $('<div class="input-group"></div>').append(_value);
    _option.parent.empty().append(_wrapper);

    var appendIco = $('<div class="input-group-append"></div>');
    _wrapper.append(appendIco);

    if (_option.isShowCalendar = true) {
        var _calendar = $('<span class="input-group-text"><i class="far fa-calendar-alt"></i></span>');
        _calendar.click(function () {
            _value.click();
        });
        appendIco.append(_calendar);
    }
    if (_option.isShowRemove == true) {
        var _remove = $('<span class="input-group-text" title="清空"><i class="fas fa-times"></i></span>');
        _remove.click(function () {
            _value.val("");
        });
        appendIco.append(_remove);
    }
    _wrapper.children().css("cursor", "pointer");
    _value.attr("readonly", "readonly").css("background-color", "white").css("border-right","0px");

    //设置时间控件
    _value.jeDate(_option); 
    _option.choosefun = function (a, b, c) {
        if ($.isFunction(_changes[_option.name])) {
            _changes[_option.name](b, a);
        }
    };

    return _option
}

$.fn.linkageSelect = function (_changes) {
    var _wrapper = $(this);

    var _select = _wrapper.find("select:first");
   
    var _option = {
        text: "text",
        value: "value"
    };
    _option.wrapper = _wrapper;
    $.extend(_option, {
        url: _wrapper.attr("data-linkage"),
        isAjax: _wrapper.attr("data-ajax"),
        text: _wrapper.attr("data-text"),
        value: _wrapper.attr("data-value")
    });

    if (_option.url != null && _option.url != "" && _option.url != undefined) {
        $.getJSON(_option.url, { type: _select.attr("data-type"), parent: "" }, function (result) {
            _option.bind(_select, result.data, true);
            _option.bindChange(_select);
        });
    }
    _option.clearNext = function (select) {
        var _next = $(select).next("select");
        if (_next.length == 0)
            return;
        _next.empty(); _next.val("");
        
        _option.clearNext(_next);
    }
    _option.bindChange = function (select) {
        var _next = $(select).next("select");
        if (_next.length == 0)
            return;
        $(select).change(function () {
            var val = $(select).val();
            _option.clearNext(select);
            if (_option.isAjax == "true" && val != "") {
                $.getJSON(_option.url, { type: _next.attr("data-type"), parent: val }, function (result) {
                    _option.bind(_next, result.data, true);
                });
            } else if (_option.isAjax == "true") {
                _option.bind(_next, {}, false);
            }
            else {
                if ($(select).get(0).selectedIndex > 0)
                   _option.bind(_next, $(select).data("bind")[$(select).get(0).selectedIndex - 1].children, true);
            }
        });
        _option.bindChange(_next);
    }
    _option.bind = function (select, data, addblank) {
        $(select).empty();
        $(select).data("bind", data);
        var actualval = $(select).attr("data-actualval");
        if (addblank)
            $(select).append($("<option value=''>===请选择===</option>"));
        for (var i in data) {
            var IsSelected = "";
            if (actualval == data[i][_option.value]) {
                IsSelected = "selected";
            }
            var option = $("<option value='" + data[i][_option.value] + "' " + IsSelected + " >" + data[i][_option.text] + "</option>");
            $(select).append(option);
        }
        if (actualval != null && actualval != "") {
            $(select).attr("data-actualval", '');
            $(select).change();
        }
    };

    _wrapper.find("select:last").change(function () {
        var exec = true;
        $.each(_wrapper.find("select"), function () {
            var name = $(this).attr("name");
            if ($.isFunction(_changes[name]) && exec) {
                var data = $(this).data("bind")[$(this).get(0).selectedIndex - 1];
                _changes[name](data, $(this));
                exec = false;
            }
        });        
    });

    return _option;
};

$.fn.treeTable = function (option) {
    var _wrapper = $(this);
    var _button = _wrapper.find(".querySuite-button");
    var _table = _wrapper.find(".querySuite-table");
    var _tree = _table.find("table");

    _button.addClass("container-fluid");
    _tree.addClass("table table-bordered table-hover");

    var treeColumn = 0;
    var _option = {
        key: "id",
        parentKey: "parentId",
        childrenKey: "children",
        treeGrid: null,
        query: null,
        delete: null
    };
    _option = $.extend(_option, option, {
        queryUrl: _table.attr("data-query-url"),
        deleteUrl: _table.attr("data-delete-url"),
        key: _table.attr("data-key"),
        parentKey: _table.attr("data-parent"),
        childrenKey: _table.attr("data-children")
    });

    _option.delete = function () {
        var ckList = _tree.find('.SelectRow:checked');
        var DataList = [];
        if (ckList.length > 0) {
            $.each(ckList, function (i, d) {
                var value = $(d).attr("value");
                DataList.push(value);
            });
        }
        if (ckList.length == 0) {
            alert("请选择要删除的项");
            return;
        }
        if (confirm("您确定要删除所选内容吗？")) {
            $.ajax({
                type: "get",
                url: _option.deleteUrl,
                datatype: "json",
                async: false,
                data: { IDs: DataList.toString() },
                success: function (result) {
                    if (result.code == 0) {
                        alert("删除成功！");
                        _option.query();
                    }
                    else {
                        alert(result.msg);
                    }
                }
            });
        }

    }

    _option.query = function () {
        $.ajax({
            type: "get",
            url: _option.queryUrl,
            datatype: "json",
            async: false,
            success: function (result) {
                if (result && result.code == 0) {
                    _tree.children("tbody").remove();
                    _tree.append("<tbody></tbody>");
                    loadTable(result.data);
                    _option.treeGrid = _tree.treegrid({
                        treeColumn: treeColumn,
                        initialState: _option.initialState
                    });
                }
                else {
                    alert(result.msg);
                }
            }
        });
    };

    //绑定全选事件
    _tree.children("thead").find("th:eq(0)").find('input[type="checkbox"]').bind('change', function () {
        var is_checked = $(this).prop("checked");
        if (is_checked == false) {
            _tree.find('.SelectRow').prop('checked', false);
        } else {
            _tree.find('.SelectRow').prop('checked', true);
        }
    });

    function loadTable(data) {
        $.each(data, function (i, d) {
            var parentClass = "";
            if (d[_option.parentKey] != "" && d[_option.parentKey] != null) {
                parentClass = "  treegrid-parent-" + d[_option.parentKey];
            }
            var tr = $("<tr class='treegrid-" + d[_option.key] + parentClass + "'></tr>");
            $.each(_tree.children("thead").children("tr").children("th"), function (index) {
                var style = $(this).attr("style");
                if (style != undefined && style != "" && style != "undefined")
                    style = "style='" + style + "'";
                else
                    style = "";
                if ($(this).find('input[type="checkbox"]').length > 0 && index == 0) {
                    treeColumn = 1;
                    var input = $("<input type=\"checkbox\" value=\"" + d[_option.key] + "\" class='SelectRow' />");
                    var td = $("<td  style=\"text-align:center\"></td>");
                    td.append(input);
                    tr.append(td);
                }
                else {
                    var formatter = $(this).attr("data-format");
                    var value = d[$(this).attr("data-field")];

                    if (formatter != "undefined" && formatter != undefined) {
                        if (formatter.indexOf("time|") == 0 || formatter == "time") {
                            var para = formatter.split('|');
                            if (para.length = 2)
                                value = time.format(value, para[1]);
                            else
                                value = time.format(value, "yyyy-MM-dd hh:mm:ss")
                        }
                        else {
                            value = eval("_option." + formatter + "(d)");
                        }
                    }
                    var td = $("<td " + style + "></td>");
                    td.append(value);
                    tr.append(td);
                }
            });
            _tree.find("tbody").append(tr);
            if (d[_option.childrenKey] != null && d[_option.childrenKey] != "")
                loadTable(d[_option.childrenKey]);
        });
    }

    _option.query();
    _button.find("[action=delete]").click(function () { _option.delete(); });
    return _option;
}

/**********************************  验证扩展(基于jquery.validate) Start  **********************************/
$.extend($.validator.prototype, {
    defaultMessage: function (element, rule) {
        return this.findDefined(
            this.customMessage(element.name, rule.method),
            this.customDataMessage(element, rule.method),
            undefined,
            $.validator.messages[rule.method],
            "<strong>警告: 没有为(" + element.name + ")定义消息</strong>"
        );
    },
    showLabel: function (element, message) {
        var place, group, errorID,
            error = this.errorsFor(element),
            elementID = this.idOrName(element),
            describedBy = $(element).attr("aria-describedby");
        if (error.length) {
            error.removeClass(this.settings.validClass).addClass(this.settings.errorClass);
            error.html("*");
        } else {
            error = $("<" + this.settings.errorElement + ">")
                .attr("id", elementID + "-error")
                .addClass(this.settings.errorClass)
                .html("*");
            place = error;
            if (this.settings.wrapper) {
                place = error.hide().show().wrap("<" + this.settings.wrapper + "/>").parent();
            }
            if (this.labelContainer.length) {
                this.labelContainer.append(place);
            } else if (this.settings.errorPlacement) {
                this.settings.errorPlacement(place, $(element));
            } else {
                place.insertAfter(element);
            }
            
            if (error.is("label")) {
                error.attr("for", elementID);
            } else if (error.parents("label[for='" + elementID + "']").length === 0) {

                errorID = error.attr("id").replace(/(:|\.|\[|\]|\$)/g, "\\$1");
                if (!describedBy) {
                    describedBy = errorID;
                } else if (!describedBy.match(new RegExp("\\b" + errorID + "\\b"))) {
                    describedBy += " " + errorID;
                }
                $(element).attr("aria-describedby", describedBy);
                
                group = this.groups[element.name];
                if (group) {
                    $.each(this.groups, function (name, testgroup) {
                        if (testgroup === group) {
                            $("[name='" + name + "']", this.currentForm)
                                .attr("aria-describedby", error.attr("id"));
                        }
                    });
                }
            }
        }
        if (!message && this.settings.success) {
            error.text("");
            if (typeof this.settings.success === "string") {
                error.addClass(this.settings.success);
            } else {
                this.settings.success(error, element);
            }
        }
        else {
            var required = message == "*";
            if (required) {
                message = "必填项";
                this.toShow = this.toShow.add(error);
            } else {
                error.hide();
            }
            $(this.currentForm).find("[name='" + elementID + "']").attr("title", message);
        }
        this.toShow = this.toShow.add(error);
    }
});
$.extend(jQuery.validator.messages, {
    required: "*",
    remote: "请指定一个不重复的值",
    email: "请输入正确格式的电子邮件",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入合法的日期 (ISO).",
    number: "请输入合法的数字",
    digits: "只能输入整数",
    creditcard: "请输入合法的信用卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    maxlength: jQuery.validator.format("允许的最大长度为 {0} 个字符"),
    minlength: jQuery.validator.format("允许的最小长度为 {0} 个字符"),
    rangelength: jQuery.validator.format("允许的长度为{0}和{1}之间"),
    range: jQuery.validator.format("请输入介于 {0} 和 {1} 之间的值"),
    max: jQuery.validator.format("请输入一个最大为 {0} 的值"),
    min: jQuery.validator.format("请输入一个最小为 {0} 的值"),
});

// 身份证号码验证
$.validator.addMethod("idcard", function (value, element) {
    var len = value.length, re;
    if (len == 15)
        re = new RegExp(/^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$/);
    else if (len == 18)
        re = new RegExp(/^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$/);
    return this.optional(element) || re.test(value);
}, "请输入正确的身份证号码。");

// 手机号码验证
$.validator.addMethod("phone", function (value, element) {
    var regMobile = /^1\d{10}$/;
    return this.optional(element) || regMobile.test(value);
}, "请输入正确的手机号码");

// 电话号码
$.validator.addMethod("telephone", function (value, element) {
    var regMobile = /^\d{3}-\d{7,8}|\d{4}-\{7,8}$/;
    return this.optional(element) || regMobile.test(value);
}, "请输入正确的电话号码");

//时间比较验证
$.validator.addMethod("starttime", function (value, element) {
    var StartData = $("#" + $(element).attr('data-starttime')).val();
    var EndData = value;
    var reg = new RegExp('-', 'g');
    StartData = StartData.replace(reg, '/');//正则替换
    EndData = EndData.replace(reg, '/');
    StartData = new Date(parseInt(Date.parse(StartData), 10));
    EndData = new Date(parseInt(Date.parse(EndData), 10));
    if (StartData > EndData) {
        return false;
    } else {
        return true;
    }
}, "结束日期必须大于开始日期");
/**********************************  验证扩展(基于jquery.validate) End    **********************************/
