/***********************
 * BitAdmin2.0框架文件
 ***********************/
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using System;

namespace {{cookiecutter.project_name}}
{
    public static class HttpContextCore
    {
        public static IServiceProvider ServiceProvider;
        public static HttpContext Current
        {
            get
            {
                object factory = ServiceProvider.GetService(typeof(IHttpContextAccessor));
                return ((IHttpContextAccessor)factory).HttpContext;
            }
        }
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :JsonConvert.DeserializeObject<T>(value);
        }
        static IConfiguration configuration;
        public static IConfiguration Configuration
        {
            get
            {
                if (configuration == null)
                    configuration = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
                return configuration;
            }
        }
    }
}
