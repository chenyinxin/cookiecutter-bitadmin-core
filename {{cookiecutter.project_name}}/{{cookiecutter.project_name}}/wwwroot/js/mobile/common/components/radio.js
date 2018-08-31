/*
* @Author: sison.luo
* @Date:   2016-03-16 14:14:05
* @Last Modified by:   sison.luo
* @Last Modified time: 2016-06-16 15:19:07

	*radio*
*/

;(function($,window,undefined){
	$.fn.radioSmart = function(options){
		var defaults = {
			callback: function(o,v){}
		}
		var opts = $.extend({},defaults,options);
		return this.each(function(){
			var radiobj = new Radiobj($(this),opts);
			radiobj.init();
		})
	}
	var Radiobj = function(ele,options){
		this.ele = ele,
		this.opts = options;
		this.id = this.ele.attr('id');
		this.name = this.ele.attr('name');
	}
	Radiobj.prototype.init = function(){
		this.render();
		this.watch();
	}
	Radiobj.prototype.render = function(){
		var self = this;
		var html = '';
		// var _nid = '_'+self.id;
		var ischk = self.ele.prop('checked') ? ' checked' : '';
		// var _nval = self.ele.attr('show-value');
		// html+='<div class="radione clearfix" id='+_nid+'>';
		html+='<i class="ant"></i>';
		// html+='<span>'+_nval+'</span>';
		// html+='</div>';
		self.ele.before(html);
		self.ele.parent().addClass(ischk);
		return this;
	}
	Radiobj.prototype.watch = function(){
		var self = this;
		var actor  = self.ele.parent('.radione');
		actor.on('click',function(e){
			$(this).addClass('checked').siblings('.radione').removeClass('checked');
			$(this).find(".ant").addClass("icon iconfont icon-dagou");
			$(this).siblings().find(".ant").removeClass("icon iconfont icon-dagou");
			self.ele.prop('checked', true);
			$(this).siblings('.radione').find('input[type=radio]').prop('checked', false);
			var v = self.ele.val();
			self.opts.callback(self.ele,v);
		});
		return this;
	}
})(jQuery,window,document);
