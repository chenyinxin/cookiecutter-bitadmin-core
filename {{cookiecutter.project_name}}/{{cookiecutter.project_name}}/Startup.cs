using System;
using System.IO;
using System.Net;
using System.Text;
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Services;
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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using UEditorNetCore;

namespace {{cookiecutter.project_name}}
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            //使用HttpContext单例
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //使用Session缓存（默认Memory，Redis二选一）
            //services.AddDistributedRedisCache(option => option.Configuration = RedisHelper.connectionString);
            services.AddSession();

            //使用登录认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.TicketDataFormat = new TicketDataFormat<AuthenticationTicket>());

            //使用Mvc服务
            services.AddMvc();

            //UEditor富文本框后端扩展
            services.AddUEditorService();

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger, IServiceProvider svc)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            //启用配置
            HttpContextCore.Configuration = this.Configuration;
            HttpContextCore.ServiceProvider = svc;
            HttpContextCore.HostingEnvironment = env;

            //添加编码支持
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            //添加安卓安装包mine
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".apk"] = "application/vnd.android.package-archive";

            //启用静态文件（uploadfiles:附件目录；apps:apk下载目录；prototyping原型数据文件）
            string fileupload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploadfiles");
            if (!Directory.Exists(fileupload)) Directory.CreateDirectory(fileupload);
            string apps = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apps");
            if (!Directory.Exists(apps)) Directory.CreateDirectory(apps);
            string prototyping = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "prototyping");
            if (!Directory.Exists(prototyping)) Directory.CreateDirectory(prototyping);
            string ueditor = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ueditor");
            if (!Directory.Exists(ueditor)) Directory.CreateDirectory(ueditor);
            app.UseStaticFiles().UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileupload),
                RequestPath = "/uploadfiles"
            }).UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(apps),
                RequestPath = "/apps",
                ContentTypeProvider = provider
            }).UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(apps),
                RequestPath = "/ueditor",
                ContentTypeProvider = provider
            });

            //启用Session缓存
            app.UseSession();

            //启用登录认证服务
            app.UseAuthentication();

            //启用Mvc服务
            app.UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=Account}/{action=Index}/{id?}"));

            //启用WebSocket服务
            app.Map("/websocket/notice", BitNoticeService.Map);

            //启用Swagger服务
            //app.UseSwagger();
            //app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "{{cookiecutter.project_name}} api v1"));
        }

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
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!SSOClient.IsLogin)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.MethodNotAllowed);
            }
        }
    }
}
