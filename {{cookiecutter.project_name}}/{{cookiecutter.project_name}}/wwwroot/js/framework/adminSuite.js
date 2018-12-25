/***********************
 * BitAdmin2.0框架文件
 * 增删改查套件
 ***********************/
$.extend({
    adminSetup: function (option) {
        $.extend($.adminSetting, option)
    },
    adminSetting: {
        addLogs: function (title, type, msg) { }
    },
    adminTools: {
        renderControls: function (_topid, _wrapper, _option) {
            var _changes = {};
            $.each(_wrapper.find("input,select,script"), function (i, d) {
                var key = $(d).attr("name"); if (key == undefined) return;
                if ($(d).attr("data-select") || $(d).attr("data-radio") || $(d).attr("data-checkbox") || $(d).attr("data-autotext") || $(d).attr("data-picker") || $(d).attr("data-file") || $(d).attr("data-ueditor")) {
                    var ctl;
                    if ($(d).attr("data-select"))
                        ctl = $(this).bitSelect(_changes);
                    else if ($(d).attr("data-radio"))
                        ctl = $(this).bitRadio(_changes);
                    else if ($(d).attr("data-checkbox"))
                        ctl = $(this).bitCheckbox(_changes);
                    else if ($(d).attr("data-autotext"))
                        ctl = $(this).bitAutoComplete(_changes);
                    else if ($(d).attr("data-picker") == "datetime")
                        ctl = $(this).datePicker(_changes);
                    else if ($(d).attr("data-file")) {
                        if ($.fn.upload) {
                            ctl = $(this).upload(_changes);
                        }
                    }
                    else if ($(d).attr("data-ueditor")) {
                        if (UE) {
                            $(this).attr("id", key);
                            ctl = UE.getEditor(key);
                        }
                    }

                    _option.controls[key] = inputChange(key, $(d), true, ctl);
                }
                else if ($(this).parent().attr("data-picker") || ($(this).parent().attr("data-linkage")) || ($(this).parent().attr("data-autoselect"))) {
                    var ctl;
                    $.each($(this).parent().children(), function () {
                        if (_option.controls[$(this).attr("name")] && _option.controls[$(this).attr("name")].bitcontrol && ctl == undefined)
                            ctl = _option.controls[$(this).attr("name")].bitcontrol;
                    });
                    if ($(this).parent().attr("data-picker") && ctl == undefined) {
                        ctl = $(this).parent().picker("#" + _topid, _changes);
                    }
                    else if ($(this).parent().attr("data-linkage") && ctl == undefined)
                        ctl = $(this).parent().linkageSelect(_changes);
                    else if ($(this).parent().attr("data-autoselect") && ctl == undefined)
                        ctl = $(this).parent().bitAutoComSelect(_changes);

                    _option.controls[key] = inputChange(key, $(d), true, ctl);
                }
                else {
                    _option.controls[key] = inputChange(key, $(d), false);
                }
            });
            function inputChange(key, input, isbit, ctl) {
                if (_option.controls[key]) {
                    _option.controls[key].controls.push(input);
                    return _option.controls[key]
                }
                var _input = {
                    key: key,
                    isbit: isbit,
                    bitcontrol: ctl,
                    controls: new Array(),
                };
                _input.controls.push(input);
                _input.change = function (action) {
                    if ($.isFunction(action)) {
                        _changes[key] = action;
                        if (!isbit) {
                            $.each(_input.controls, function () {
                                $(this).change(function () {
                                    action($(this).val(), $(this));
                                });
                            })
                        };
                    }
                    return _option;
                }
                return _input;
            }
        },
        initForm(_option, _callback, _wrapper, _form) {
            $.extend(_option, {
                controls: {},
                logType: "add",
                title: _form.attr("data-title"),
                key: _form.attr("data-key"),
                loadFormUrl: _form.attr("data-load-url"),
                saveFormUrl: _form.attr("data-save-url"),
            });
            if (_option.key != undefined && $("[name=" + _option.key + "]").length == 0) _form.find("td:eq(0)").append('<input type="hidden" name="' + _option.key + '" />');
            _wrapper.find("[action=save]").click(function () { _option.submit(); });

            _option.add = function (callback) {
                if ($.isFunction(callback)) { _callback.add = callback; }
                else {
                    _option.reset();
                    if ($.isFunction(_callback.add)) { _callback.add(); }
                }
                return _option;
            };
            _option.valid = function () { return _form.valid(); };

            _option.submit = function (callback) {
                if ($.isFunction(callback)) { _callback.submit = callback; }
                else {
                    _form.validate(_option.validator);
                    _form.submit();
                }
                return _option;
            };
        },
        clearForm(_form, _option) {
            _option.logType = "add";
            _form.resetForm();
            _form.find("input[type=hidden]").val("");
            _form.find("label[class^=error]").remove();
            _form.find(".error").removeClass("error");
            $.each(_form.find("input,select"), function (i) {
                if (($(this).attr("class") != undefined && $(this).attr("class").indexOf("required") > -1) || $(this).attr("required")) {
                    var prev = $(this).parent().prev();
                    if (!prev.is("th"))
                        prev = $(this).parent().parent().prev();
                    var text = prev.text();
                    prev.html('<label id="' + $(this).attr("name") + '-error" class="error error-lab" for="' + $(this).attr("name") + '">*</label>' + text);
                }
            });

            $.each(_form.find("[data-linkage]"), function () {
                $(this).find('select').attr('data-actualval', '');
                $(this).find('select:first').change();
            });
            $.each(_form.find("[data-file]"), function () {
                _option.controls[$(this).attr('name')].bitcontrol.clear();
            });
            $.each(_form.find("[data-ueditor]"), function () {
                if (UE) {
                    var ue = _option.controls[$(this).attr('name')].bitcontrol;
                    ue.ready(function () {
                        ue.setContent("");
                    });
                }
            });
        },
        renderForm(_form, data, _option, pkval) {
            for (var key in data) {
                var input = _form.find(" [name='" + key + "']");
                if (input.length > 0) {
                    var name = input.attr("name");
                    var type = input.attr("type");
                    switch (type) {
                        case "radio":
                            _form.find("input[name='" + key + "'][value='" + data[key] + "']").prop("checked", "checked");
                            input.parents("form").attr("data-" + name, data[key]);
                            break;
                        case "checkbox":
                            if (data[key] != null && data[key].toString() != "") {
                                var arr = data[key].toString().split('|');
                                _form.find("input[name='" + key + "']").removeAttr("checked");
                                for (var i in arr) {
                                    _form.find("input[name='" + key + "'][value='" + arr[i] + "']").prop("checked", "checked");
                                }
                            }
                            input.parents("form").attr("data-" + name, data[key]);
                            break;
                        default:
                            {
                                var value = data[key];
                                if (input[0].tagName == "SELECT") {
                                    input.attr("data-actualval", value);
                                    input.val(value);
                                    continue;
                                }
                                var _control = input;
                                var _controltype = _control.attr("data-picker");
                                if (_controltype == "datetime") {
                                    var formatter = _control.attr("data-format");
                                    value = time.format(value, formatter);
                                }
                                input.val(value);
                            }
                            break;
                    }
                }
            }

            $.each(_form.find("[data-linkage]"), function () {
                $(this).find('select:first').change();
            });
            $.each(_form.find("[data-file]"), function () {
                _option.controls[$(this).attr('name')].bitcontrol.query(pkval);
            });
            $.each(_form.find("[data-ueditor]"), function () {
                if (UE) {
                    var ue = _option.controls[$(this).attr('name')].bitcontrol;
                    ue.ready(function () {
                        ue.setContent(data[$(this).attr('name')]);
                    });
                }
            });
        },
    }
});

