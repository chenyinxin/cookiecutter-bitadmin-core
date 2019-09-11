/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace {{cookiecutter.project_name}}.Controllers
{
    [BitAuthorize]
    public class PickerController : Controller
    {        
        #region 模块选择
        /// <summary>
        /// 模块选择
        /// </summary>
        /// <returns></returns>
        public JsonResult GetModuleTreeData()
        {
            try
            {
                string sql = @" select moduleID,parentId,moduleName from SysModule order by OrderNo asc";
                DataTable dt = SqlHelper.Query(sql).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt, "parentId", "ModuleID") });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion

        #region 组织架构选择
        /// <summary>
        /// 获取组织架构
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepData(string userType,string departmentName)
        {
            try
            {
                List<SqlParameter> list = new List<SqlParameter>(); //添加查询条件
                string sql = "select departmentId, parentId, departmentName, departmentFullName from SysDepartment where 1=1 ";
                
                if (!string.IsNullOrEmpty(departmentName))
                {
                    sql += " and departmentName like @departmentName ";
                    list.Add(new SqlParameter("@departmentName", string.Format("%{0}%", departmentName)));
                }
                sql += " order by departmentName desc ";
                DataTable dt = SqlHelper.Query(sql, list.ToArray()).Tables[0];

                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt, "parentId", "departmentId") });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取已选择的部门信息
        /// </summary>
        /// <param name="DepID"></param>
        /// <returns></returns>
        public JsonResult GetSelectDepData(string DepID)
        {
            try
            {
                if (!string.IsNullOrEmpty(DepID))
                {
                    string sql = string.Format("select  departmentId, parentId, departmentName, departmentFullName from SysDepartment where departmentId in ('{0}')", DepID.Replace(",", "','"));
                    DataTable dt = SqlHelper.Query(sql).Tables[0];
                    return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt) });
                }
                return Json(new { Code = 0, Data = "" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion

        #region 用户选择
        /// <summary>
        /// 根据部门获取用户信息
        /// </summary>
        /// <param name="DepID"></param>
        /// <returns></returns>
        public JsonResult GetUserData(string userType, string departmentId, string userName)
        {
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                string sql = "select userId,userName from SysUser where 1=1 ";
                if (!string.IsNullOrEmpty(departmentId) && string.IsNullOrEmpty(userName))//管理人员
                {
                    sql += " and departmentId=@departmentId ";
                    list.Add(new SqlParameter("@departmentId", departmentId));
                }
                if (!string.IsNullOrEmpty(userName))
                {
                    sql += " and userName like @userName ";
                    list.Add(new SqlParameter("@userName", string.Format("%{0}%", userName)));
                }
                sql += " order by userName desc ";
                DataTable dt = SqlHelper.Query(sql, list.ToArray()).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取已选择的用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public JsonResult GetSelectUserData(string userID)
        {
            try
            {
                if (!string.IsNullOrEmpty(userID))
                {
                    string sql = string.Format("select userName,userID from SysUser where UserID in ('{0}')", userID.Replace(",", "','"));
                    DataTable dt = SqlHelper.Query(sql).Tables[0];
                    var json = QuerySuite.ToDictionary(dt);
                    return Json(new { Code = 0, Data = json });
                }
                return Json(new { Code = 0, Data = "" });
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