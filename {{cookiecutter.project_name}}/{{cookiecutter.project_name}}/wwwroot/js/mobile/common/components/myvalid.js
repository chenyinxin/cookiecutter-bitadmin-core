/*
* @Author: sison.luo
* @Date:   2016-05-10 17:42:03
* @Last Modified by:   sison.luo
* @Last Modified time: 2016-06-14 11:44:54

	// 验证插件
*/
// var radio = require(radio);
// var checkbox = require(chekcbox);
// var select = require('select');


; (function ($, window, undefined) {
    $.fn.validSmart = function (options) {
        var defaults = {
            isAsync: false,  // 默认 form 提交， true 为异步
            asyncFun: function () {}
        }
        var opts = $.fn.extend(defaults, options) || {};
        return this.each(function () {
            var myvalid = new myValid($(this), opts);
            myvalid.init();
        });
    }
    var isok = false;
    var msg = {
        empty: '该字段不能为空',
        mail: '邮箱格式不正确',
        mobile: '手机格式不正确',
        idcard: '身份证格式不正确',
        six: '长度不能小于6位数',
        number: '请输入数字',
        website: '网址格式不正确',
        chinese: '请输入中文',
        psw1: '新旧密码不能相同',
        psw2: '新旧密码不能相同',
        psw2b: '两个新密码不相同',
        upload: '请选择上传文件',
        uploadformat: '上传的文件格式不正确'
    }
    var regex = {
        mail: /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\_-])+\.)+([a-zA-Z0-9]{2,5})+$/,
        mobile: /^1[345789]\d{9}$/,
        chinese: /^[\u4E00-\u9FA5]+$/,
        num: /^\d+$/,
        idcard: /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/,
        six: /^\S{6,}$/,
        website: /(http\:\/\/)?([\w.]+)(\/[\w- \.\/\?%&=]*)?/gi
    }

    var myValid = function (ele, opts) {
        this.ele = ele;
        this.opts = opts;
    }
    myValid.prototype.init = function () {
        this.unfocus();
        this.watch();
    }
    myValid.prototype.unfocus = function () {
        var self = this;
        $('.txt, .psw1, .psw2, .psw2b, textarea, select', self.ele).each(function () {
            var _this = $(this);
            _this.on('blur', function () {
                var reqs = _this.data('reqs') || '';
                var req = _this.data('req') || false;
                var arr = reqs.split(',');
                var v = getValue(_this);
                if ($.trim(v) == '') {
                    showErr(_this, msg.empty, req);
                } else {
                    showErr(_this,msg.empty, false);
                    // 检查self规则
                    for (var i = 0; i < arr.length; i++) {
                        var a = arr[i].trim();
                        if (chkErr(_this, a, v)) {
                            return;
                        }
                    }
                }
            })
        });
        $('.upload-group', self.ele).each(function () {
            var _this = $(this);
            var req = _this.data('req') || false;
            var _file = _this.find('input[type="file"]').eq(0);
            var _name = _this.find('input[type="text"]').eq(0);
            _file.on('change', function () {
                var _val = $(this).val();
                var _idx = _val.lastIndexOf('\\');
                var _fname = _val.substring(_idx + 1, _val.length);
                _name.val(_fname);
                chkUpload(_this, _val);
            });
        });
        $('.check-group', self.ele).each(function () {
            var _this = $(this);
            var req = _this.data('req') || false;
            if (req) {
                var _chk = _this.find('input[type="checkbox"]');
                _chk.on('click', function () {
                    showErrEmpty(_this);
                });
            }
        });
        $('input[type=radio]', self.ele).radioSmart({
        	callback: function(o,v){
        		showErrEmpty(o.closest('.radio-group'));
        		console.log(o);
        		console.log(v);
        	}
        });
        // $('.radio-group', self.ele).each(function () {
        //     var _this = $(this);
        //     var req = _this.data('req') || false;
        //     if (req) {
        //         var _rad = _this.find('input[type=radio]');
        //         _this.on('change', function () {
        //             showErrEmpty($(this));
        //         });
        //     }
        // })
        return this;
    }
    myValid.prototype.subcheck = function () {
        var self = this;
        isok = true;
        $('.txt, .psw1, .psw2, .psw2b, .radio-group, .check-group, .upload-group, textarea, select', self.ele).each(function () {
            var _this = $(this);
            var status = _this.data('status') || false;
            // debugger;
            // 这一步可以再改进，当实际为false的时候，省得验证，直接跳过， isok = false
            if (!status) {
                var reqs = _this.data('reqs') || '';
                var arr = reqs.split(',') || [];
                var req = _this.data('req') || false;
                var v = getValue(_this);
                if ($.trim(v) == '') {
                    showErr(_this, msg.empty, req);
                    if (req) {
                        isok = false;
                    }
                } else {
                    showErr(_this, msg.empty, false);
                    if (arr.length > 0) {
                        for (var i = 0; i < arr.length; i++) {
                            var a = $.trim(arr[i]);
                            // 表单的时候没办法异步先验证重名，只能由后端验证
                            // debugger;
                            if (a != 'query') {
                                if (chkErr(_this, a, v)) {
                                    isok = false;
                                    return;
                                }
                            } else {
                                return;
                            }
                        }
                    }
                }
            }
        });
        return this;
    }
    myValid.prototype.watch = function () {
        var self = this;
        self.ele.on('submit', function (event) {
            self.subcheck();
            if (isok) {
                // debugger;
                if (self.opts.isAsync) {
                    event.preventDefault();
                    self.opts.asyncFun();
                    return;
                }
            }
            return isok;
            //debugger;
        });
        return this;
    }
    var getValue = function (o) {
        var reqs = o.data('reqs') || '';
        var arr = reqs.split(',');
        var v;
        var regTag1 = /(input|textarea)/;
        var regTag2 = /select/;
        if (regTag1.test(o.get(0).nodeName.toLowerCase())) {
            v = o.val() || '';
        }
        if (regTag2.test(o.get(0).nodeName.toLowerCase())) {
            v = o.find('option:selected').val() || '';
        }
        if (o.hasClass('radio-group') || o.hasClass('check-group')) {
            var name = o.find('input').eq(0).attr('name');
            v = o.find('input[name=' + name + ']:checked').val() || '';
        }
        if (o.hasClass('upload-group')) {
            var v = o.find('input[type=file]').val() || '';
        }
        return v;
    }
    var chkErr = function (o, r, v) {
        switch (r) {
            case 'six':
                return chkRegex(regex.six, v) ? showErr(o, msg.six, false) : showErr(o, msg.six, true);
                break;
            case 'mail':
                return chkRegex(regex.mail, v) ? showErr(o, msg.mail, false) : showErr(o, msg.mail, true);
                break;
            case 'mobile':
                return chkRegex(regex.mobile, v) ? showErr(o, msg.mobile, false) : showErr(o, msg.mobile, true);
                break;
            case 'idcard':
                return chkRegex(regex.idcard, v) ? showErr(o, msg.idcard, false) : showErr(o, msg.idcard, true);
                break;
            case 'chinese':
                return chkRegex(regex.chinese, v) ? showErr(o, msg.chinese, false) : showErr(o, msg.chinese, true);
                break;
            case 'psw1':
                return chkPsw(o, v, r);
                break;
            case 'psw2':
                return chkPsw(o, v, r);
                break;
            case 'psw2b':
                return chkPsw(o, v, r);
                break;
            case 'upload':
                return chkUpload(o, v);
                break;
            case 'query':
                chkQuery(o, v);
                break;
            default:
                return false;
        }
    }
    var chkRegex = function (regex, v) {
        return regex.test(v);
    }

    // true: 加 ->false     false: 减 ->true
    var showErr = function (o, msg, b, sty) {
        var style = sty || 'append';
        switch (style) {
            case 'append':
                var timer;
                o.parent().find('.errtip').remove();
                if (b) {
                    o.addClass('errself');
                    o.parent().append('<p class="errtip">' + msg + '</p>');
                    clearTimeout(timer);
                    // timer = setTimeout(function(){
                    // 	o.parent().find('.errtip').remove();
                    // },1000);
                } else {
                    o.parent().find('.errtip').remove();
                }
                o.attr('data-status', !b);
                break;
            case sty:
                layer.alert(msg);
                break;
        }
        return b;
    }
    var showErrEmpty = function (o, sty) {
        var v = getValue(o);
        var bool = v == '';
        showErr(o, msg.empty, bool, sty);
        return bool;
    }
    var chkUpload = function (o, v) {
        var fm = o.data('format') || '';
        if (fm.length) {
            var idx = v.lastIndexOf('\.');
            var suffix = v.substring(idx + 1, v.length);
            var bool = fm.indexOf(suffix) == -1;
            return showErr(o, msg.uploadformat, bool);
        } else {
            return showErrEmpty(o);
        }
    }
    var chkPsw = function (o, v, who) {
        var pswv1 = o.closest('form').find('.psw1').eq(0).val();
        var pswv2 = o.closest('form').find('.psw2').eq(0).val();
        var pswv2b = o.closest('form').find('.psw2b').eq(0).val();
        switch (who) {
            case 'psw1':
                if (pswv2 != '' && v != '') {
                    return pswv2 == v ? showErr(o, msg.psw1, true) : showErr(o, msg.psw1, false);
                }
                break;
            case 'psw2':
                if (pswv1 != '' && v != '') {
                    return pswv1 == v ? showErr(o, msg.psw2, true) : showErr(o, msg.psw2, false);
                }
                if (pswv2b != '' && v != '') {
                    return pswv2b != v ? showErr(o, msg.psw2b, true) : showErr(o, msg.psw2b, false);
                }
                break;
            case 'psw2b':
                if (pswv2 != '' && v != '') {
                    return pswv2 != v ? showErr(o, msg.psw2b, true) : showErr(o, msg.psw2b, false);
                }
                break;
            default:
                return false;
        }
    }

    var chkQuery = function (o, v) {
        var pid = o.attr('id') || '';
        var url = o.data('query') || '';
        var mid = $('#ModelLibraryId').val() || 0;
        if (url.length) {
            $.ajax({
                type: 'GET',
                dataType: 'json',
                url: url,
                data: 'name=' + v + '&id=' + pid + '&mid=' + mid,
                beforeSend: function(){}
            })
            .done(function (data) {
                var bool = !data.Status || false;
                showErr(o, data.Message, bool);
            })
            .fail(function (a, b, c) {
                //console(b);
            })
            .always(function () {});
        }
    }

})(jQuery, window, document);