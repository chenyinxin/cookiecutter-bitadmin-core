$(function(){
    var tmpl = '<li class="weui-uploader__file" style="background-image:url(#url#)"></li>',
        $gallery = $("#gallery"), $galleryImg = $("#galleryImg"),
        $uploaderInput = $("#uploaderInput"),
        $uploaderFiles = $("#uploaderFiles"),
        srcArr=[];

    $uploaderInput.on("change", function(e){
        var src, url = window.URL || window.webkitURL || window.mozURL, files = e.target.files;



        for (var i = 0, len = files.length; i < len; ++i) {
            var file = files[i];

            if (url) {
                src = url.createObjectURL(file);
            } else {
                src = e.target.result;
            }
            srcArr.push(src)
            $uploaderFiles.append($(tmpl.replace('#url#', src)));
        }
        alert(srcArr)
    });

    //图片预览
    $uploaderFiles.on("click", "li", function(){
        var len=$uploaderFiles.find("li").length;
        var ind=$("#uploaderFiles li").index(this)+1;
        $galleryImg.attr("style", this.getAttribute("style"));
        $gallery.fadeIn(100);
        $(".comment-content #gallery .img-all").text(len);
        $(".comment-content #gallery .img-num").text(ind);
    });
    $gallery.on("click",".weui-back", function(){
        $gallery.fadeOut(100);
    });

    //图片预览操作
    $gallery.on("click",".weui-gallery__del", function(){
        $(".delDialog").show();
        $(".delDialog .line-del").click(function(){

            $(".delDialog").hide();
            $gallery.fadeOut(100);
            var ind=$(".comment-content #gallery .img-num").text()-1;
            $("#uploaderFiles li").eq(ind).remove();
        });

        $(".delDialog .cancel-del").click(function(){
            $(".delDialog").hide();
        })
    });
});
