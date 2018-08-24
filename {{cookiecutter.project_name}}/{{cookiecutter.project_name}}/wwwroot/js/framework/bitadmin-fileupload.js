/***********************
 * BitAdmin2.0框架文件
 ***********************/
$.fn.upload = function (option) {
    var _wrapper = $(this);
    _wrapper.hide();

    var _option = {
        mode: "ico",                                //结果模式（ico|list|view）
        buttonTxt: "上传",
        buttonStyle: "float:right;",
        uploadUrl: "../../fileservice/upload",                  //上传路径
        downloadUrl: "../../fileservice/download",              //下载地址
        saveUrl: "../../fileservice/SaveData",                  //保存地址
        queryUrl: "../../fileservice/QueryData",                //查询地址
        multiple: "true",                           //是否上传多个文件
        onlyFile: "false",                          //列表只保存唯一文件
        fileTypes: null,                            //文件类型（jpg|png|gif）
        minFileSize: 0,                             //文件最小Size（字节）
        maxFileSize: 2147483648,                    //文件最大Size（字节）
        thumbnailSizes: '50;155',                   //压缩尺寸
        createByName: null,                         //文件创建人名称（一般都是当前登录人）
        formatter: 'yyyy-MM-dd hh:mm:ss',           //列表时间格式
        date: [],                                   //文件列表
    };
    $.extend(_option, {
        mode: _wrapper.attr('data-file'),
        buttonTxt: _wrapper.attr('data-buttontxt'),
        buttonStyle: _wrapper.attr('data-button-style'),
        uploadUrl: _wrapper.attr("data-uploadurl"),
        saveUrl: _wrapper.attr("data-saveurl"),
        queryUrl: _wrapper.attr("data-queryurl"),
        downloadUrl: _wrapper.attr("data-downloadrrl"),
        target: _wrapper.attr('data-target'),
        multiple: _wrapper.attr('data-multiple'),
        onlyFile: _wrapper.attr('data-onlyfile'),
        fileTypes: _wrapper.attr('data-filetypes'),
        minFileSize: _wrapper.attr('data-minfilesize'),
        maxFileSize: _wrapper.attr('data-maxfilesize'),
        thumbnailSizes: _wrapper.attr('data-thumbnailsizes'),
        formatter: _wrapper.attr('data-format')
    });
    var Queue_add = 0;          //用于添加文件回调方法，当前队列是否首次触发
    var Queue_always = 0;       //用于回调完成方法，当前队列是否结束

    _option.targetId = guid.new();
    _option.fileCode = guid.new();
    _wrapper.attr("data-code", _option.fileCode);

    if (_option.target == undefined) _option.target = _wrapper.attr("name") + "view";
    var _target = $(_option.target);
    if (_target.length == 0) {
        _target = $("<div></div>").attr("name", _option.target.replace("#",""));
        $(this).parent().append(_target);
    }

    var buttonUploadBox=$('<div style="float:left;width:100%;"><button type="button" class="btn btn-default fileControl-marBot10" style="' + _option.buttonStyle + '"><span class="glyphicon glyphicon-cloud-upload" aria-hidden="true"></span> ' + _option.buttonTxt + '</button></ div>');
    _option.buttonUpload = buttonUploadBox.find('button').on("click", function () { $('[data-code=' + _option.fileCode + ']').click(); });
    _wrapper.after(buttonUploadBox);

    _option.modal = $(string.format('<div class="modal fade" tabindex="-1" role="dialog" aria-labelledby="{0}ModalLab" id="{0}">\
            <div class="modal-dialog" role="document">\
                <div class="modal-content">\
                    <div class="modal-header">\
                        <button type="button" class="close" style="margin-top:0px;" title="关闭"><span aria-hidden="true">&times;</span></button>\
                        <h4 class="modal-title" id="{0}Lab"></h4>\
                    </div>\
                    <div class="modal-body thumbnail" style="border-width:0px;">\
                        <img src="" alt="找不到图片" class="img-rounded">\
                    </div>\
                </div>\
            </div>\
        </div>', _option.targetId));
    _option.modal.find(".close").on("click", function () {
        _option.modal.modal('hide');
    })
    $("body:first").append(_option.modal);

    var fileBox = '<div class="fileControl-queueID"></div>';
    switch (_option.mode) {
        case 'ico':
        case 'list':
            fileBox += '<table class="table table-bordered table-hover fileControl-table"><tbody class="fileControl-content"></tbody></table>';
            break;
        case 'view':
            fileBox += '<div class="row fileControl-content"></div>';
            break;
    }

    _target.append($(fileBox));

    if ($.trim(_option.fileTypes).length > 0 && _option.fileTypes != null)
        _wrapper.attr("accept", getMIME(_option.fileTypes));
    if (_option.multiple == "true")
        _wrapper.attr("multiple", "");

    _option._saveCallback = function () { }
    _option._bindCallback = function () { }

    _wrapper.fileupload({
        /********   ajax选项    ********/
        type: 'POST',
        url: _option.uploadUrl,
        dataType: 'json',
        /********   一般选项    ********/
        paramName: 'BitFile',                       //文件参数名称
        autoUpload: true,                           //自动上传
        formData: function (form) {                 //form参数
            var data = form.serializeArray();
            data.push({ name: 'thumbnailSizes', value: _option.thumbnailSizes });
            data.push({ name: 'token', value: 'admin' });
            data.push({ name: 'password', value: 'admin' });
            return data;
        },
        /********   回调函数   ********/
        add: function (e, data) {
            //添加文件 回调方法
            if (Queue_add == 0) {
                Queue_add++;
                _target.find('.fileControl-queueID').show();
            }

            //验证文件Size
            if (_option.minFileSize != null || _option.minFileSize != undefined) {
                if (data.files[0].size < _option.minFileSize) {
                    alert("【" + data.files[0].name + "】文件的size必须大于或等于" + _option.minFileSize);
                    return;
                }
            }
            if (_option.maxFileSize != null || _option.maxFileSize != undefined) {
                if (data.files[0].size > _option.maxFileSize) {
                    alert("【" + data.files[0].name + "】文件的size必须小于或等于" + _option.maxFileSize);
                    return;
                }
            }

            //文件类型判断
            if (_option.fileTypes != null && _option.fileTypes != undefined && _option.fileTypes.length > 0) {
                var strRegex = ".*\.(" + _option.fileTypes + ")$";
                var re = new RegExp(strRegex);
                if (!re.test(data.files[0].name.toLowerCase())) {
                    alert("【" + data.files[0].name + "】文件的类型不正确，必须是【" + _option.fileTypes + "】");
                    return
                }
            }

            //
            if (_option.multiple == "false" && _option.onlyFile == "true") {
                _option.clear();
            }
            //添加进度条
            var _control = $(string.format('<div class="row" style="margin:10px 0px;" name="{0}">\
                                                <div name="{0}" class="progress col-md-11 fileControl-margin0 fileControl-padding0">\
                                                    <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" name="ProgressBar" title="{1}{2}" style="min-width:20%; width:0%;">{1}{2}</div>\
                                                </div>\
                                                <div class="col-md-1 text-right fileControl-margin0 fileControl-padding0">\
                                                    <a href="javascript:void(0);" title="取消" data-type="cancel" style="margin-left:5px;" class="text-danger"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></a>\
                                                </div>\
                                            </div>', data.files[0].lastModifiedDate.toJSON(), data.files[0].name, "（上传：0%）"));
            _control.find('a[data-type="cancel"]').on("click", function () {
                data.abort();
            });
            _target.find('.fileControl-queueID').append(_control);
            //提交
            data.submit();
        },
        done: function (e, data) {
            //上传成功 回调方法
            _target.find('div[name="' + data.files[0].lastModifiedDate.toJSON() + '"]').remove();
            _option.bind(data.result.data, false);
            if ($.isFunction(_uploadCallback))
                _uploadCallback(data.result.data);
        },
        fail: function (e, data) {
            //上传失败 回调方法
            if (data.errorThrown == 'abort') {
                //上传取消
                _target.find('div[name="' + data.files[0].lastModifiedDate.toJSON() + '"]').remove();
            } else {
                //上传失败
                alert("【" + data.files[0].name + "】文件上传失败，请联系管理员");
            }
        },
        progress: function (e, data) {
            //单文件上载进度 回调方法
            var progress = parseInt(data.loaded / data.total * 100, 10);
            var _control = _target.find('div[name="' + data.files[0].lastModifiedDate.toJSON() + '"]');
            if (progress >= 100) {
                _control.find('div[name="ProgressBar"]')
                    .removeClass("active")
                    .addClass("progress-bar-success")
                    .css('width', '100%')
                    .attr("title", data.files[0].name + "（上传：100%；处理中）")
                    .text(data.files[0].name + "（上传：100%；处理中）");
                _control.find('a[data-type="cancel"]').remove();
                return;
            }

            _control.find('div[name="ProgressBar"]')
                .css('width', progress + '%')
                .attr("title", data.files[0].name + "（上传：" + progress + "%）")
                .text(data.files[0].name + "（上传：" + progress + "%）");
        },
        always: function (e, data) {
            Queue_always++;
            if (data.originalFiles.length == Queue_always) {
                _target.find('.fileControl-queueID').hide();
                Queue_add = 0;
                Queue_always = 0;
            }
        }
    });

    //根据后缀获取MIME类型
    function getMIME(val) {
        if (val == null || val == undefined || $.trim(val).length == 0)
            return '';
        var MIME = [{ key: '3gpp', val: 'audio/3gpp,video/3gpp' },
                    { key: 'ac3', val: 'audio/ac3' },
                    { key: 'asf', val: 'allpication/vnd.ms-asf' },
                    { key: 'au', val: 'audio/basic' },
                    { key: 'css', val: 'text/css' },
                    { key: 'csv', val: 'text/csv' },
                    { key: 'doc', val: 'application/msword' },
                    { key: 'dot', val: 'application/msword' },
                    { key: 'dtd', val: 'application/xml-dtd' },
                    { key: 'dwg', val: 'image/vnd.dwg' },
                    { key: 'dxf', val: 'image/vnd.dxf' },
                    { key: 'gif', val: 'image/gif' },
                    { key: 'htm', val: 'text/html' },
                    { key: 'html', val: 'text/html' },
                    { key: 'jp2', val: 'image/jp2' },
                    { key: 'jpe', val: 'image/jpeg' },
                    { key: 'jpeg', val: 'image/jpeg' },
                    { key: 'jpg', val: 'image/jpeg' },
                    { key: 'js', val: 'text/javascript,application/javascript' },
                    { key: 'json', val: 'application/json' },
                    { key: 'mp2', val: 'audio/mpeg,video/mpeg' },
                    { key: 'mp3', val: 'audio/mpeg' },
                    { key: 'mp4', val: 'audio/mp4,video/mp4' },
                    { key: 'mpeg', val: 'video/mpeg' },
                    { key: 'mpg', val: 'video/mpeg' },
                    { key: 'mpp', val: 'application/vnd.ms-project' },
                    { key: 'ogg', val: 'application/ogg,audio/ogg' },
                    { key: 'pdf', val: 'application/pdf' },
                    { key: 'png', val: 'image/png' },
                    { key: 'pot', val: 'application/vnd.ms-powerpoint' },
                    { key: 'pps', val: 'application/vnd.ms-powerpoint' },
                    { key: 'ppt', val: 'application/vnd.ms-powerpoint' },
                    { key: 'rtf', val: 'application/rtf,text/rtf' },
                    { key: 'svf', val: 'image/vnd.svf' },
                    { key: 'tif', val: 'image/tiff' },
                    { key: 'tiff', val: 'image/tiff' },
                    { key: 'txt', val: 'text/plain' },
                    { key: 'wdb', val: 'application/vnd.ms-works' },
                    { key: 'wps', val: 'application/vnd.ms-works' },
                    { key: 'xhtml', val: 'application/xhtml+xml' },
                    { key: 'xlc', val: 'application/vnd.ms-excel' },
                    { key: 'xlm', val: 'application/vnd.ms-excel' },
                    { key: 'xls', val: 'application/vnd.ms-excel' },
                    { key: 'xlt', val: 'application/vnd.ms-excel' },
                    { key: 'xlw', val: 'application/vnd.ms-excel' },
                    { key: 'xml', val: 'text/xml,application/xml' },
                    { key: 'zip', val: 'aplication/zip' },
                    { key: 'xlsx', val: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }]

        var valArr = val.split('|');
        var valArr2 = new Array();
        var i = 0;
        $.each(MIME, function (index, object) {
            $.each(valArr, function (index2, value2) {
                if (object.key == value2) {
                    valArr2[i] = object.val;
                    i++;
                }
            });
        });
        return valArr2.join(',');
    }

    //判断是否图片
    function checkImg(val) {
        var strRegex = ".*\.(jpg|jpeg|png|gif|bmp)$"; //用于验证图片扩展名的正则表达式
        var re = new RegExp(strRegex);
        if (re.test(val.toLowerCase())) {
            return true;
        } else {
            return false;
        }
    }

    //获取非图片的图标
    function getClassIco(fileSuffix) {
        var classICO;
        switch (fileSuffix) {
            case '.txt':
                classICO = 'fa-file-text-o text-muted';
                break;
            case '.docx':
            case '.doc':
                classICO = 'fa-file-word-o text-primary';
                break;
            case '.xlsx':
            case '.xls':
                classICO = 'fa-file-excel-o text-success';
                break;
            case '.pptx':
            case '.ppt':
                classICO = 'fa-file-powerpoint-o text-danger';
                break;
            case '.pdf':
                classICO = 'fa-file-pdf-o text-danger';
                break;
            default:
                classICO = 'fa-file-o text-muted';
                break;
        }
        return classICO;
    }

    //判断空 或者空字符串
    function IsNullString(val) {
        if (val == null || val == undefined || $.trim(val).length == 0)
            return true;
        else
            return false;
    }

    //绑定ICO 数据
    function setIcoData(data) {
        var _control = '';
        var strICO = '';
        var strAction = '';
        var fileNameArr = data.names.split('|');
        if (checkImg(data.suffix)) {
            strICO = string.format('<img style="width: 36px;height: 36px;" class="img-rounded" src="{0}" alt="图片不存在">', data.path + '/' + fileNameArr[0]);
            strAction = '<a href="javascript:void(0);" title="预览" data-type="search" class="btn btn-default fileControl-color"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
        }
        else {
            strICO = string.format('<i class="fa {0}" style="font-size:50px;" aria-hidden="true"></i>', getClassIco(data.suffix));
        }

        strAction = strAction + '<a href="javascript:void(0);" title="下载" data-type="download" class="btn btn-default fileControl-color"><i class="fa fa-download" aria-hidden="true"></i></a>'
                   + '<a href="javascript:void(0);" title="删除" data-type="remove" class="btn btn-default fileControl-color"><i class="fa fa-trash" aria-hidden="true"></i></a>';

        
        _control = $(string.format('<tr id="{0}">\
                                    <td style="width:60px;">{1}</td>\
                                    <td style="text-align:left;">{2}</td>\
                                    <td style="width:160px;">{3}</td>\
                                    <td style="width:160px;">{4}</td>\
                                    <td style="width:140px;text-align:left;">{5}</td>\
                                </tr>', data.id, strICO, data.name, time.format(data.createTime, _option.formatter), data.createByName, strAction));
        _control.find('a[data-type="search"]').on('click', function () {
            search(data);
        });
        _control.find('a[data-type="download"]').attr('href', _option.downloadUrl + "?fileURL=" + data.url);
        _control.find('a[data-type="remove"]').on('click', function () {
            remove(data);
        });
        _target.find(".fileControl-content").append(_control);
    }

    //绑定列表 数据
    function setListData(data) {
        var _control = '';
        var strAction = '';
        if (checkImg(data.suffix)) {
            strAction = '<a href="javascript:void(0);" title="预览" data-type="search">预览</a>';
        }
        strAction += '<a href="javascript:void(0);" title="下载" data-type="download">下载</a>'
            + '<a href="javascript:void(0);" title="删除" data-type="remove">删除</a>';
        _control = $(string.format('<tr id="{0}">\
                                    <td style="text-align:left;">{1}</td>\
                                    <td style="width:160px;">{2}</td>\
                                    <td style="width:160px;">{3}</td>\
                                    <td style="width:134px;text-align:left;">{4}</td>\
                                </tr>', data.id, data.name, time.format(data.createTime, _option.formatter), data.createByName, strAction));
        _control.find('a[data-type="search"]').on('click', function () {
            search(data);
        });
        _control.find('a[data-type="download"]').attr('href', _option.downloadUrl + "?fileURL=" + data.url);
        _control.find('a[data-type="remove"]').on('click', function () {
            remove(data);
        });
        _target.find(".fileControl-content").append(_control);
    }

    //绑定缩略图 数据
    function setThumbnailData(data) {
        var _control = '';
        var strAction = '';
        var strICO = '';
        var fileNameArr = data.names.split('|');
        if (checkImg(data.suffix)) {
            strICO = string.format('<img class="img-rounded" style="width:155px;height:155px;" src="{0}" alt="图片不存在">', data.path + '/' + fileNameArr[1]);
            strAction = '<a href="javascript:void(0);" title="预览" data-type="search" class="btn btn-default fileControl-color"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
        }
        else {
            strICO = string.format('<i class="fa {0}" style="font-size:155px;" aria-hidden="true"></i>', getClassIco(data.suffix));
        }
        strAction += '<a href="javascript:void(0);" title="下载" data-type="download" class="btn btn-default fileControl-color"><i class="fa fa-download" aria-hidden="true"></i></a>'
                   + '<a href="javascript:void(0);" title="删除" data-type="remove" class="btn btn-default fileControl-color"><i class="fa fa-trash" aria-hidden="true"></i></a>';

        _control = $(string.format('<div class="col-md-2" id="{0}">\
                                        <div class="thumbnail text-center">\
                                            {1}\
                                            <div class="caption" style="padding-left:0px; padding-right:0px;">\
                                                <p class="fileControl-size fileControl-color" title="{2}">{3}</p>\
                                                <p class="fileControl-btn">{4}</p>\
                                            </div>\
                                        </div>\
                                    </div>', data.id, strICO, data.name, string.subString(data.name, 13), strAction));
        _control.find('a[data-type="search"]').on('click', function () {
            search(data);
        });
        _control.find('a[data-type="download"]').attr('href', _option.downloadUrl + "?fileURL=" + data.url);
        _control.find('a[data-type="remove"]').on('click', function () {
            remove(data);
        });
        _target.find(".fileControl-content").append(_control);
    }

    //预览
    function search(data) {
        var _modal = $('#' + _option.targetId);
        _modal.find('#' + _option.targetId + 'Lab').text(data.name);
        _modal.find('img').attr('src', data.url);
        $('#' + _option.targetId).modal("show");
    }

    //添加文件变量的值
    function addFileData(data) {
        _option.date.push({
            id: data.id,
            name: data.name,
            url: data.url,
            type: data.type,
            suffix: data.suffix,
            path: data.path,
            names: data.names,
            createTime: time.format(data.createTime, 'yyyy-MM-dd hh:mm:ss')
        });
    }
    //删除文件变量的值
    function removeFileData(data) {
        $.each(_option.date, function (index, value) {
            if (value.id == data.id) {
                _option.date = array.remove(_option.date, value);
                return false
            }
        });
    }
    //清空文件变量的值
    function removeAllFileData() {
        _option.date = array.removeAll(_option.date);
    }
    
    //绑定数据
    var _bindCallback;
    _option.bind = function (param, IsCallback) {
        if ($.isFunction(param)) {
            _bindCallback = param;
        }
        else {
            if (param.names == null || param.names == undefined)
                param.names = "";
            if (IsNullString(param.createByName))
                param.createByName = _option.createByName;
            switch (_option.mode) {
                case "ico":
                    setIcoData(param);
                    addFileData(param);
                    break;
                case "list":
                    setListData(param);
                    addFileData(param);
                    break;
                case "view":
                    setThumbnailData(param);
                    addFileData(param);
                    break;
            }

            if (IsCallback != false)
                _option._bindCallback(param);
        }
    }

    //删除
    var _removeCallback;
    function remove(param) {
        if ($.isFunction(param)) {
            _removeCallback = param;
        }
        else {
            removeFileData(param);
            _target.find(".fileControl-content").find('#' + param.id).remove();
            if ($.isFunction(_removeCallback))
                _removeCallback(param);
        }
    }

    //清空数据
    _option.clear = function () {
        removeAllFileData();
        _target.find(".fileControl-content").html('');
    }

    //保存：附件信息
    var _saveCallback;
    _option.save = function (param) {
        if ($.isFunction(param)) {
            _saveCallback = param;
        }
        else {
            $.ajax({
                url: _option.saveUrl,
                cache: false,
                type: "POST",
                data: {
                    RelationID: param,
                    files: _option.date
                },
                success: function (result) {
                    if (result.code == 1) {
                        alert(result.msg);
                        return;
                    }
                    _option._saveCallback(result.data);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('保存文件错误，请联系管理员');
                    console.log(XMLHttpRequest.responseText);
                }
            })
        }
        
    }

    //查询数据
    _option.query = function (relationId) {
        $.ajax({
            url: _option.queryUrl,
            cache: false,
            type: "POST",
            data: {
                RelationID: relationId,
            },
            success: function (result) {
                if (result.code == 1) {
                    alert(result.msg);
                    return;
                }
                _option.clear();
                $.each(result.data, function (index, value) {
                    _option.bind(value);
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('保存文件错误，请联系管理员');
                console.log(XMLHttpRequest.responseText);
            }
        })
    }

    //列表操作按钮不可见：data=[{key:'search',val:true},{key:'download',val:true},{key:'remove',val:true}]
    _option.buttonVisible = function (data) {
        $.each(data, function (index, value) {
            if (value.val == true)
                _target.find('a[data-type="' + value.key + '"]').hide();
            else
                _target.find('a[data-type="' + value.key + '"]').show();
        });
    }

    //列表操作按钮不可用：data=[{key:'search',val:true},{key:'download',val:true},{key:'remove',val:true}]
    _option.buttonDisabled = function (data) {
        $.each(data, function (index, value) {
            if (value.val == true)
                _target.find('a[data-type="' + value.key + '"]').attr('disabled', "disabled");
            else
                _target.find('a[data-type="' + value.key + '"]').removeAttr("disabled");
        });
    }

    //上传按钮不可见：val：true|false
    _option.uploadVisible = function (val) {
        if (val == true)
            _option.buttonUpload.hide();
        else
            _option.buttonUpload.show();
    }

    //上传按钮不可编辑：val：true|false
    _option.uploadEnabled = function (val) {
        if (val == true)
            _option.buttonUpload.attr("readonly", "readonly");
        else
            _option.buttonUpload.removeAttr("readonly");

    }

    var _uploadCallback;
    _option.upload = function (callback) {
        if ($.isFunction(callback)) {
            _uploadCallback = callback;
        }
    }

    return _option
}