/** =======================================
* adminSuite是基于jQuery，Bootstrap，Ajax，Json构建的管理模块开发套件。
* 引用组件：jquery;jquery.form;jquery.validate;bootstrap
*
* @Author  chenyinxin
* @Support <http://adminSuite.bitdao.cn>
* @Email   <chenyinxin@qq.com>
* @Version 1.0.0
*/

$.extend({
    adminSetup: function (option) {
        $.extend($.adminSetting, option)
    },
    adminSetting: {
        addLogs: function (title, type, msg) { },
        Pickers: {},
        DataApis: {}
    },
    adminTools: {
        renderControls: function (_topid, _wrapper, _changes) {
            $.adminTools.renderPicker(_topid, _wrapper, _changes);
            $.adminTools.renderDatePicker(_topid, _wrapper, _changes);
            $.adminTools.renderSelect(_topid, _wrapper, _changes);
            $.adminTools.renderRadio(_topid, _wrapper, _changes);
            $.adminTools.renderCheckbox(_topid, _wrapper, _changes);
            $.adminTools.renderAutoComplete(_topid, _wrapper, _changes);
            $.adminTools.renderAutoComSelect(_topid, _wrapper, _changes);
        },
        renderPicker: function (_topid, _wrapper, _changes) {
            var _controls = _wrapper.find('[data-control=picker]');
            $.each(_controls, function (index, el) {
                var _type = $(this).attr("data-control-type");
                var _name = $(this).attr("data-control-name");
                $(this).attr("data-url", $.adminSetting.Pickers[_type]);
                $(this).picker("#" + _topid).change(_changes,_name);
            });
        },
        renderDatePicker: function (_topid, _wrapper, _changes) {
            var _controls = _wrapper.find('[data-control=datePicker]');
            $.each(_controls, function (index, el) {
                var _name = $(this).attr("data-control-name");
                
                $(this).datePicker().change(_changes,_name);
            });
        },
        renderSelect: function (_topid, _wrapper, _changes) {
            var _controls = _wrapper.find('[data-control=bitSelect]');
            $.each(_controls, function (index, el) {
                var _type = $(this).attr("data-control-type");
                var _name = $(this).attr("data-control-name");
                $(this).attr("data-url", $.adminSetting.DataApis[_type]);
                $(this).bitSelect().change(_changes, _name);
            });
        },
        renderAutoComplete: function (_topid, _wrapper, _changes) {
            var _controls = _wrapper.find('[data-control=bitAutoComplete]');
            $.each(_controls, function (index, el) {
                var _type = $(this).attr("data-control-type");
                var _name = $(this).attr("data-control-name");
                $(this).attr("data-url", $.adminSetting.DataApis[_type]);
                $(this).bitAutoComplete().change(_changes, _name);
            });
        },
        renderAutoComSelect: function(_topid, _wrapper, _changes) {
            var _controls = _wrapper.find('[data-control=bitAutoComSelect]');
            $.each(_controls, function (index, el) {
                var _type = $(this).attr("data-control-type");
                var _name = $(this).attr("data-control-name");
                $(this).attr("data-url", $.adminSetting.DataApis[_type]);
                $(this).bitAutoComSelect().change(_changes, _name);
            });
        },
        renderRadio: function (_topid, _wrapper, _changes) {
            var _controls = _wrapper.find('[data-control=bitRadio]');
            $.each(_controls, function (index, el) {
                var _type = $(this).attr("data-control-type");
                var _name = $(this).attr("data-control-name");
                $(this).attr("data-url", $.adminSetting.DataApis[_type]);

                $(this).bitRadio().change(_changes,_name);
            });
        }, 
        renderCheckbox: function (_topid, _wrapper, _changes) {
            var _controls = _wrapper.find('[data-control=bitCheckbox]');
            $.each(_controls, function (index, el) {
                var _type = $(this).attr("data-control-type");
                var _name = $(this).attr("data-control-name");
                $(this).attr("data-url", $.adminSetting.DataApis[_type]);

                $(this).bitCheckbox();
            });
        },
    }
});

