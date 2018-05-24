/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System;

namespace {{cookiecutter.project_name}}
{
    public static class DateTimeHelper
    {
        public static string WeekName(this DateTime datetime)
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return Day[(int)datetime.DayOfWeek];
        }
    }
}