$.fn.zk_table = function (_option, querySuite) {
    var _wrapper = $(this);

    _option.table = $('<table class="table table-bordered table-hover"></table>');
    _option.checkbox = _wrapper.find("input[type=checkbox]").length;

    var querySuiteSearch = function (option) {
        var content = $('<div class="querySuiteSearch"></div>')
            .css('left', event.pageX - 30)
            .css('top', event.pageY + 20);

        var btnContent = $('<div class="btnContnet"></div>').append(
            $('<div class="btnClose">&times;</div>')
                .bind('click', function () {
                    content.remove();
                    if ($.isFunction(option.cancleCallback)) {
                        option.cancleCallback();
                    }
                }));

        var inContent = $('<div class="inputContent"></div>');
        var input = $('<input type="text" placeholder="查询关键字" />');
        var btnSearch = $('<span class="btn btn-primary"">查询</span>')
            .bind('click', function () {
                content.remove();
                if ($.isFunction(option.queryCallback)) {
                    option.queryCallback(input.val());
                }
            });

        $('body').append(content.append(btnContent).append(inContent.append(input).append(btnSearch)));
        input.bind('keypress', function (event) { if (event.keyCode == "13") { btnSearch.click(); } }).focus();
    };

    _option.sortable = function (key, rule) {
        querySuite.queryOrderType = rule;
        querySuite.queryOrderBy = key;
        querySuite.refresh();
    };

    _option.renderTable = function () {
        _wrapper.empty().append($('<div></div>').append(_option.table));
    }

    _option.renderHead = function () {
        var thtml = _wrapper.find('thead');
        _option.table.append(thtml);

        if (_option.checkbox) {
            thtml.find('input[type=checkbox]').change(function () {
                var is_checked = $(this).prop("checked");
                thtml.parent().find("tbody").find(".SelectRow").prop("checked", is_checked);
            });
        }
        $.each(_option.table.find("thead").find("[data-sort=true]"), function (index, element) {
            var field = $(this).attr('data-field');
            $(this).append($('<i class="sortbutton fas fa-sort" style="top:3px;cursor: pointer;"></i>').data('sort', 'sort')
                .bind('click', function () {
                    if ($(this).data('sort') == 'sort') {
                        $.each(_option.table.find('thead').find('.sortbutton'), function (index, element) {
                            $(this).removeClass('fa-sort-alpha-up').removeClass('fa-sort-alpha-down').addClass('fa-sort').data('sort', 'sort');
                        });
                        _option.table.find('thead').find('th[data-field=' + field + ']').find('.sortbutton').removeClass('fa-sort').addClass('fa-sort-alpha-up').data('sort', 'desc');
                        _option.sortable(field, 'desc');
                    }
                    else if ($(this).data('sort') == 'desc') {
                        $(this).addClass('fa-sort-alpha-down').removeClass('fa-sort-alpha-up').data('sort', 'asc');
                        _option.sortable(field, 'asc');
                    }
                    else if ($(this).data('sort') == 'asc') {
                        $(this).addClass('fa-sort-alpha-up').removeClass('fa-sort-alpha-down').data('sort', 'desc');
                        _option.sortable(field, 'desc');
                    }
                }));
        });

        $.each(_option.table.find("thead").find("[data-filter=true]"), function (index, element) {
            var field = $(this).attr('data-field');
            $(this).append($(' <small><i class="fas fa-filter" style="top:3px;cursor: pointer;"></i></small>')
                .bind('click', function () {
                    querySuiteSearch({
                        queryCallback: function (keyword) {
                            querySuite.column = field;
                            querySuite.keyword = keyword;
                            querySuite.query();
                        }
                    });
                }));
        });
    }

    ///组织列表体
    _option.renderBody = function (rows) {
        _option.table.find('tbody').remove();
        _option.table.append($('<tbody></tbody>'));
        if (rows != null && rows.length > 0) {
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                var tr = $('<tr></tr>').data('data', row);
                ///绑定全选按钮
                if (_option.checkbox) {
                    var style = _option.table.find("thead").find('th:eq(0)').attr('style');
                    var inpt = $('<td><input class="SelectRow" type="checkbox"></td>').attr("style", style);
                    inpt.find('.SelectRow').data('data', row)
                        .change(function () {
                            if ($(this).prop("checked")) {
                                if (_option.table.find('.SelectRow:checked').length == rows.length)
                                    _option.table.find('thead').find('input[type="checkbox"]').prop('checked', true);
                            } else {
                                _option.table.find('thead').find('input[type="checkbox"]').prop('checked', false);
                            }
                        });
                    tr.append(inpt);
                }
                ///使用数据填充列表部分
                for (var m = 0; m < _option.keys.length; m++) {
                    var td = $('<td></td>');
                    var key = _option.keys[m];
                    var isFormatter = _option.formatters[key];
                    if (isFormatter == undefined) {
                        td.html(row[key]);
                    } else {
                        td.bind("format", isFormatter);
                        td.trigger("format", [row[key], row]);
                    }
                    var style = _option.table.find("thead").find('th:eq(' + (m + _option.checkbox) + ')').attr('style');
                    if (style) { td.attr("style", style); }
                    tr.append(td);
                }
                _option.table.find('tbody').append(tr);
            }
        }
    };
    
    ///表格更新
    _option.updata = function (data) {
        $("body").find('.querySuiteSearch').remove();
        _option.table.find("input[type=checkbox]").prop('checked', false);
        _option.renderBody(data);
    };

    ///获取所有的选择
    _option.selected = function () {
        var ckList = _option.table.find('.SelectRow:checked');
        var DataList = [];
        if (ckList.length > 0) {
            for (var i = 0; i < ckList.length; i++) {
                var node = ckList[i];
                DataList.push($(node).data('data'));
            }
        }
        return DataList;
    };

    _option.renderHead();
    _option.renderBody();
    _option.renderTable();

    $(window).resize(function () {
        _option.renderTable();
    });
    
    return _option;
}

