using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace {{cookiecutter.project_name}}.Controllers
{
    /// <summary>
    /// 微信公众号相关功能
    /// </summary>
    public class WeixinMPController : Controller
    {
        DataContext dbContext = new DataContext();

        #region 对话服务
        //在微信后台配置
        readonly string token = "bitadmincore";

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
        [HttpPost]
        public async Task<ActionResult> Entry(PostModel model)
        {
            if (!CheckSignature.Check(model.Signature, model.Timestamp, model.Nonce, token))
                return Content("参数错误！");

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
        #endregion

        #region 网页服务
        readonly string _secret = "d52257abea1018eec3a798005ba4f841";
        readonly string _authorizeUrl = "https://www.bitadmincore.com/weixinmp/signin";
        private string AppId => AccessTokenContainer.GetFirstOrDefaultAppId(PlatformType.MP);
        public ActionResult Authorize_Base(string state)
        {
            if (SSOClient.IsLogin)
                return ToMenu(state);

            return Redirect(OAuthApi.GetAuthorizeUrl(AppId, _authorizeUrl, state, OAuthScope.snsapi_base));
        }
        public ActionResult Authorize_User(string state, string scope)
        {
            if (SSOClient.IsLogin)
                return ToMenu(state);

            return Redirect(OAuthApi.GetAuthorizeUrl(AppId, _authorizeUrl, state, OAuthScope.snsapi_userinfo));
        }
        public ActionResult SignIn(string code, string state)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return Redirect("/pages/error/error.html");
                
                OAuthAccessTokenResult result = OAuthApi.GetAccessToken(AppId, _secret, code);
                if (result.errcode != 0)
                    return Redirect("/pages/error/error.html");

                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == result.openid).FirstOrDefault();
                if (userOpenId == null)
                {
                    //逻辑1：跳转到绑定页面，适用于企业用户或已存在账号情况。
                    return Redirect("/weixin/account/bind.html?openid=" + result.openid);

                    //逻辑2：创建本地用户，适用公众网站，项目根据需要调整逻辑。
                    //var wxUser = OAuthApi.GetUserInfo(result.access_token, result.openid);
                    //SysUser user = new SysUser();
                    //user.UserId = Guid.NewGuid();
                    //user.UserName = wxUser.nickname;
                    //user.UserCode = "wx" + Guid.NewGuid().ToString("N").Substring(18);
                    //user.DepartmentId = new Guid("2379788E-45F0-417B-A103-0B6440A9D55D");
                    //dbContext.SysUser.Add(user);

                    //userOpenId = new SysUserOpenId();
                    //userOpenId.OpenId = result.openid;
                    //userOpenId.UserId = user.UserId;
                    //userOpenId.CreateTime = DateTime.Now;
                    //userOpenId.BindTime = DateTime.Now;
                    //dbContext.SysUserOpenId.Add(userOpenId);

                    //dbContext.SaveChanges();
                }

                SSOClient.SignIn(userOpenId.UserId.Value);
                return ToMenu(state);
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        private ActionResult ToMenu(string state)
        {
            switch (state)
            {
                case "menu1":
                    return Redirect("/weixin/templates/exampleone.html");
                case "menu2":
                    return Redirect("/weixin/templates/exampletow.html");
                default:
                    return Redirect("/weixin/home/index.html");
            }
        }
        #endregion
    }
}
