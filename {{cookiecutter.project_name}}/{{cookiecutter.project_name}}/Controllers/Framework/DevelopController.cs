/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers
{
    [BitAuthorize]
    public class DevelopController: Controller
    {
        DataContext dbContext = new DataContext();

        #region 系统菜单
        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryMeunsData()
        {
            try
            {
                string sql = @"select * from (SELECT moduleId AS id, parentId, moduleName AS name,'' as url,moduleIcon as icon,'' as pageSign,0 AS [type],orderNo
                               FROM SysModule module
                               UNION
                               SELECT  id, ModuleID AS parentId, PageName AS Name,PageUrl as Url, PageIcon as Icon,PageSign, 1 AS [Type],OrderNo
                               FROM SysModulePage) t order by OrderNo";
                DataTable dt = SqlHelper.Query(sql).Tables[0];

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
        /// 加载模块详情
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadModuleData(Guid? id)
        {
            try
            {
                string sql = @" select m.*,p.ModuleName as parentName from SysModule m 
                            left join SysModule p on m.parentId=p.ModuleID where m.ModuleID=@id";

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
                    int OrderNo = (int)SqlHelper.GetSingle(@"select ISNULL(MAX(OrderNo),0) from SysModule where ISNULL(cast(parentId as varchar(50)),'')=@parentId and OrderNo <90 ",
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
                                        SELECT  parentId,OrderNo FROM SysModule module
                                        UNION
                                        SELECT  ModuleID AS parentId ,OrderNo FROM SysModulePage) t where parentId=@parentId",
                                        new SqlParameter("@parentId", model.ModuleId));
                    model.OrderNo = OrderNo + 1;
                    dbContext.SysModulePage.Add(model);

                    //添加操作和权限
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.Id, OperationSign = "query" });
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.Id, OperationSign = "add" });
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.Id, OperationSign = "save" });
                    dbContext.SysPageOperation.Add(new SysPageOperation() { Id = Guid.NewGuid(), PageId = model.Id, OperationSign = "delete" });
                    dbContext.SysRoleOperatePower.Add(new SysRoleOperatePower() { Id = Guid.NewGuid(), RoleId = Guid.Parse("3F9578C5-C0A2-4C7A-B0FD-C93FAE47194B"), ModulePageId = model.Id, ModuleParentId = model.ModuleId, OperationSign = "query,add,save,delete" });
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
        public JsonResult QueryMeunsDataForSql(string id, string type)
        {
            try
            {
                string sql = type == "SysModule" ? @"select * from SysModule where ModuleId  = '" + id + "'" : @"select * from SysModulePage where Id  = '" + id + "'";
                DataTable dt = SqlHelper.Query(sql).Tables[0];

                var result = QuerySuite.ToDictionary(dt);
                return Json(new { Code = 0, Data = result });
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
        public JsonResult QueryOperationData(int offset, int limit)
        {
            try
            {
                var list = dbContext.Set<SysOperation>().OrderBy(a => a.OrderNo).Skip(offset).Take(limit).ToList();
                return Json(new { Code = 0, Total = dbContext.Set<SysOperation>().Count(), Data = list });
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
        public JsonResult SaveOperationData(Guid? id, string operationSign)
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
        public JsonResult LoadOperationData(Guid id)
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
        public JsonResult DeleteOperationData(string IDs)
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
        /// <summary>
        /// 排序页面操作
        /// </summary>
        /// <returns></returns>
        public JsonResult SortOperationData(string IDs)
        {
            try
            {
                var result = SqlHelper.ExecuteSql(QuerySuite.SortSql(IDs, "SysOperation", "orderNo", "id"));
                return Json(new { Code = 0, Msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion

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
        #endregion
    }
}
