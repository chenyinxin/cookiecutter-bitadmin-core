/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using BitAdminService.Jobs;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BitAdminService
{
    public partial class BitAdminService : ServiceBase
    {
        public BitAdminService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Run().GetAwaiter().GetResult();
            IPCServer.Run();
            LogHelper.SaveLog("service", "started");
        }

        protected override void OnStop()
        {
            if (Scheduler != null)
            {
                Scheduler.Shutdown();
            }
            LogHelper.SaveLog("service", "stopped");
        }
        public static IScheduler Scheduler { get; private set; }

        private async Task Run()
        {
            try
            {
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                Scheduler = await factory.GetScheduler();

                await Scheduler.Start();

                var jobs = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob)))).ToArray();
                foreach (var job in jobs)
                {
                    Scheduler.AddJob(job);
                }
            }
            catch (SchedulerException ex)
            {
                LogHelper.SaveLog(ex);
            }
        }
    }

    public static class JobHelper
    {
        public static void AddJob(this IScheduler scheduler,Type type) 
        {
            //var type = typeof(T);
            object[] objAttrs = type.GetCustomAttributes(typeof(InvokeAttribute), true);
            if (objAttrs.Length > 0)
            {
                if (objAttrs[0] is InvokeAttribute attr)
                {
                    attr.Type = type;
                    Jobs.Add(attr.Name, attr);

                    var stop = ConfigurationManager.AppSettings["Stop"].Split(',');
                    if (stop.Contains(attr.Name))
                        return;

                    IJobDetail job = JobBuilder.Create(type).WithIdentity(attr.Name).Build();
                    ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                        .StartAt(DateTime.Parse(attr.StartTime))                            //设置任务开始时间    
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(attr.Interval)     //循环的时间
                        .RepeatForever())
                        .Build();
                    scheduler.ScheduleJob(job, trigger);
                }
            }
        }
        public static Dictionary<string, InvokeAttribute> Jobs = new Dictionary<string, InvokeAttribute>();
        public static InvokeAttribute GetJobInvoke(string name)
        {
            return Jobs[name];
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InvokeAttribute : Attribute
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Remark { get; set; }
        public string StartTime { get; set; }
        public int Interval { get; set; }
        public Type Type { get; set; }
    }
}
