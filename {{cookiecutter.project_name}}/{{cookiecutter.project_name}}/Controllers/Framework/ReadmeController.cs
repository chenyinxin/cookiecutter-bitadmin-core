using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers
{
    [BitAuthorize]
    public class ReadmeController : Controller
    {
        [HttpGet]
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
        [HttpGet]
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
    }
}
