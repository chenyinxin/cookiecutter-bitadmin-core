using Baidu.Aip.Face;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class FaceHelper
    {
        static string appid = "11129681";
        static string appkey = "EDyBDG1mQnBPUY5exsOMNyS6";
        static string secretkey = "bMVGearZZvbLclZvEMFStSBUyN6DXaNE ";
        public static bool Verify(string uid, string imgStr)
        {
            Face client = new Face(appkey, secretkey);
            client.Timeout = 60 * 1000;
            var result = client.Verify(uid, "user", Convert.FromBase64String(imgStr));

            LogHelper.SaveLog("face", result.ToString());

            foreach (JValue val in (JArray)result["result"])
            {
                if ((double)val.Value > 80) return true;
            }

            return false;
        }
    }
}