$.fn.zk_filter = function (querySuite) {
    var _filter = querySuite.jqFilter;
    var _option = {
        getfilters: null
    };
    //获得查询条件输入值
    _option.getfilters = function () {
        var data = {};
        $.each(_filter.find("input,select"), function () {
            var name = $(this).attr("name");
            var type = $(this).attr("type");
            var value = "";
            if (name != undefined) {
                switch (type) {
                    case "radio":
                        value = $("input[name='" + name + "']:checked").val();
                        break;
                    case "checkbox":
                        $("input[name='" + name + "']:checked").each(function (i) {
                            value += $(this).val() + ',';
                        });
                        value = value.substring(0, value.length - 1);
                        break;
                    default:
                        if ($(this).val() != "")
                            value = $(this).val();
                        break;
                }
                if (value != "" && value != undefined && value != "undefined")
                    data[name] = value;
            }
        });
        return data;
    };
    return _option;
}

$.fn.zk_table = function (option, querySuite) {
    var self = this;
    var _default = {
        columns: [],
        initiaTBody: function (msg) { },
        rows: null,
        isSelect: false,
        sortable: function (key, rule) { },
        formatter: null,
        update: null,
        Table: null
    }
    var _option = $.extend(_default, option);

    //表格行内检索组件
    var querySuiteSearch = function (option) {
        //默认选项
        var defaults = {
            event: event,
            TableData: null,
            cancleCallback: function () { },
            queryCallback: function () { }
        }
        //合并参数
        var option = $.extend(defaults, option);
        var self = this;

        var _remove = function () {
            $("body").find('.querySuiteSearch').remove();
        }

        var _click = function (event) {
            //获取触发容器的位置坐标：        
            _remove();
            var content = $('<div class="querySuiteSearch"></div>')
                .css('left', event.pageX - 30)
                .css('top', event.pageY + 20);

            var btnContent = $('<div class="btnContnet"></div>');
            var btnClose = $('<div class="btnClose">&times;</div>')
                .bind('click', function () {
                    content.remove();
                    if ($.isFunction(option.cancleCallback)) {
                        option.cancleCallback();
                    }
                });
            content.append(btnContent.append(btnClose));

            var inContent = $('<div class="inputContent"></div>');
            var input = $('<input type="text" placeholder="输入关键字" />');
            var btnSearch = $('<span class="btn btn-primary"">查询</span>')
                .bind('click', function () {
                    if ($.isFunction(option.queryCallback)) {
                        option.queryCallback(option.TableData, input.val());
                        content.remove();
                    }
                });
            content.append(inContent.append(input).append(btnSearch));

            $('body').append(content);
        }
        _click(option.event);
    };
    var _searchTable = function (tdName, event) {
        querySuiteSearch({
            event: event,
            queryCallback: function (data, key) {
                querySuite.column = tdName;
                querySuite.keyword = key;
                querySuite.query();
            }
        })
    };
    _option.sortable = function (key, rule) {
        querySuite.queryOrderType = rule;
        querySuite.queryOrderBy = key;
        querySuite.refresh();
    };

    ///初始化表格
    _option.Table = $('<table class="table table-bordered table-hover"></table>');

    ///初始化表头
    var createHeadHTML = function () {
        var thtml = $(self).find('thead');
        _option.Table.append(thtml);

        if (_option.isSelect) {
            thtml.find('input[type="checkbox"]').change(function () {
                var is_checked = $(this).prop("checked");
                thtml.parent().find("tbody").find(".SelectRow").prop("checked", is_checked);
            });
        }

        ///验证表头是否存在检索
        var columns = _option.Table.find("thead").find('th');
        $.each(columns, function (index, element) {
            var key = $(this).attr('data-search');
            var field = $(this).attr('data-field');
            if (key == "true") {
                var sch = $('<span class="glyphicon glyphicon-filter" style="top:3px;cursor: pointer;"></span>');
                $(this).append(sch);
                sch.bind('click', function (event) {
                    _searchTable(field, event);
                })
            }
        });

        ///验证表头是否存在排序
        ///排序的状态===初始状态、、、升序、、、、降序
        $.each(columns, function (index, element) {
            var key = $(this).attr('data-sortable');
            var field = $(this).attr('data-field');
            if (key == "true") {
                var sch = $('<span class="querySuite-sortable glyphicon glyphicon-sort"></span>');
                sch.data('data', 'glyphicon-sort');
                $(this).append(sch);
                sch.bind('click', function (event) {
                    ///如果点击项是默认状态。 修改其余项成为默认状态。修改当期项降序
                    if ($(this).data('data') == 'glyphicon-sort') {
                        var ot = _option.Table.find('thead').find('.querySuite-sortable');
                        $.each(ot, function (index, element) {
                            $(this).data('data', 'glyphicon-sort');
                            $(this).removeClass('glyphicon-sort-by-attributes-alt');
                            $(this).removeClass('glyphicon-sort-by-attributes');
                            $(this).addClass('glyphicon-sort');
                        });
                        var ops = _option.Table.find('thead').find('th[data-field=' + field + ']');
                        ops.find('.querySuite-sortable').removeClass('glyphicon-sort');
                        ops.find('.querySuite-sortable').addClass('glyphicon-sort-by-attributes-alt');
                        ops.find('.querySuite-sortable').data('data', 'glyphicon-sort-by-attributes-alt');
                        ///控制复制的table层次。
                        var list = $('.querySuite-sortable');
                        $.each(list, function (index, element) {
                            $(this).data('data', 'glyphicon-sort');
                            $(this).removeClass('glyphicon-sort-by-attributes-alt');
                            $(this).removeClass('glyphicon-sort-by-attributes');
                            $(this).addClass('glyphicon-sort');
                        })
                        $(this).removeClass('glyphicon-sort');
                        $(this).addClass('glyphicon-sort-by-attributes-alt');
                        $(this).data('data', 'glyphicon-sort-by-attributes-alt');
                        _option.sortable(field, 'desc');
                        return;
                    }

                    if ($(this).data('data') == 'glyphicon-sort-by-attributes-alt') {
                        var ops = _option.Table.find('thead').find('th[data-field=' + field + ']');
                        ops.find('.querySuite-sortable').removeClass('glyphicon-sort-by-attributes-alt');
                        ops.find('.querySuite-sortable').addClass('glyphicon-sort-by-attributes');
                        ops.find('.querySuite-sortable').data('data', 'glyphicon-sort-by-attributes');
                        $(this).addClass('glyphicon-sort-by-attributes');
                        $(this).removeClass('glyphicon-sort-by-attributes-alt');
                        $(this).data('data', 'glyphicon-sort-by-attributes');
                        _option.sortable(field, 'asc');
                        return;
                    }

                    if ($(this).data('data') == 'glyphicon-sort-by-attributes') {
                        var ops = _option.Table.find('thead').find('th[data-field=' + field + ']');
                        ops.find('.querySuite-sortable').removeClass('glyphicon-sort-by-attributes');
                        ops.find('.querySuite-sortable').addClass('glyphicon-sort-by-attributes-alt');
                        ops.find('.querySuite-sortable').data('data', 'glyphicon-sort-by-attributes-alt');
                        $(this).addClass('glyphicon-sort-by-attributes-alt');
                        $(this).removeClass('glyphicon-sort-by-attributes');
                        $(this).data('data', 'glyphicon-sort-by-attributes-alt');
                        _option.sortable(field, 'desc');
                        return;
                    }
                })
            }
        })

    }

    ///组织列表体
    var _GetTbodyHtml = function (rows, table, isFirst) {
        if (rows != null && rows.length > 0) {
            for (var i = 0; i < rows.length; i++) {
                var node = rows[i];
                var index_th = 0;
                var tr = $('<tr></tr>');
                ///绑定全选按钮
                if (_option.isSelect) {
                    var style = _option.Table.find("thead").find('th:eq(' + index_th + ')').attr('style');
                    style = (style != undefined) ? "stype='" + style + "'" : "";
                    var inpt = $('<td ' + style + '><input class="SelectRow" type="checkbox"></td>');

                    index_th++;

                    ///绑定全选的方法
                    inpt.find('.SelectRow').data('data', node);
                    inpt.find('.SelectRow').change(function () {
                        var is_checked = $(this).prop("checked");
                        if (is_checked == false) {
                            querySuite.jqTable.find('thead').find('input[type="checkbox"]').prop('checked', false);
                        } else {
                            var length = querySuite.jqTable.find('.SelectRow:checked').length;
                            if (length == querySuite.pageSize) {
                                querySuite.jqTable.find('thead').find('input[type="checkbox"]').prop('checked', true);
                            }
                        }
                    });
                    tr.append(inpt);
                    tr.bind('click', function () {
                        $(this).siblings("tr").removeClass("Selected");
                        $(this).addClass("Selected");
                    });
                }
                ///使用数据填充列表部分
                for (var m = 0; m < _option.columns.length; m++) {
                    var td = $('<td></td>');
                    var key = _option.columns[m];
                    var isFormatter = _option.formatter[key];
                    if (isFormatter == undefined) {
                        td.html(node[key]);
                    } else {
                        var html = isFormatter(node[key], node);
                        td.append(html);
                    }
                    var style = _option.Table.find("thead").find('th:eq(' + index_th + ')').attr('style');
                    if (style != undefined) {
                        td.attr("style", style);
                        if (style.indexOf("display:none") > -1)
                            td.show();
                    }
                    index_th++;
                    tr.append(td);
                }
                table.find('tbody').append(tr);
            }
        }
    }

    ///生成表体
    var createBodyHTML = function (isFirst) {
        _option.Table.find('tbody').remove();
        _option.Table.append($('<tbody></tbody>'));
        if ($.isFunction(_option.initiaTBody)) {
            _option.initiaTBody(_option.Table);
        } else {
            ///使用key和data来生成数据     
            _GetTbodyHtml(_option.rows, _option.Table, isFirst);
        }
    }

    ///表格更新
    _option.updata = function (data) {
        $("body").find('.querySuiteSearch').remove();
        _option.Table.find("input[type=checkbox]").prop('checked', false);
        _option.Table.find('tbody').remove();
        _option.Table.append($('<tbody></tbody>'));
        _GetTbodyHtml(data, _option.Table, false);
        _option.resize();
    }

    ///获取所有的选择
    _option.getSelect = function () {
        var ckList = _option.Table.find('.SelectRow:checked');
        var DataList = [];
        if (ckList.length > 0) {
            for (var i = 0; i < ckList.length; i++) {
                var node = ckList[i];
                DataList.push($(node).data('data'));
            }
        }
        return DataList;
    }

    ///调整页面大小
    _option.resize = function () {
        var content = $('<div></div>')
        content.append(_option.Table);
        $(self).html('');
        $(self).append(content);
    }
    ///浏览器窗口修改事件
    $(window).resize(function () {
        _option.resize();
    })
    
    createHeadHTML()
    createBodyHTML(true);

    var content = $('<div></div>')
    content.append(_option.Table);
    $(self).html('');
    $(self).append(content);
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
    var _init = $('<td style="vertical-align: middle;"><span>当前 ' + _option.pageIndex + ' / ' + pagezNum + ' 页，每页<input type="text" class="pageSize" title="离开此文本自动设置" style="width:40px;height: 20px;text-align: center; margin:0px 5px;border: #d2d6de 1px solid;border-radius: 4px;" value="' + _option.pageSize + '" />条\
                        ,总共 ' + _option.totalContent + ' 条记录</span></td>');
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

    table.append(_init);

    var _nav = $('<td style="text-align:right"><nav class="pageNum"></nav></td>');
    var ul = $('<ul class="pagination" style="margin: 5px 0px;">');
    _nav.find('nav').append(ul);

    table.append(_nav);

    var initialPage = function () {
        ul.html('');
        ///添加首页
        var li_first = $('<li><a href="javascript:void(0);">&laquo;</a></li>');
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
            var li = $('<li><a href="javascript:void(0);">' + i + '</a></li>');
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
        var li_last = $('<li><li><a href="javascript:void(0);">&raquo;</a></li></li>');
        ul.append(li_last);

        li_last.bind('click', function () {
            _option.pageIndex = pagezNum;
            if ($.isFunction(_option.onPageIndexChange)) {
                _option.onPageIndexChange(_option.pageIndex);
            }
            initialPage();
        })
    }
    initialPage();
    $(self).append(table);
}

