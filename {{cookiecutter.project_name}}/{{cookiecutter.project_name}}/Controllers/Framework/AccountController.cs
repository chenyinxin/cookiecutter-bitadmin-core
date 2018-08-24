/***********************
 * BitAdmin2.0框架文件
 * 登录权限公共功能
 ***********************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using {{cookiecutter.project_name}}.Services;
using System.DrawingCore;
using System.IO;
using System.DrawingCore.Imaging;
using Microsoft.Extensions.Caching.Memory;

namespace {{cookiecutter.project_name}}.Controllers
{
    public class AccountController : Controller
    {
        DataContext dbContext = new DataContext();
        public ActionResult Index()
        {
            return Redirect("/pages/account/login.html");
        }
        public JsonResult IsLogin()
        {
            return Json(Convert.ToString(SSOClient.IsLogin).ToLower());
        }
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

                HttpContextCore.Current.Session.Set("VerificationCode", code);
                return File(bytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult AppLogin(string account, string password)
        {
            try
            {
                LogHelper.SaveLog("app", account + password);
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
        public JsonResult AppAuthLogin(string openId)
        {
            try
            {
                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == openId).FirstOrDefault();
                if (userOpenId == null || userOpenId.UserId == Guid.Empty)
                    return Json(new { Code = -1, Msg = "用户未绑定！" });

                SSOClient.SignIn(userOpenId.UserId.Value);
                return Json(new { Code = 0 ,User= userOpenId.UserId });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult AppUpdate()
        {
            try
            {
                string apps = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apps");
                Dictionary<string,string> vers = new Dictionary<string, string>();
                foreach(var file in Directory.GetFiles(apps))
                {
                    var path = Path.GetFileNameWithoutExtension(file).Split("_");
                    string key = "";
                    foreach (var v in path[path.Length - 1].Split("."))
                        key += v.PadLeft(5, '0');
                    vers.Add(key, file);                    
                }
                var ver = vers.Keys.Max();

                string url = string.Format("{0}://{1}/apps/{2}", Request.Scheme, Request.Host, Path.GetFileName(vers[ver]));
                var varx = Path.GetFileNameWithoutExtension(vers[ver]).Split("_");

                return Json(new { Code = 0, update = true, Ver = varx[varx.Length - 1], Url = url });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult Login(string account, string password,string verifyCode)
        {
            try
            {
                string vcode = HttpContextCore.Current.Session.Get<string>("VerificationCode");
                if (Convert.ToString(verifyCode).ToLower() != Convert.ToString(vcode).ToLower())
                    return Json(new { Code = 1, Msg = "验证码不正确，请重新输入！" });

                if (!SSOClient.Validate(account, password, out Guid userId))
                    return Json(new { Code = 1, Msg = "帐号或密码不正确，请重新输入！" });

                HttpContextCore.Current.Session.Set("VerificationCode", string.Empty);

                SSOClient.SignIn(userId);
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public ActionResult SMSCode(string account, string t)
        {
            try
            {
                if (!SSOClient.Validate(account,  out SysUser user))
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

        public JsonResult SMSLogin(string account, string t, string code)
        {
            try
            {
                if (!SSOClient.Validate(account, out SysUser user))
                    return Json(new { Code = 1, Msg = "帐号不存在，请重新输入！" });

                var item = dbContext.SysSmsCode.FirstOrDefault(x => x.Mobile == user.Mobile && x.SmsCode == code && x.SmsSign == t && x.OverTime > DateTime.Now);
                if(item==null)
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

        [BitAuthorize]
        public JsonResult GetUser()
        {
            try
            {
                SysUser user = SSOClient.User;
                SysDepartment department = SSOClient.Department;
                return Json(new
                {
                    user.UserCode,
                    user.UserName,
                    user.IdCard,
                    user.Mobile,
                    user.Email,
                    department.DepartmentName
                });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 登录出
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            SSOClient.SignOut();
            return Json(new { Code = 0 });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [BitAuthorize]
        public JsonResult UpdatePassword(string OldPwd, string NewPwd)
        {
            try
            {
                var userModel = dbContext.SysUser.FirstOrDefault(t => t.UserId.Equals(SSOClient.UserId));
                if (userModel.UserPassword != EncryptHelper.MD5(OldPwd))
                {
                    return Json(new { Code = 1, Msg = "原密码不正确" });
                }
                userModel.UserPassword = EncryptHelper.MD5(NewPwd);
                if (dbContext.SaveChanges() < 0)
                {
                    return Json(new { Code = 1, Msg = "修改密码失败，请联系管理员" });
                }
                return Json(new { Code = 0, Msg = "修改密码成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }


        #region 系统加载菜单
        [BitAuthorize]
        public JsonResult GetMenus()
        {
            try
            {
                var roles = SSOClient.Roles.Select(x => x.Value).ToList();
                string rolesTxt = roles.Count == 0 ? " and 1=2 " : " and t2.roleId in ('" + string.Join("','", roles) + "') ";
                string sql = string.Format(@"
                            select * into #ModulePage from (SELECT  moduleID AS id, parentId, moduleName, '' pageSign , moduleName AS name,'' as url,moduleIcon as icon,0 AS [type],[description],orderNo
                            FROM SysModule module
                            UNION
                            SELECT  id, p.moduleId AS parentId,moduleName,pageSign, pageName AS name,pageUrl as url,pageIcon as icon, 1 AS [type],p.[description],p.orderNo
                            FROM SysModulePage p left join SysModule m on p.ModuleID=m.ModuleID) t 
                            --获取菜单
                            select distinct t1.* from #ModulePage as t1 join SysRoleOperatePower as t2 on t1.id=t2.ModulePageID
                            where 1=1 {0} order by OrderNo asc", rolesTxt);

                var dt = SqlHelper.Query(sql).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt, "parentId", "id") });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        #endregion

        #region 获取操作权限
        /// <summary>
        /// 获取操作code，用于判断是否有操作权限
        /// </summary>
        /// <returns></returns>
        [BitAuthorize]
        public JsonResult GetOperationCode(string sign)
        {
            try
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                result["Operation"] = false;

                if (string.IsNullOrEmpty(sign))
                    return Json(new { Code = 0, Data = result });

                if (SSOClient.Roles.Count() == 0)
                    return Json(new { Code = 0, Data = result });

                string sql = string.Format(@"select count(0) from SysModule m join SysModulePage p on m.moduleId=p.moduleId where p.pageSign='{0}' ", sign);
                if ((int)SqlHelper.GetSingle(sql) == 0)
                    return Json(new { Code = 0, Data = result });

                var roles = SSOClient.Roles.Select(x => x.Value).ToList();
                string rolesTxt = "'" + string.Join("','", roles) + "'";
                sql = string.Format(@"select distinct r.OperationSign from SysModulePage p join SysRoleOperatePower r on p.id = r.ModulePageID where p.PageSign='{0}' and r.roleId in ({1}) group by r.OperationSign", sign, rolesTxt);
                var dt = SqlHelper.Query(sql).Tables[0];
                if (dt.Rows.Count == 0)
                    return Json(new { Code = 0, Data = result });

                List<string> signs = new List<string>();
                foreach (DataRow dr in dt.Rows)
                    signs.Add(Convert.ToString(dr["OperationSign"]));

                result["Operation"] = string.Join("','", signs);
                return Json(new { Code = 0, Data = result });
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