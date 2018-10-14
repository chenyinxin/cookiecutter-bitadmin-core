using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;

namespace {{cookiecutter.project_name}}.Controllers.Framework.Auth
{
    public class ADController : Controller
    {
        //配置以下四个参数，开放389端口。
        string domainName = "bitdao.cn";
        string domainRoot = "组织单位";
        string domainUser = "user";
        string domainPass = "password";

        DataContext dbContext = new DataContext();

        public ActionResult Login(string account, string password, string verifyCode)
        {
            try
            {
                string vcode = HttpContextCore.Current.Session.Get<string>("VerificationCode");
                if (Convert.ToString(verifyCode).ToLower() != Convert.ToString(vcode).ToLower())
                    return Json(new { Code = 1, Msg = "验证码不正确，请重新输入！" });
                HttpContextCore.Current.Session.Set("VerificationCode", string.Empty);

                if (account == "admin")
                {
                    if (!SSOClient.Validate(account, password, out Guid userId))
                        return Json(new { Code = 1, Msg = "帐号或密码不正确，请重新输入！" });

                    SSOClient.SignIn(userId);
                }
                else
                {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain, domainName, account, password);
                    if (!context.ValidateCredentials(account + "@" + domainName, password, ContextOptions.SimpleBind))
                        return Json(new { Code = 1, Msg = "帐号或密码不正确，请重新输入！" });

                    var userModel = dbContext.SysUser.FirstOrDefault(t => t.UserCode == account);
                    if (userModel == null)
                        return Json(new { Code = 1, Msg = "验证成功但用户不存在，请同步用户信息！" });
                    SSOClient.SignIn(userModel.UserId);
                }

                return Json(new { Code = 0, Msg = "登录成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        [BitAuthorize]
        public ActionResult ChangePassword(string OldPassword, string NewPassword)
        {
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain, domainName, SSOClient.User.UserCode, OldPassword);
                if (!context.ValidateCredentials(SSOClient.User.UserCode + "@" + domainName, OldPassword, ContextOptions.SimpleBind))
                    return Json(new { Code = 1, Msg = "验证失败，请重试！" });

                DirectoryEntry domain = new DirectoryEntry();
                domain.Path = string.Format("LDAP://{0}", domainName);
                domain.Username = domainUser;
                domain.Password = domainPass;
                domain.AuthenticationType = AuthenticationTypes.Secure;
                domain.RefreshCache();

                DirectorySearcher deSearch = new DirectorySearcher(domain);
                deSearch.Filter = "(&(objectCategory=person)(objectClass=user)(samaccountName=" + SSOClient.User.UserCode + "))";
                deSearch.SearchScope = SearchScope.Subtree;
                SearchResult result = deSearch.FindOne();

                DirectoryEntry oUser = new DirectoryEntry(result.Path, domainUser, domainPass);
                oUser.AuthenticationType = AuthenticationTypes.Secure;
                oUser.RefreshCache();

                oUser.Invoke("ChangePassword", new Object[] { OldPassword, NewPassword });
                oUser.CommitChanges();

                oUser.Close();
                domain.Close();
                return Json(new { Code = 0, Msg = "修改密码成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        [BitAuthorize]
        public ActionResult Sync()
        {
            try
            {
                //连接域
                DirectoryEntry domain = new DirectoryEntry();
                domain.Path = string.Format("LDAP://{0}", domainName);
                domain.Username = domainUser;
                domain.Password = domainPass;
                domain.AuthenticationType = AuthenticationTypes.Secure;
                domain.RefreshCache();

                DirectoryEntry entryOU = domain.Children.Find("OU=" + domainRoot);
                DirectorySearcher mySearcher = new DirectorySearcher(entryOU, "(objectclass=organizationalUnit)"); //查询组织单位
                DirectoryEntry root = mySearcher.SearchRoot;   //查找根OU

                if (root.Properties.Contains("ou") && root.Properties.Contains("objectGUID"))
                {
                    string rootOuName = root.Properties["ou"][0].ToString();
                    byte[] bGUID = root.Properties["objectGUID"][0] as byte[];
                    Guid id = new Guid(bGUID);

                    departments.Add(new SysDepartment() { DepartmentId = id, DepartmentCode = id.ToString(), DepartmentName = rootOuName, DepartmentFullName = rootOuName });

                    SyncSubOU(root, id);
                }

                //入库
                foreach (var d in departments)
                {
                    var department = dbContext.SysDepartment.Find(d.DepartmentId);
                    if (department == null)
                        dbContext.SysDepartment.Add(d);
                    else
                    {
                        department.DepartmentName = d.DepartmentName;
                        department.DepartmentFullName = d.DepartmentFullName;
                        department.ParentId = d.ParentId;
                    }
                    dbContext.SaveChanges();
                }
                foreach (var u in users)
                {
                    var user = dbContext.SysUser.Find(u.UserId);
                    if (user == null)
                    {
                        user.UserCode = u.UserCode;
                        user.UserName = u.UserName;
                        user.DepartmentId = u.DepartmentId;
                        user.Mobile = u.Mobile;
                        user.Email = u.Email;
                        u.CreateBy = u.UserId;
                        u.CreateTime = DateTime.Now;
                        dbContext.SysUser.Add(u);
                    }
                    else
                    {
                        user.UserCode = u.UserCode;
                        user.UserName = u.UserName;
                        user.DepartmentId = u.DepartmentId;
                        user.Mobile = u.Mobile;
                        user.Email = u.Email;
                        user.UpdateBy = u.UserId;
                        user.UpdateTime = DateTime.Now;
                    }
                    dbContext.SaveChanges();
                }

                //初始化所有用户普通角色
                SqlHelper.ExecuteSql(string.Format("insert into SysRoleUser select newid(),'{0}',userId from SysUser where userId not in(select userId from SysRoleUser where roleId='{0}')", "E813C5FF-8764-4324-9A13-44ED5A600412"));


                return Json(new { Code = 0, Msg = "同步成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        List<SysUser> users = new List<SysUser>();
        List<SysDepartment> departments = new List<SysDepartment>();

        private void SyncSubOU(DirectoryEntry entry, Guid parentId)
        {
            foreach (DirectoryEntry subEntry in entry.Children)
            {
                string entrySchemaClsName = subEntry.SchemaClassName;

                string[] arr = subEntry.Name.Split('=');
                string categoryStr = arr[0];
                string nameStr = arr[1];

                byte[] bGUID = subEntry.Properties["objectGUID"][0] as byte[];
                Guid id = new Guid(bGUID);

                switch (entrySchemaClsName)
                {
                    case "organizationalUnit":
                        departments.Add(new SysDepartment() { DepartmentId = id, ParentId = parentId, DepartmentCode = id.ToString(), DepartmentName = nameStr, DepartmentFullName = nameStr });

                        SyncSubOU(subEntry, id);
                        break;
                    case "user":
                        users.Add(new SysUser()
                        {
                            UserId = id,
                            UserCode = subEntry.Properties["samaccountName"][0].ToString(),
                            DepartmentId = parentId,
                            UserName = subEntry.Properties["displayName"][0].ToString(),
                            Email = subEntry.Properties.Contains("mail") ? subEntry.Properties["mail"][0].ToString() : "",
                            Mobile = subEntry.Properties.Contains("telephoneNumber") ? subEntry.Properties["telephoneNumber"][0].ToString() : ""
                        });

                        break;
                }
            }
        }
    }
}