$.fn.zk_paging = function (option) {
    var self = this;
    var _default = {
        pageSize: 10,
        pageIndex: 1,
        pageShow: 10,
        totalContent: 1001,
        onPageIndexChange: function (index) {
            alert(index);
        },
        onPageSizeChange: function (index, size) {
        }
    }

    var _option = $.extend(_default, option);
    var pagezNum = Math.ceil(_option.totalContent / _option.pageSize);

    $(self).html('');

    var table = $('<table style="width:100%;"><tr></tr></table>');
    var startNum = (_option.pageIndex - 1) * _option.pageSize + 1;
    var endNum = startNum + _option.pageSize - 1;

    if (_option.pageIndex == pagezNum) {
        if (_option.totalContent <= _option.pageShow) {
            endNum = _option.totalContent;
        } else {
            endNum = _option.totalContent;
        }
    }
    if (pagezNum < 1)
        pagezNum = 1;
    var _init = $('<td style="vertical-align: middle;text-align: right;"><span>当前 ' + _option.pageIndex + ' / ' + pagezNum + ' 页，每页<input type="text" class="pageSize" title="离开此文本自动设置" style="width:40px;height: 20px;text-align: center; margin:0px 5px;border: #d2d6de 1px solid;border-radius: 4px;" value="' + _option.pageSize + '" />条\
                        ，总共 ' + _option.totalContent + ' 条记录</span></td>');
    _init.find(".pageSize").bind('keydown', function (event) {
        var e = event || window.event || arguments.callee.caller.arguments[0];
        // enter 键
        if (e && e.keyCode == 13) { 
            $(this).blur();
        }
    });
    _init.find(".pageSize").bind('blur', function () {
        if (_option.pageSize == _init.find(".pageSize").val()) {
            return;
        }
        if (isNaN(parseFloat(_init.find(".pageSize").val())) || parseFloat(_init.find(".pageSize").val()) <= 0) {
            _init.find(".pageSize").val(10);
        }
        _option.pageIndex = 1;
        _option.pageSize = parseInt(_init.find(".pageSize").val());
        if ($.isFunction(_option.onPageSizeChange)) {
            _option.onPageSizeChange(_option.pageIndex, _option.pageSize);
        }
    });


    var _nav = $('<td><nav class="pageNum"></nav></td>');
    var ul = $('<ul class="pagination" style="margin: 5px 0px;">');
    _nav.find('nav').append(ul);


    var initialPage = function () {
        ul.html('');
        ///添加首页
        var li_first = $('<li class="page-item"><a class="page-link" href="javascript:void(0);">&laquo;</a></li>');
        ul.append(li_first);
        li_first.bind('click', function () {
            _option.pageIndex = 1;
            if ($.isFunction(_option.onPageIndexChange)) {
                _option.onPageIndexChange(_option.pageIndex);
            }
            initialPage();
        })

        var StartNum = 1;
        var EndNum = pagezNum;
        if (_option.pageIndex >= _option.pageShow) {
            StartNum = _option.pageIndex - Math.ceil(_option.pageShow / 2);
        }

        if (StartNum + _option.pageShow >= pagezNum) {
            EndNum = pagezNum;
        } else {
            EndNum = StartNum + _option.pageShow - 1;
        }

        for (var i = StartNum; i <= EndNum; i++) {
            var li = $('<li class="page-item"><a class="page-link" href="javascript:void(0);">' + i + '</a></li>');
            li.data('index', (i));
            if (i == _option.pageIndex) {
                li.addClass('active');
            }
            li.bind('click', function () {
                _option.pageIndex = $(this).data('index');
                if ($.isFunction(_option.onPageIndexChange)) {
                    _option.onPageIndexChange(_option.pageIndex);
                }
                initialPage();
            })
            ul.append(li);
        }
        ///添加最后一页
        var li_last = $('<li class="page-item"><a class="page-link" href="javascript:void(0);">&raquo;</a></li>');
        ul.append(li_last);

        li_last.bind('click', function () {
            _option.pageIndex = pagezNum;
            if ($.isFunction(_option.onPageIndexChange)) {
                _option.onPageIndexChange(_option.pageIndex);
            }
            initialPage();
        });

        table.append(_nav);
        table.append(_init);
    }
    initialPage();
    $(self).append(table);
}

