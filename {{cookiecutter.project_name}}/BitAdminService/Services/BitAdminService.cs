/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using BitAdminService.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceProcess;
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
            LogHelper.SaveLog("service", "started");
        }

        protected override void OnStop()
        {
            if (scheduler != null)
            {
                scheduler.Shutdown();
            }
            LogHelper.SaveLog("service", "stopped");
        }

        IScheduler scheduler;
        private async Task Run()
        {
            try
            {
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                scheduler = await factory.GetScheduler();

                await scheduler.Start();

                Jobs.Jobs.Config(scheduler);
            }
            catch (SchedulerException ex)
            {
                LogHelper.SaveLog(ex);
            }

        }
    }

    public static class JobHelper
    {
        public static void AddJob<T>(this IScheduler scheduler) where T : IJob
        {
            var type = typeof(T);
            object[] objAttrs = type.GetCustomAttributes(typeof(InvokeAttribute), true);
            if (objAttrs.Length > 0)
            {
                if (objAttrs[0] is InvokeAttribute attr)
                {
                    IJobDetail job = JobBuilder.Create<T>().WithIdentity(attr.Name, attr.Group).Build();
                    ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                        .StartAt(DateTime.Parse(attr.Begin))                                //设置任务开始时间    
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(attr.Interval)     //循环的时间
                        .RepeatForever())
                        .Build();
                    scheduler.ScheduleJob(job, trigger);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InvokeAttribute : Attribute
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Remark { get; set; }
        public string Begin { get; set; }
        public int Interval { get; set; }
    }
}
