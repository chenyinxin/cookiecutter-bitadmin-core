using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers.Framework.Auth
{
    public class SMSController: Controller
    {
        DataContext dbContext = new DataContext();

        public ActionResult VerifyCode(string account, string t)
        {
            try
            {
                if (!SSOClient.Validate(account, out SysUser user))
                    return Json(new { Code = 1, Msg = "帐号不存在，请重新输入！" });

                string code = VerifyHelper.CreateNumber(4);
                SMSService.Send(user.Mobile, code);
                dbContext.SysSmsCode.Add(new SysSmsCode()
                {
                    Id = Guid.NewGuid(),
                    Mobile = user.Mobile,
                    CreateTime = DateTime.Now,
                    OverTime = DateTime.Now.AddMinutes(3),
                    IsVerify = 0,
                    SmsCode = code,
                    SmsSign = t
                });
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "发送成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult Login(string account, string t, string code)
        {
            try
            {
                if (!SSOClient.Validate(account, out SysUser user))
                    return Json(new { Code = 1, Msg = "帐号不存在，请重新输入！" });

                var item = dbContext.SysSmsCode.FirstOrDefault(x => x.Mobile == user.Mobile && x.SmsCode == code && x.SmsSign == t && x.OverTime > DateTime.Now);
                if (item == null)
                    return Json(new { Code = 1, Msg = "验证码验证失败，请重新输入！" });
                item.IsVerify = 1;
                item.VerifyTime = DateTime.Now;
                dbContext.SaveChanges();

                SSOClient.SignIn(user.UserId);
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
