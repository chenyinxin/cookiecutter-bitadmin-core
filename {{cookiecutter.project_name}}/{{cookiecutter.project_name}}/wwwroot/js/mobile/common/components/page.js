/*
* @Author: sison.luo
* @Date:   2016-03-28 14:14:05
* @Last Modified by:   sison.luo
* @Last Modified time: 2016-04-26 17:45:21

	*分页组件*
*/

;(function($,window,document,undefined){

	$.fn.pager = function(options){
		var defaults = {
			pageIndex : 1,
			pageSize : 10,
			totalItem : 100,
			pageSizeArr: [10,20,50,100] || [10],
			url: '',
			action: function(){}
		}
		var opts = $.extend({},defaults,options);
		return this.each(function(){
			var tempage = new Pages($(this),opts);
			tempage.init(opts.pageIndex, opts.pageSize);
		});
	}
	var Pages = function(ele,opts){
		this.ele = ele;
		this.opts = opts;
	}
	Pages.prototype.init = function(pi, ps){
		var self = this;
		self.render(pi, ps);
		self.watch();
		self.callback(pi, ps, self.getTotalPage(ps));
	}
	Pages.prototype.getTotalPage = function(ps){
		var self = this;
		var totalPage = Math.ceil(self.opts.totalItem/ps);
		return totalPage;
	}
	Pages.prototype.getState = function(pi, ps){
		var self = this;
		var state = self.getTotalPage(pi, ps) == 1 ? 'disabled' : '';
		return state;
	}
	Pages.prototype.render = function(pi, ps){
		var self = this;
		var state = self.getState(pi, ps);
		var selectHtml = ''
		for(var i=0;i<self.opts.pageSizeArr.length;i++){
			selectHtml += '<option value='+self.opts.pageSizeArr[i]+' class="ffm fs">'+self.opts.pageSizeArr[i]+'</option>';
		};
		var renderHTML = '';
		renderHTML += '<ul class="pagUl clearfix">';
		renderHTML += '<li class="pagLi">';
		renderHTML += '<label class="control-label">共<span class="results sps">'+self.opts.totalItem+'</span>条记录</label>';
		renderHTML += '</li>';
		renderHTML += '<li class="pagLi pagNum">';
		renderHTML += '<span class="sps" id="curpage"></span> / <span class="sps" id="totalpage">1</span>';
		renderHTML += '<a href="javascript:;" class="ant '+state+'" id="firstPage">首页</a>';
		renderHTML += '<a href="javascript:;" class="ant '+state+'" id="prevPage">上一页</a>';
		renderHTML += '<a href="javascript:;" class="ant '+state+'" id="nextPage">下一页</a>';
		renderHTML += '<a href="javascript:;" class="ant '+state+'" id="lastPage">末页</a>';
		renderHTML += '</li>';
		renderHTML += '<li class="pagLi">';
		renderHTML += '<div class="form-group pull-left">';
		renderHTML += '<label class="control-label">每页显示</label>';
		renderHTML += '<div class="se-border">';
		renderHTML += '<div class="se-hidden">';
		renderHTML += '<select id="sizePage" class="ffm fs">';
		renderHTML += selectHtml;
		renderHTML += '</select>';
		renderHTML += '</div>';
		renderHTML += '</div>';
		renderHTML += '&nbsp;条</div>';
		renderHTML += '</li>';
		renderHTML += '<li class="pagLi">';
		renderHTML += '<div class="form-group pull-left">';
		renderHTML += '<div class="btn-group jump-txt">';
		renderHTML += '<label class="control-label">跳转到&nbsp;';
		renderHTML += '<input class="txt" id="thePage" '+state+' />&nbsp;页&nbsp;';
		renderHTML += '<button type="button" class="btn" id="btnJump" '+state+'>确定</button>';
		renderHTML += '</label>';
		renderHTML += '</div>';
		renderHTML += '</div>';
		renderHTML += '</li>';
		renderHTML += '</ul>';
		self.ele.append(renderHTML);
	}
	Pages.prototype.watch = function(){
		var self = this;
		$('.pagNum a').on('click',function(){
			var pi = self.ele.find('#curpage').text();
			var ps = self.ele.find('#sizePage').val();
			var tp = self.getTotalPage(ps);
			if(!$(this).hasClass('disabled')){
				var tp = self.getTotalPage(ps);
				var id = $(this).attr('id');
				switch(id){
					case 'firstPage' :
						pi=1;
						break;
					case 'prevPage' :
						pi--;
						break;
					case 'nextPage' :
						pi++;
						break;
					case 'lastPage' :
						pi=tp;
						break;
				}
				self.callback(pi, ps, tp);
				self.opts.action(pi, ps);
			}
		});
		self.ele.find('#sizePage').on('change',function(){
			var ps = $(this).val();
			var tp = self.getTotalPage(ps)
			self.callback(1,ps,tp);
			self.opts.action(1, ps);
		});
		self.ele.find('#thePage').keyup(function(){
			var ps = parseInt($('#sizePage').val());
			var tp = self.getTotalPage(ps);
			if($(this).val()>tp){
				$(this).addClass('error');
				self.ele.find('#btnJump').prop('disabled',true);
			}else{
				self.ele.find('#btnJump').prop('disabled',false);
			}
		});
		self.ele.find('#btnJump').on('click', function(){
			var pi = parseInt($('#thePage').val());
			var ps = parseInt($('#sizePage').val());
			var tp = self.getTotalPage(ps);
			self.callback(pi,ps,tp);
			self.opts.action(pi, ps);
		});
	}
	Pages.prototype.callback = function(pi, ps, tp){
		var self = this;
		// console.log(pi+' , '+ps+' , '+tp);
		if(pi>=tp){
			pi = tp;
			self.ele.find('#nextPage, #lastPage').addClass('disabled');
			if(tp>1){
				self.ele.find('#prevPage, #firstPage').removeClass('disabled');
			}
		}
		if(pi<tp&&pi>1){
			self.ele.find('.pagNum a').removeClass('disabled');
		}
		if(pi<=1){
			pi = 1;
			self.ele.find('#prevPage, #firstPage').addClass('disabled');
			if(tp>1){
				self.ele.find('#nextPage, #lastPage').removeClass('disabled');
			}
		}
		// self.getContent(pi, ps);
		self.renderCallback(pi, ps);
		// self.opts.action(pi, ps);

	}
	Pages.prototype.renderCallback = function(pi, ps){
		var self = this;
		var tp = self.getTotalPage(ps);
		self.ele.find('#curpage').text(pi);
		self.ele.find('#totalpage').text(tp);
	}
	// Pages.prototype.getContent = function(pi, ps){
	// 	var self = this;
	// 	var loading;
	// 	var data = {
	// 		pageIndex: pi,
	// 		pageSize: ps
	// 	}
	// 	$.ajax({
	// 		url: self.opts.url,
	// 		type: 'GET',
	// 		dataType: 'html',
	// 		data: data,
	// 		beforeSend: function(){
	// 			loading = layer.load();
	// 		},
	// 		success: function(d){
	// 			layer.close(loading);
	// 			self.renderCallback(pi, ps);
	// 			self.opts.action(d);
	// 		},
	// 		error: function(xhr, status, errth){
	// 			console.log(xhr.status);
	// 			console.log(xhr.readyState);
	// 			console.log(status);
	// 		}
	// 	});
	// }


})(jQuery,window,document);
