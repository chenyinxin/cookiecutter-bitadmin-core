/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace {{cookiecutter.project_name}}.Controllers
{
    [BitAuthorize]
    public class ReadmeController : Controller
    {
        public async Task<JsonResult> NoticWebSocket()
        {
            try
            {
                BitNoticeMessage message = new BitNoticeMessage()
                {
                    Sender = "readme",
                    Receiver = SSOClient.UserId.ToString(),
                    Content = "<h4 class=\"spop-title\">系统通知示例</h4><img src=\"../../qrcode/encode/user\" width=\"100\" height=\"100\" /><br />生成二维码",
                    MessageType = "notice"
                };
                //仅是一个推送通知示例,顺带个二维码示例
                await BitNoticeService.SendStringAsync(message);
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public JsonResult NoticIgetui()
        {
            try
            {
                //string clientid = "cf6f30e351b39bce2978705e20db8578";
                var msg = IgetuiNoticeService.Create("BitAdmin消息", "这是BitAdmin后台发送给用户【" + SSOClient.User.UserName + "】的消息", "", "", "");
                IgetuiNoticeService.Push(SSOClient.UserId, msg);
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public async Task<JsonResult> Apk()
        {
            try
            {
                string apps = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apps");
                Dictionary<string, string> vers = new Dictionary<string, string>();
                foreach (var file in Directory.GetFiles(apps))
                {
                    var path = Path.GetFileNameWithoutExtension(file).Split("_");
                    string key = "";
                    foreach (var v in path[path.Length - 1].Split("."))
                        key += v.PadLeft(5, '0');
                    vers.Add(key, file);
                }
                var ver = vers.Keys.Max();
                string url = string.Format("{0}://{1}/apps/{2}", Request.Scheme, Request.Host, Path.GetFileName(vers[ver]));
                

                BitNoticeMessage message = new BitNoticeMessage()
                {
                    Sender = "readme",
                    Receiver = SSOClient.UserId.ToString(),
                    Content = "<h4 class=\"spop-title\">扫码下载框架APP示例</h4><img src=\"../../qrcode/encode?msg=" + HttpUtility.UrlEncode(url) + "\" width=\"100\" height=\"100\" /><br />",
                    MessageType = "notice"
                };
                await BitNoticeService.SendStringAsync(message);
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public JsonResult CutVideo()
        {
            try
            {
                string inputFile = "inputFiles/source.mp4";
                string outputFile = string.Format("outputFiles/{0}.mp4", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string img = VideoHelper.CreateImage("视频添加文字：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string msg = VideoHelper.CutAndWater(inputFile, outputFile, img, "00:00:00", "00:00:15");

                LogHelper.SaveLog("video", msg);
                return Json(new { Code = 0, Msg = msg });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }
}
