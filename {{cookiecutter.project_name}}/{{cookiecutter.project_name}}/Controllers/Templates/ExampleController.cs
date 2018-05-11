/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
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
                QuerySuite querySuite = new QuerySuite(this, "CreateTime asc");

                querySuite.Select("select * from Example");
               
                querySuite.AddParam("ExampleName", "like");

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
        public JsonResult LoadExample(Guid? ID)
        {
            try
            {
         
                if (!ID.HasValue)
                    return Json(new { Code = 0, Msg = "" });
             
                 string sql = @"select e.*,s.userName,p.departmentName from Example e 
                                 left join SysUser s on e.ExampleUser=s.UserID 
                                 left join SysDepartment p on e.Department=p.DepartmentID 
                                 where e.ID=@ID";
                
                 DataTable dt = SqlHelper.Query(sql, new SqlParameter("@ID", ID)).Tables[0];
                 return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt).FirstOrDefault() });
                
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        //保存
        public JsonResult SaveExample(Guid? ID)
        {
            try
            {
                Example model = dbContext.Example.FirstOrDefault(s => s.Id ==ID);

                if (model != null)
                {
                    //修改
                    this.ToModel(model);
                }
                else
                {
                    //新增
                    model = new Example();
                    this.ToModel(model);
                    model.Id = Guid.NewGuid();
                    model.CreateTime = DateTime.Now;
                    dbContext.Example.Add(model);
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
                string sql = QuerySuite.DeleteSql(IDs, "Example", "ID");
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
                string sql = @"select *,dbo.fn_GetDepartmentFullName(DepartmentID) FullName from SysDepartment order by DepartmentCode ";
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
                string sql = @"select * from Example";
                DataTable dt = SqlHelper.Query(sql).Tables[0];

                var result = QuerySuite.ToDictionary(dt, "ParentID", "", "ID", "child");
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
                if (!ID.HasValue)
                    return Json(new { Code = 0, Msg = "" });

                string sql = @" select e.*,Parent.exampleName as parentName,s.userName as userName, p.DepartmentName as departmentName from Example e 
                            left join Example Parent on e.ParentID=Parent.ID
                            left join SysUser s on e.ExampleUser=s.UserID 
                            left join SysDepartment p on e.Department=p.DepartmentID 
                            where e.ID=@ID";

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