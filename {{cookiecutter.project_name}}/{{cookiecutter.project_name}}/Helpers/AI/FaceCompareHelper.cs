/***********************
 * BitAdmin2.0框架文件
 ***********************/
using Baidu.Aip.Face;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class FaceCompareHelper
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

        static string domain = "http://api.bitdao.cn";
        public static string ExtractFeature(string imgfile)
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(HttpHelper.CreateStreamContent("face.jpg", imgfile));
            HttpResponseMessage res = client.PostAsync(domain + "/FaceCompare/ExtractFeature", form).Result;
            var json = res.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<UploadResult>(json);
            return result.Data;
        }

        public static string MatchFeature(Bitmap image, string feature)
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(HttpHelper.CreateByteArrayContent("feature", feature));
            form.Add(HttpHelper.CreateStreamContent("face.jpg", image));
            HttpResponseMessage res = client.PostAsync(domain + "/FaceCompare/MatchFeature", form).Result;
            var json = res.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<UploadResult>(json);
            return result.Data;
        }

        private class UploadResult
        {
            public int Code { get; set; }
            public string Msg { get; set; }
            public string Data { get; set; }
        }
    }
}
