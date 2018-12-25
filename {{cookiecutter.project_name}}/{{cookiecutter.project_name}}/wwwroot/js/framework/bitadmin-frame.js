/***********************
 * BitAdmin2.0框架文件
 * 主页页面功能
 ***********************/
$.extend(BitAdmin,
    {
        //内容模式：tab,iframe,load
        //load模式需要使用LayoutLoad，tab,iframe模式使用LayoutIFrame.
        mode: "tab",
        addTab: function (options) {
            //option:
            //tabMainName:tab标签页所在的容器
            //tabName:当前tab的名称
            //tabTitle:当前tab的标题
            //tabUrl:当前tab所指向的URL地址
            var exists = $("#" + options.tabMainName + " > #tab_li_" + options.tabName).length > 0;
            if (exists) {
                $("#tab_a_" + options.tabName).click();
            } else {
                //var pageUrl = bitPage.GetRedirect(options);
                if (options.pageUrl == undefined) return;

                var li = $('<li class="nav-item" id="tab_li_' + options.tabName + '"></li>');
                var a = $('<a class="nav-link" href="#tab_content_' + options.tabName + '" data-toggle="tab" id="tab_a_' + options.tabName + '">' + options.tabTitle + '  </a>');
                li.on('contextmenu', function (e) {
                    e.preventDefault();
                    $("#menu").css({
                        "display": "block",
                        "left": e.clientX,
                        "top": e.clientY
                    });
                    $("#menu").attr("data-id", options.tabName);
                });
                var spanClose = $('  <i class="fas fa-times"></i>');
                spanClose.bind('click', function () { BitAdmin.closeTab(this); });

                $("#" + options.tabMainName).append(li.append(a.append(spanClose)));

                var tabIframe = $('<iframe id="iframe_' + options.tabName + '" style="height:' + this.getIframeHeight() + ' ; width: 100%;" src="' + options.pageUrl + '" frameborder="0" border="0" marginwidth="0" marginheight="0" scrolling="auto" allowtransparency="true"></iframe>');

                var tabDiv = $('<div id="tab_content_' + options.tabName + '" role="tabpanel" class="tab-pane" id="' + options.tabName + '"></div>');
                $("#" + options.tabContentMainName).append(tabDiv.append(tabIframe));
                $("#tab_a_" + options.tabName).click();
                this.setTabsWidth();
            }
        },
        closeTab: function (button) {
            //通过该button找到对应li标签的id
            var li = $(button).parent().parent();
            var id = li.attr('id').replace("tab_li_", "");
            //bootstrap4的bug，需要先激活移除的项，否则会报错。
            li.find("a").click();
            //如果关闭的是当前激活的TAB，激活他的前一个TAB
            var isCur = li.parent().find(".active").attr('id') == "tab_a_" + id;
            var active = isCur == true ? li.prev().find("a") : li.find("a");
            //关闭TAB
            $(li).remove();
            $("#tab_content_" + id).remove();

            active.click();
        },
        refreshTab: function (id) {
            $('#' + id).attr('src', $('#' + id).attr('src') + "&isRefresh=true");
        },
        getIframeHeight: function () {
            if (BitAdmin.mode == "tab")
                return ($(window).height() - $('body > .wrapper >.main-header').height() - $('body > .wrapper .tab-title').height() * 2 - $('body > .wrapper .main-footer').height() + 10) + 'px';
            if (BitAdmin.mode == "iframe")
                return ($(window).height() - $('body > .wrapper >.main-header').height() - $('body > .wrapper .tab-title').height() * 2 - $('body > .wrapper .main-footer').height() - 20) + 'px';
        },
        setTabsWidth: function () {
            var width = window.innerWidth - $(".main-sidebar").width() - 55;
            var doment = $('#page-tabs')[0].scrollWidth;
            $('#page-tabs').width(width);
            if (width < doment) {
                $('#page-tabs').scrollLeft(doment - width + 100);
            }
        },
        setMentHeight: function () {
            $('.sidebar').height($('.main-sidebar').height() - 10);
        },
        Pages: {},
        Signs: new Array(),
    }
);

$.fn.extend({
    //获取登录用户信息
    GetUserInfo: function () {
        $.get("../../account/getUser", function (result) {
            if (result.code == 1) {
                alert(result.msg);
            } else {
                for (var key in result) {
                    if ($("." + key) != null)
                        $("." + key).text(result[key]);
                }
                if (result.userImage != "")
                    $(".userImage").attr("src", result.userImage);
            }
        });
    },
    GetMenus: function () {
        var _this = $(this);
        $.get("../../account/getMenus", function (data) {
            if (data.code == 0)
                BindTreeViewMenu(data.data, _this, "");
            else
                alert(data.msg);
        });

        function BindTreeViewMenu(menus, pul, title) {
            if (title != "") title = title + ">";
            $.each(menus, function (index, menu) {
                var moduleICO = menu.icon == null || menu.icon == "" ? "fa-edit" : menu.icon;
                if (menu.children != null && menu.children.length > 0) {
                    var li = $('<li class="treeview"><a href="javascript:void(0);"><i class="' + moduleICO + '" style="margin-right:5px;"></i><span>' + menu.name + '</span><span class="pull-right-container"><i class="fa fa-angle-left pull-right"></i></span></a></li>');
                    var ul = $('<ul class="treeview-menu"></ul>');
                    li.append(ul);
                    li.click(function () {
                        BitAdmin.setMentHeight();
                    });
                    pul.append(li);
                    BindTreeViewMenu(menu.children, ul, title + menu.name);//绑定新的树结构<li class="treeview"></li>
                }
                else {
                    var li = $('<li><a href="javascript:void(0);"><i class="' + moduleICO + '" style="margin-right:5px;"></i>' + menu.name + '</a></li>');
                    pul.append(li);
                    if (menu.url != undefined && menu.url != null && menu.url != "") {
                        var option = {
                            tabMainName: "page-tabs",
                            tabName: "page_" + menu.id,
                            tabTitle: menu.name,
                            tabUrl: menu.url,
                            tabContentMainName: "tab-content",
                            title: title + menu.name
                        }
                        option.pageUrl = bitPage.GetRedirect(option);
                        var key = url.query("page", option.pageUrl).split("?")[0].toLowerCase().replace(/\//g, "").replace(/\./g, "");
                        BitAdmin.Pages[key] = {
                            pageKey: key,
                            pageSign: menu.pageSign,
                            pageUrl: menu.url,
                            pageModuleName: menu.moduleName,
                            pageName: menu.name,
                            pageDescription: menu.description
                        };

                        li.click(function () {
                            if (BitAdmin.mode == "tab") {
                                BitAdmin.addTab(option);
                            } else if (BitAdmin.mode == "iframe") {
                                $("[mode=" + BitAdmin.mode + "] iframe").attr("src", option.pageUrl);
                            } else if (BitAdmin.mode == "load") {
                                $("[mode=" + BitAdmin.mode + "]").load(option.pageUrl);
                            }
                        });
                    }
                }
            });
        }
    }
});