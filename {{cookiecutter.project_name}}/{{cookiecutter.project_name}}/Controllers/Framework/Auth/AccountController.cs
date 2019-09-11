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
using System.IO;
using System.Drawing.Imaging;

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
        public ActionResult SignOut()
        {
            SSOClient.SignOut();
            return Json(new { Code = 0 });
        }

        public JsonResult BindUser(string account, string password, string openId)
        {
            try
            {
                if (!SSOClient.Validate(account, password, out Guid userId))
                    return Json(new { Code = 1, Msg = "帐号或密码不正确，请重新输入！" });
                //公众号绑定
                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == openId).FirstOrDefault();
                if (userOpenId != null)
                {
                    userOpenId.UserId = userId;
                    userOpenId.BindTime = DateTime.Now;
                }
                else
                {
                    userOpenId = new SysUserOpenId();
                    userOpenId.OpenId = openId;
                    userOpenId.UserId = userId;
                    userOpenId.CreateTime = DateTime.Now;
                    userOpenId.BindTime = DateTime.Now;
                    dbContext.SysUserOpenId.Add(userOpenId);
                }
                dbContext.SaveChanges();

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