/***********************
 * BitAdmin2.0框架文件
 ***********************/
using Baidu.Aip.Face;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class FaceService
    {
        /* 百度人脸识别API更换成3.0，人脸识别功能用不了啦。
         * 换种思路实现，本地用户存储“身份证”和“姓名”，然后与公安数据库进行对比。
         */
        //static string appid = "11129681";
        static string appkey = "EDyBDG1mQnBPUY5exsOMNyS6";
        static string secretkey = "bMVGearZZvbLclZvEMFStSBUyN6DXaNE ";
        public static bool Verify(string idCardNumber, string name, string imgStr)
        {
            Face client = new Face(appkey, secretkey);
            client.Timeout = 60 * 1000;
            var result = client.PersonVerify(imgStr, "BASE64", idCardNumber, name);

            LogHelper.SaveLog("face", result.ToString());

            foreach (JValue val in (JArray)result["result"])
            {
                if ((double)val.Value > 80) return true;
            }

            return false;
        }
    }
}
