/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace BitAdminService.Jobs
{
    /*Interval单位为秒*/
    [Invoke(Name = "HelloJob", Remark = "HelloJob示例服务", StartTime = "2018-05-01 12:00:00", Interval = 11)]
    public class HelloJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            LogHelper.SaveLog("HelloJob", "execute");
            Thread.Sleep(5000);
            return null;
        }
    }

    [Invoke(Name = "BitAdminJob", Remark = "BitAdminJob示例服务", StartTime = "2018-05-01 12:00:00", Interval = 18)]
    public class BitAdminJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            LogHelper.SaveLog("BitAdminJob", "execute");
            Thread.Sleep(10000);
            return null;
        }
    }
}
