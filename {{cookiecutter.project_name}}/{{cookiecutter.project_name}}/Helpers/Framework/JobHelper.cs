/***********************
 * BitAdmin2.0框架文件
 ***********************/
using Microsoft.AspNetCore.Builder;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace {{cookiecutter.project_name}}.Helpers
{
    /// <summary>
    /// 定时服务不适合运行网站上，部署单实例无所谓。需要分布式时，会出现多服务器重复运行的情况。
    /// 解决办法也有，可以通过数据库进行配置，只在某一台服务器运行。
    /// </summary>
    public static class JobHelper
    {
        public static Dictionary<string, Dictionary<string, object>> Jobs = new Dictionary<string, Dictionary<string, object>>();
        public static void RunTime(this Job job)
        {
            if (Jobs.TryGetValue(job.GetType().ToString(), out Dictionary<string, object> jobitem))
            {
                jobitem["runtime"] = DateTime.Now;
            }
        }
        public static bool IsRuning(this Job job)
        {
            if (Jobs.TryGetValue(job.GetType().ToString(), out Dictionary<string, object> jobitem))
            {
                return Convert.ToString(jobitem["state"]) == "runing";
            }
            return false;
        }
        public static Dictionary<string, object> Register(this Type jobType)
        {
            var job = new Dictionary<string, object>();
            // 取属性上的自定义特性
            foreach (MethodInfo propInfo in jobType.GetMethods())
            {
                if (propInfo.Name != "Run") continue;

                object[] objAttrs = propInfo.GetCustomAttributes(typeof(InvokeAttribute), true);
                if (objAttrs.Length > 0)
                {
                    if (objAttrs[0] is InvokeAttribute attr)
                    {
                        job["id"] = jobType.ToString();
                        job["invoke"] = string.Format("Begin:{0}，Interval:{1} 秒。", attr.Begin, attr.Interval / 1000);
                        job["state"] = "runing";
                        job["runtime"] = "";
                        Jobs[jobType.ToString()] = job;
                    }
                }
                objAttrs = propInfo.GetCustomAttributes(typeof(JobNameAttribute), true); if (objAttrs.Length > 0)
                {
                    if (objAttrs[0] is JobNameAttribute attr)
                    {
                        job["name"] = attr.Name;
                        job["remark"] = attr.Remark;
                    }
                }
            }
            return job;
        }
        
        public static void Start(string job)
        {
            if (Jobs.TryGetValue(job, out Dictionary<string, object> jobitem))
            {
                jobitem["state"] = "runing";
            }
        }
        public static void Stop(string job)
        {
            if (Jobs.TryGetValue(job, out Dictionary<string, object> jobitem))
            {
                jobitem["state"] = "stop";
            }
        }
        public static void RegisterTimedJob(this IApplicationBuilder app)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(typeof(Job)))).ToList();

            foreach (var item in types)
            {
                item.Register();
            }
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class JobNameAttribute : Attribute
    {
        public string Name { get; set; }
        public string Remark { get; set; }
    }
}
