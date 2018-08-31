
function slideDel(){
    $(".line-scroll-wrapper").width($(".line-wrapper").width() + $(".line-btn-delete").width());

    $(".line-normal-wrapper").width($(".line-wrapper").width());

    // 设置监听
    var lines = $(".line-normal-wrapper");
    var len = lines.length;
    var lastX, lastXForMobile;


    var pressedObj;
    var lastLeftObj;
    var start;

    for (var i = 0; i < len; ++i) {
        lines[i].addEventListener('touchstart', function(e){
            lastXForMobile = e.changedTouches[0].pageX;
            pressedObj = this;
            var marleft=parseFloat($(pressedObj).css("margin-left"));
            if(marleft<0){
                $(pressedObj).animate({marginLeft:"0"}, 300);
            }

            start = {
                x: touches.pageX,
                y: touches.pageY
            };
        });

        lines[i].addEventListener('touchmove',function(e){
            var touches = event.touches[0];
            delta = {
                x: touches.pageX - start.x,
                y: touches.pageY - start.y
            };

            if (Math.abs(delta.x) > Math.abs(delta.y)) {
                event.preventDefault();
            }
        });

        //滑动结束按钮显示
        lines[i].addEventListener('touchend', function(e){
            if (lastLeftObj && pressedObj != lastLeftObj) {
                $(lastLeftObj).animate({marginLeft:"0"}, 300);
                lastLeftObj = null;
            }
            var diffX = e.changedTouches[0].pageX - lastXForMobile;
            if (diffX < -10) {
                $(".line-wrapper .line-btn-delete").show();
                $(pressedObj).animate({marginLeft:"-90px"}, 300);
                lastLeftObj && lastLeftObj != pressedObj &&
                $(lastLeftObj).animate({marginLeft:"0"}, 300);
                lastLeftObj = pressedObj;


                //删除提示
                $(".line-wrapper .line-btn-delete").click(function(){
                    var Id=$(this).parent().parent().attr("id");
                    $("#dialog").attr("data-role",Id);
                    $("#dialog").show();
                });

                $("#dialog .line-del").click(function(){
                    var dr= $("#dialog").attr("data-role");
                    $("#"+dr).remove();
                    $("#dialog").hide();
                });

                $("#dialog .cancel-del").click(function(){
                    $(".delDialog").hide();
                    $(lastLeftObj).animate({marginLeft:"0"}, 300);
                })
            } else if (diffX > 10) {
                if (pressedObj == lastLeftObj) {
                    $(pressedObj).animate({marginLeft:"0"}, 300);
                    lastLeftObj = null;
                }
            }
        });
    }
}