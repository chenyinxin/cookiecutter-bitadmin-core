using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers.Framework.Weixin
{
    /// <summary>
    /// 微信开放平台相关功能
    /// </summary>
    public class WeixinController : Controller
    {
        DataContext dbContext = new DataContext();
        public ActionResult Login(string code)
        {
            try
            {
                string openid = string.Empty;

                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == openid).FirstOrDefault();
                if (userOpenId != null && userOpenId.UserId != Guid.Empty)
                {
                    SSOClient.SignIn(userOpenId.UserId.Value);
                    return Redirect("/pages/home/index.html");
                }
                return Redirect("/pages/account/bind.html?openid=" + openid);

                //自动创建本地用户，适用面向公众网站，项目根据需要调整逻辑。
                //url = string.Format("https://api.weixin.qq.com//sns/userinfo?access_token={0}&openid={1}", weixin_access_token, appid, openid);
                //wcl.Encoding = Encoding.UTF8;
                //json = wcl.DownloadString(url);
                //JObject weixinUser = JObject.Parse(json.Replace("\\", ""));

                //var user = new SysUser();
                //user.UserId = Guid.NewGuid();
                //user.UserName = (string)weixinUser["nickname"];//更多可用属性请查看官方接入文档(openid,nickname,sex,province,city,country,headimgurl,privilege[],unionid)
                //user.UserCode = Guid.NewGuid().ToString("N").Substring(20);
                //user.DepartmentId = new Guid("2379788E-45F0-417B-A103-0B6440A9D55D");
                //dbContext.SysUser.Add(user);

                //userOpenId = new SysUserOpenId();
                //userOpenId.OpenId = openid;
                //userOpenId.UserId = user.UserId;
                //userOpenId.CreateTime = DateTime.Now;
                //userOpenId.BindTime = DateTime.Now;
                //dbContext.SysUserOpenId.Add(userOpenId);

                //dbContext.SaveChanges();

                //SSOClient.SignIn(userOpenId.UserId.Value);
                //return Redirect("/pages/home/index.html");
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

    }
}
