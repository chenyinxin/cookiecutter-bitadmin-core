//点击空白处隐藏
function share(){
    $(".top .icon-more").click(function(){

        $(".top .more-box").toggle();
        return false;
    });

    $('.content,.detail-top').bind("click",function(){
        var _con =  $(".top .more-box");
            if(!_con.is(event.target) && _con.has(event.target).length === 0){
                $(".top .more-box").hide();
            }
    });

    $(window).scroll(function(){
        var top=$(window).scrollTop();
        if(top>0){
            $(".top .more-box").hide();
        }
    })
}

function skipReact(){
    $("#list1View .list1-detail li,#list>div li,#list .list3-imgTop>div").click(function(){
      window.location.href="news.html";
    });
}

//底部菜单切换
function bottomTab(){
   $("#footer li").click(function(){
      $(this).find("i").css("background-position","0 -25px");
      $(this).find("span").css("color","#3eaeee");
      $(this).siblings().find("i").css("background-position","0 0")
      $(this).siblings().find("span").css("color","#999")
   }).eq(0).click();
}


function listAuto(){

    $(".list3-content .list3-imgTop>div:even").css("margin-right","2%");
    $("#list2 .list2img-show .no-mgright").css("margin-right","0");
}

function marLeft(){
    var ft=$(".detail-tab  .tab1-classify2 .classify2-top");
    var sw=ft.find(".classify2-tl").outerWidth()+parseFloat(ft.find(".classify2-tm").css("margin-right"));
    $(".detail-tab  .tab1-classify2 .classify2-bot").css("margin-left",sw);
}

//首页广告滑动
function adverSlide(){
    var simg=$("#slideAdv .img-list img"),ist= document.getElementById("imgList"),dot=$(".index-list"),len=dot.find("li").length-1;
    var stx,start,ind=1;
    $("#slideAdv").height(simg.height());
      var roll=function(){
          $("#imgList").animate({marginLeft:"-100%"},1500,"linear",function(){
              $("#imgList").css({marginLeft:"0px"});
              dot.find("li").eq(ind).find("i").addClass("blue-dot")
              .parent().siblings().find("i").removeClass("blue-dot");
              if(ind<len){
                  ind++;
              }else{
                  ind=0;
              }
              $("#imgList a:first").remove().clone(true).appendTo("#imgList");
          })
      };
    var startRoll=setInterval(roll,2000);
   ist.addEventListener('touchstart',function(e){
       clearInterval(startRoll);
        stx = e.changedTouches[0].pageX;
       var touches = event.touches[0];
       start = {
           x: touches.pageX,
           y: touches.pageY
       };
    });
    ist.addEventListener('touchmove',function(e){
        var touches = event.touches[0];
        delta = {
            x: touches.pageX - start.x,
            y: touches.pageY - start.y
        };
        if (Math.abs(delta.x) > Math.abs(delta.y)) {
            event.preventDefault();
        }
    });
   ist.addEventListener('touchend',function(e){
       startRoll=setInterval(roll,2000);
        var diffX = e.changedTouches[0].pageX - stx;
        if(diffX<0){
            roll();
        }else{
            $("#imgList").animate({marginLeft:"0"}, 500);
        }
    })
}

//顶部菜单切换
function tab(){
    $(" #tabBtn li").click(function(){
        var ind=$(" #tabBtn li").index(this);
        $(this).addClass("tab-select").siblings().removeClass("tab-select");
        $("#detailTab .tab-content>div").eq(ind).show().addClass("fadeInRight").siblings().hide().removeClass("fadeInRight");
    }).eq(0).click();
}

//查看更多
function showMore(){
    $("#tab1 .tab1-content .check-more").click(function(){
        if($("#tab1 .tab1-content .tab1-classify1").hasClass("more-show")){
            $(this).find("span").text("查看全部");
            $(this).css({"position":"absolute","bottom":"-3px"});
            $(this).find("i").removeClass("icon-up").addClass("icon-down");
        }else{
            $(this).prev().css("margin-bottom","55px;");
            $(this).find("span").text("收起");
            $(this).css("position","static");
            $(this).find("i").addClass("icon-up").removeClass("icon-down");
        }
        $("#tab1 .tab1-content .tab1-classify1").toggleClass("more-show");
    });
}

//表单验证原文本覆盖
function valHide(){
    $(".date-select .weui-input").on("input propertychange",function(){
        var val=$(".date-select .weui-input").val();
        if(val==""){
            $("#addForm .date-select .date-tips").text("请输入您的出生年月");
            $(".date-select .weui-input").css("opacity","0");
        }else{
            $("#addForm .date-select .date-tips").text("");
            $(".date-select .weui-input").css("opacity","1");
        }
    });
}

