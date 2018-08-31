//价格日历渲染
var config = {
    modules: {
        'price-calendar': {
            fullpath: 'src/js/common/calendar/price-calendar.js',
            type    : 'js',
            requires: ['price-calendar-css']
        },
        'price-calendar-css': {
            fullpath: 'src/css/theme/price-calendar.css',
            type    : 'css'
        }
    }
};
YUI(config).use('price-calendar', function(Y) {
    var oCal = new Y.PriceCalendar(),ls=[],showData={};
    function calrender(){
        var td=Y.all(".price-calendar-bounding-box table td");
        for(var i=0;i<td.size();i++){
            var date=td._nodes[i].getAttribute("data-date");
            if(date!==""){
                ls.push(date);
            }
        }
        for(var j in ls){
            showData[ls[j]]={"price"  : "5000", "roomNum": "10"}
        }
        oCal.set('data',showData);
    }
    calrender();
    oCal.on('nextmonth', function(e) {
        calrender();
    });
    oCal.on('prevmonth', function(e) {
        calrender();
    });


    Y.one('.price-calendar-bounding-box').delegate('click', function(e) {
        var value= e.currentTarget.ancestor('td').getAttribute('data-date');
        Y.all('.price-calendar-bounding-box td p').removeClass("pr-show");
        Y.all('.price-calendar-bounding-box td p b').setStyle("color","#000");
        Y.all('.price-calendar-bounding-box td p span').setStyle("display","none");
        e.currentTarget.addClass("pr-show");
        e.currentTarget.one("b").setStyle("color","#fff");
        e.currentTarget.all("span").setStyle("display","block");
        Y.one(".cal-select .sel-date").setHTML(value)
    }, 'p', oCal);
});
