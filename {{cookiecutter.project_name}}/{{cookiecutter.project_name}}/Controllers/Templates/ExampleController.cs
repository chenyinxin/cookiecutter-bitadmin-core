/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace {{cookiecutter.project_name}}.Controllers
{
    [BitAuthorize]
    public class ExampleController : Controller
    {
        DataContext dbContext = new DataContext();

        #region 完整示例

        //获取示例
        public JsonResult QueryExample()
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "exampleName desc,CreateTime asc");
                querySuite.Select("select * from GeneralExample");
               
                querySuite.AddParam("exampleName", "like");

                DataSet ds = SqlHelper.Query(querySuite.QuerySql, querySuite.Params);

                return Json(new { Code = 0, Total = ds.Tables[0].Rows[0][0], Data = QuerySuite.ToDictionary(ds.Tables[1]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        //加载
        public JsonResult LoadExample(Guid? exampleId)
        {
            try
            {             
                 string sql = @"select e.*,s.userName exampleUserName,p.departmentName from GeneralExample e 
                                 left join SysUser s on e.ExampleUser=s.UserID 
                                 left join SysDepartment p on e.Department=p.DepartmentID 
                                 where e.exampleId=@exampleId";
                
                 DataTable dt = SqlHelper.Query(sql, new SqlParameter("@exampleId", exampleId)).Tables[0];
                 return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt).FirstOrDefault() });
                
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        //保存
        public JsonResult SaveExample(Guid? exampleId)
        {
            try
            {
                GeneralExample model = dbContext.GeneralExample.FirstOrDefault(s => s.ExampleId == exampleId);

                if (model != null)
                {
                    //修改
                    this.ToModel(model);
                }
                else
                {
                    //新增
                    model = new GeneralExample();
                    this.ToModel(model);
                    model.ExampleId = Guid.NewGuid();
                    model.CreateTime = DateTime.Now;
                    dbContext.GeneralExample.Add(model);
                }
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "保存成功" ,Data=model});
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        //删除
        public JsonResult DeleteExample(string IDs)
        {
            try
            {
                string sql = QuerySuite.DeleteSql(IDs, "GeneralExample", "exampleId");
                var result = SqlHelper.ExecuteSql(sql);

                return Json(new { Code = 0, Msg = "删除成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        //获取下拉联动,异步
        public JsonResult GetLinkageSelect(string type, Guid? parent)
        {
            try
            {
                var list = dbContext.SysDepartment.Where(x => x.ParentId == parent);
                return Json(new { Code = 0, Data = list });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        //获取下拉联动,同步
        public JsonResult QueryTreeTableData()
        {
            try
            {
                string sql = @"select * from SysDepartment order by DepartmentCode ";
                DataTable dt = SqlHelper.Query(sql).Tables[0];

                var result = QuerySuite.ToDictionary(dt, "ParentID", "DepartmentID");
                return Json(new { Code = 0, Data = result });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult QueryExampleTree()
        {
            try
            {
                string sql = @"select * from GeneralExample";
                DataTable dt = SqlHelper.Query(sql).Tables[0];

                var result = QuerySuite.ToDictionary(dt, "parentID", "exampleId");
                return Json(new { Code = 0, Total = result.Count, Data = result });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult LoadExampleTree(Guid? ID)
        {
            try
            {
                string sql = @" select e.*,Parent.exampleName as parentName,s.userName as userName, p.DepartmentName as departmentName from GeneralExample e 
                            left join GeneralExample Parent on e.ParentID=Parent.exampleId
                            left join SysUser s on e.ExampleUser=s.UserID 
                            left join SysDepartment p on e.Department=p.DepartmentID 
                            where e.exampleId=@ID";

                DataTable dt = SqlHelper.Query(sql, new SqlParameter("@ID", ID)).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt).FirstOrDefault() });
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