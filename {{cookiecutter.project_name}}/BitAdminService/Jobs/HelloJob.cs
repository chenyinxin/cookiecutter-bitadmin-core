/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using Quartz;
using System.Threading.Tasks;

namespace BitAdminService.Jobs
{
    /*Interval单位为秒*/
    [Invoke(Name = "HelloJob", Group = "group", Begin = "2018-05-01 12:00:00", Interval = 20)]
    public class HelloJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            LogHelper.SaveLog("HelloJob", "execute");
            return null;
        }
    }
}
