using {{cookiecutter.project_name}}.Helpers;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace BitAdminService
{
    public class IPCServer
    {
        public static void Run()
        {
            try
            {
                BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
                provider.TypeFilterLevel = TypeFilterLevel.Full;
                Hashtable ht = new Hashtable();
                ht["portName"] = "BitAdminChannel";
                ht["name"] = "ipc";
                ht["authorizedGroup"] = "Everyone";
                IpcServerChannel channel = new IpcServerChannel(ht, provider);
                ChannelServices.RegisterChannel(channel, false);

                RemotingConfiguration.ApplicationName = "WindowsServices";
                RemotingConfiguration.RegisterActivatedServiceType(typeof(ServiceManager));
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
            }
        }
    }
    public class ServiceManager : MarshalByRefObject
    {
        public ServiceManager()
        {
        }

        private string IsRuning(string name)
        {
            var execing = BitAdminService.Scheduler.GetCurrentlyExecutingJobs().Result;
            foreach (var item in execing)
            {

                if (item.JobDetail.Key.Name == name)
                    return "运行中:执行" + item.JobRunTime.TotalSeconds.ToString("0.00") + "秒";
            }
            return "已启动";

        }
        public List<BitServiceJob> GetServices()
        {
            List<BitServiceJob> result = new List<BitServiceJob>();
            try
            {

                foreach (var group in BitAdminService.Scheduler.GetJobGroupNames().Result)
                {
                    foreach (var jobkey in BitAdminService.Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group)).Result)
                    {
                        foreach (var trigger in BitAdminService.Scheduler.GetTriggersOfJob(jobkey).Result)
                        {
                            BitServiceJob jobitem = new BitServiceJob();
                            jobitem.Name = jobkey.Name;
                            jobitem.Group = jobkey.Group;
                            jobitem.Remark = JobHelper.GetJobInvoke(jobkey.Name).Remark;
                            jobitem.Interval = JobHelper.GetJobInvoke(jobkey.Name).Interval.ToString();
                            jobitem.StartTime = trigger.StartTimeUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                            jobitem.EndTime = trigger.EndTimeUtc.HasValue ? trigger.EndTimeUtc.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "";
                            jobitem.FinalFireTime = trigger.FinalFireTimeUtc.HasValue ? trigger.FinalFireTimeUtc.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "";
                            jobitem.PreviousFireTime = trigger.GetPreviousFireTimeUtc().HasValue ? trigger.GetPreviousFireTimeUtc().Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "";
                            jobitem.NextFireTime = trigger.GetNextFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "";

                            jobitem.Status = IsRuning(jobkey.Name);
                            result.Add(jobitem);
                        }
                    }
                }
                //添加停止服务
                var stop = ConfigurationManager.AppSettings["Stop"].Split(',');
                foreach (var job in JobHelper.Jobs)
                {
                    if (stop.Contains(job.Value.Name))
                    {
                        BitServiceJob jobitem = new BitServiceJob();
                        jobitem.Name = job.Value.Name;
                        jobitem.Group = job.Value.Group ?? "DEFAULT";
                        jobitem.Remark = job.Value.Remark;
                        jobitem.Interval = job.Value.Interval.ToString();
                        jobitem.Status = "已停止";
                        result.Add(jobitem);

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
            }
            return result.OrderBy(x => x.Name).ToList();
        }

        public void Start(string name, bool isSystem = false)
        {
            try
            {
                var attr = JobHelper.Jobs[name];
                IJobDetail job = JobBuilder.Create(attr.Type).WithIdentity(attr.Name).Build();
                ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                    .StartAt(DateTime.Parse(attr.StartTime))                            //设置任务开始时间    
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(attr.Interval)     //循环的时间
                    .RepeatForever())
                    .Build();
                BitAdminService.Scheduler.ScheduleJob(job, trigger);

                var stop = ConfigurationManager.AppSettings["Stop"].Split(',').ToList();
                if (stop.Contains(name))
                {
                    stop.Remove(name);
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["Stop"].Value = string.Join(",", stop.ToArray());
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
            }

        }
        public void Stop(string name)
        {
            try
            {
                BitAdminService.Scheduler.DeleteJob(JobKey.Create(name));

                var stop = ConfigurationManager.AppSettings["Stop"].Split(',').ToList();
                if (!stop.Contains(name))
                {
                    stop.Add(name);
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["Stop"].Value = string.Join(",", stop.ToArray());
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
            }
        }
    }
    [Serializable]
    public class BitServiceJob
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Remark { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string FinalFireTime { get; set; }
        public string Interval { get; set; }
        public string Status { get; set; }
        public string PreviousFireTime { get; set; }
        public string NextFireTime { get; set; }
    }
}