//查询套件
$.fn.querySuite = function (option) {
    var _wrapper = $(this);
    var _filter = _wrapper.find(".querySuite-filter");
    var _button = _wrapper.find(".querySuite-button");
    var _table = _wrapper.find(".querySuite-table");
    var _paging = _wrapper.find(".querySuite-paging");

    _filter.addClass("container-fluid");
    _button.addClass("container-fluid");

    var _option = {
        jqFilter: _filter,
        jqButton: _button,
        jqTable: _table,
        jqPaging: _paging,
        query: null,            //重新查询第一页数据
        queryUrl: null,
        delete: null,           //删除列表选择的数据
        deleteUrl: null,
        export: null,           //导出当前查询条件的数据
        exportUrl: null,
        refresh: null,          //刷新当前页的数据
        queryOrderType: null,
        queryOrderBy: null,
        columnsKey: [],            //要显示的列集合
        key: null,          //主键，组合主键用(，)隔开
        pageIndex: 1,           //当前显示页数
        pageSize: null
    };
    $.extend(_option, option);

    _option.queryUrl = _table.attr("data-query-url");
    _option.queryOrderBy = _table.attr("data-order-by");
    _option.queryOrderType = _table.attr("data-order-type");

    _option.deleteUrl = _table.attr("data-delete-url");
    _option.exportUrl = _table.attr("data-export-url");
    _option.key = _table.attr("data-key");

    _option.pageSize = _paging.attr("data-page-size");
    if (_option.pageSize == "undefined" || _option.pageSize == undefined)
        _option.pageSize = 10;
    _option.pageSize = parseInt(_option.pageSize);

    //查询条件行数，大于1行显示收缩
    if (_filter.find("tr").length > 1) {
        _filter.find("tr").addClass("tr_shrink").hide();
        _filter.find("tr:eq(0)").removeClass("tr_shrink").show();
        _filter.find("tr").append("<td></td>");
        var expand = $('<button type="button" class="btn btn-link">\
                               <span class="glyphicon glyphicon-zoom-in"></span> 高级查询\
                            </button>');
        expand.bind("click", function () {
            _filter.find(".tr_shrink").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
            $(this).css("display", "none");
            shrink.css("display", "block");
        });
        var shrink = $('<button type="button" class="btn btn-link">\
                                <span class="glyphicon glyphicon-zoom-out"></span> 高级查询\
                            </button>');
        shrink.hide();
        shrink.bind("click", function () {
            _filter.find(".tr_shrink").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
            $(this).css("display", "none");
            expand.css("display", "block");
        });
        var span = $("<span></span>");
        span.append(expand).append(shrink);
        _filter.find("tr:eq(0)").find("td:last").append(span);
    }

    var _zkFilter = _filter.zk_filter(_option);

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
        var filter = _zkFilter.getfilters();
        $.extend(data, filter);

        $.ajax({
            url: _option.queryUrl,
            async: false,
            data: data,
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
    _option.query = function () {
        if (_zkTable){
            _option.pageIndex = 1;
            return _option.refresh();
        }
        else {
            _zkTable = _table.zk_table({
                columns: _option.columnsKey,
                isSelect: isSelect,
                formatter: formatters
            }, _option);
            _zkTable.updata(_option.GetData().data);
            return _option;
        }
    }

    //刷新
    _option.refresh = function () {
        _zkTable.updata(_option.GetData().data);
        return _option;
    };

    //删除
    var _deleteCallback;
    _option.delete = function (param) {
        if ($.isFunction(param)) { _deleteCallback = param; }
        else {
            var se = _zkTable.getSelect();
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
                return;
            }

            if (confirm("您确定要删除所选内容吗？")) {
                $.post(_option.deleteUrl, { IDs: selections.toString() }, function (result) {
                    if (result && result.code == 0) {
                        $.adminSetting.addLogs($(document).attr("title"), "删除", "删除数据" + selections.toString());   //添加日志
                        _option.query();
                    }
                    alert(result.msg);
                    if ($.isFunction(_deleteCallback)) {
                        _deleteCallback(result);
                    }
                });
            }
        }
        return _option;
    };

    _option.export = function () {
        var data = {
            r: new Date().getTime()
        };
        var filter = _zkFilter.getfilters();
        $.extend(data, filter);
        $.post(_option.exportUrl, data, function () { })
        return _option;
    };
    _option.getSelect = function () {
        return _zkTable.getSelect();
    }
    var isSelect = (_table.find("input[type=checkbox]").length > 0);
    var formatters = {};
    _option.columns = {};
    $.each(_option.jqTable.find("th"), function (i, d) {
        var key = $(d).attr("data-field");
        if (key != "undefined" && key != undefined) {
            _option.columnsKey.push(key);
            _option.columns[key] = columnformat(key);
        }

        var format = $(d).attr("data-format");
        if (format != "undefined" && format != undefined) {
            formatters[key] = function (val, data) {
                if (format.indexOf("time|") == 0 || format == "time") {
                    var para = format.split('|');
                    if (para.length == 2)
                        return time.format(val, para[1]);
                    return time.format(val, "yyyy-MM-dd hh:mm:ss");
                }
                else if (format.indexOf("edit|") == 0 || format == "edit") {
                    var span = $('<span class="btn btn-link">' + val + '</span>');
                    span.bind('click', function () {
                        var para = format.split('|');
                        if (para.length == 1)
                            para.push($(".formSuite-wrapper").attr("id"));

                        var loadform = $("#" + para[1] + ' [data-action="loadform"]');
                        var primarys = _option.key.split(',');
                        var param = {};
                        $.each(primarys, function (j, d) { param[d] = data[d]; })
                        loadform.attr("data-param", JSON.stringify(param)).click();
                    });
                    return span;
                }
            };
        }
    });

    function columnformat(column) {
        _column = {};
        _column.format = function (action, fmt) {
            if ($.isFunction(action)) {
                formatters[column] = action;
            }
            return _option;
        }
        return _column;
    }

    _button.find("[action='query']").click(function () {
        _option.column = "";
        _option.keyword = "";
        _option.query();
    });
    _button.find("[action='delete']").click(function () { _option.delete(); });
    _button.find("[action='export']").click(function () { _option.export(); });

    _option.controls = {};
    _option.changes = {};
    $.each(_option.jqFilter.find("input,select"), function (i, d) {
        var key = $(d).attr("name");
        if (key != "undefined" && key != undefined) {
            _option.controls[key] = inputChange($(d));
        }
    });
    $.each(_option.jqFilter.find("[data-control-name]"), function (i, d) {
        _option.controls[key] = inputChange($(d), true, $(d).attr("data-control-name"));
    });
    function inputChange(input, isBit, key) {
        _input = {};
        _input.change = function (action) {
            if ($.isFunction(action)) {
                if (isBit) _option.changes[key] = action;
                 input.change(action);
            }
            return _option;
        }
        return _input;
    }
    $.adminTools.renderControls(_wrapper.attr("id"), _filter, _option.changes);
    return _option;
}

$.fn.formSuite = function (option) {
    var _wrapper = $(this);
    var _header = $('<div class="modal-header">\
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
        <h4 class="modal-title"></h4></div>');
    if (_wrapper.find(".modal-content:first").children(".modal-header").length == 0)
        _header.insertBefore(_wrapper.find(".modal-body:first"));

    var h4id = _wrapper.attr("id") + "H4";
    var _title = _wrapper.find(".modal-title").attr("id", h4id);
    _wrapper.attr("aria-labelledby", h4id).attr("tabindex", "-1");

    var _form = _wrapper.find("form");
    var _addButton = $("[action-modal=" + _wrapper.attr("id") + "]").attr("data-toggle", "modal").attr("data-target", "#" + _wrapper.attr("id")).click(function () { _option.add(); });;
    var _saveButton = _wrapper.find("[action=save]").click(function () { _option.submit(); });

    var _loadform = $('<span data-action="loadform"></span>');
    _loadform.click(function () { _option.load(JSON.parse($(this).attr("data-param"))); });
    _wrapper.append(_loadform);

    var _option = {
        jqModal: _wrapper,
        jqForm: _form
    };
    $.extend(_option, option,
        {
            title: _form.attr("data-title"),
            submitClose: _form.attr("data-submit-close"),
            loadFormUrl: _form.attr("data-load-url"),
            saveFormUrl: _form.attr("data-save-url")
        });


    var lock = false;

    _option

    //添加
    var _addCallback;
    _option.add = function (param) {
        if ($.isFunction(param)) { _addCallback = param; }
        else {
            _option.reset();
            if ($.isFunction(_addCallback)) { _addCallback(); }
        }
        return _option;
    }

    //验证
    _option.valid = function () {
        return _form.valid();
    };

    //保存（不验证）
    var _saveCallback;
    _option.save = function (param) {
        if ($.isFunction(param)) { _saveCallback = param; }
        else {
            if (!lock) {
                lock = true;
                $.showLoading();
                var data = _form.formSerialize();
                $.post(_option.saveFormUrl, data, function (result) {
                    if (result.code == 0) {
                        if ($.isFunction(_saveCallback)) {
                            _saveCallback(result);
                        }
                    }
                    else {
                        alert(result.msg);
                    }
                    lock = false;
                    $.hideLoading();
                });
            }
        }
        return _option;
    };

    //提交（验证）
    var _submitCallback;
    _option.submit = function (param) {
        if ($.isFunction(param)) { _submitCallback = param; }
        else {
            _form.validate(_validator);
            _form.submit();
        }
        return _option;
    };
    var _validator = {
        submitHandler: function (form) {
            if (lock) return false;

            lock = true;
            $.showLoading();
            var data = _form.formSerialize();
            $.post(_option.saveFormUrl, data, function (result) {
                if (result.code == 0) {
                    $.adminSetting.addLogs($(document).attr("title"), (logType != "edit") ? "新增" : "编辑", data);   //添加日志
                    if (_option.submitClose != "false") {
                        _wrapper.modal('hide');
                        _form.resetForm(); // 提交后重置表单
                    }

                    if ($.isFunction(_submitCallback)) {
                        _submitCallback(result);
                    }
                }
                else { alert(result.msg); }
                lock = false;
                $.hideLoading();
            });

            return false;
        }
    };

    _option.reset = function () {
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

        logType = "add";
        _title.text("新增" + _option.title);
        return _option;
    };

    var _editCallback;
    _option.edit = function (param) {
        if ($.isFunction(param)) { _editCallback = param; }
        else {
            _option.reset();
            _wrapper.modal("show");
            $.showLoading();
            logType = "edit";
            _title.text("修改" + _option.title);
            $.ajax({
                type: "post",
                url: _option.loadFormUrl,
                datatype: "json",
                data: param,
                success: function (result) {
                

                    for (var key in result.data) {
                        var input = _form.find(" [name='" + key + "']");
                        if (input.length > 0) {
                            var name = input.attr("name");
                            var type = input.attr("type");
                            switch (type) {
                                case "radio":
                                    _form.find("input[name='" + key + "'][value='" + result.data[key] + "']").prop("checked", "checked");
                                    input.parent().attr("data-actualval", result.data[key]);
                                    break;
                                case "checkbox":
                                    input.parent().attr("data-actualval", result.data[key]);
                                    if (result.data[key] != null && result.data[key].toString() != "") {
                                        var arr = result.data[key].toString().split(',');
                                        _form.find("input[name='" + key + "']").removeAttr("checked");
                                        for (var i in arr) {
                                            _form.find("input[name='" + key + "'][value='" + arr[i] + "']").prop("checked", "checked");
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        
                                        var value = result.data[key];
                                        if (input[0].tagName == "SELECT") {
                                            input.attr("data-actualval", value);
                                            input.val(value);
                                            continue;
                                        }
                                        var _control = input.parent();
                                        var _controltype = _control.attr("data-control");
                                        if (_controltype == "datePicker") {
                                            var formatter = _control.attr("data-format");
                                            value = time.format(value, formatter);
                                        }
                                        input.val(value);
                                    }
                                    break;
                            }
                        }
                    }
                    $.hideLoading();
                    
                    if ($.isFunction(_editCallback)) {
                        _editCallback(result);
                    }
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
    _option.controls = {};
    _option.changes = {};
    $.each(_option.jqForm.find("input,select"), function (i, d) {
        var key = $(d).attr("name");
        if (key != "undefined" && key != undefined) {
            _option.controls[key] = inputChange($(d));
        }
    });
    $.each(_option.jqForm.find("[data-control-name]"), function (i, d) {
        _option.controls[$(d).attr("data-control-name")] = inputChange($(d), true, $(d).attr("data-control-name"));
    });
    function inputChange(input, isBit, key) {
        _input = {};
        _input.change = function (action) {
            if ($.isFunction(action)) {
                if (isBit) _option.changes[key] = action;
                else input.change(action);
            }
            return _option;
        }
        return _input;
    }
    $.adminTools.renderControls(_wrapper.attr("id"), _form, _option.changes);
    return _option;
}

$.fn.generalForm = function (option) {
    var _wrapper = $(this);

    var _form = _wrapper.find("form");
    var _saveButton = _wrapper.find("[action=save]").click(function () { _option.submit(); });;

    var _option = {
        jqModal: _wrapper,
        jqForm: _form,
        loadFormUrl: _form.attr("data-load-url"),
        saveFormUrl: _form.attr("data-save-url")
    };
    $.extend(_option, option);

    var lock = false;    
    //添加
    var _addCallback;
    _option.add = function (param) {
        if ($.isFunction(param)) { _addCallback = param; }
        else {
            _option.reset();
            if ($.isFunction(_addCallback)) { _addCallback(); }
        }
        return _option;
    }

    //验证
    _option.valid = function () {
        return _form.valid();
    };

    //保存（不验证）
    var _saveCallback;
    _option.save = function (param) {
        if ($.isFunction(param)) { _saveCallback = param; }
        else {
            if (lock) return _option;

            lock = true;
            $.showLoading();
            $.post(_option.saveFormUrl, _form.formSerialize(), function (result) {
                if (result.code == 0) {
                    if ($.isFunction(_saveCallback)) { _saveCallback(result); }
                }
                else {
                    alert(result.msg);
                }
                lock = false;
                $.hideLoading();
            });
        }
        return _option;
    };

    //提交（验证）
    var _submitCallback;
    _option.submit = function (param) {
        if ($.isFunction(param)) { _submitCallback = param; }
        else {
            _form.validate(_validator);
            _form.submit();
        }
        return _option;
    };
    var _validator = {
        submitHandler: function (form) {
            if (lock) return false;

            lock = true;
            $.showLoading();
            var data = _form.formSerialize();
            $.post(_option.saveFormUrl, data, function (result) {
                if (result.code == 0) {
                    $.adminSetting.addLogs($(document).attr("title"), (logType != "edit") ? "新增" : "编辑", data);   //添加日志
                    if ($.isFunction(_submitCallback)) {
                        _submitCallback(result);
                    }
                }
                else { alert(result.msg); }
                lock = false;
                $.hideLoading();
            });

            return false;
        }
    };

    _option.reset = function () {
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

        logType = "add";
        return _option;
    };

    var _editCallback;
    _option.edit = function (param) {
        if ($.isFunction(param)) { _editCallback = param; }
        else {
            _option.reset();
            $.showLoading();
            logType = "edit";
            $.ajax({
                type: "post",
                url: _option.loadFormUrl,
                datatype: "json",
                data: param,
                success: function (result) {
                    for (var key in result.data) {
                        var input = _form.find(" [name='" + key + "']");
                        if (input.length > 0) {
                            var name = input.attr("name");
                            var type = input.attr("type");
                            switch (type) {
                                case "radio":
                                    _form.find("input[name='" + key + "'][value='" + result.data[key] + "']").prop("checked", "checked");
                                    input.parent().attr("data-actualval", result.data[key]);
                                    break;
                                case "checkbox":
                                    input.parent().attr("data-actualval", result.data[key]);
                                    if (result.data[key] != null && result.data[key].toString() != "") {
                                        var arr = result.data[key].toString().split(',');
                                        _form.find("input[name='" + key + "']").removeAttr("checked");
                                        for (var i in arr) {
                                            _form.find("input[name='" + key + "'][value='" + arr[i] + "']").prop("checked", "checked");
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        var value = result.data[key];
                                        if (input[0].tagName == "SELECT") {
                                            input.attr("data-actualval", value);
                                            input.val(value);
                                            continue;
                                        }
                                        var _control = input.parent();
                                        var _controltype = _control.attr("data-control");
                                        if (_controltype == "datePicker") {
                                            var formatter = _control.attr("data-format");
                                            value = time.format(value, formatter);
                                        }
                                        input.val(value);
                                    }
                                    break;
                            }
                        }
                    }
                    $.hideLoading();

                    if ($.isFunction(_editCallback)) { _editCallback(result); }
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
    _option.controls = {};
    _option.changes = {};
    $.each(_option.jqForm.find("select:not([data-control-name]),input:not([data-control-name])"), function (i, d) {
        var key = $(d).attr("name");
        if (key != "undefined" && key != undefined) {
            _option.controls[key] = inputChange($(d));
        }
    });
    $.each(_option.jqForm.find("[data-control-name]"), function (i, d) {
        _option.controls[$(d).attr("data-control-name")] = inputChange($(d), true, $(d).attr("data-control-name"));
    });
    function inputChange(input, isBit, key) {
        _input = {};
        _input.change = function (action) {
            if ($.isFunction(action)) {
                if (isBit) _option.changes[key] = action;
                else input.change(action);
            }
            return _option;
        }
        return _input;
    }

    $.adminTools.renderControls(_wrapper.attr("id"), _form, _option.changes);
    return _option;
}