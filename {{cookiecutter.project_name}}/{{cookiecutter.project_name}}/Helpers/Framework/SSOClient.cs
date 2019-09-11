/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace {{cookiecutter.project_name}}.Helpers
{
    public partial class SSOClient
    {
        public static bool IsLogin
        {
            get
            {
                if (HttpContextCore.Current.User == null || HttpContextCore.Current.User.Identity == null)
                    return false;
                return HttpContextCore.Current.User.Identity.IsAuthenticated;
            }
        }
        public static bool Validate(string sign, string password, out Guid userid)
        {
            userid = Guid.Empty;
            DataContext dbContext = new DataContext();
            password = EncryptHelper.MD5(password);
            var userModel = dbContext.SysUser.FirstOrDefault(t => (t.Mobile == sign || t.Email == sign || t.UserCode == sign) && t.UserPassword == password);
            if (userModel == null)
                return false;

            userid = userModel.UserId;
            return true;
        }
        public static bool Validate(string sign, out SysUser user)
        {
            DataContext dbContext = new DataContext();
            user = dbContext.SysUser.FirstOrDefault(t => (t.Mobile == sign || t.Email == sign || t.UserCode == sign));
            if (user == null)
                return false;

            return true;
        }
        public static void SignIn(Guid userid)
        {
            DataContext dbContext = new DataContext();
            SysUser user = dbContext.SysUser.FirstOrDefault(x => x.UserId == userid);
            var roles = dbContext.SysRoleUser.Where(x => x.UserId == user.UserId).ToList();

            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserCode));

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleId.ToString()));
            }
            SignOut();
            HttpContextCore.Current.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
        public static void SignOut()
        {
            HttpContextCore.Current.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContextCore.Current.Session.Clear();
        }

        public static SysUser User
        {
            get
            {
                SysUser user = HttpContextCore.Current.Session.Get<SysUser>("bitadmin_user");
                if (user == null)
                {
                    string userCode = HttpContextCore.Current.User.Identity.Name;
                    DataContext dbContext = new DataContext();
                    user = dbContext.SysUser.FirstOrDefault(x => x.UserCode == userCode);
                    HttpContextCore.Current.Session.Set("bitadmin_user", user);
                }
                return user;
            }
        }
        public static SysDepartment Department
        {
            get
            {
                SysDepartment department = HttpContextCore.Current.Session.Get<SysDepartment>("bitadmin_department");
                if (department == null)
                {
                    DataContext dbContext = new DataContext();
                    department = dbContext.SysDepartment.FirstOrDefault(x => x.DepartmentId == User.DepartmentId);
                    HttpContextCore.Current.Session.Set("bitadmin_department", department);
                }
                return department;
            }
        }
        public static bool IsAdmin { get { return !Department.ParentId.HasValue; } }

        public static Guid UserId => User.UserId;
        public static IEnumerable<Claim> Roles => (HttpContextCore.Current.User.Identity as ClaimsIdentity).FindAll(x => x.Type == ClaimTypes.Role);
    }
}