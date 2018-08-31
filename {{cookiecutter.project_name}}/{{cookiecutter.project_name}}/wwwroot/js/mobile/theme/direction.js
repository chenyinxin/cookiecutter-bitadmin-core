var slideDown = function (option) {
    var start,
        end,
        length,
        isTouchPad = (/hp-tablet/gi).test(navigator.appVersion),
        hasTouch = 'ontouchstart' in window && !isTouchPad;
    var obj = document.querySelector(option.container);
    var offset = option.offset;

    var fn =
    {
        //移动
        translate: function (diff) {
            obj.style.webkitTransform = 'translate3d(0,' + diff + 'px,0)';
            obj.style.transform = 'translate3d(0,' + diff + 'px,0)';
        },
        //时间
        setTransition: function (time) {
            obj.style.webkitTransition = 'all ' + time + 's';
            obj.style.transition = 'all ' + time + 's';
        },
        //返回
        back: function () {
            fn.translate(0 - offset);
        },
        //顶部
        isTop: function () {
            return (document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop) == 0
        },
        addEvent: function (element, event_name, event_fn) {
            if (element.addEventListener) {
                element.addEventListener(event_name, event_fn, false);
            } else if (element.attachEvent) {
                element.attachEvent('on' + event_name, event_fn);
            } else {
                element['on' + event_name] = event_fn;
            }
        }
    };

    fn.back();
    fn.addEvent(obj, 'touchstart', start);
    fn.addEvent(obj, 'touchmove', move);
    fn.addEvent(obj, 'touchend', end);

    //开始
    function start(e) {
        var even = typeof event == "undefined" ? e : event;
        start = hasTouch ? even.touches[0].pageY : even.pageY;
        end=null;
    }

    //中
    function move(e) {
        var even = typeof event == "undefined" ? e : event;
        end = hasTouch ? even.touches[0].pageY : even.pageY;
        if (start && end && start < end && fn.isTop()) {
            even.preventDefault();
            fn.setTransition(0);
            //移动
            if (end - start <= 150) {
                length = (end - start)/3;
                fn.translate(-23+length);
                if (end - start>= offset*2) {
                    $("#pullDown .pull-tips").text('松手加载上一页');
                }else{
                    $("#pullDown .pull-tips").text('下拉首页');
                }
            }
            else {
                length += 0.3;
                fn.translate(length);
            }
        }
    }

    //结束
    function end(e) {
        if (start && end && end - start >= offset*2) {
            if(fn.isTop()) {
                if (typeof option.next == "function") {
                    option.next.call(fn, e);
                }
            }
        } else {
            //返回
            fn.back();
        }
        start=end=null;
    }

    return fn;
};

var slideUp = function (option) {
    var start,
        end,
        length,
        isTouchPad = (/hp-tablet/gi).test(navigator.appVersion),
        hasTouch = 'ontouchstart' in window && !isTouchPad;
    var obj = document.querySelector(option.container);
    var offset = option.offset;
    var u = navigator.userAgent;
    var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1;

    var fn =
    {
        //移动
        translate: function (diff) {
            obj.style.webkitTransform = 'translate3d(0,' + diff + 'px,0)';
            obj.style.transform = 'translate3d(0,' + diff + 'px,0)';
        },
        //时间
        setTransition: function (time) {
            obj.style.webkitTransition = 'all ' + time + 's';
            obj.style.transition = 'all ' + time + 's';
        },
        //返回
        back: function () {
            fn.translate(0);
        },
        isBottom: function () {
            return (document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop) + $(window).height() >= $(obj).height();
        },
        addEvent: function (element, event_name, event_fn) {
            if (element.addEventListener) {
                element.addEventListener(event_name, event_fn, false);
            } else if (element.attachEvent) {
                element.attachEvent('on' + event_name, event_fn);
            } else {
                element['on' + event_name] = event_fn;
            }
        }
    };

    fn.back();
    fn.addEvent(obj, 'touchstart', start);
    fn.addEvent(obj, 'touchmove', move);
    fn.addEvent(obj, 'touchend', end);

    //开始
    function start(e) {
        var even = typeof event == "undefined" ? e : event;
        start = hasTouch ? even.touches[0].pageY : even.pageY;
        end=null
    }

    //中
    function move(e) {
        var even = typeof event == "undefined" ? e : event;
        end = hasTouch ? even.touches[0].pageY : even.pageY;
        if (start && end && start > end && fn.isBottom()) {
            even.preventDefault();
            fn.setTransition(0);
            //移动
            if (start - end <= 150) {
                length = (end - start) / 3;
                fn.translate(length);
                if(!isAndroid){
                    if (start - end >= offset*1.3) {
                        $("#pullup .pull-tips").text('松手加载下一页');
                    }else{
                        $("#pullup .pull-tips").text('上拉更多');
                    }
                }else{
                    if (start - end >= offset*1.5) {
                        $("#pullup .pull-tips").text('松手加载下一页');
                    }else{
                        $("#pullup .pull-tips").text('上拉更多');
                    }
                }}
            else {
                length -= 0.3;
                fn.translate(length);
            }
        }
    }

    //结束
    function end(e) {
        if (start && end && start - end >= offset*1.3) {
            if(fn.isBottom()) {
                //回调
                if (typeof option.next == "function") {
                    option.next.call(fn, e);
                }
            }
        } else {
            //返回
            fn.back();
        }
        start=end=null;
    }

    return fn;
};

var downFn,upFn;

$(function () {
    downFn = slideDown({
        container: "#downWrapper",
        offset: 50,
        next: function (e) {
            $("#downWrapper").hide();
            $("#upWrapper").show();
            $("#tabBtn").hide();
            document.documentElement.scrollTop = window.pageYOffset = document.body.scrollTop = 0;
            upFn.back.call();
        }
    });

    upFn = slideUp({
        container: "#upWrapper",
        offset: 50,
        next: function (e) {
            $("#downWrapper").show();
            $("#upWrapper").hide();
            $("#tabBtn").show();
            marLeft();
            document.documentElement.scrollTop = window.pageYOffset = document.body.scrollTop = 0;
            downFn.back.call();
        }
    });
});






