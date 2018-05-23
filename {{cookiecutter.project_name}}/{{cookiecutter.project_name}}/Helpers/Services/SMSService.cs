/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class SMSService
    {
        public static bool Send(string mobile,string msg)
        {
            LogHelper.SaveLog("sms", string.Format("发送信息：{0}:{1}", mobile, msg));
            return true;
        }
    }
}
