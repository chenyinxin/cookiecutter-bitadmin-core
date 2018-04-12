using System;
using System.IO;
using System.Net;
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
            services.AddMvc();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //登录认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.TicketDataFormat = new TicketDataFormat<AuthenticationTicket>();
                });
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger, IServiceProvider svc)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            HttpContextCore.Configuration = this.Configuration;
            HttpContextCore.ServiceProvider = svc;

            app.UseSession();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".apk"] = "application/vnd.android.package-archive";
            //静态文件
            string fileupload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploadfiles");
            if (!Directory.Exists(fileupload)) Directory.CreateDirectory(fileupload);
            string apps = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apps");
            if (!Directory.Exists(apps)) Directory.CreateDirectory(apps);
            app.UseStaticFiles().UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileupload),
                RequestPath = "/uploadfiles"
            }).UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(apps),
                RequestPath = "/apps",
                ContentTypeProvider = provider
            });
            //登录认证
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Index}/{id?}");
            });
            app.Map("/websocket/notice", BitNoticeService.Map);
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