$.fn.querySuite = function (option) {
    var _wrapper = $(this);
    var _filter = _wrapper.find(".querySuite-filter");
    var _button = _wrapper.find(".querySuite-button");
    var _table = _wrapper.find(".querySuite-table");
    var _paging = _wrapper.find(".querySuite-paging");

    _filter.addClass("container-fluid");
    _button.addClass("container-fluid").addClass("text-right");

    var _option = {
        jqFilter: _filter,
        jqButton: _button,
        jqTable: _table,
        jqPaging: _paging,
        keys: [],
        columns: {},
        pageIndex: 1,
        pageSize: 10,
    };
    $.extend(_option, option, {
        key: _table.attr("data-key"),
        queryUrl: _table.attr("data-query-url"),
        deleteUrl: _table.attr("data-delete-url"),
        importUrl: _table.attr("data-import-url"),
        exportUrl: _table.attr("data-export-url"),
        sortUrl: _table.attr("data-sort-url"),
        pageSize: _paging.attr("data-page-size"),
    });
    var _callback = {};

    //查询条件行数，大于1行显示收缩
    if (_filter.find("tr").length > 1) {
        _filter.find("tr").addClass("tr_shrink").hide();
        _filter.find("tr:eq(0)").removeClass("tr_shrink").show();
        _filter.find("tr").append("<td></td>");
        var expand = $('<button type="button" class="btn btn-link"><i class="fas fa-search-plus"></i> 高级查询</button>')
            .bind("click", function () {
                _filter.find(".tr_shrink").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                $(this).css("display", "none");
                shrink.css("display", "block");
            });
        var shrink = $('<button type="button" class="btn btn-link"><i class="fas fa-search-minus"></i> 高级查询</button>')
            .bind("click", function () {
                _filter.find(".tr_shrink").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                $(this).css("display", "none");
                expand.css("display", "block");
            }).hide();

        _filter.find("tr:eq(0)").find("td:last").append($("<span></span>").append(expand).append(shrink));
    }

    _option.GetData = function () {
        var re = null;

        var data = {
            offset: (_option.pageIndex - 1) * _option.pageSize,
            limit: _option.pageSize,
            ordertype: _option.queryOrderType,
            orderby: _option.queryOrderBy,
            condition: _option.keyword,
            column: _option.column,
            r: new Date().getTime()
        };

        var filter = _filter.formSerialize();
        for (var key in data) {
            if (data[key] != undefined) filter += (filter != "" ? "&" : "") + key + "=" + data[key];
        }
        $.ajax({
            url: _option.queryUrl,
            async: false,
            data: filter,
            type: "post",
            success: function (result) {
                re = result;
                _option.jqPaging.zk_paging({
                    pageSize: _option.pageSize,
                    pageIndex: _option.pageIndex,
                    totalContent: result.total,
                    onPageIndexChange: function (index) {
                        _option.pageIndex = index;
                        _option.refresh();
                    },
                    onPageSizeChange: function (index, size) {
                        _option.pageIndex = index;
                        _option.pageSize = size;
                        _option.refresh();
                    }
                });
            }
        });
        return re;
    };

    var _zkTable;
    //查询
    _option.query = function (callback) {
        if ($.isFunction(callback)) {
            _callback.query = callback;
            return _option;
        }

        if (_zkTable == undefined) {
            _zkTable = _table.zk_table({
                keys: _option.keys,
                formatters: formatters
            }, _option);
        }
        _option.pageIndex = 1;
        return _option.refresh();
    }

    //刷新
    _option.refresh = function () {
        _zkTable.updata(_option.GetData().data);
        if ($.isFunction(_callback.query)) _callback.query();
        if (_option.sortUrl != undefined && _option.sortUrl != "") _option.sortable();
        return _option;
    };

    //拖动排序
    var _sortCallback;
    _option.sortable = function (callback) {
        if ($.isFunction(callback)) {
            _sortCallback = callback;
            return _option;
        }
        _option.jqTable.find("tbody").sortable({
            helper: function (e, tr) {
                var $originals = tr.children();
                var $helper = tr.clone();
                $helper.children().each(function (index) {
                    $(this).width($originals.eq(index).width())
                });
                return $helper;
            },
            stop: function (e, ui) {
                $('td.index', ui.item.parent()).each(function (i) {
                    $(this).html(i + 1);
                });
                var ids = new Array();
                var primarys = _option.key.split(',');
                _option.jqTable.find("tbody tr").each(function (index, row) {
                    var row = $(this).data("data");
                    var val = "";
                    $.each(primarys, function (j, d) {
                        if (val != "")
                            val += "_";
                        val += row[d];
                    })
                    if (val != "") ids.push(val);
                });
                $.post(_option.sortUrl, { ids: ids.toString() }, function (result) {
                    if (result && result.code == 0) {
                        if ($.isFunction(_sortCallback)) {
                            _sortCallback(ids);
                        }
                    }
                });
            }
        }).disableSelection();
        return _option;
    }

    _option.delete = function (param) {
        if ($.isFunction(param)) { _callback.delete = param; }
        else {
            var se = _zkTable.selected();
            var selections = [];
            var primarys = _option.key.split(',');
            for (var i = 0; i < se.length; i++) {
                var val = "";
                $.each(primarys, function (j, d) {
                    if (val != "")
                        val += "_";
                    val += se[i][d];
                })
                if (val != "")
                    selections.push(val);
            }

            if (selections.length == 0) {
                alert("请选择要删除的项");
                return _option;
            }

            if (confirm("您确定要删除所选内容吗？")) {
                $.post(_option.deleteUrl, { ids: selections.toString() }, function (result) {
                    if (result && result.code == 0) {
                        $.adminSetting.addLogs($(document).attr("title"), "删除", "删除数据" + selections.toString());
                        _option.query();
                    }
                    alert(result.msg);
                    if ($.isFunction(_callback.delete)) { _callback.delete(result); }
                });
            }
        }
        return _option;
    };

    _option.import = function (callback) {
        if ($.isFunction(callback)) { _callback.import = callback; }
        return _option;
    };

    _option.export = function (exportUrl) {
        var filter = (_filter.formSerialize() + "&r=" + new Date().getTime()).split('&');
        var form = $("<form></form>").attr("action", exportUrl).attr("method", "post");
        for (var idx in filter) {
            var item = filter[idx].split("=");
            form.append($("<input></input>").attr("type", "hidden").attr("name", item[0]).attr("value", item[1]));
        }
        form.appendTo('body').submit().remove();

        return _option;
    };
    _option.selected = function () {
        return _zkTable.selected();
    }
    var formatters = {};
    $.each(_option.jqTable.find("th"), function (i, d) {
        var key = $(d).attr("data-field");
        if (key != "undefined" && key != undefined) {
            _option.keys.push(key);
            _option.columns[key] = columnformat(key);
        }

        var format = $(d).attr("data-format");
        if (format != "undefined" && format != undefined) {
            formatters[key] = function (event,val, data) {
                if (format.indexOf("time|") == 0 || format == "time") {
                    var para = format.split('|');
                    if (para.length == 2)
                        $(this).html(time.format(val, para[1]));
                    else
                        $(this).html(time.format(val, "yyyy-MM-dd hh:mm:ss"));                 
                }
                else if (format.indexOf("cut|") == 0 || format == "cut") {
                    var para = format.split('|');
                    if (para.length == 2) {
                        $(this).attr("title", val);
                        $(this).html(string.subString(val, para[1]));
                    }
                    else
                        $(this).html(val);
                }
                else if (format.indexOf("edit|") == 0 || format == "edit") {
                    var span = $('<span class="btn btn-link">' + val + '</span>');
                    span.bind('click', function () {
                        var para = format.split('|');
                        if (para.length == 1)
                            para.push($(".formSuite-wrapper").attr("id"));

                        var param = {};
                        $.each(_option.key.split(','), function (j, d) { param[d] = data[d]; })
                        $("#" + para[1] + ' [data-action="loadform"]').attr("data-param", JSON.stringify(param)).click();
                    });
                    $(this).append(span);
                }
            };
        }
    });

    function columnformat(key) {
        _column = {};
        _column.format = function (action, fmt) {
            if ($.isFunction(action)) {
                formatters[key] = action;
            }
            return _option;
        }
        return _column;
    }

    _button.find("[action=query]").click(function () {
        _option.column = "";
        _option.keyword = "";
        _option.query();
    });
    _button.find("[action=delete]").click(function () { _option.delete(); });

    //导入初始化
    $("[action=import]").each(function () {
        var importUrl = $(this).attr("data-url") || _option.importUrl;
        var importFile = $('<input style="display:none;" type="file" class="queryImport" />');
        importFile.fileupload({
            url: importUrl,
            autoUpload: true,
            add: function (e, data) {
                var fileName = data.files[0].name.toLowerCase();
                var suffix = fileName.substring(fileName.lastIndexOf("."));
                if (suffix != ".xlsx" && suffix != ".xls" && suffix != ".txt") {
                    alert("文件类型错误，请选择 xls,xlsx,txt 类型文件!");
                    return false;
                }
                data.submit();
            },
            success: function (result) {
                if (result.code == 0) {
                    if ($.isFunction(_callback.import)) { _callback.import(result); }
                } else {
                    alert(result.msg);
                }
            },
            error: function (msg) {
                alert('数据文件导入失败，请稍后重试！');
            }
        });
        $(this).before(importFile).on("click", function () { importFile.click(); });
    });
    $("[action=export]").each(function () {
        var exportUrl = $(this).attr("data-url") || _option.exportUrl;
        $(this).click(function () { _option.export(exportUrl); });

    });
    _option.controls = {};
    $.adminTools.renderControls(_wrapper.attr("id"), _filter, _option);

    _filter.find("input").bind('keypress', function (event) { if (event.keyCode === 13) { _option.query(); } });
    return _option;
}

