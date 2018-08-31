using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.AdvancedAPIs.OAuth2;
using Senparc.Weixin.Work.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace {{cookiecutter.project_name}}.Controllers
{
    public class WeixinWorkController : Controller
    {
        DataContext dbContext = new DataContext();

        #region 网页服务
        readonly string _corpId = "ww991092455544b3f4";
        readonly string _agentid = "1000002";
        readonly string _secret = "J2ZSqRn0BPHkbJeQVcORU1rP7-JJP945vvjiGmhnEw4";
        readonly string _authorizeUrl = "https://www.bitadmincore.com/weixinwork/signin";
        readonly string _response_type = "code";

        public ActionResult Authorize_User(string state)
        {
            if (SSOClient.IsLogin)
                return ToMenu(state);

            return Authorize(state, "snsapi_userinfo");
        }
        public ActionResult Authorize_Private(string state)
        {
            if (SSOClient.IsLogin)
                return ToMenu(state);

            return Authorize(state, "snsapi_privateinfo");
        }
        public ActionResult Authorize(string state, string scope)
        {
            if (SSOClient.IsLogin)
                return ToMenu(state);

            return Redirect(OAuth2Api.GetCode(_corpId, _authorizeUrl, state, _agentid, _response_type, scope));
        }
        public ActionResult SignIn(string code, string state)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return Redirect("/pages/error/error.html");

                var token = CommonApi.GetToken(_corpId, _secret);
                if (token.errcode != 0)
                    return Redirect("/pages/error/error.html");

                GetUserInfoResult result = OAuth2Api.GetUserId(token.access_token, code);
                if (result.errcode != 0)
                    return Redirect("/pages/error/error.html");

                SysUser user = dbContext.Set<SysUser>().Where(x => x.UserCode == result.UserId).FirstOrDefault();
                if (user == null)
                {
                    //没有账号：根据业务调整
                    return Redirect("/pages/error/error.html");
                }

                SSOClient.SignIn(user.UserId);
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
