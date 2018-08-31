       var toph=120;
       var windowHeight=$(window).height();
       var InitHeight=windowHeight-toph;
       var ls = [];
       var arr=[];

       function citySelect(){
           letterIndex();
           $(window).scroll(function(){
               letterScroll();
           });
       }
       //滚动渲染
      function letterScroll(){
              var t= document.documentElement.scrollTop || document.body.scrollTop;
              var sort_letter = $(".sort_letter");
              var ll = $(".initials ul li");
              sort_letter.each(function(){
                  if($(this).offset().top-toph<=t && t>0){
                      ls.push($(this).attr("id"));
                      $("#"+ls[ls.length-1]).css({"position":"fixed","top":toph,"width":"100%","z-index":"2"});
                      $("#"+ls[ls.length-1]).siblings(".sort_letter").css("position","static");
                  }else if(t==0){
                      $(".sort_letter").css("position","static");
                  }
              });
              ll.each(function(){
                  if($(this).html()==ls[ls.length-1]){
                      $(this).addClass("active").siblings().removeClass("active");
                  }
              })
        }

       //初始化
       function letterIndex(){
           var Initials=$('.initials');
           var LetterBox=$('#letter');

           Initials.find('ul').append('<li>A</li><li>B</li><li>C</li><li>D</li><li>E</li><li>F</li><li>G</li><li>H</li><li>I</li><li>J</li><li>K</li><li>L</li><li>M</li><li>N</li><li>O</li><li>P</li><li>Q</li><li>R</li><li>S</li><li>T</li><li>U</li><li>V</li><li>W</li><li>X</li><li>Y</li><li>Z</li><li>#</li>');
           initials();

           //检测高度变化
           $(".select-wrapper .sort_box .num_name").click(function(){
               var txt=$(this).text();
                  var rt= arr.indexOf(txt);
                   if(rt>=0){
                       return false;
                   }else{
                       arr.push(txt);
                       $(".select-content .groom").append('<span>'+arr[arr.length-1]+'<i class="select-cancel">×</i></span>');
                   }
               $('.select-wrapper .select-recommend span .select-cancel').click(function(){
                   var par=$(this).parent();
                   var txt=$(this).parent().text().replace("×","");
                   par.remove();
                  for(var j=0;j<arr.length;j++){
                     if(arr[j]==txt){
                         arr.splice(j,1);
                         console.log(arr)
                     }
                 }
               });

               toph=$(".select-wrapper .top").height()+$(".select-wrapper .select-recommend").height();
               InitHeight=windowHeight-toph;
               Initials.height(InitHeight);
               var LiHeight=InitHeight/27;
               Initials.find('li').height(LiHeight);
               Initials.find('li').css("line-height",LiHeight+"px");

               $(".select-wrapper .select-all").css("margin-top", toph);
               $(".select-wrapper .initials").css("top", toph);
           });

           //字母点击
           $(".initials ul li").click(function(){
               var _this=$(this);
               var LetterHtml=_this.html();
               LetterBox.html(LetterHtml).fadeIn();
               _this.addClass("ind").siblings().removeClass("ind");
               _this.css("line-height",($(this).height()+"px"));
               //Initials.css('background','rgba(145,145,145,0.6)');

               setTimeout(function(){
                   Initials.css('background','rgba(145,145,145,0)');
                   LetterBox.fadeOut();
               },1000);

               var _index = _this.index();
               if(_index==0){
                   $('html,body').animate({scrollTop: '0px'}, 300);//点击第一个滚到顶部
               }else if(_index==27){
                   var DefaultTop=$('#default').position().top;
                   $('html,body').animate({scrollTop: DefaultTop+'px'}, 300);//点击最后一个滚到#号
               }else{
                   var letter = _this.text();
                   if($('#'+letter).length>0){
                       var LetterTop = $('#'+letter).position().top;
                       $('html,body').animate({scrollTop: LetterTop-45+'px'}, 300);
                   }
               }
           })

           Initials.height(InitHeight);
           var LiHeight=InitHeight/27;
           Initials.find('li').height(LiHeight);
           Initials.find('li').css("line-height",LiHeight+"px");
           $(".select-wrapper .select-all").css("margin-top", toph+1);
           $(".select-wrapper .initials").css("top", toph);

           //按首字母排序
           function initials() {
               var SortList=$(".sort_list");
               var SortBox=$(".sort_box");
               SortList.sort(asc_sort).appendTo('.sort_box');
               function asc_sort(a, b) {
                   return makePy($(b).find('.num_name').text().charAt(0))[0].toUpperCase() < makePy($(a).find('.num_name').text().charAt(0))[0].toUpperCase() ? 1 : -1;
               }

               var initials = [];
               var num=0;
               SortList.each(function(i) {
                   var initial = makePy($(this).find('.num_name').text().charAt(0))[0].toUpperCase();
                   if(initial>='A'&&initial<='Z'){
                       if (initials.indexOf(initial) === -1)
                           initials.push(initial);
                   }else{
                       num++;
                   }

               });

               $.each(initials, function(index, value) {
                   SortBox.append('<div class="sort_letter" id="'+ value +'">' + value + '</div>');
               });
               if(num!=0){SortBox.append('<div class="sort_letter" id="default">#</div>');}

               //插入到对应的首字母后面
               for (var i =0;i<SortList.length;i++) {
                   var letter=makePy(SortList.eq(i).find('.num_name').text().charAt(0))[0].toUpperCase();
                   switch(letter){
                       case "A":
                           $('#A').after(SortList.eq(i));
                           break;
                       case "B":
                           $('#B').after(SortList.eq(i));
                           break;
                       case "C":
                           $('#C').after(SortList.eq(i));
                           break;
                       case "D":
                           $('#D').after(SortList.eq(i));
                           break;
                       case "E":
                           $('#E').after(SortList.eq(i));
                           break;
                       case "F":
                           $('#F').after(SortList.eq(i));
                           break;
                       case "G":
                           $('#G').after(SortList.eq(i));
                           break;
                       case "H":
                           $('#H').after(SortList.eq(i));
                           break;
                       case "I":
                           $('#I').after(SortList.eq(i));
                           break;
                       case "J":
                           $('#J').after(SortList.eq(i));
                           break;
                       case "K":
                           $('#K').after(SortList.eq(i));
                           break;
                       case "L":
                           $('#L').after(SortList.eq(i));
                           break;
                       case "M":
                           $('#M').after(SortList.eq(i));
                           break;
                       case "N":
                           $('#N').after(SortList.eq(i));
                           break;
                       case "O":
                           $('#O').after(SortList.eq(i));
                           break;
                       case "P":
                           $('#P').after(SortList.eq(i));
                           break;
                       case "Q":
                           $('#Q').after(SortList.eq(i));
                           break;
                       case "R":
                           $('#R').after(SortList.eq(i));
                           break;
                       case "S":
                           $('#S').after(SortList.eq(i));
                           break;
                       case "T":
                           $('#T').after(SortList.eq(i));
                           break;
                       case "U":
                           $('#U').after(SortList.eq(i));
                           break;
                       case "V":
                           $('#V').after(SortList.eq(i));
                           break;
                       case "W":
                           $('#W').after(SortList.eq(i));
                           break;
                       case "X":
                           $('#X').after(SortList.eq(i));
                           break;
                       case "Y":
                           $('#Y').after(SortList.eq(i));
                           break;
                       case "Z":
                           $('#Z').after(SortList.eq(i));
                           break;
                       default:
                           $('#default').after(SortList.eq(i));
                           break;
                   }
               };
           }
       }