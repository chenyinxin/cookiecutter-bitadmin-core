/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.UEditor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.Open;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.Work;

namespace {{cookiecutter.project_name}}
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            HttpContextCore.Configuration = Configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            //使用HttpContext单例
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //使用Session缓存（默认Memory，Redis二选一）
            //services.AddDistributedRedisCache(option => option.Configuration = RedisHelper.connectionString);
            services.AddSession();

            //使用登录认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.TicketDataFormat = new TicketDataFormat<AuthenticationTicket>();
                    //跨域ajax时，需要开放Cookie，默认不开启。
                    //options.Cookie = new CookieBuilder()
                    //{
                    //    SameSite = SameSiteMode.None,
                    //    HttpOnly = true,
                    //    SecurePolicy = CookieSecurePolicy.SameAsRequest,
                    //    IsEssential = true,
                    //};
                });

            //使用Mvc服务
            services.AddMvc();

            //UEditor富文本框后端扩展
            services.AddUEditorService();

            //Senparc.CO2NET 全局注册
            services.AddSenparcGlobalServices(Configuration).AddSenparcWeixinServices(Configuration);

            //使用Swagger服务
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v1",
            //        Title = "{{cookiecutter.project_name}} api v1"
            //    });
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly().GetName().Name}.xml");
            //    options.IncludeXmlComments(xmlPath);
            //});
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            //启用配置
            HttpContextCore.ServiceProvider = app.ApplicationServices;

            //添加编码支持
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            //添加安卓安装包mine
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".apk"] = "application/vnd.android.package-archive";

            //启用静态文件（uploadfiles:附件目录；import:导入临时文件；export:导出临时文件；）
            string fileupload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploadfiles");
            if (!Directory.Exists(fileupload)) Directory.CreateDirectory(fileupload);
            string import = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "import");
            if (!Directory.Exists(import)) Directory.CreateDirectory(import);
            string export = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export");
            if (!Directory.Exists(export)) Directory.CreateDirectory(export);

            //启用Session缓存
            app.UseSession();

            //启用登录认证服务
            app.UseAuthentication();

            //拦截html文件非法请求(WebApi在Mvc服务中控制)
            app.UseWhen(w =>
            {
                //跨域ajax需要授权，默认不开启。
                //w.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                //w.Response.Headers.Add("Access-Control-Allow-Origin", "http://origin.bitadmincore.con");

                ////非html页面，不拦截
                //var request = w.Request.AbsoluteUri().ToLower();
                //if (!request.Contains(".html"))
                //    return false;

                ////模块页面，有权限不拦截，特定页面不拦截
                //var pages = w.Session.Get<List<string>>("htmlpagerights");
                //if (pages == null)
                //{
                //    pages = new List<string>();
                //    //添加不需要权限过滤页面
                //    pages.AddRange(new string[] { "/css/", "/images/", "/js/", "/lib/", "/mobile/", "/portal/", "/weixin/" });
                //    pages.AddRange(new string[] { "/pages/account/", "/pages/error/", "/pages/home/", "/pages/picker/", "/pages/shared/" });

                //    //多页面模块处理，入口地址以外的页面需要添加。
                //    Dictionary<string, string[]> extends = new Dictionary<string, string[]>();
                //    extends.Add("role", new string[] { "/pages/system/roleoperate.html" });
                //    extends.Add("flowsetting", new string[] { "/pages/system/flowdesign.html" });

                //    var roles = SSOClient.Roles.Select(x => x.Value).ToList();
                //    string rolesTxt = roles.Count == 0 ? " and 1=2 " : " and t2.roleId in ('" + string.Join("','", roles) + "') ";
                //    string sql = string.Format(@"select pageSign, pageUrl from SysModulePage as t1 join SysRoleOperatePower as t2 on t1.id=t2.ModulePageID where 1=1 {0}", rolesTxt);
                //    var dt = SqlHelper.Query(sql).Tables[0];
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        var page = ConvertUrl(Convert.ToString(dr["pageUrl"]));
                //        if (!pages.Contains(page))
                //            pages.Add(page);

                //        if(extends.TryGetValue(Convert.ToString(dr["pageSign"]).ToLower(),out string[] extend))
                //        {
                //            foreach (var item in extend)
                //            {
                //                page = item.ToLower();
                //                if (!pages.Contains(page))
                //                    pages.Add(page);
                //            }
                //        }
                //    }
                //    w.Session.Set("htmlpagerights", pages);
                //}
                //if (IsRight(request, pages))
                //    return false;

                //return true;

                //默认不较验，直接绕过。
                return false;

            }, u => { u.Run(async context => { context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8"); await context.Response.WriteAsync("非法请求!", Encoding.GetEncoding("utf-8")); }); });

            //启用静态文件
            app.UseStaticFiles().UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileupload),
                RequestPath = "/uploadfiles"
            });

            //启用Mvc服务
            app.UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=account}/{action=index}/{id?}"));

            //启用WebSocket服务
            app.Map("/websocket/notice", BitNoticeService.Map);

            //启动Senparc微信SDK功能
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value).UseSenparcGlobal();
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value);
            register.RegisterMpAccount(senparcWeixinSetting.Value);
            //register.RegisterTenpayV3(senparcWeixinSetting.Value);
            //register.RegisterOpenComponent(senparcWeixinSetting.Value,
            //        //getComponentVerifyTicketFunc
            //        componentAppId =>
            //        {
            //            var dir = Path.Combine(Server.GetMapPath("~/App_Data/OpenTicket"));
            //            if (!Directory.Exists(dir))
            //            {
            //                Directory.CreateDirectory(dir);
            //            }

            //            var file = Path.Combine(dir, string.Format("{0}.txt", componentAppId));
            //            using (var fs = new FileStream(file, FileMode.Open))
            //            {
            //                using (var sr = new StreamReader(fs))
            //                {
            //                    var ticket = sr.ReadToEnd();
            //                    return ticket;
            //                }
            //            }
            //        },

            //         //getAuthorizerRefreshTokenFunc
            //         (componentAppId, auhtorizerId) =>
            //         {
            //             var dir = Path.Combine(Server.GetMapPath("~/App_Data/AuthorizerInfo/" + componentAppId));
            //             if (!Directory.Exists(dir))
            //             {
            //                 Directory.CreateDirectory(dir);
            //             }

            //             var file = Path.Combine(dir, string.Format("{0}.bin", auhtorizerId));
            //             if (!File.Exists(file))
            //             {
            //                 return null;
            //             }

            //             using (Stream fs = new FileStream(file, FileMode.Open))
            //             {
            //                 var binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //                 var result = (RefreshAuthorizerTokenResult)binFormat.Deserialize(fs);
            //                 return result.authorizer_refresh_token;
            //             }
            //         },

            //         //authorizerTokenRefreshedFunc
            //         (componentAppId, auhtorizerId, refreshResult) =>
            //         {
            //             var dir = Path.Combine(Server.GetMapPath("~/App_Data/AuthorizerInfo/" + componentAppId));
            //             if (!Directory.Exists(dir))
            //             {
            //                 Directory.CreateDirectory(dir);
            //             }

            //             var file = Path.Combine(dir, string.Format("{0}.bin", auhtorizerId));
            //             using (Stream fs = new FileStream(file, FileMode.Create))
            //             {
            //                 //这里存了整个对象，实际上只存RefreshToken也可以，有了RefreshToken就能刷新到最新的AccessToken
            //                 var binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //                 binFormat.Serialize(fs, refreshResult);
            //                 fs.Flush();
            //             }
            //         }, "【盛派网络】开放平台");


            //启用Swagger服务
            //app.UseSwagger();
            //app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "{{cookiecutter.project_name}} api v1"));
        }

        //private bool IsRight(string url,List<string> pages)
        //{
        //    foreach (string page in pages)
        //    {
        //        if (url.Contains(page))
        //            return true;
        //    }
        //    return false;
        //}

        //private string ConvertUrl(string url)
        //{
        //    var lowerUrl = url.ToLower();
        //    if (lowerUrl.Contains("_blank"))
        //        return lowerUrl.Replace("_blank:", "");
        //    if (lowerUrl.Contains(".html"))
        //        return lowerUrl;
        //    else
        //        return (lowerUrl.Contains("..") ? lowerUrl.Replace("..", "") : "/pages" + lowerUrl) + ".html";
        //}

    }

    public class TicketDataFormat<T> : ISecureDataFormat<T> where T : AuthenticationTicket
    {
        public string Protect(T data, string purpose)
        {
            TicketSerializer _serializer = new TicketSerializer();
            byte[] userData = _serializer.Serialize(data);
            return Convert.ToBase64String(userData);
        }

        public T Unprotect(string protectedText, string purpose)
        {
            TicketSerializer _serializer = new TicketSerializer();
            byte[] bytes = Convert.FromBase64String(protectedText);
            return _serializer.Deserialize(bytes) as T;
        }

        string ISecureDataFormat<T>.Protect(T data)
        {
            TicketSerializer _serializer = new TicketSerializer();
            byte[] userData = _serializer.Serialize(data);
            return Convert.ToBase64String(userData);
        }

        T ISecureDataFormat<T>.Unprotect(string protectedText)
        {
            TicketSerializer _serializer = new TicketSerializer();
            byte[] bytes = Convert.FromBase64String(protectedText);
            return _serializer.Deserialize(bytes) as T;
        }
    }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BitAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 操作声明，格式不区分大小写：pagesign:action,action|pagesign:action,action
        /// </summary>
        public string Actions { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (string.IsNullOrEmpty(Actions) && !SSOClient.IsLogin)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.MethodNotAllowed);
            }
            else if (!string.IsNullOrEmpty(Actions)) {
                var rights = context.HttpContext.Session.Get<Dictionary<string, List<string>>>("webapirights");
                if (rights == null)
                {
                    rights = new Dictionary<string, List<string>>();

                    var roles = SSOClient.Roles.Select(x => x.Value).ToList();
                    string rolesTxt = "'" + string.Join("','", roles) + "'";
                    var sql = string.Format(@"select distinct p.PageSign,r.OperationSign from SysModulePage p join SysRoleOperatePower r on p.id = r.ModulePageID where r.roleId in ({0}) group by p.PageSign,r.OperationSign", rolesTxt);
                    var dt = SqlHelper.Query(sql).Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        var hasKey = rights.TryGetValue(Convert.ToString(dr["PageSign"]), out List<string> right);
                        if (!hasKey)
                        {
                            right = new List<string>();
                            rights[Convert.ToString(dr["PageSign"]).ToLower()] = right;
                        }
                        var signs = Convert.ToString(dr["OperationSign"]).Split(",");
                        foreach (var sign in signs)
                        {
                            if (!right.Contains(sign))
                                right.Add(sign.ToLower());
                        }
                    }
                    context.HttpContext.Session.Set("webapirights", rights);
                }
                if(!Vaild(rights))
                    context.Result = new StatusCodeResult((int)HttpStatusCode.MethodNotAllowed);
            }
        }
        private bool Vaild(Dictionary<string, List<string>> rights)
        {
            var modules = Actions.Split("|");
            foreach (var module in modules)
            {
                var actions = module.Split(":");
                if (!rights.TryGetValue(actions[0].ToLower(), out List<string> right))
                    continue;
                foreach (var action in actions[1].Split(","))
                {
                    if (right.Contains(action.ToLower()))
                        return true;
                }
            }

            return false;
        }
    }
}
