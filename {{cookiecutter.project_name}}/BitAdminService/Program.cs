/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitAdminService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0 && args.Contains("-svc"))
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new BitAdminService()
                };
                ServiceBase.Run(ServicesToRun);

            }
            else
            {
                Application.Run(new ServiceManagerForm());
            }
        }
    }
}
