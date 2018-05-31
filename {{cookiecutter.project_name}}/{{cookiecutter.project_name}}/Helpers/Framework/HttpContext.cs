/***********************
 * BitAdmin2.0框架文件
 ***********************/
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace {{cookiecutter.project_name}}
{
    public static class HttpContextCore
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static HttpContext Current => ((IHttpContextAccessor)ServiceProvider.GetService(typeof(IHttpContextAccessor))).HttpContext;

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

    }
}