$.fn.formSuite = function () {
    var _wrapper = $(this);
    var _form = _wrapper.find("form");

    var _option = {};
    var _callback = {};

    $.adminTools.initForm(_option, _callback, _wrapper, _form);

    var _header = $('<div class="modal-header">\
        <h4 class="modal-title"></h4>\
        <button type="button" class="close" data-dismiss="modal">&times;</button></div>');
    if (_wrapper.find(".modal-content:first").children(".modal-header").length == 0)
        _header.insertBefore(_wrapper.find(".modal-body:first"));

    var h4id = _wrapper.attr("id") + "H4";
    var _title = _wrapper.find(".modal-title").attr("id", h4id);

    $("[action-modal=" + _wrapper.attr("id") + "]").attr("data-toggle", "modal").attr("data-target", "#" + _wrapper.attr("id")).click(function () { _option.add(); });
    _wrapper.append($('<span data-action="loadform"></span>').click(function () { _option.load(JSON.parse($(this).attr("data-param"))); }));

    _option.save = function (callback) {
        if ($.isFunction(callback)) { _callback.save = callback; }
        else {
            if ($.lock) return _option;
            $.showLoading();
            var data = _form.formSerialize();
            $.post(_option.saveFormUrl, data, function (result) {
                if (result.code == 0) {
                    $.each(_form.find("[data-file]"), function () {
                        _option.controls[$(this).attr('name')].bitcontrol.save(result.data[_option.key]);
                    });
                    if ($.isFunction(_callback.save)) { _callback.save(result); }
                }
                else {
                    alert(result.msg);
                }
                $.hideLoading();
            });

        }
        return _option;
    };

    _option.validator = {
        submitHandler: function (form) {
            if ($.lock) return false;

            $.showLoading();
            var data = _form.formSerialize();
            $.post(_option.saveFormUrl, data, function (result) {
                if (result.code == 0) {
                    $.adminSetting.addLogs($(document).attr("title"), (_option.logType != "edit") ? "新增" : "编辑", data);
                    $.each(_form.find("[data-file]"), function () {
                        _option.controls[$(this).attr('name')].bitcontrol.save(result.data[_option.key]);
                    });
                    if ($.isFunction(_callback.submit)) { if (_callback.submit(result) != false) _wrapper.modal('hide');  }
                }
                else { alert(result.msg); }
                $.hideLoading();
            });

            return false;
        }
    };

    _option.reset = function () {
        $.adminTools.clearForm(_form, _option);
        _title.text("新增" + _option.title);
        return _option;
    };

    _option.edit = function (param) {
        if ($.isFunction(param)) { _callback.edit = param; }
        else {
            _option.reset();
            _wrapper.modal("show");
            $.showLoading();
            _option.logType = "edit";
            _title.text("修改" + _option.title);
            $.ajax({
                type: "post",
                url: _option.loadFormUrl,
                datatype: "json",
                data: param,
                success: function (result) {
                    $.adminTools.renderForm(_form, result.data, _option, param[_option.key]);
                    $.hideLoading();
                    if ($.isFunction(_callback.edit)) { _callback.edit(result); }
                },
                error: function (msg) {
                    $.hideLoading();
                }
            });
        }
        return _option;
    };
    _option.load = function (data) {
        if (data)
            _option.edit(data);
        else
            _option.add();

        return _option;
    };
    $.adminTools.renderControls(_wrapper.attr("id"), _form, _option);
    return _option;
}

