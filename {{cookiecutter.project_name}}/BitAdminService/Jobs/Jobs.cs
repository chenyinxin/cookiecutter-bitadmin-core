using Quartz;
using System.Net.WebSockets;
/***********************
* BitAdmin2.0框架文件
***********************/

namespace BitAdminService.Jobs
{
    public class Jobs
    {
        public static void Config(IScheduler jobs)
        {
            //在这里添加任务
            jobs.AddJob<HelloJob>();
        }
    }
}
