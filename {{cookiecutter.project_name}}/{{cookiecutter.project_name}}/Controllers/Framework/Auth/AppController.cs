using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers.Framework.Auth
{
    public class AppController: Controller
    {
        DataContext dbContext = new DataContext();

        public JsonResult Login(string account, string password)
        {
            try
            {
                if (!SSOClient.Validate(account, password, out Guid userId))
                    return Json(new { Code = 1, Msg = "帐号或密码不正确，请重新输入！" });

                SSOClient.SignIn(userId);
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public JsonResult AuthLogin(string openId)
        {
            try
            {
                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == openId).FirstOrDefault();
                if (userOpenId == null || userOpenId.UserId == Guid.Empty)
                    return Json(new { Code = 1, Msg = "用户未绑定！" });

                SSOClient.SignIn(userOpenId.UserId.Value);
                return Json(new { Code = 0, User = userOpenId.UserId });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        [BitAuthorize]
        public JsonResult BindClientId(string clientId, Guid? userId)
        {
            try
            {
                if (!userId.HasValue) userId = SSOClient.UserId;
                var item = dbContext.SysUserClientId.FirstOrDefault(x => x.ClientId == clientId);
                if (item == null)
                {
                    item = new SysUserClientId() { ClientId = clientId, UserId = userId, UpdateTime = DateTime.Now };
                    dbContext.SysUserClientId.Add(item);
                }
                else
                {
                    item.UserId = userId;
                    item.UpdateTime = DateTime.Now;
                }
                dbContext.SaveChanges();

                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult Update()
        {
            try
            {
                string apps = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploadfiles", "app", "release");
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

                string url = string.Format("{0}://{1}/uploadfiles/app/release/{2}", Request.Scheme, Request.Host, Path.GetFileName(vers[ver]));
                var varx = Path.GetFileNameWithoutExtension(vers[ver]).Split("_");

                return Json(new { Code = 0, update = true, Ver = varx[varx.Length - 1], Url = url });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }
}
