/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using Pomelo.AspNetCore.TimedJob;

namespace {{cookiecutter.project_name}}.Services
{
    public class BitAdminJob : Job
    {
        //Begin 起始时间；Interval执行时间间隔，单位是毫秒，建议使用以下格式，此处为20秒；
        //SkipWhileExecuting是否等待上一个执行完成，true为等待；
        [Invoke(Begin = "2018-04-12 22:00", Interval = 1000 * 20, SkipWhileExecuting = true)]
        [JobName(Name = "BitAdmin定期任务示例",Remark ="这只是一个示例，没啥功能。")]
        public void Run()
        {
            if (!this.IsRuning()) return;
            this.RunTime();

            //任务内容写在这里
        }
    }

}
