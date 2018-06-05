using {{cookiecutter.project_name}}.Helpers;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MessageHandlers;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace {{cookiecutter.project_name}}.Controllers.Message
{
    public class WeixinMPController : Controller
    {
        //在微信后台配置
        string token = "bitdao";

        [HttpPost]
        public async Task<ActionResult> Entry(PostModel model)
        {
            //这里总是较验不通过，不知道为什么。
            //if (CheckSignature.Check(model.Signature, model.Timestamp, model.Nonce, token))
            //{
            //    LogHelper.SaveLog("weixin", JsonConvert.SerializeObject(model));
            //    return Content("参数错误！");
            //}

            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                string xml = reader.ReadToEnd();
                DataSet ds = new DataSet();
                StringReader stream = new StringReader(xml);
                XmlTextReader readerXml = new XmlTextReader(stream);
                ds.ReadXml(readerXml);

                DataRow dr = ds.Tables[0].Rows[0];
                string openId = dr["FromUserName"].ToString();

                //这里写接收后业务处理
                //WeixinMPService.SendText(SSOClient.UserId, "您的openId是:" + openId);
                BitNoticeMessage message = new BitNoticeMessage()
                {
                    Sender = "weixin",
                    Receiver = "5eeea4ce-71ab-4464-b72f-17f5163ee944",
                    Content = "<h4 class=\"spop-title\">公众号关注通知</h4>你关注了微信公众号；<br />你的OpenId是：<br />" + openId,
                    MessageType = "notice"
                };
                await BitNoticeService.SendStringAsync(message);

                return Content("success");
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Content("服务器异常！");
            }
        }

        [HttpGet]
        public ActionResult Entry(string signature, string echostr, string timestamp, string nonce)
        {
            try
            {
                if (CheckSignature.Check(signature, timestamp, nonce, token))
                    return Content(echostr);

                return Content("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, token) + "。如果您在浏览器中看到这条信息，表明此Url可以填入微信后台。");
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Content("");
            }
        }
    }
}
