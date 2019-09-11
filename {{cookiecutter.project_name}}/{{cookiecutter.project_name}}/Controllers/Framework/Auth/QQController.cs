using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers.Framework.Auth
{
    public class QQController: Controller
    {
        DataContext dbContext = new DataContext();
        
        //QQ互联配置缓存信息
        static string access_token;
        static string refresh_token;
        static DateTime expires_time;

        //QQ互联配置信息
        string appid = "101107448";
        string appkey = "ae7af6e66a8655f5dce06dce7fe20859";
        string status = "";
        string reurl = "https://www.bitadmincore.com/qq/login";

        #region QQ互联登录
        public ActionResult Login(string code)
        {
            try
            {
                /* 作者说明：续期功能未测试，有谁帮忙测一下。
                 */
                if (string.IsNullOrEmpty(code))
                    return Json(new { code = 1, Msg = "参数错误" });
                
                WebClient wcl = new WebClient();
                /*access_token有次数限制，不能每次都取一个新的，在有效期内可以使用，过了有效期则续期就可以了。*/
                if (string.IsNullOrEmpty(access_token))
                {
                    var json = wcl.DownloadString(string.Format("https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&state={3}&redirect_uri={4}", appid, appkey, code, status, UrlEncoder.Default.Encode(reurl)));
                    //string token = "access_token=06B07A1CA3144B67BB8ECE03577E4DC5&expires_in=7776000&refresh_token=8DB41D424D74B1F79322946F0E3A17B0";
                    var param = QueryHelpers.ParseQuery(json);
                    access_token = param["access_token"];
                    refresh_token = param["refresh_token"];
                    expires_time = DateTime.Now.AddSeconds(Convert.ToInt32(param["expires_in"].ToString()));
                }
                else if (expires_time < DateTime.Now.AddMinutes(30))//提前30分钟续期
                {
                    var json = wcl.DownloadString(string.Format("https://graph.qq.com/oauth2.0/token?grant_type=refresh_token&client_id={0}&client_secret={1}&refresh_token={2}", appid, appkey, refresh_token));
                    //string token = "access_token=06B07A1CA3144B67BB8ECE03577E4DC5&expires_in=7776000&refresh_token=8DB41D424D74B1F79322946F0E3A17B0";
                    var param = QueryHelpers.ParseQuery(json);
                    access_token = param["access_token"];
                    refresh_token = param["refresh_token"];
                    expires_time = DateTime.Now.AddSeconds(Convert.ToInt32(param["expires_in"].ToString()));
                }
                var result = wcl.DownloadString("https://graph.qq.com/oauth2.0/me?access_token=" + access_token);
                //json = "callback( {\"client_id\":\"101107448\",\"openid\":\"1B09CF38A9D917645272095DFF8B6074\"} );";
                var mc = new Regex("(?<=\\u0028).*?(?= \\u0029)", RegexOptions.None).Matches(result)[0];//提取出json对象
                JObject me = JObject.Parse(mc.Value);
                string client_id = (string)me["client_id"];
                string openId = (string)me["openid"];

                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == openId).FirstOrDefault();
                if (userOpenId != null && userOpenId.UserId != Guid.Empty)
                {
                    SSOClient.SignIn(userOpenId.UserId.Value);
                    return Redirect("/pages/home/index.html");
                }
                return Redirect("/pages/account/bind.html?openid=" + openId);

                //自动创建本地用户，适用面向公众网站，项目根据需要调整逻辑。
                //url = string.Format("https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}", access_token, appid, openId);
                //wcl.Encoding = Encoding.UTF8;
                //json = wcl.DownloadString(url);
                //JObject qqUser = JObject.Parse(json.Replace("\\", ""));

                //var user = new SysUser();
                //user.UserId = Guid.NewGuid();
                //user.UserName = (string)qqUser["nickname"];//更多可用属性请查看官方接入文档
                //user.UserCode = Guid.NewGuid().ToString("N").Substring(20);
                //user.DepartmentId = new Guid("2379788E-45F0-417B-A103-0B6440A9D55D");
                //dbContext.SysUser.Add(user);

                //userOpenId = new SysUserOpenId();
                //userOpenId.OpenId = openId;
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
        #endregion
    }
}