$.fn.generalForm = function () {
    var _wrapper = $(this);
    var _form = _wrapper.find("form");
    var _option = {};
    var _callback = {};

    $.adminTools.initForm(_option, _callback, _wrapper, _form);

    _option.save = function (param) {
        if ($.isFunction(param)) { _callback.save = param; }
        else {
            if ($.lock) return _option;

            $.showLoading();
            $.post(_option.saveFormUrl, _form.formSerialize(), function (result) {
                if (result.code == 0) {
                    $.each(_form.find("[data-file]"), function () {
                        _option.controls[$(this).attr('name')].bitcontrol.save(result.data[_option.key]);
                    });
                    if ($.isFunction(_callback.save)) { _callback.save(result); }
                }
                else {
                    alert(result.msg);
                }
                $.hideLoading();
            });
        }
        return _option;
    };

    _option.validator = {
        submitHandler: function (form) {
            if ($.lock) return false;

            $.showLoading();
            var data = _form.formSerialize();
            $.post(_option.saveFormUrl, data, function (result) {
                if (result.code == 0) {
                    $.adminSetting.addLogs($(document).attr("title"), (_option.logType != "edit") ? "新增" : "编辑", data);
                    $.each(_form.find("[data-file]"), function () {
                        _option.controls[$(this).attr('name')].bitcontrol.save(result.data[_option.key]);
                    });
                    if ($.isFunction(_callback.submit)) { _callback.submit(result); }
                }
                else { alert(result.msg); }
                $.hideLoading();
            });

            return false;
        }
    };

    _option.reset = function () {
        $.adminTools.clearForm(_form, _option);
        return _option;
    };

    _option.edit = function (param) {
        if ($.isFunction(param)) { _callback.edit = param; }
        else {
            _option.reset();
            $.showLoading();
            _option.logType = "edit";
            $.ajax({
                type: "post",
                url: _option.loadFormUrl,
                datatype: "json",
                data: param,
                success: function (result) {
                    $.adminTools.renderForm(_form, result.data, _option, param[_option.key]);
                    $.hideLoading();
                    if ($.isFunction(_callback.edit)) { _callback.edit(result); }
                },
                error: function (msg) {
                    $.hideLoading();
                }
            });
        }
        return _option;
    };
    _option.load = function (data) {
        var val = "";
        for (var key in data) {
            val += data[key];
        }

        if (val != "")
            _option.edit(data);
        else
            _option.add();
        return _option;
    };
    $.adminTools.renderControls(_wrapper.attr("id"), _form, _option);
    return _option;
}