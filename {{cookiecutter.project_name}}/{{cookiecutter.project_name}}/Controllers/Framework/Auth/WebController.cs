using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers.Framework.Auth
{
    public class WebController: Controller
    {
        DataContext dbContext = new DataContext();

        public ActionResult VerifyCode()
        {
            try
            {
                var code = VerifyHelper.CreateCode(4);
                var image = VerifyHelper.CreateImage(code);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.GetBuffer();
                ms.Close();

                HttpContextCore.Current.Session.Set("VerifyCode", code);
                return File(bytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult Login(string account, string password, string verifyCode)
        {
            try
            {
                string vcode = HttpContextCore.Current.Session.Get<string>("VerifyCode");
                if (Convert.ToString(verifyCode).ToLower() != Convert.ToString(vcode).ToLower())
                    return Json(new { Code = 1, Msg = "验证码不正确，请重新输入！" });

                if (!SSOClient.Validate(account, password, out Guid userId))
                    return Json(new { Code = 1, Msg = "帐号或密码不正确，请重新输入！" });

                HttpContextCore.Current.Session.Set("VerifyCode", string.Empty);

                SSOClient.SignIn(userId);
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        [BitAuthorize]
        public JsonResult UpdatePassword(string OldPwd, string NewPwd)
        {
            try
            {
                var userModel = dbContext.SysUser.FirstOrDefault(t => t.UserId.Equals(SSOClient.UserId));
                if (userModel.UserPassword != EncryptHelper.MD5(OldPwd))
                    return Json(new { Code = 1, Msg = "原密码不正确" });

                userModel.UserPassword = EncryptHelper.MD5(NewPwd);
                if (dbContext.SaveChanges() < 0)
                    return Json(new { Code = 1, Msg = "修改密码失败，请联系管理员" });

                return Json(new { Code = 0, Msg = "修改密码成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }
}
