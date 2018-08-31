
var ls=JSON.parse(localStorage.getItem("value"))||[];
$(function(){
    //储存
    $('#inputWord').bind('search', function () {
        var val=$(this).val();
        if(val!==""){
            ls.push(val);
            noTrace();
            localStorage.setItem("value",JSON.stringify(ls));
            $('#inputWord').val('');
        }else{
            return false;
        }
    });
    for(var i in ls){
        $(".search-content .search-keywords .history-list").append('<div class="word-record"><i class="icon iconfont icon-his"></i>'+ls[i]+'<i class="icon iconfont icon-guanbi"></i></div>');
        $(".search-content .search-keywords .no-record").html("<i class='icon iconfont icon-del'></i>清空搜索记录");
    }

    //清空
    $('#inputWord').on('input propertychange', function () {
        var val=$(this).val();
        if(val!==""){
         $("#searchForm span").show();
        }else{
            $("#searchForm  span").hide();
        }
    });

    $("#searchForm span").on("click",function(){
        $('#inputWord').val("");
        $(this).hide();
    });

    //热门
    $(".search-content  .search-keywords .hot-word a").click(function(){
       var txt=$(this).text();
        $('#inputWord').val(txt);
        $('#inputWord').focus();
    });

   //清除记录
    $(".search-keywords .search-history .no-record").click(function(){
        $(".search-content  .history-list .word-record").remove();
        localStorage.removeItem("value");
        ls=[];
        $(".search-content .search-keywords .no-record").html("暂无搜索记录");
    });
    $(".search-content  .history-list .icon-guanbi").click(function(){
        var par=$(this).parent();
        var ind=$(".search-content  .history-list .word-record").index(par);

        var len=$(".search-content  .history-list .word-record").length-1;

        par.remove();
        if(len==0){
            $(".search-content .search-keywords .no-record").html("暂无搜索记录");
        }
        ls.splice(ind,1);
        localStorage.setItem("value",JSON.stringify(ls));
    });
});

//safari
function noTrace(){
    if (typeof localStorage === 'object') {
        try {
            localStorage.setItem('localStorage', 1);
            localStorage.removeItem('localStorage');
        } catch (e) {
            Storage.prototype._setItem = Storage.prototype.setItem;
            Storage.prototype.setItem = function() {};
            alert('请关闭浏览器右下角的无痕模式（Private Browsing Mode）');
        }
    }
}

