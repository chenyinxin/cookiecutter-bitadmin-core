/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.SqlClient;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using {{cookiecutter.project_name}}.Helpers;
using System.Net;

namespace {{cookiecutter.project_name}}.Controllers
{
    [BitAuthorize]
    public class SystemController : Controller
    {
        DataContext dbContext = new DataContext();

        #region 组织架构
        /// <summary>
        /// 获取组织机构
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartmentTree()
        {
            try
            {
                var list = dbContext.SysDepartment.OrderBy(x => x.OrderNo).ToList();
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(list, "parentId", "departmentId") });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 根据父级id获取 部门数据
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryDepartmentData()
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "createTime desc");

                querySuite.Select("select departmentId,departmentCode,departmentName,departmentFullName,createTime from SysDepartment");

                querySuite.AddParam("parentId", "=");
                querySuite.AddParam("departmentName", "like");
                querySuite.AddParam("departmentCode", "like");

                DataSet ds = SqlHelper.Query(querySuite.QuerySql, querySuite.Params);

                return Json(new { Code = 0, Total = ds.Tables[0].Rows[0][0], Data = QuerySuite.ToDictionary(ds.Tables[1]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 加载部门信息
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadDepartment(Guid? departmentId)
        {
            try
            {
                var model = dbContext.SysDepartment.Find(departmentId);
                return Json(new { Code = 0, Data = model });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 保存部门信息(新增、修改)
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveDepartment(Guid? departmentId, Guid? parentDepartmentId, string departmentCode, string isEdit)
        {
            try
            {
                List<SysDepartment> models = dbContext.SysDepartment.Where(x => x.DepartmentCode == departmentCode || x.DepartmentId == departmentId).ToList();

                if (models.Count(x => x.DepartmentId != departmentId && x.DepartmentCode == departmentCode) > 0)
                    return Json(new { Code = 1, Msg = "部门编号已存在！" });

                var model = models.FirstOrDefault(x => x.DepartmentId == departmentId);
                if (model == null)
                {
                    model = new SysDepartment();
                    this.ToModel(model);
                    model.DepartmentId = Guid.NewGuid();
                    model.CreateBy = SSOClient.UserId;
                    model.CreateTime = DateTime.Now;
                    dbContext.SysDepartment.Add(model);
                }
                else
                {
                    this.ToModel(model);
                }
                model.ParentId = parentDepartmentId;
                dbContext.SaveChanges();
                //修改部门全称
                SqlHelper.ExecuteSql(@"
	                declare @fullName nvarchar(1024);
	                with info as (
	                select DepartmentID, cast(DepartmentName as nvarchar(1024)) as fullName,ParentID from SysDepartment where DepartmentID=@departmentId
	                union all
	                select d.DepartmentID, cast((d.DepartmentName + '→' + info.fullName)as nvarchar(1024)) as fullName,d.ParentID
	                from SysDepartment d inner join info on d.DepartmentID = info.ParentID)
	                select top 1 @fullName=fullName from info where ParentID is null;

                    update SysDepartment set departmentFullName=@fullName where departmentId=@departmentId", new SqlParameter("@departmentId", model.DepartmentId));

                return Json(new { Code = 0, Msg = "保存成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteDepartmentData(string IDs)
        {
            try
            {
                string idfilter = "'" + IDs.Replace(",", "','") + "'";
                var ds = SqlHelper.Query(string.Format(@"select count(0) from SysDepartment where parentId in ({0});select count(0) from SysUser where departmentId in ({0})", idfilter));
                if((int)ds.Tables[0].Rows[0][0]>0|| (int)ds.Tables[1].Rows[0][0]>0)
                    return Json(new { Code = 1, Msg = "选择删除部门包含下级部门或用户，不允许删除！" });

                SqlHelper.ExecuteSql(string.Format(@"delete SysDepartment where departmentId in ({0});", idfilter));
                return Json(new { Code = 0, Msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 用户拖动排序
        /// </summary>
        /// <returns></returns>
        public JsonResult SortDepartmentData(string ids)
        {
            try
            {
                var result = SqlHelper.ExecuteSql(QuerySuite.SortSql(ids, "SysDepartment", "orderNo", "departmentId"));
                return Json(new { Code = 0, Msg = "保存成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        #endregion

        #region 用户信息
        /// <summary>
        /// 获取页面操作数据
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryUserData(Guid? parentId)
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "orderNo asc");
                querySuite.Select("select * from SysUser");

                querySuite.AddParam(" and departmentId=@departmentId", new SqlParameter("departmentId", parentId));
                querySuite.AddParam("userName", "like");

                DataSet ds = SqlHelper.Query(querySuite.QuerySql, querySuite.Params);
                return Json(new { Code = 0, Total = ds.Tables[0].Rows[0][0], Data = QuerySuite.ToDictionary(ds.Tables[1]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }

        }

        /// <summary>
        /// 加载页面操作数据
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadUserData(Guid? UserID)
        {
            try
            {
                string sql = @"SELECT t1.*,t2.departmentName FROM [SysUser] t1 left join SysDepartment t2 on t1.departmentId=t2.departmentId where t1.UserID=@UserID";

                DataTable dt = SqlHelper.Query(sql, new SqlParameter("@UserID", UserID)).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt).FirstOrDefault() });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        
        public JsonResult SaveUserData(Guid? UserID, string userCode)
        {
            try
            {
                List<SysUser> models = dbContext.SysUser.Where(x => x.UserCode == userCode || x.UserId == UserID).ToList();

                if (models.FirstOrDefault(x => x.UserCode == userCode && x.UserId != UserID) != null)
                    return Json(new { Code = 1, Msg = "用户标识已经存在！" });

                var model = models.FirstOrDefault(x => x.UserId == UserID);
                if (model == null)
                {
                    model = new SysUser();
                    this.ToModel(model);
                    model.UserId = Guid.NewGuid();
                    model.UserPassword = EncryptHelper.MD5("123456");
                    dbContext.SysUser.Add(model);
                }
                else
                {
                    this.ToModel(model);
                }
                model.UpdateBy = SSOClient.UserId;
                dbContext.SaveChanges();

                return Json(new { Code = 0, Msg = "保存成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 删除页面操作
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteUserData(string ids)
        {
            try
            {
                var result = SqlHelper.ExecuteSql(QuerySuite.DeleteSql(ids, "SysUser", "userID"));
                return Json(new { Code = 0, Msg = "删除成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 用户拖动排序
        /// </summary>
        /// <returns></returns>
        public JsonResult SortUserData(string ids)
        {
            try
            {
                var result = SqlHelper.ExecuteSql(QuerySuite.SortSql(ids, "SysUser", "orderNo", "userID"));
                return Json(new { Code = 0, Msg = "保存成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion

        #region 日志管理
        /// <summary>
        /// 获取日志数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLogData()
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "createTime desc");
                querySuite.Select("select * from SysLog");

                querySuite.AddParam("userName", "like");
                querySuite.AddParam("departmentName", "like");
                querySuite.AddParam("title", "like");
                querySuite.AddParam("type", "like");
                querySuite.AddParam("description", "like");

                DataSet ds = SqlHelper.Query(querySuite.QuerySql, querySuite.Params);

                return Json(new { Code = 0, Total = ds.Tables[0].Rows[0][0], Data = QuerySuite.ToDictionary(ds.Tables[1]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public JsonResult LoadLogData(int id)
        {
            try
            {
                return Json(new { Code = 0, Data = dbContext.SysLog.FirstOrDefault(x => x.Id == id) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <returns></returns>
        public JsonResult AddLog(string title, string type, string description)
        {
            try
            {
                var currentUser = SSOClient.User;
                //负载均衡环境下IP地址
                string ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();
                
                SysLog model = new SysLog
                {
                    UserId = currentUser.UserId,
                    UserCode = currentUser.UserCode,
                    UserName = currentUser.UserName,
                    DepartmentName = SSOClient.Department.DepartmentFullName,
                    IpAddress = string.IsNullOrEmpty(ip) ? HttpContext.Connection.RemoteIpAddress.ToString() : ip,
                    UserAgent= Request.Headers["User-Agent"],
                    CreateTime = DateTime.Now,
                    Type = type,
                    Title = title,
                    Description = description,
                };
                dbContext.Set<SysLog>().Add(model);
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "添加成功！" });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion 日志管理
                
        #region 角色权限
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoleData()
        {
            try
            {
                var model = dbContext.Set<SysRole>().ToList();
                return Json(new { Code = 0, Data = model });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 加载角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult LoadRoleData(Guid? id)
        {
            try
            {
                    var model = dbContext.Set<SysRole>().FirstOrDefault(a => a.Id == id);
                    return Json(new { Code = 0, Data = model });
               
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveRoleData(Guid? id, string RoleName)
        {
            try
            {
                SysRole model = dbContext.SysRole.FirstOrDefault(a => a.RoleName == RoleName && a.Id != id);
                if (model != null)
                    return Json(new { Code = 1, Msg = "角色名称已经存在，请重新输入！" });

                model = dbContext.SysRole.FirstOrDefault(a => a.Id == id);
                if (model == null)
                {
                    model = new SysRole();
                    model.Id = Guid.NewGuid();
                    model.RoleName = RoleName;
                    dbContext.Set<SysRole>().Add(model);
                }
                else
                {
                    model.RoleName = RoleName;
                }
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "保存成功！" });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public JsonResult DeleteRoleData(Guid roleID)
        {
            try
            {
                var model = dbContext.SysRole.FirstOrDefault(a => a.Id == roleID);
                dbContext.Set<SysRole>().Remove(model);
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取角色用户
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public JsonResult QueryRoleUserData(Guid? RoleID, string userName)
        {
            try
            {
                int offset = Convert.ToInt32(Request.Form["offset"].FirstOrDefault());
                int limit = Convert.ToInt32(Request.Form["limit"].FirstOrDefault());
                string order = Request.Form["order"].FirstOrDefault();
                if (string.IsNullOrEmpty(order))
                    order = "asc";
                string ordername = Request.Form["ordername"].FirstOrDefault();
                if (string.IsNullOrEmpty(ordername))
                    ordername = "users.UserID";

                string sql = string.Format(@"select ROW_NUMBER()OVER (order by {0} {1}) RowNumber, users.userName,users.mobile,users.post,dep.departmentFullName
                                             from SysRoleUser roles join SysUser users on roles.UserID=users.UserID 
                                             left join SysDepartment dep on users.departmentId=dep.departmentId where roles.roleId=@RoleID ",
                                    ordername, order);
                List<SqlParameter> list = new List<SqlParameter>();//添加查询条件

                list.Add(new SqlParameter("@RoleID", RoleID));

                if (!string.IsNullOrEmpty(userName))
                {
                    sql += " and users.userName like @userName ";
                    list.Add(new SqlParameter("@userName", string.Format("%{0}%", userName)));
                }

                string strSql = string.Format(@"select count(1) from ({0})t where 1=1 ;
                                             select * from ({0})t where RowNumber between {1} and {2} ", sql, offset + 1, offset + limit);

                SqlParameter[] param = list.ToArray();
                DataSet ds = SqlHelper.Query(strSql, param);
                var result = QuerySuite.ToDictionary(ds.Tables[1]);
                var totals = ds.Tables[0].Rows[0][0];
                return Json(new { Code = 0, Total = totals, Data = result });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取角色下的用户
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public JsonResult LoadRoleUserEdit(string RoleID)
        {
            try
            {
                if (string.IsNullOrEmpty(RoleID))
                    return Json(new { Code = 0, Data = "" });

                string sql = "select userId from SysRoleUser where roleId=@roleId";
                SqlParameter[] param ={new SqlParameter("@roleId",RoleID)};

                DataTable dt = SqlHelper.Query(sql, param).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 保存角色用户
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public JsonResult SaveRoleUserData(string roleId, string userId)
        {
            try
            {
                Dictionary<object, Dictionary<string, SqlParameter[]>> execSqlList = new Dictionary<object, Dictionary<string, SqlParameter[]>>();

                Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
                string sqlDel = "delete SysRoleUser where roleId=@roleId";
                SqlParameter[] param ={new SqlParameter("@roleId", roleId)};
                dic.Add(sqlDel, param);
                execSqlList.Add(Guid.NewGuid(), dic);

                foreach (string userid in userId.Split(','))
                {
                    if (string.IsNullOrEmpty(userid)) continue;

                    Dictionary<string, SqlParameter[]> dicinsert = new Dictionary<string, SqlParameter[]>();
                    string sql = "insert into SysRoleUser(id,roleId,UserID) values(NEWID(),@roleId,@userID)";
                    dicinsert[sql] = new[] { new SqlParameter("@roleId", roleId), new SqlParameter("@userID", userid) };
                    execSqlList.Add(Guid.NewGuid(), dicinsert);
                }

                SqlHelper.ExecuteSqlTran(execSqlList);
                return Json(new { Code = 0, Msg = "保存成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoleMeunsData(Guid? roleId)
        {
            try
            {
                string sql = @"select * from ( SELECT  moduleId AS id, parentId, moduleName AS name,0 AS [type],orderNo,'' as operationSign,'' as pageOperation
                               FROM SysModule module
                               UNION
                               SELECT  page.id, page.ModuleID AS parentId, page.PageName AS Name, 1 AS [Type],page.OrderNo,r.operationSign,
                               STUFF((SELECT ','+ o.operationSign+'_'+o.operationName FROM SysPageOperation p  join SysOperation o on p.operationSign=o.operationSign
                               where PageID=page.id order by o.OrderNo asc FOR XML PATH('')), 1 ,1, '') as PageOperation
                               FROM SysModulePage page left join SysRoleOperatePower r on page.id=r.ModulePageID and r.roleId=@roleId
                               ) t order by OrderNo";
                SqlParameter[] param ={new SqlParameter("@roleId",roleId)};

                DataTable dt = SqlHelper.Query(sql, param).Tables[0];
                var result = QuerySuite.ToDictionary(dt, "parentId", "id");
                return Json(new { Code = 0, Total = result.Count, Data = result });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 保存页面权限设置
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveRoleModuleData(Guid? roleId, Guid? modulePageId, string operationSign, Guid? parentId)
        {
            try
            {
                Dictionary<object, Dictionary<string, SqlParameter[]>> execSqlStringList = new Dictionary<object, Dictionary<string, SqlParameter[]>>();
                Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
                //先删除
                dbContext.SysRoleOperatePower.RemoveRange(dbContext.SysRoleOperatePower.Where(x => x.RoleId == roleId && x.ModulePageId == modulePageId));
                //再添加
                if (!string.IsNullOrEmpty(operationSign))
                {
                    dbContext.SysRoleOperatePower.Add(new SysRoleOperatePower() { Id = Guid.NewGuid(), RoleId = roleId, ModulePageId = modulePageId, ModuleParentId = parentId, OperationSign = operationSign });
                }
                dbContext.SaveChanges();
                //处理父节点（子节点有父节点无，则添加；反之则删除；其它则不变。）
                if (dbContext.SysRoleOperatePower.Count(x => x.RoleId == roleId && x.ModuleParentId == parentId) > 0 
                    && dbContext.SysRoleOperatePower.Count(x => x.RoleId == roleId && x.ModulePageId == parentId) == 0)
                {
                    dbContext.SysRoleOperatePower.Add(new SysRoleOperatePower() { Id = Guid.NewGuid(), RoleId = roleId, ModulePageId = parentId, OperationSign = "query" });
                }
                if (dbContext.SysRoleOperatePower.Count(x => x.RoleId == roleId && x.ModuleParentId == parentId) == 0
                    && dbContext.SysRoleOperatePower.Count(x => x.RoleId == roleId && x.ModulePageId == parentId) > 0)
                {
                    dbContext.SysRoleOperatePower.RemoveRange(dbContext.SysRoleOperatePower.Where(x => x.RoleId == roleId && x.ModulePageId == parentId));
                }
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "保存成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion

        #region 数据字典
        public JsonResult Dictionaries(string type)
        {
            try
            {
                var list = dbContext.SysDictionary.Where(x => x.Type == type).OrderBy(x => x.OrderNo).ToList();
                return Json(new { Code = 0, Data = list });
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