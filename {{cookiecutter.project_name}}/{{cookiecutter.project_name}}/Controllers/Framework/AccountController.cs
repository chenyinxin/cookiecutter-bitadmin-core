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
                string code = VerifyHelper.CreateCode(4);
                Bitmap image = VerifyHelper.CreateImage(code);
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

                string code = Helpers.VerifyHelper.CreateNumber(4);
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

        public JsonResult FaceLogin(string account, string imgStr)
        {
            try
            {
                if (!SSOClient.Validate(account, out SysUser user))
                    return Json(new { Code = 1, Msg = "帐号不存在，请重新输入！" });
                if (!FaceHelper.Verify(account, imgStr))
                    return Json(new { Code = 1, Msg = "验证不通过！" });
                SSOClient.SignIn(user.UserId);
                return Json(new { Code = 0,Msg="登录成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
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
                    userCode = Convert.ToString(user.UserCode),
                    userName = Convert.ToString(user.UserName),
                    idCard = Convert.ToString(user.IdCard),
                    mobile = Convert.ToString(user.Mobile),
                    email = Convert.ToString(user.Email),
                    departmentName = Convert.ToString(department.DepartmentName)
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
                using (DataContext dbcontext = new DataContext())
                {
                    var userModel = dbcontext.SysUser.FirstOrDefault(t => t.UserId.Equals(SSOClient.UserId));
                    if (userModel.UserPassword != EncryptHelper.MD5(OldPwd))
                    {
                        return Json(new { Code = 1, Msg = "原密码不正确" });
                    }
                    userModel.UserPassword = EncryptHelper.MD5(NewPwd);
                    if (dbcontext.SaveChanges() < 0)
                    {
                        return Json(new { Code = 1, Msg = "修改密码失败，请联系管理员" });
                    }
                    return Json(new { Code = 0, Msg = "修改密码成功" });
                }
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
                DataContext dbContext = new DataContext();


                //                string sql = string.Format(@"select * from (SELECT  ModuleID AS id, parentId, ModuleName AS Name,'' as Url,ModuleIcon as Icon,0 AS [Type],OrderNo
                //                           FROM SysModule module 
                //                           UNION
                //                           SELECT  id, ModuleID AS parentId, PageName AS Name,PageUrl as Url,PageIcon as Icon, 1 AS [Type],OrderNo
                //                           FROM SysModulePage) t order by OrderNo asc");


                string rolesTxt = string.Empty;
                var roleList = SSOClient.Roles;
                List<string> roles = new List<string>();
                foreach (var role in roleList)
                    roles.Add(role.Value);

                rolesTxt = roles.Count == 0 ? " and 1=2 " : " and t2.roleId in ('" + string.Join("','", roles) + "') ";
                string sql = string.Format(@"--合并数据源 生成临时表
                            select * into #ModulePage from (SELECT  moduleID AS id, parentId, moduleName, '' pageSign , moduleName AS name,'' as url,moduleIcon as icon,0 AS [type],[description],orderNo
                            FROM SysModule module
                            UNION
                            SELECT  id, p.moduleId AS parentId,moduleName,pageSign, pageName AS name,pageUrl as url,pageIcon as icon, 1 AS [type],p.[description],p.orderNo
                            FROM SysModulePage p left join SysModule m on p.ModuleID=m.ModuleID) t 
                            --根据角色权限 获取菜单
                            select distinct t1.* from #ModulePage as t1
                            join SysRoleOperatePower as t2 on t1.id=t2.ModulePageID
                            where 1=1 {0}
                            order by OrderNo asc
                            --删除临时表
                            drop table #ModulePage", rolesTxt);

                DataSet ds = SqlHelper.Query(sql); ;
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(ds.Tables[0], "parentId", "id") });
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
                if (string.IsNullOrEmpty(sign))
                {
                    result.Add("Operation", false);
                    return Json(new { Code = 0, Data = result });
                }
                string sql = string.Format(@"select m.moduleName,p.pageName,p.description
                                            from SysModule m join SysModulePage p on m.moduleId=p.moduleId 
                                             where p.pageSign='{0}' ", sign);
                DataTable dt = SqlHelper.Query(sql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    result.Add("Operation", false);
                    return Json(new { Code = 0, Data = result });
                }

                string rolesTxt = string.Empty;
                var roleList = SSOClient.Roles.ToList();
                if (roleList == null && roleList.Count == 0)
                {
                    result.Add("Operation", false);
                    return Json(new { Code = 0, Data = result });
                }

                List<string> roles = new List<string>();
                foreach (var role in roleList)
                    roles.Add(role.Value);
                rolesTxt = "'" + string.Join("','", roles) + "'";
                sql = string.Format(@"select r.OperationSign from  SysModulePage p join SysRoleOperatePower r on p.id = r.ModulePageID where p.PageSign='{0}' and r.roleId in ({1}) group by r.OperationSign", sign, rolesTxt);
                dt = SqlHelper.Query(sql).Tables[0];
                if (dt == null || dt.Rows.Count == 0)
                {
                    result.Add("Operation", false);
                    return Json(new { Code = 0, Data = result });
                }

                string opSign = "";
                foreach (DataRow dr in dt.Rows)
                {
                    string code = Convert.ToString(dr["OperationSign"]);
                    if (!opSign.Contains(code))
                        opSign += "," + code;
                }
                result.Add("Operation", opSign.Trim(','));

                return Json(new { Code = 0, Data = result });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        } 
        #endregion


        #region QQ互联登录
        public ActionResult QqSignIn(string code)
        {
            try
            {

                if (string.IsNullOrEmpty(code))
                    return Json(new { code = 1, Msg = "参数错误" });

                //QQ互联配置信息
                string appid = "101107448";
                string appkey = "ae7af6e66a8655f5dce06dce7fe20859";
                string status = "";
                string reurl = "http%3A%2F%2Fbit.bitdao.cn%2Faccount%2Fqqsignin";

                WebClient wcl = new WebClient();
                string url = string.Format("https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&state={3}&redirect_uri={4}", appid, appkey, code, status, reurl);
                string json = wcl.DownloadString(url);
                //string token = "access_token=06B07A1CA3144B67BB8ECE03577E4DC5&expires_in=7776000&refresh_token=8DB41D424D74B1F79322946F0E3A17B0";
                
                string[] gtoken = json.Split('&');
                string access_token = gtoken[0].Split('=')[1];
                string expires_in = gtoken[1].Split('=')[1];
                string refresh_token = gtoken[2].Split('=')[1];

                url = "https://graph.qq.com/oauth2.0/me?access_token=" + access_token;
                json = wcl.DownloadString(url);
                //json = "callback( {\"client_id\":\"101107448\",\"openid\":\"1B09CF38A9D917645272095DFF8B6074\"} );";

                Regex re = new Regex("(?<=\").*?(?=\")", RegexOptions.None);
                MatchCollection mc = re.Matches(json);
                List<string> list = new List<string>();
                foreach (Match ma in mc)
                    list.Add(ma.Value);

                string client_id = list[2];
                string openId = list[6];

                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == openId).FirstOrDefault();
                if (userOpenId != null && userOpenId.UserId != Guid.Empty)
                {
                    SSOClient.SignIn(userOpenId.UserId.Value);
                    return Redirect("/pages/home/index.html");
                }
                return Redirect("/pages/account/bind.html?sign=pc&openid=" + openId);

                //自动创建本地用户，适用面向公众网站，项目根据需要调整逻辑。
                //url = string.Format("https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}", access_token, appid, openId);
                //wcl.Encoding = Encoding.UTF8;
                //json = wcl.DownloadString(url);
                //QQUser qqUser = JsonConvert.DeserializeObject<QQUser>(json.Replace("\\", ""));

                //user = new SysUser();
                //user.UserId = Guid.NewGuid();
                //user.UserName = qqUser.nickname;
                //user.UserCode = Guid.NewGuid().ToString("N").Substring(20);
                //user.DepartmentId = new Guid("2379788E-45F0-417B-A103-0B6440A9D55D");
                //dbContext.SysUser.Add(user);

                //userOpenId = new SysUserOpenId();
                //userOpenId.OpenId = token.openid;
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
        public class QQUser
        {
            public int ret { get; set; }
            public int is_lost { get; set; }
            public string msg { get; set; }
            public string nickname { get; set; }
            public string gender { get; set; }
            public string figureurl { get; set; }
            public string figureurl_1 { get; set; }
            public string figureurl_2 { get; set; }
            public string figureurl_qq_1 { get; set; }
            public string figureurl_qq_2 { get; set; }
            public string is_yellow_vip { get; set; }
            public string vip { get; set; }
            public string yellow_vip_level { get; set; }
            public string level { get; set; }
            public string is_yellow_year_vip { get; set; }
        }
        #endregion

        #region 微信互联登录
        public ActionResult WeixinSignIn(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return Json(new { code = 1, Msg = "参数错误" });

                //微信互联配置信息
                string appid = "";
                string appkey = "";

                WebClient wcl = new WebClient();
                string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, appkey, code);
                string json = wcl.DownloadString(url);
                json = wcl.DownloadString(url);
                WeixinUser weixinUser = JsonConvert.DeserializeObject<WeixinUser>(json.Replace("\\", ""));
                
                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == weixinUser.openid).FirstOrDefault();
                if (userOpenId != null && userOpenId.UserId != Guid.Empty)
                {
                    SSOClient.SignIn(userOpenId.UserId.Value);
                    return Redirect("/pages/home/index.html");
                }
                return Redirect("/pages/account/bind.html?sign=pc&openid=" + weixinUser.openid);

                //自动创建本地用户，适用面向公众网站，项目根据需要调整逻辑。
                //url = string.Format("https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}", access_token, appid, openId);
                //wcl.Encoding = Encoding.UTF8;
                //json = wcl.DownloadString(url);
                //QQUser qqUser = JsonConvert.DeserializeObject<QQUser>(json.Replace("\\", ""));

                //user = new SysUser();
                //user.UserId = Guid.NewGuid();
                //user.UserName = qqUser.nickname;
                //user.UserCode = Guid.NewGuid().ToString("N").Substring(20);
                //user.DepartmentId = new Guid("2379788E-45F0-417B-A103-0B6440A9D55D");
                //dbContext.SysUser.Add(user);

                //userOpenId = new SysUserOpenId();
                //userOpenId.OpenId = token.openid;
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
        public class WeixinUser
        {
            public string access_token { get; set; }
            public int? expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
            public string unionid { get; set; }
            public int? errcode { get; set; }
            public string errmsg { get; set; }
        }
        #endregion

        #region 微信公众号登录
        public ActionResult WeixinGZHSignIn(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return Json(new { code = 1, Msg = "参数错误" });

                string appId = "wx806943202a75a124";
                string appSecret = "d52257abea1018eec3a798005ba4f841";

                WebClient wcl = new WebClient();
                string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appId, appSecret, code);
                string json = wcl.DownloadString(url);
                WeixinToken token = JsonConvert.DeserializeObject<WeixinToken>(json.Replace("\\", ""));

                LogHelper.SaveLog("wxgzhsignin", url);
                LogHelper.SaveLog("wxgzhsignin", json);

                if (token.errcode != null)
                    return Json(new { Code = 1, Msg = "获取信息失败" });

                SysUserOpenId userOpenId = dbContext.Set<SysUserOpenId>().Where(x => x.OpenId == token.openid).FirstOrDefault();
                if (userOpenId != null && userOpenId.UserId != Guid.Empty)
                {
                    SSOClient.SignIn(userOpenId.UserId.Value);
                    return Redirect("/pages/home/weixin.html");

                }
                else
                {
                    return Redirect("/pages/account/bind.html?sign=wx&openid=" + token.openid);
                }

                //自动创建本地用户，适用面向公众网站，项目根据需要调整逻辑。
                //url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", token.access_token, token.openid);
                //json = wcl.DownloadString(url);
                //WeixinGZHUser wxUser = JsonConvert.DeserializeObject<WeixinGZHUser>(json.Replace("\\", ""));
                //if (wxUser.errcode != null)
                //    return Json(new { Code = 1, Msg = "获取信息失败" });

                //SysUser user = new SysUser();
                //user.UserId = Guid.NewGuid();
                //user.UserName = wxUser.nickname;
                //user.UserCode = Guid.NewGuid().ToString("N").Substring(20);
                //user.DepartmentId = new Guid("2379788E-45F0-417B-A103-0B6440A9D55D");
                //dbContext.SysUser.Add(user);

                //userOpenId = new SysUserOpenId();
                //userOpenId.OpenId = token.openid;
                //userOpenId.UserId = user.UserId;
                //userOpenId.CreateTime = DateTime.Now;
                //userOpenId.BindTime = DateTime.Now;
                //dbContext.SysUserOpenId.Add(userOpenId);

                //dbContext.SaveChanges();

                //SSOClient.SignIn(userOpenId.UserId.Value);
                //return Redirect("/pages/home/weixin.html");

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion

        #region 微信企业号登录
        public ActionResult WeixinQYHSignIn(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return Json(new { code = 1, Msg = "参数错误" });

                string corpId = "wwa26d4508575b5fe9";
                string secret = "cwcclxDJ0GMIlxsn2U_3kWQUPoiDupZOZMrKFqDDcnI";

                WebClient wcl = new WebClient();
                string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", corpId, secret);
                string json = wcl.DownloadString(url);
                WeixinToken token = JsonConvert.DeserializeObject<WeixinToken>(json.Replace("\\", ""));
                if (token.errcode != 0)
                    return Json(new { Code = 1, Msg = "获取信息失败" });

                //获取微信用户OpenId
                url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", token.access_token, code);
                json = wcl.DownloadString(url);
                WeixinQYHUser wxUser = JsonConvert.DeserializeObject<WeixinQYHUser>(json.Replace("\\", ""));
                if (wxUser.errcode != 0)
                    return Json(new { Code = 1, Msg = "获取信息失败" });

                //获取微信用户信息
                url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserdetail?access_token={0}", token.access_token);
                json = wcl.UploadString(url, "{\"user_ticket\": \"" + wxUser.user_ticket + "\"}");
                WeixinQYHUserDetail wxUserDetial = JsonConvert.DeserializeObject<WeixinQYHUserDetail>(json.Replace("\\", ""));
                if (wxUserDetial.errcode != 0)
                    return Json(new { Code = 1, Msg = "获取信息失败" });

                SysUser user = dbContext.Set<SysUser>().Where(x => x.UserCode == wxUserDetial.userid).FirstOrDefault();
                if (user == null)
                    return Json(new { Code = 1, Msg = wxUserDetial.userid + "不存在！" });

                SSOClient.SignIn(user.UserId);
                return Redirect("/pages/home/weixin.html");
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion


        public class WeixinToken
        {
            /// <summary>
            /// 
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int expires_in { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string refresh_token { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string openid { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string scope { get; set; }
            /// <summary>
            /// 错误时
            /// </summary>
            public int? errcode { get; set; }
            /// <summary>
            /// 错误时
            /// </summary>
            public string errmsg { get; set; }
        }

        public class WeixinGZHUser
        {
            /// <summary>
            /// 非0，错误
            /// </summary>
            public int? errcode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string errmsg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string openid { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string nickname { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sex { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string province { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string city { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string country { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string headimgurl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> privilege { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unionid { get; set; }
        }

        public class WeixinQYHUser
        {
            /// <summary>
            /// 非0，错误
            /// </summary>
            public int errcode { get; set; }
            public string errmsg { get; set; }
            public int expires_in { get; set; }
            public string UserId { get; set; }
            public string DeviceId { get; set; }
            public string user_ticket { get; set; }
            public string OpenId { get; set; }
        }

        public class WeixinQYHUserDetail
        {
            /// <summary>
            /// 非0，错误
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string errmsg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string userid { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<int> department { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string position { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mobile { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string gender { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string email { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string avatar { get; set; }
        }
         
    }
}