/*
* @Author: sison.luo
* @Date:   2016-03-16 14:14:05
* @Last Modified by:   Administrator
* @Last Modified time: 2016-04-17 18:35:47

	*开关组件*
*/

;(function($,window,undefined){
	$.fn.switchSmart = function(options){
		var defaults = {
			leftName : '启用',
			rightName : '禁用',
			enable : function(){},
			disable : function(){}
		}
		var opts = $.extend({},defaults,options);
		return this.each(function(){
			var switcher = new Switcher($(this),opts);
			switcher.init();
		})
	}
	var Switcher = function(ele,options){
		this.ele = ele,
		this.opts = options;
		this.id = this.ele.attr('id');
	}
	Switcher.prototype.init = function(){
		this.render();
		this.watch();
	}
	Switcher.prototype.render = function(){
		var self = this;
		var _html = '';
		var nid = '_'+self.id;
		var align = self.ele.data('align') || 'left';
		var state = self.ele.val() == 0 ? 'off' : 'on';
		_html+='<div class="switch-actor '+state+'" id="'+nid+'">';
		_html+='<div class="switch-mover ant">';
		_html+='<span class="switch-left">'+self.opts.leftName+'</span>';
		_html+='<span class="switch-dot"></span>';
		_html+='<span class="switch-right">'+self.opts.rightName+'</span>';
		_html+='</div>';
		_html+='</div>';
		self.ele.after(_html);
		var actor  = $('#' + nid);
		switch(align){
			case 'center':
				actor.css('margin','0 auto');
				break;
			case 'right':
				actor.css('float','right');
				break;
		}
		return this;
	}
	Switcher.prototype.watch = function(){
		var self = this;
		var nid = '_' + self.id;
		var actor  = $('#' + nid);
		actor.on('click',function(){
			if($(this).hasClass('on')){
				$(this).removeClass('on').addClass('off');
				self.ele.val(0);
				self.opts.disable(self.ele);
			}else{
				$(this).removeClass('off').addClass('on');
				self.ele.val(1);
				self.opts.enable(self.ele);
			}
		});
		return this;
	}
})(jQuery,window,document);
