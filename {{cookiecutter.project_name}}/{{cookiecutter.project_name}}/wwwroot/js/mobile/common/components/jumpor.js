/*
* @Author: sison.luo
* @Date:   2016-03-21 11:17:47
* @Last Modified by:   sison.luo
* @Last Modified time: 2016-03-21 11:48:42
*/

// 'use strict';


;(function($,window,undefined){

	$.fn.jumporSmart = function(options){
		var defaults = {
			event: 'mouseover'
		}
		var opts = $.extend({},defaults,optons);
		return this.each(function(){
			var jumpor = new Jumpor($(this),opts);
			jumpor.init();
		});
		function Jumpor(ele, opts) {
			this.ele = ele;
			this.opts = opts
		}




	}


})(jQuery,window,document);
