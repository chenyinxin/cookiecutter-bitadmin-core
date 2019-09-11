using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.AdvancedAPIs.MailList;
using Senparc.Weixin.Work.AdvancedAPIs.MailList.Member;
using Senparc.Weixin.Work.AdvancedAPIs.OAuth2;
using Senparc.Weixin.Work.CommonAPIs;
using Senparc.Weixin.Work.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace {{cookiecutter.project_name}}.Controllers
{
    /// <summary>
    /// 企业微信相关功能
    /// </summary>
    public class WeixinWorkController : Controller
    {
        DataContext dbContext = new DataContext();

        #region 网页服务
        readonly string _agentid = "1000002";
        readonly string _authorizeUrl = "https://www.bitadmincore.com/weixinwork/signin";
        readonly string _response_type = "code";

        public ActionResult Authorize_User(string state)
        {
            if (SSOClient.IsLogin)
                return ToMenu(state);

            return Authorize(state, "snsapi_userinfo");
        }
        public ActionResult Authorize_Private(string state)
        {
            if (SSOClient.IsLogin)
                return ToMenu(state);

            return Authorize(state, "snsapi_privateinfo");
        }
        public ActionResult Authorize(string state, string scope)
        {
            if (SSOClient.IsLogin)
                return ToMenu(state);

            return Redirect(OAuth2Api.GetCode(WeixinWorkHelper.CorpId, _authorizeUrl, state, _agentid, _response_type, scope));
        }
        public ActionResult SignIn(string code, string state)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return Redirect("/pages/error/error.html");

                var token = CommonApi.GetToken(WeixinWorkHelper.CorpId, WeixinWorkHelper.AgentSecrets[_agentid]);
                if (token.errcode != 0)
                    return Redirect("/pages/error/error.html");

                GetUserInfoResult result = OAuth2Api.GetUserId(token.access_token, code);
                if (result.errcode != 0)
                    return Redirect("/pages/error/error.html");

                SysUser user = dbContext.Set<SysUser>().Where(x => x.UserCode == result.UserId).FirstOrDefault();
                if (user == null)
                {
                    //没有账号：根据业务调整
                    return Redirect("/pages/error/error.html");
                }

                SSOClient.SignIn(user.UserId);
                return ToMenu(state);

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        private ActionResult ToMenu(string state)
        {
            switch (state)
            {
                case "menu1":
                    return Redirect("/work/templates/exampleone.html");
                case "menu2":
                    return Redirect("/work/templates/exampletow.html");
                default:
                    return Redirect("/work/home/index.html");
            }
        }
        #endregion

        public ActionResult UploadUser()
        {
            try
            {
                //同步部门
                GetDepartmentListResult result = MailListApi.GetDepartmentList(WeixinWorkHelper.GetToken());
                var departments = dbContext.SysDepartment.OrderByDescending(c => c.DepartmentName).ToList();
                foreach (var item in departments)
                {
                    if (item.DepartmentId == Guid.Parse("2379788E-45F0-417B-A103-0B6440A9D55D"))
                        continue;

                    var parentId = Convert.ToInt64(departments.Where(c => c.DepartmentId == item.ParentId).FirstOrDefault().WeixinWorkId);
                    DepartmentList qyDep = result.department.Where(c => c.id == item.WeixinWorkId).FirstOrDefault();
                    if (qyDep == null)
                    {

                        var createResult = MailListApi.CreateDepartment(WeixinWorkHelper.GetToken(), item.DepartmentName, parentId == 0 ? 1 : parentId, 1000 - (item.OrderNo.HasValue ? item.OrderNo.Value : 0));
                        item.WeixinWorkId = createResult.id;
                    }
                    else
                        MailListApi.UpdateDepartment(WeixinWorkHelper.GetToken(), qyDep.id, item.DepartmentName, parentId == 0 ? 1 : parentId, 1000 - (item.OrderNo.HasValue ? item.OrderNo.Value : 0));
                    dbContext.SaveChanges();
                }
                //同步用户
                var users = dbContext.SysUser.Where(c => c.UserCode != "admin").ToList();
                foreach (var userItem in users)
                {
                    long[] longArr = new long[1];
                    longArr[0] = Convert.ToInt64(dbContext.SysDepartment.Where(c => c.DepartmentId == userItem.DepartmentId).FirstOrDefault().WeixinWorkId);
                    try
                    {
                        var memberResult = MailListApi.GetMember(WeixinWorkHelper.GetToken(), userItem.UserCode);
                        if (memberResult.errcode == Senparc.Weixin.ReturnCode_Work.UserID不存在)
                        {
                            MemberCreateRequest request = new MemberCreateRequest();
                            request.email = userItem.Email;
                            request.department = longArr;
                            request.enable = 1;
                            request.mobile = userItem.Mobile;
                            request.name = userItem.UserName;
                            request.userid = userItem.UserCode;
                            MailListApi.CreateMember(WeixinWorkHelper.GetToken(), request);
                        }
                        else
                        {
                            MemberUpdateRequest updateRequest = new MemberUpdateRequest();
                            updateRequest.department = longArr;
                            updateRequest.email = userItem.Email;
                            updateRequest.enable = 1;
                            updateRequest.mobile = userItem.Mobile;
                            updateRequest.name = userItem.UserName;
                            updateRequest.userid = userItem.UserCode;
                            MailListApi.UpdateMember(WeixinWorkHelper.GetToken(), updateRequest);
                        }
                    }
                    catch { }
                }
                return Json(new { Code = 0, Msg = "同步成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }
}
