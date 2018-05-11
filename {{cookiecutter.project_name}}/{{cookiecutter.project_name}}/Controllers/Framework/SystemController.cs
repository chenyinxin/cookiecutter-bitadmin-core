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
        public JsonResult GetAllDPData()
        {
            try
            {
                string sql = "select departmentId,parentId,departmentName,createTime from SysDepartment order by OrderNo ";
                DataTable dt = SqlHelper.Query(sql).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt, "parentId", "departmentId") });
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
        public JsonResult GetDepartmentData()
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
                //if (!parentDepartmentId.HasValue)
                //{
                //    return Json(new { Code = 1, Msg = "请选中上级部门后再新增部门！" });
                //}

                List<SysDepartment> models = dbContext.SysDepartment.Where(x => x.DepartmentCode == departmentCode || x.DepartmentId == departmentId).ToList();

                //部门编号唯一性验证
                if (models.FirstOrDefault(x => x.DepartmentId != departmentId && x.DepartmentCode == departmentCode) != null)
                    return Json(new { Code = 1, Msg = "部门编号已存在！" });

                SysDepartment model = models.FirstOrDefault(x => x.DepartmentId == departmentId);
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
                SqlHelper.ExecuteSql("update SysDepartment set departmentFullName=dbo.fn_GetDepartmentFullName(@departmentId) where departmentId=@departmentId", new SqlParameter("@departmentId", model.DepartmentId));

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
        public JsonResult DelDepartment(string IDs)
        {
            try
            {
                Dictionary<string, SqlParameter[]> dicsql = new Dictionary<string, SqlParameter[]>();
                foreach (string id in IDs.Split(','))
                {
                    SqlParameter[] sqlpara = new SqlParameter[] { new SqlParameter("@id", id) };

                    //删除 部门下 管理人员
                    string deluser = "delete from [SysUser] where [departmentId] in (select * from fn_GetDepartmentsByID(@id))";
                    dicsql.Add(deluser, sqlpara);
                    //删除该部门级 子级部门
                    string deldp = " delete from [SysDepartment] where [departmentId] in (select * from fn_GetDepartmentsByID(@id))";
                    dicsql.Add(deldp, sqlpara);
                }
                SqlHelper.ExecuteSqlTran(dicsql);
                return Json(new { Code = 0, Msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
            }
            return Json(new { Code = 1, Msg = "删除失败！" });
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
        public JsonResult LoadUser(Guid? UserID)
        {
            try
            {
                if (!UserID.HasValue)
                    return Json(new { Code = 0, Msg = "" });

                string sql = @"SELECT t1.*,t2.departmentName FROM [SysUser] t1 
                                left join SysDepartment t2 on t1.departmentId=t2.departmentId
                                where t1.UserID=@UserID";

                DataTable dt = SqlHelper.Query(sql, new SqlParameter("@UserID", UserID)).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt).FirstOrDefault() });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        
        public JsonResult SaveUser(Guid? UserID, string userCode)
        {
            try
            {
                List<SysUser> models = dbContext.SysUser.Where(x => x.UserCode == userCode || x.UserId == UserID).ToList();
                SysUser model = new SysUser();

                if (models.FirstOrDefault(x => x.UserCode == userCode && x.UserId != UserID) != null)
                    return Json(new { Code = 1, Msg = "用户标识已经存在！" });

                model = models.FirstOrDefault(x => x.UserId == UserID);
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
        public JsonResult DeleteUser(string ids)
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
        public JsonResult SortUser(string ids)
        {
            try
            {
                string sql = QuerySuite.SortSql(ids, "SysUser", "orderNo", "userID");
                var result = SqlHelper.ExecuteSql(sql);

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

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <returns></returns>
        public JsonResult AddLog(string title, string Type, string Description)
        {
            try
            {
                SysLog model = new SysLog();
                var currentUser = SSOClient.User;
                model.UserId = currentUser.UserId;
                model.UserName = currentUser.UserName;
                model.UserCode = currentUser.UserCode;
                model.DepartmentName = SSOClient.Department.DepartmentFullName;
                model.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                model.CreateTime = DateTime.Now;
                model.Type = Type;
                model.Description = Description;
                model.Title = title;
                dbContext.Set<SysLog>().Add(model);
                dbContext.SaveChanges();
                return Json(true);

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
            }
            return Json(false);
        }

        #endregion 日志管理

        #region 数据字典
        /// <summary>
        /// 获取数据字典数据
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryDictionary()
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "orderNo asc");

                querySuite.Select("select * from SysDictionary");

                querySuite.AddParam("Type", "like");
                querySuite.AddParam("Member", "like");
                querySuite.AddParam("MemberName", "like");

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
        /// 保存数据字典(添加 修改)
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveDictionary(string Type, string Member)
        {
            try
            {
                SysDictionary model = dbContext.SysDictionary.FirstOrDefault(s => s.Type == Type && s.Member == Member);
                
                if (model == null)
                {
                    model = new SysDictionary();
                    this.ToModel(model);
                    dbContext.SysDictionary.Add(model);
                }
                else
                {
                    this.ToModel(model);
                }
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
        /// 加载数据字典
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadDictionary(string Type, string Member)
        {
            try
            {
                var model = dbContext.Set<SysDictionary>().FirstOrDefault(s => s.Type == Type && s.Member == Member);
                return Json(new { Code = 0, Data = model });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteDictionary(string IDs)
        {
            try
            {
                var result = SqlHelper.ExecuteSql(QuerySuite.DeleteSql(IDs, "SysDictionary", "type", "member"));

                return Json(new { Code = 0, Msg = "删除成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }

        }
                
        public JsonResult GetDictionary(string type)
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

        #region 系统菜单
        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryMeunsData()
        {
            try
            {
                string sql = @"select * from ( SELECT  moduleId AS id, parentId, moduleName AS name,'' as url,moduleIcon as icon,'' as pageSign,0 AS [type],orderNo
                               FROM SysModule module
                               UNION
                               SELECT  id, ModuleID AS parentId, PageName AS Name,PageUrl as Url, PageIcon as Icon,PageSign, 1 AS [Type],OrderNo
                               FROM SysModulePage) t order by OrderNo";
                DataTable dt = SqlHelper.Query(sql).Tables[0];

                var result = QuerySuite.ToDictionary(dt, "parentId", "", "id", "child");
                return Json(new { Code = 0, Total = result.Count, Data = result });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 加载模块详情
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadModuleData(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                    return Json(new { Code = 0, Msg = "" });

                string sql = @" select Module.*,Parent.ModuleName as parentName from SysModule Module 
                            left join SysModule Parent on Module.parentId=Parent.ModuleID where Module.ModuleID=@id";

                DataTable dt = SqlHelper.Query(sql, new SqlParameter("@id", id)).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt).FirstOrDefault() });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 保存模块信息
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveModuleData(Guid? ModuleID, string ModuleName, Guid? parentId)
        {
            try
            {
                SysModule model = dbContext.SysModule.FirstOrDefault(a => a.ModuleId != ModuleID && a.ModuleName == ModuleName
                        && (parentId.HasValue ? true : a.ParentId == parentId));
                if (model != null)
                    return Json(new { Code = 1, Msg = "模块名称已经存在，请重新输入！" });

                model = dbContext.SysModule.FirstOrDefault(a => a.ModuleId == ModuleID);
                if (model != null)
                {
                    //修改
                    this.ToModel(model);
                }
                else
                {
                    //新增
                    model = new SysModule();
                    this.ToModel(model);
                    ModuleID = Guid.NewGuid();
                    model.ModuleId = ModuleID.Value;
                    int OrderNo = (int)SqlHelper.GetSingle(@"select ISNULL(MAX(OrderNo),0) from SysModule 
                        where ISNULL(cast(parentId as varchar(50)),'')=@parentId and OrderNo <90 ",
                        new SqlParameter("@parentId", parentId.HasValue ? parentId.Value.ToString() : ""));
                    model.OrderNo = OrderNo + 1;
                    dbContext.SysModule.Add(model);

                    //添加操作和权限
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.ModuleId, OperationSign = "query" });
                    dbContext.SysRoleOperatePower.Add(new SysRoleOperatePower() { Id = Guid.NewGuid(), RoleId = Guid.Parse("3F9578C5-C0A2-4C7A-B0FD-C93FAE47194B"), ModulePageId = model.ModuleId, OperationSign = "query" });
                }
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
        /// 加载页面详情
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadMeunsData(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                    return Json(new { Code = 0, Msg = "" });

                string sql = @" select page.*,module.moduleName from SysModulePage page 
                                left join SysModule module on page.ModuleID=module.ModuleID where page.id=@id";

                DataTable dt = SqlHelper.Query(sql, new SqlParameter("@id", id)).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt).FirstOrDefault() });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 保存页面
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveMeunsData(Guid? id, string PageSign)
        {
            try
            {
                List<SysModulePage> models = dbContext.SysModulePage.Where(x => x.PageSign == PageSign || x.Id == id).ToList();
                SysModulePage model = new SysModulePage();

                //部门编号唯一性验证
                if (models.FirstOrDefault(x => x.Id != id) != null)
                    return Json(new { Code = 1, Msg = "页面标识已经存在，请重新输入！" });

                model = models.FirstOrDefault(x => x.Id == id);
                if (model != null)
                {
                    //修改
                    this.ToModel(model);
                }
                else
                {
                    //新增
                    model = new SysModulePage();
                    this.ToModel(model);
                    id = Guid.NewGuid();
                    model.Id = id.Value;
                    int OrderNo = (int)SqlHelper.GetSingle(@"select ISNULL(MAX(OrderNo),0) from (
                                        SELECT  parentId,OrderNo
                                        FROM SysModule module
                                        UNION
                                        SELECT  ModuleID AS parentId ,OrderNo
                                        FROM SysModulePage) t where parentId=@parentId", new SqlParameter("@parentId", model.ModuleId));
                    model.OrderNo = OrderNo + 1;
                    dbContext.SysModulePage.Add(model);

                    //添加操作和权限
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.Id, OperationSign = "query" });
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.Id, OperationSign = "add" });
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.Id, OperationSign = "save" });
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.Id, OperationSign = "delete" });
                    dbContext.SysRoleOperatePower.Add(new SysRoleOperatePower() { Id = Guid.NewGuid(), RoleId = Guid.Parse("3F9578C5-C0A2-4C7A-B0FD-C93FAE47194B"), ModulePageId = model.Id,ModuleParentId=model.ModuleId, OperationSign = "query,add,save,delete" });
                }
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
        /// 删除菜单
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public JsonResult DeleteMeunsData(string IDs)
        {
            try
            {
                string sql = string.Format(@"delete SysModulePage where id in ('{0}');
                                             delete SysModule where ModuleID in ('{0}');
                                            delete SysPageOperation where PageID in ('{0}');
                                            delete SysRoleOperatePower where ModulePageID in ('{0}')",
                                            IDs.Replace(",", "','"));
                SqlHelper.ExecuteSql(sql);
                return Json(new { Code = 0, Msg = "删除成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取页面的操作按钮
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPageOperationData(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                    return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });

                string sql = @"select SysOperation.operationSign,SysOperation.operationName,SysPageOperation.PageID from 
                               SysOperation left join SysPageOperation on SysOperation.operationSign=SysPageOperation.operationSign
                               and SysPageOperation.PageID=@PageID order by SysOperation.OrderNo asc";
                SqlParameter[] param = { new SqlParameter("@PageID", id) };
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
        /// 保存页面操作
        /// </summary>
        /// <param name="operationSign"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public JsonResult SavePageOperationData(string operationSign, string PageID)
        {
            try
            {
                Dictionary<object, Dictionary<string, SqlParameter[]>> listDic = new Dictionary<object, Dictionary<string, SqlParameter[]>>();
                string[] signs = operationSign.Split(',');
                Dictionary<string, SqlParameter[]> DelDic = new Dictionary<string, SqlParameter[]>();
                string DelSql = @"DELETE [dbo].[SysPageOperation] WHERE PageID=@PageID";
                SqlParameter[] DelParam = { new SqlParameter("@PageID", PageID) };
                DelDic.Add(DelSql, DelParam);
                listDic.Add(Guid.NewGuid(), DelDic);
                foreach (string sign in signs)
                {
                    if (!string.IsNullOrEmpty(sign))
                    {
                        Dictionary<string, SqlParameter[]> Sqldic = new Dictionary<string, SqlParameter[]>();
                        string sql = @"INSERT INTO [dbo].[SysPageOperation]([id],[PageID],[operationSign])
                                    VALUES (NEWID(),@PageID,@operationSign)";
                        SqlParameter[] param ={
                                              new SqlParameter("@PageID",PageID),
                                              new SqlParameter("@operationSign",sign),
                                         };
                        Sqldic.Add(sql, param);
                        listDic.Add(Guid.NewGuid(), Sqldic);
                    }
                }
                SqlHelper.ExecuteSqlTran(listDic);
                return Json(new { Code = 0, Msg = "保存成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        #endregion

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
        public JsonResult LoadRoleEdit(Guid? id)
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
        public JsonResult DelRoleData(Guid roleID)
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
                SqlParameter[] param ={
                                          new SqlParameter("@roleId",RoleID)
                                     };
                DataTable dt = SqlHelper.Query(sql, param).Tables[0];
                var json = QuerySuite.ToDictionary(dt);
                return Json(new { Code = 0, Data = json });
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
                Dictionary<object, Dictionary<string, SqlParameter[]>> DicSQLStringList = new Dictionary<object, Dictionary<string, SqlParameter[]>>();
                Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
                string sqlDel = "delete SysRoleUser where roleId=@roleId";
                SqlParameter[] param ={
                                        new SqlParameter("@roleId", roleId)
                                    };
                dic.Add(sqlDel, param);
                DicSQLStringList.Add(Guid.NewGuid(), dic);

                foreach (string userid in userId.Split(','))
                {
                    if (!string.IsNullOrEmpty(userid))
                    {
                        Dictionary<string, SqlParameter[]> dicinsert = new Dictionary<string, SqlParameter[]>();
                        string sql = "insert into SysRoleUser(id,roleId,UserID) values(NEWID(),@roleId,@UserID)";
                        SqlParameter[] para ={
                                        new SqlParameter("@roleId", roleId),
                                        new SqlParameter(string.Format("@UserID", userid), userid)
                                             };
                        dicinsert.Add(sql, para);
                        DicSQLStringList.Add(Guid.NewGuid(), dicinsert);
                    }
                }
                SqlHelper.ExecuteSqlTran(DicSQLStringList);
                return Json(true);
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
                SqlParameter[] param ={
                                        new SqlParameter("@roleId",roleId)
                                    };

                DataTable dt = SqlHelper.Query(sql, param).Tables[0];

                var result = QuerySuite.ToDictionary(dt, "parentId", "", "id", "child");
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
                Dictionary<object, Dictionary<string, SqlParameter[]>> DicSQLStringList = new Dictionary<object, Dictionary<string, SqlParameter[]>>();
                Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
                string sqlDel = "delete SysRoleOperatePower where roleId=@roleId and ModulePageID=@ModulePageID ";
                SqlParameter[] param ={
                                        new SqlParameter("@roleId", roleId),
                                        new SqlParameter("@ModulePageID",modulePageId)
                                    };
                dic.Add(sqlDel, param);
                DicSQLStringList.Add(Guid.NewGuid(), dic);
                //获取页面所有父级模块
                string ParentSql = "select * from fn_GetModuleParentID(@parentId)";
                SqlParameter[] Pparam ={
                                            new SqlParameter("@parentId",parentId)
                                      };
                DataTable Pdt = SqlHelper.Query(ParentSql, Pparam).Tables[0];
                //判断同一模块中其它页面的是否配置了权限
                string sqlSelect = "select count(1) from SysRoleOperatePower where roleId=@roleId and ModuleParentID=@ModuleParentID ";
                SqlParameter[] paramSelect ={
                                            new SqlParameter("@roleId", roleId),
                                            new SqlParameter("@ModuleParentID",parentId)
                                            };
                int ParentCount = Convert.ToInt32(SqlHelper.GetSingle(sqlSelect, paramSelect));
                if (ParentCount < 2 && string.IsNullOrEmpty(operationSign))//如果同一模块中其它页面都没权限，且本页面取消所有权限，则删除本页面所有父级模块权限
                {
                    Dictionary<string, SqlParameter[]> dicDel = new Dictionary<string, SqlParameter[]>();
                    string sqlDelete = "delete SysRoleOperatePower where roleId=@roleId and ModulePageID=@ModulePageID ";
                    SqlParameter[] Deleteparam ={
                                        new SqlParameter("@roleId", roleId),
                                        new SqlParameter("@ModulePageID",parentId)
                                    };
                    dicDel.Add(sqlDelete, Deleteparam);
                    DicSQLStringList.Add(Guid.NewGuid(), dicDel);
                    foreach (DataRow dr in Pdt.Rows)
                    {
                        dicDel = new Dictionary<string, SqlParameter[]>();
                        sqlDelete = "delete SysRoleOperatePower where roleId=@roleId and ModulePageID=@ModulePageID ";
                        SqlParameter[] Deleteparam1 ={
                                        new SqlParameter("@roleId", roleId),
                                        new SqlParameter("@ModulePageID",dr["ModuleID"])
                                    };
                        dicDel.Add(sqlDelete, Deleteparam1);
                        DicSQLStringList.Add(Guid.NewGuid(), dicDel);
                    }
                }
                if (!string.IsNullOrEmpty(operationSign))
                {
                    Dictionary<string, SqlParameter[]> dicinsert = new Dictionary<string, SqlParameter[]>();
                    string sqlinser = @"INSERT INTO [dbo].[SysRoleOperatePower] ([id],[roleId],[ModulePageID],[operationSign],[ModuleParentID])
                                                 VALUES (newid(),@roleId,@ModulePageID,@operationSign,@ModuleParentID)";
                    SqlParameter[] paraminser ={
                                        new SqlParameter("@roleId", roleId),
                                        new SqlParameter("@ModulePageID",modulePageId),
                                        new SqlParameter("@operationSign",operationSign),
                                        new SqlParameter("@ModuleParentID",parentId)
                                    };
                    dicinsert.Add(sqlinser, paraminser);
                    DicSQLStringList.Add(Guid.NewGuid(), dicinsert);

                    foreach (DataRow dr in Pdt.Rows)
                    {
                        dicinsert = new Dictionary<string, SqlParameter[]>();
                        sqlinser = @"if (select COUNT(1)from [dbo].[SysRoleOperatePower] where [roleId]=@roleId and ModulePageID=@ModulePageID)=0
                                        begin
                                        INSERT INTO [dbo].[SysRoleOperatePower] ([id],[roleId],[ModulePageID],[operationSign])
                                                                                         VALUES (newid(),@roleId,@ModulePageID,@operationSign)
                                        end ";
                        SqlParameter[] para1 ={
                                        new SqlParameter("@roleId", roleId),
                                        new SqlParameter("@ModulePageID",dr["ModuleID"]),
                                        new SqlParameter("@operationSign","query"),
                                    };
                        dicinsert.Add(sqlinser, para1);
                        DicSQLStringList.Add(Guid.NewGuid(), dicinsert);
                    }
                }

                SqlHelper.ExecuteSqlTran(DicSQLStringList);
                return Json(new { Code = 0, Msg = "保存成功！" });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }



        #endregion

        #region 页面操作
        /// <summary>
        /// 获取页面操作数据
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryOperationData(int offset,int limit)
        {
            try
            {
                var list = dbContext.Set<SysOperation>().OrderBy(a => a.OrderNo).ToList();
                var displaylist = list.OrderBy(a => a.OrderNo).Skip(offset).Take(limit).ToList();
                return Json(new { Code = 0, Total = list.Count(), Data = displaylist });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 保存页面操作(新增、修改)
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveOperationData(Guid? id,string operationSign)
        {
            try
            {
                //标识唯一性验证
                var smodel = dbContext.Set<SysOperation>().FirstOrDefault(so => so.OperationSign == operationSign && so.Id != id);
                if (smodel != null)
                    return Json(new { Code = 1, Msg = "该标识已存在！" });

                SysOperation model = dbContext.Set<SysOperation>().FirstOrDefault(so => so.Id == id);
                if (model == null)
                {
                    model = new SysOperation();
                    this.ToModel(model);
                    model.Id = Guid.NewGuid();
                    model.CreateBy = Convert.ToString(SSOClient.UserId);
                    model.CreateTime = DateTime.Now;
                    model.OrderNo = SqlHelper.GetMaxID("OrderNo", "SysOperation");
                    dbContext.Set<SysOperation>().Add(model);
                }
                else
                {
                    this.ToModel(model);
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
        /// 加载页面操作数据
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadOperation(Guid id)
        {
            try
            {
                var model = dbContext.Set<SysOperation>().FirstOrDefault(so => so.Id == id);
                return Json(new { Code = 0, Data = model });
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
        public JsonResult DeleteOperation(string IDs)
        {
            try
            {
                var result = SqlHelper.ExecuteSql(QuerySuite.DeleteSql(IDs, "SysOperation", "id"));
                return Json(new { Code = 0, Msg = "删除成功！" });
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