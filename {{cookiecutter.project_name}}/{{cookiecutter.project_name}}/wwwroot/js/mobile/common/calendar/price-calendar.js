/**
 * 酒店价格日历组件
 * 
 * Author: Angtian
 * E-mail: Angtian.fgm@taobao.com
 */
YUI.add('price-calendar', function(Y) {

/**
 * 酒店价格日历组件是一个UI控件，可以让用户直观的在日历上看到当天的房态，选择入住日期和离店日期。
 * 日历动态生成
 * 酒店房态信息异步拉取
 *
 * @module price-calendar
 */

var L      = Y.Lang,
    each   = Y.each,
    toHTML = L.sub,
    
    REG    = /-|\//g,
    RDATE  = /^((19|2[01])\d{2})-(0?[1-9]|1[012])-(0?[1-9]|[12]\d|3[01])$/;

/**
  * 创建日历构造函数
  *
  * @class   Calendar
  * @extends {Base}
  * @param   {Object} config 配置对象 (详情见API)
  * @constructor
  */    
function Calendar() {
    Calendar.superclass.constructor.apply(this, arguments);
}

Y.PriceCalendar = Y.extend(Calendar, Y.Base, {

    /**
     * 唯一日历ID
     * 
     * @type {String}
     * @private
     */
    _calendarId: 'price-calendar-' + (+new Date),
    
    /**
     * 日历外容器
     * 
     * @type {Node}
     * @private
     */
    _boundingBox: null,
    
    /**
     * 内容容器
     * 
     * @type {Node}
     * @private
     */  
    _contentBox: null,
    
    /**
     * 日历日期容器
     * 
     * @type {Node}
     * @private
     */
    _dateBox: null,   

    /**
     * 日历初始化
     *
     * @method initializer
     */   
    initializer: function() {
        this.renderUI();
    },
    
    /**
     * 渲染日历结构
     * 
     * @method renderUI
     */ 
    renderUI: function() {
        Y.one(this.get('container') || 'body').append(this._initCalendarHTML(this.get('date')));
        this._boundingBox = Y.one('#' + this._calendarId);
        this._dateBox     = this._boundingBox.one('.date-box');
        this._contentBox  = this._boundingBox.one('.content-box');
        this.bindUI()._setTips()._setWidth()._setBtnStates()._setConfirmStatus()._setSelectedRangeStyle();
        return this;
    },
    
    /**
     * 事件绑定
     * 
     * @method bindUI
     */ 
    bindUI: function() {
        this.on('render', this._setWidth);        
        this.after(['dateChange', 'dataChange'], this.render);
        this._boundingBox.delegate(['mouseenter', 'mouseleave'], this._mouseDelegate, 'td', this);
        this._boundingBox.delegate('click', this._clickDelegate, '.next-btn, .prev-btn, .in, .out, .confirm-btn, .cancel-btn', this);  
        return this;
    },
    
    /**
     * 渲染方法
     * 
     * @method render
     */     
    render: function() {
        this._dateBox.setContent(this._dateHTML());
        this._setTips()._setBtnStates()._setConfirmStatus()._setSelectedRangeStyle();
        this.fire('render');
        return this;
    },
    
    /**
     * 渲染下月日历
     * 
     * @method nextMonth
     */         
    nextMonth: function() {
        this.set('date', this._getSiblingMonth(this.get('date'), 1));
        this.fire('nextmonth');
        return this;
    },
    
    /**
     * 渲染上月日历
     * 
     * @method prevMonth
     */         
    prevMonth: function() {
        this.set('date', this._getSiblingMonth(this.get('date'), -1));
        this.fire('prevmonth');
        return this;
    },
    
    /**
     * 入住点击事件
     * 
     * @method _checkin
     * @param {Object} oTarget 事件对象
     * @private
     */
    _checkin: function(oTarget) {
        var boundingBox = this._boundingBox;
            oParent     = oTarget.ancestor('td'),
            sDate       = oParent.getAttribute('data-date'),
            aTd         = boundingBox.all('td'),
            oDep        = boundingBox.one('.dep-date'),
            oEnd        = boundingBox.one('.end-date');
        this.get('endDate') && this._toNumber(sDate) > this._toNumber(this.get('endDate')) && this.set('endDate', '');
        if(!this._getRangeRoomStatus(sDate, this.get('endDate'))) return;
        this.set('depDate', sDate);
        oDep && oDep.one('.mark').setContent('');
        aTd.removeClass('dep-date');
        oParent.addClass('dep-date');
        if(this._toNumber(sDate) >= this._toNumber(this.get('endDate'))) {
            var sNextDate = this._getSiblingDate(sDate, 1),
                oNext     = boundingBox.one('td[data-date="' + sNextDate + '"]');
            oEnd && oEnd.one('.mark').setContent('');
            aTd.removeClass('end-date');
            this.set('endDate', sNextDate);
            oNext && oNext.addClass('end-date');
            oNext && oNext.one('.mark').setContent(oNext.one('.out').get('innerHTML').substr(0, 2));
        }
        oParent.one('.mark').setContent(oTarget.get('innerHTML').substr(0, 2));
        this._setConfirmStatus()._setSelectedRangeStyle();        
        this.fire('checkin');
    },
    
    /**
     * 离店点击事件
     * 
     * @method _checkout
     * @param {Object} oTarget 事件对象    
     * @private
     */
    _checkout: function(oTarget) {
        var oParent = oTarget.ancestor('td'),
            sDate   = oParent.getAttribute('data-date'),
            aTd     = this._boundingBox.all('td'),
            oEnd    = this._boundingBox.one('.end-date');
        if(!this._getRangeRoomStatus(this.get('depDate'), sDate)) return;
        this.set('endDate', sDate);
        oEnd && oEnd.one('.mark').setContent('');
        aTd.removeClass('end-date');
        oParent.addClass('end-date');
        if(this._toNumber(sDate) <= this._toNumber(this.get('depDate'))) {
            oTarget.addClass('disabled');
        }
        oParent.one('.mark').setContent(oTarget.get('innerHTML').substr(0, 2));
        this._setConfirmStatus()._setSelectedRangeStyle();
        this.fire('checkout');
    },
    
    /**
     * 鼠标移入事件
     * 
     * @method _mouseenter
     * @param {Object} oTarget 事件对象    
     * @private
     */
    _mouseenter: function(oTarget) {
        var curDate   = this._toNumber(oTarget.getAttribute('data-date')),
            depDate   = this.get('depDate'),
            endDate   = this.get('endDate'),
            minDate   = this.get('minDate'),
            maxDate   = this.get('maxDate'),
            oIn       = oTarget.one('.in'),
            oOut      = oTarget.one('.out'),
            sDate     = oTarget.getAttribute('data-date'),
            sNextDate = this._getSiblingDate(sDate, 1),
            sPrevDate = this._getSiblingDate(sDate, -1);
        oTarget.addClass('active');
        switch(true) {
            case !!oTarget.one('.no-room'):
                oIn.addClass('disabled');
                oOut.addClass('disabled');
                break;
            case this.get('data') && !this._getRoomStatus(sNextDate):
            case maxDate && curDate >= this._toNumber(maxDate):            
                oIn.addClass('disabled');
                break;
            case this.get('depDate') && curDate <= this._toNumber(depDate):
            case this.get('data') && !this._getRoomStatus(sPrevDate):
            case minDate && curDate <= this._toNumber(minDate):
                oOut.addClass('disabled');
                break;
        }   
    },
    
    /**
     * 鼠标移出事件
     * 
     * @method _mouseleave
     * @param {Object} oTarget 事件对象    
     * @private
     */
    _mouseleave: function(oTarget) {
        oTarget.removeClass('active');
        this._boundingBox.all('.out').removeClass('disabled');        
    },
    
    /**
     * 点击事件代理
     * 
     * @method _clickDelegate  
     * @private
     */
    _clickDelegate: function(e) {
        var oTarget = e.currentTarget;            
        switch(true) {
            case oTarget.hasClass('prev-btn'):
                this.prevMonth();
                break;
            case oTarget.hasClass('next-btn'):
                this.nextMonth();
                break;
            case oTarget.hasClass('in') && !oTarget.hasClass('disabled'):
                this._checkin(oTarget);
                break;
            case oTarget.hasClass('out') && !oTarget.hasClass('disabled'):
                this._checkout(oTarget);
                break;
            case oTarget.hasClass('confirm-btn') && !oTarget.hasClass('disabled'):
                this.fire('confirm');
                break;
            case oTarget.hasClass('cancel-btn'):
                this.fire('cancel');
                break;
        }   
    },
    
    /**
     * 鼠标移入/移出事件代理
     * 
     * @method _mouseDelegate  
     * @private
     */    
    _mouseDelegate: function(e) {
        var oTarget = e.currentTarget;
        if(oTarget.hasClass('disabled')) return;
        e.type == 'mouseenter' ? this._mouseenter(oTarget) : this._mouseleave(oTarget);   
    },    
    
    /**
     * 获取指定的日期入住/离开标识
     * 
     * @method _getMark
     * @param {String} v 日期字符串
     * @private
     * @return {String}
     */
    _getMark: function(v) {
        switch(true) {
            case !v:
                return v;
            case this.get('depDate') == v:
                return '\u5165\u4f4f';
            case this.get('endDate') == v:
                return '\u79bb\u5e97';
            default:
                return '';
        }
    },
    
    /**
     * 获取指定的日期房态
     * 
     * @method _getRoomStatus
     * @param {String} v 日期字符串
     * @private
     * @return {Boole}
     */
    _getRoomStatus: function(v) {
        var oData = this.get('data');
        return oData && oData[v] && oData[v].price > 0 && oData[v].roomNum > 0;
    },
    
    /**
     * 获取选择日期范围房态
     * 
     * @method _getRangeRoomStatus
     * @param {String} d1 入住日期字符串
     * @param {String} d2 离店日期字符串
     * @private
     * @return {Boole}
     */
    _getRangeRoomStatus: function(d1, d2) {
        if(!d1 || !d2) return true;
        for(var i = 1, o = null, len = this._getDateDiff(d1, d2); i < len; i++) {
            o = this._boundingBox.one('td[data-date="' + this._getSiblingDate(d1, i) + '"]');
            if(o && o.one('.no-room')) {
                alert('\u60a8\u9009\u62e9\u7684\u65f6\u95f4\u8303\u56f4\u5305\u542b\u65e0\u623f\u7684\u65e5\u671f\uff0c\u8bf7\u91cd\u65b0\u9009\u62e9\uff01');
                return false;
            }
        };
        return true;
    },    
    
    /**
     * 获取指定日期的兄弟日期
     * 
     * @method _getSiblingDate
     * @param {String} v 日期字符串
     * @param {Number} n 整数，支持负数
     * @private
     * @return {String} 新的日期
     */
    _getSiblingDate: function(v, n) {
        v = v.split(REG);
        return this._toStringDate(new Date(v[0], v[1] - 1, v[2] * 1 + n));
    },
    
    /**
     * 获取指定月份的兄弟月份
     * 
     * @method _getSiblingMonth
     * @param {Date} v 日期对象
     * @param {Number} n 整数，支持负数
     * @private
     * @return {Date} 新的日期对象
     */
    _getSiblingMonth: function(v, n) {
        return new Date(v.getFullYear(), v.getMonth() * 1 + n);
    },
    
    /**
     * 获取指定的日期状态
     * 
     * @method _getDateStatus
     * @param {String} v 日期字符串
     * @private
     * @return {Boole}
     */
    _getDateStatus: function(v) {
        return (this.get('minDate') && this._toNumber(v) < this._toNumber(this.get('minDate'))) || 
               (this.get('maxDate') && this._toNumber(v) > this._toNumber(this.get('maxDate')));
    },
    
    /**
     * 获取两个日期间隔天数
     * 
     * @method _getDateDiff
     * @param {String} sDate1 日期字符串1
     * @param {String} sDate2 日期字符串2
     * @private
     * @return {Number}
     */
    _getDateDiff: function(sDate1, sDate2) {
        var oDate1 = +this._toDate(sDate1);
        var oDate2 = +this._toDate(sDate2);
        return parseInt(Math.abs(oDate1 - oDate2) / 24 / 60 / 60 / 1000);
    },
    
    /**
     * 设置房态注释状态
     * 
     * @method _setTips
     * @private
     */
    _setTips: function() {
        this._boundingBox.one('.tips').setStyle('display', this.get('data') ? 'block' : 'none');
        return this;
    },
    
    /**
     * 设置价格日历容器宽度
     * 
     * @method _setWidth
     * @private
     */
    _setWidth: function() {
        (function(that, boundingBox, contentBox) {
            boundingBox.all('.inner').setStyle('width', '100%');
            boundingBox.setStyle('width','100%');
        })(this, this._boundingBox, this._contentBox);
        return this;
    },
    
    /**
     * 设置确定按钮状态
     * 
     * @method _setConfirmStatus
     * @private
     */
    _setConfirmStatus: function() {
        var oConfirm = this._boundingBox.one('.confirm-btn');
        !!this.get('depDate') && !!this.get('endDate') ? oConfirm.removeClass('disabled') : oConfirm.addClass('disabled');
        return this;
    },
    
    /**
     * 设置入住与离店之间的样式
     * 
     * @method _setSelectedRangeStyle
     * @private
     */
    _setSelectedRangeStyle: function() {
        var boundingBox = this._boundingBox,
            curDate     = this.get('depDate'),
            endDate     = this.get('endDate'),
            iDiff       = this._getDateDiff(curDate, endDate),            
            aTd         = boundingBox.all('td'),
            oTd         = null;
        aTd.removeClass('selected-range');
        if(this._toNumber(curDate) > this._toNumber(endDate)) return this;
        for(var i = 0; i < iDiff - 1; i++) {
            curDate = this._getSiblingDate(curDate, 1);
            oTd     = boundingBox.one('td[data-date="' + curDate + '"]');
            oTd && oTd.addClass('selected-range');
        }
        return this;
    },
    
    /**
     * 设置是否显示上下月按钮
     * 
     * @method _setBtnStates
     * @private
     */    
    _setBtnStates: function() {
        var curDate = +this._getSiblingMonth(this.get('date'), 0),
            maxDate = this.get('maxDate'),
            minDate = this.get('minDate'),
            prevBtn = this._boundingBox.one('.prev-btn'),
            nextBtn = this._boundingBox.one('.next-btn');
            if(minDate) {
                minDate = +this._toDate(minDate);
            }
            if(maxDate) {
                maxDate = +this._getSiblingMonth(this._toDate(maxDate), 1 - this.get('count'));
            }
            curDate <= (minDate || Number.MIN_VALUE) ? prevBtn.hide() : prevBtn.show();
            curDate >= (maxDate || Number.MAX_VALUE) ? nextBtn.hide() : nextBtn.show();
            return this;
    },
    
    /**
     * 同排显示的日历中最大的单元格数
     * 
     * @method _maxCell
     * @private
     * @return {Number} 返回最大数
     */
    _maxCell: function() {
        var oDate  = this.get('date'),
            iYear  = oDate.getFullYear(),
            iMonth = oDate.getMonth() + 1,
            aCell  = [];
        for(var i = 0; i < this.get('count'); i++) {
            aCell.push(new Date(iYear, iMonth - 1 + i, 1).getDay() + new Date(iYear, iMonth * 1 + i, 0).getDate());
        }
        return Math.max.apply(null, aCell);
    },
    
    /**
     * 不足两位数的数字补零
     * 
     * @method _filled
     * @param {Number} v 要转换的数字
     * @private
     */    
    _filled: function(v) {
        return v.toString().replace(/^(\d)$/, '0$1');
    },
    
    /**
     * 将日历字符串格式化为数字
     * 
     * @method _toNumber
     * @param {String} v 日期字符串 
     * @private
     */        
    _toNumber: function(v) {
        return v.toString().replace(/-|\//g, '');
    },
    
    /**
     * 将日期对象转为字符串格式
     * 
     * @method _toStringDate
     * @param {Date} v 日期对象
     * @private
     */
    _toStringDate: function(v) {
        return v.getFullYear() + '-' + this._filled(v.getMonth() * 1 + 1) + '-' + this._filled(v.getDate());
    },
    
    /**
     * 将日期字符串转为日期对象
     * 
     * @method _toDate
     * @param {String} v 日期字符串
     * @private
     */
    _toDate: function(v) {
        v = v.split(REG);
        return new Date(v[0], v[1] - 1, v[2]);
    },

    /**
     * 生成日历模板
     * 
     * @method _initCalendarHTML
     * @param {String} date 日期字符串yyyy-mm-dd
     * @private
     * @return {String} 返回价格日历字符串
     */        
    _initCalendarHTML: function() {
        //calendar template object
        var calendar_template                    = {};
            calendar_template['bounding_box_id'] = this._calendarId;          
            calendar_template['date_template']   = this._dateHTML();
            calendar_template['bottom_template'] = Calendar.BOTTOM_TEMPLATE;
        //return Y.Calendar template string    
        return toHTML(Calendar.CALENDAR_TEMPLATE, calendar_template);
    },
    
    /**
     * 生成多日历模板
     * 
     * @method _dateHTML
     * @param {Date} date 日期对象 
     * @private
     * @return {String} 返回双日历模板字符串
     */        
    _dateHTML: function(date) {
        var date   = this.get('date'),
            iYear  = date.getFullYear(),
            iMonth = date.getMonth();
        //calendar date template string
        var date_template = '';
        for(var i = 0; i < this.get('count'); i++) {
            date_template += 
                toHTML(Calendar.DATE_TEMPLATE, this._singleDateHTML(new Date(iYear, iMonth + i)));    
        }
        return date_template;
    },
    
    /**
     * 生成单日历模板
     * 
     * @method _singleDateHTML
     * @param {Date} date 日期对象 
     * @private
     * @return {Object} 返回单个日历模板对象
     */    
    _singleDateHTML: function(date) {
        var iYear     = date.getFullYear(),
            iMonth    = date.getMonth() + 1,
            firstDays = new Date(iYear, iMonth - 1, 1).getDay(),
            monthDays = new Date(iYear, iMonth, 0).getDate(),
            weekdays  = [{wd: '\u65e5', weekend: 'weekend'},
                         {wd: '\u4e00'},
                         {wd: '\u4e8c'},
                         {wd: '\u4e09'},
                         {wd: '\u56db'},
                         {wd: '\u4e94'},
                         {wd: '\u516d', weekend: 'weekend'}];
        //week template string                
        var weekday_template = '';
            each(weekdays, function(v) {
                weekday_template +=
                    toHTML(Calendar.HEAD_TEMPLATE, {weekday_name: v.wd, weekend: v.weekend || ''});
            });
        //tbody template string    
        var body_template = '',
            days_array    = [];
            for(;firstDays--;) days_array.push(0);
            for(var i = 1; i <= monthDays; i++) days_array.push(i);
        days_array.length = this._maxCell();
        var rows  = Math.ceil(days_array.length / 7),
            oData = this.get('data');
        for(var i = 0; i < rows; i++) {
            var calday_row = '';
            for(var j = 0; j <= 6; j++) {
                var days = days_array[j + 7 * i] || '';
                var date = days ? iYear + '-' + this._filled(iMonth) + '-' + this._filled(days) : '';
                calday_row += 
                    toHTML(Calendar.DAY_TEMPLATE,
                        {
                            'day': days,
                            'date': date,
                            'disabled': this._getDateStatus(date) || !days ? 'disabled' : '',
                            'saturday': j == 6 ? ' bor-r-0' : '',
                            'dep_date': (date != '' && this.get('depDate') == date) ? ' dep-date' : '',
                            'end_date': (date != '' && this.get('endDate') == date) ? ' end-date' : '',
                            'price_class': !this._getDateStatus(date) && oData ? this._getRoomStatus(date) ? 'price' : 'no-room' : '',
                            'price': !this._getDateStatus(date) && oData ? this._getRoomStatus(date) ? '¥' + oData[date].price : '\u65e0\u623f' : '',
                            'status_class': this._getRoomStatus(date) ? 'status' : '',
                            'status': this._getRoomStatus(date) ? '\u4f59'+ oData[date].roomNum: '',
                            'pos': this.get('data') ? ' pos' : '',
                            'mark': this._getMark(date)
                        }
                    )  
            }
            body_template +=
                toHTML(Calendar.BODY_TEMPLATE, {calday_row: calday_row})
        }                    
        //table template object                
        var table_template = {};
            //thead string
            table_template['head_template'] = weekday_template;
            //tbody string            
            table_template['body_template'] = body_template;
        //single calendar object
        var single_calendar_template = {};
            single_calendar_template['date'] = iYear + '\u5e74' + iMonth + '\u6708';
            single_calendar_template['table_template'] = toHTML(Calendar.TABLE_TEMPLATE, table_template);
        //return single calendar template object
        return single_calendar_template;
    }
}, 
{
    /**
     * 日历模板
     *
     * @property CALENDAR_TEMPLATE
     * @type {String}
     * @static
     */
    CALENDAR_TEMPLATE: '<div id="{bounding_box_id}" class="price-calendar-bounding-box">' +
                            '<div class="container">' +
                                '<div class="content-box">' +
                                    '<div class="arrow">' +
                                        '<span class="icon iconfont icon-circleLeft prev-btn" title="\u4e0a\u6708"></span>' +
                                        '<span class="icon iconfont icon-circleright next-btn" title="\u4e0b\u6708"></span>' +
                                    '</div>' +                               
                                    '<div class="date-box">' +
                                        '{date_template}' +
                                    '</div>' +
                                '</div>' +
                                '<div class="bottom">' +
                                    '{bottom_template}' +
                                '</div>' +
                            '</div>' +
                        '</div>',
                        
    DATE_TEMPLATE: '<div class="inner">' +
                        '<h4>' +
                            '{date}' +
                        '</h4>' +
                        '{table_template}' +
                    '</div>',
                        
    TABLE_TEMPLATE: '<table>' +    
                        '<thead>' +
                            '<tr>' +
                                '{head_template}' +
                            '</tr>' +
                        '</thead>' +                        
                        '<tbody>' +
                            '{body_template}' +
                        '</tbody>' +                    
                    '</table>',
                    
    HEAD_TEMPLATE: '<th class="{weekend}">{weekday_name}</th>',
    
    BODY_TEMPLATE: '<tr>' +
                        '{calday_row}' +
                    '</tr>',
                    
    DAY_TEMPLATE: '<td data-date="{date}" class="{disabled}{saturday}{dep_date}{end_date}">' +
                        '<p>' +
                            '<b class="date">' +
                                '{day}' +
                            '</b>' +
                            //'<span class="mark{pos}">{mark}</span>' +
                            '<span class="{price_class}">' +
                                '{price}' +
                            '</span>' +
                            '<span class="{status_class}">' +
                                '{status}' +
                            '</span>' +                            
                        '</p>' +
                       //'<p class="select">' +
                       //    '<a href="javascript:;" class="in">\u5165\u4f4f\u65e5\u671f</a>' +
                       //    '<a href="javascript:;" class="out">\u79bb\u5e97\u65e5\u671f</a>' +
                       //'</p>' +
                    '</td>',
                    
    BOTTOM_TEMPLATE: '<p class="tips">' +
                        '<em>' +
                            '<s class="blue"></s>' +
                        '</em>' +
                        '<span>\u4ef7\u683c</span>' +
                        '<em>' +
                            '<s class="green"></s>' +
                        '</em>' +
                        '<span>\u5269\u4f59\u623f\u95f4</span>' +    
                        '<em>' +
                            '<s class="red"></s>' +
                        '</em>' +
                        '<span>\u65e0\u623f</span>' +    
                    '</p>' +
                    '<p class="btns">' +
                        '<input type="button" value="\u786e\u5b9a" class="confirm-btn" />' +
                        '<input type="button" value="\u53d6\u6d88" class="cancel-btn" />' +
                    '</p>',
    /**
     * 日历组件标识
     *
     * @property NAME
     * @type {String}
     * @default 'PriceCalendar'
     * @readOnly
     * @protected
     * @static
     */                     
    NAME: 'PriceCalendar',

    /**
     * 默认属性配置
     *
     * @property ATTRS
     * @type {Object}
     * @protected
     * @static
     */   
    ATTRS: {
        /**
         * 放置日历的容器
         *
         * @attribute container
         * @type {String}
         * @default null
         */
        container: {
            value: null,
            getter: function(v) {
                if(/\,/.test(v)) {
                    v = v.replace(/\s+/g, '');
                    v = v.split(new RegExp('\\s+' + v + '+\\s', 'g'));
                    v = v.join().replace(/^,+|,+$/g, '');
                }
                return v            
            }
        },    
        /**
         * 日历初始日期
         *
         * @attribute date
         * @type {Date|String}
         * @default new Date()
         */
        date: {
            value: new Date(),
            setter: function(v) {
                if(!L.isDate(v)) {
                    v = RDATE.test(v) ? v : new Date();
                }
                return v;
            },            
            getter: function(v) {
                if(L.isDate(v)) {
                    return v;
                }
                else if(L.isString(v)) {
                    v = v.split(REG);
                    return new Date(v[0], v[1] - 1);    
                }
            }
        },
		
        /**
         * 酒店房态数据
         *
         * @attribute data
         * @type {Object}
         * @default null
         */        
        data: {
            value:null
            },
		
        /**
         * 日历个数
         *
         * @attribute count
         * @type {Number}
         * @default 2
         */         
        count: {
            value: 1
        },
		
        /**
         * 允许操作的最小日期
         *
         * @attribute minDate
         * @type {Date|String}
         * @default null
         */          
        minDate: {
            value: null,
            setter: function(v) {
                if(L.isDate(v)) {
                    v = this._toStringDate(v);
                }
                return RDATE.test(v) ? v : null;
            },
            getter: function(v) {
                if(L.isString(v)) {
                    v = v.split(REG);
                    v = v[0] + '-' + this._filled(v[1]) + '-' + this._filled(v[2]);
                }
                return v || '';
            }
        },
		
        /**
         * 允许操作的最大日期
         *
         * @attribute maxDate
         * @type {Date|String}
         * @default null
         */         
        maxDate: {
            value: null,
            setter: function(v) {
                if(L.isDate(v)) {
                    v = this._toStringDate(v);
                }
                return RDATE.test(v) ? v : null;
            },
            getter: function(v) {
                if(L.isString(v)) {
                    v = v.split(REG);
                    v = v[0] + '-' + this._filled(v[1]) + '-' + this._filled(v[2]);
                }
                else if(this.get('afterDays')) {
                    var oDate = this.get('minDate').split(REG);
                    v = new Date(oDate[0], oDate[1] - 1, oDate[2] * 1 + this.get('afterDays') * 1 - 1);
                    v = this._toStringDate(v);
                }			
                return v || '';
            }
        },
		
        /**
         * 入住时间
         *
         * @attribute depDate
         * @type {String}
         * @default ''
         */           
        depDate: {
            value: ''    
        },
		
        /**
         * 离店时间
         *
         * @attribute endDate
         * @type {String}
         * @default ''
         */         
        endDate: {
            value: ''
        },
		
        /**
         * 等价于设置minDate和maxDate，minDate未设置时取当前日期
         *
         * @attribute afterDays
         * @type {Number}
         * @default 0
         */         
        afterDays: {
            value: 0,
            getter: function(v) {
                v && (this.get('minDate') || this.set('minDate', new Date()));
                return v;
            }
        }
    }
});

}, '1.0', {requires: ['node', 'base-base', 'event-mouseenter']});