//面包屑提示
function addSubmit(){
   $(".hide-show").height($(window).height());
    $("#checkAdd").click(function(){
        var check=$(".line-wrapper").find(".checked");
        if(check.length>0){
            $("#toast").fadeIn(500);
            setTimeout(function(){$("#toast").fadeOut(1000)},1500);
        }else{
            $("#dialog2").show();
            $("#dialog2 .weui-dialog__ft").click(function(){
                $("#dialog2").hide();
            })
        }
    });

    $(".content .add-line").click(function(){
        $(this).hide();
        $(".top .back").attr("href","delete.html");
        $(".top .top-title").text("");
        $(".top #checkAdd").text("");
        $("#addSuccess").show();
        $("#lineList").hide();
    })

}

//提示页弹出框
function popTips(){
    $("#diaBtn").click(function(){
        $("#dialog1").show();
    });
    $("#diaBtn2").click(function(){
        $("#dialog2").show();
    });

    $("#msgBtn").click(function(){
        $("#succPage").show();
        $(".demo-btn").hide();
    });
    $("#msgBtn2").click(function(){
        $("#failPage").show();
        $(".demo-btn").hide();
    });
    $("#toaBtn").click(function(){
        $("#toast1").fadeIn(500);
        setTimeout(function(){$("#toast1").fadeOut(1000)},1500);
    });
    $("#toaBtn2").click(function(){
        $("#loadingToast").fadeIn(500);
        setTimeout(function(){$("#loadingToast").fadeOut(1000)},1500);
    });
    $(".msg-page .msg-hide").click(function(){
        $(this).parents(".pop-page").hide();
        $(".demo-btn").show();
    });
    $(".box-close").click(function(){
        $(this).parents(".pop-box").hide();
    });
    $(".box-confirm").click(function(){
        alert("操作确认");
        $(this).parents(".pop-box").hide();
    });
}

//价格日历显示
function calShow(){

    $("#upWrapper .letter-calendar .calendar-skip").click(function() {
        $(window).scrollTop("0");
        $('.price-calendar-bounding-box').show();
        $("#scrollContent,.detail-top").hide();
        if($('.price-calendar-bounding-box .top').length==0 && $('.price-calendar-bounding-box .price-cal').length==0 ){
            $('.price-calendar-bounding-box').prepend('<div class="top"><a href="#" class="back"><i class="icon iconfont icon-ttpodicon"></i>返回</a>'+
                '<span class="top-title nomore">出发日期</span></div>');

            $('.price-calendar-bounding-box').append('<div class="price-cal">'+
                '<div class="cal-select"><span>已选日期：</span><span class="sel-date">2016-12-20</span></div>'+
                '<div class="count-select">'+
                '<div><span>数量：</span><a><b class="minus">-</b><span>1</span><b class="plus">+</b></a></div>'+
                '<div class="room-select"><span>房间数：</span><a><b class="minus">-</b><span>1</span><b class="plus">+</b></a></div>'+
                '</div></div>');
        }

        $(".price-calendar-bounding-box .price-cal .count-select .plus").unbind('click').click(function(){
            var i=1;
            var txt=parseFloat($(this).siblings("span").text());
            $(this).siblings("span").text(txt+i);
            $(this).siblings("b").css("background-color","#3eaeee");
        });

        $(".price-calendar-bounding-box .price-cal .count-select .minus").unbind('click').click(function(){
            var i=1;
            var txt=parseFloat($(this).siblings("span").text());
            if(txt<=2 && txt>1){
                $(this).css("background-color","#999");
                $(this).siblings("span").text(txt-i);
            }else if(txt<=1){
                $(this).siblings("span").text("1");
                return false;
            }else{
                $(this).siblings("span").text(txt-i);
            }
        });

        $(".price-calendar-bounding-box .top .back").click(function(){
            $('.price-calendar-bounding-box').hide();
            $("#scrollContent,.detail-top").show();
            $(".price-calendar-bounding-box .price-cal .count-select span").text('1');
            $(".price-calendar-bounding-box .price-cal .count-select .minus").css("background-color","#999");
        })
    })
}


function goto(){
    $("#upWrapper .letter-calendar .letterIndex-skip").click(function() {
        window.location.href="letterIndex.html";
    });

    $("#selectWrapper .top .back").click(function() {
        window.location.href="detail.html";
    });
}




