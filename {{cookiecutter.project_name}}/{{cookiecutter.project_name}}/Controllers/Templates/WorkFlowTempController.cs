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
    public class WorkFlowTempController : Controller
    {
        DataContext dbContext = new DataContext();

        #region 完整示例

        public ActionResult QueryCompleteExample()
        {

            try
            {
                QuerySuite querySuite = new QuerySuite(this, "exampleName desc");
                querySuite.Select("select a.*,b.userName,c.departmentName from GeneralExample a left join SysUser b on a.UserPicker=b.UserID left join SysDepartment c on a.OUPicker=c.DepartmentID");

                querySuite.AddParam("ExampleName", "like");
                querySuite.AddParam("PlainText", "=");
                querySuite.AddParam("DateTimePicker", ">=");

                DataSet ds = SqlHelper.Query(querySuite.QuerySql, querySuite.Params);

                return Json(new { Code = 0, Total = ds.Tables[0].Rows[0][0], Data = QuerySuite.ToDictionary(ds.Tables[1]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
       
        [HttpPost]
        public ActionResult LoadCompleteExample(Guid? ExampleID)
        {
            try
            {
                if(!ExampleID.HasValue)
                    return Json(new { Code = 0, Msg = "" });

                string sql = @"select a.*,b.userName,c.departmentName from GeneralExample a 
                                left join SysUser b on a.UserPicker=b.UserID 
                                left join SysDepartment c on a.OUPicker=c.DepartmentID 
                                where a.ExampleID=@ExampleID";

                DataTable dt = SqlHelper.Query(sql, new SqlParameter("@ExampleID", ExampleID)).Tables[0];
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(dt).FirstOrDefault() });
            }
            catch(Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });

            }
        }
        [HttpPost]
        public ActionResult SaveCompleteExample(Guid? ExampleID)
        {
            try
            {
                FormSuite formSuite = new FormSuite(this);

                GeneralExample model = new GeneralExample();
                model = dbContext.GeneralExample.FirstOrDefault(x => x.ExampleId == ExampleID);
                if (model != null)
                {
                    //修改操作
                    formSuite.ToModel(model);
                }
                else
                {
                    model = new GeneralExample();
                    formSuite.ToModel(model);
                    model.ExampleId = Guid.NewGuid();
                    dbContext.GeneralExample.Add(model);
                }
                dbContext.SaveChanges();

                return Json(new { Code = 0, Msg = "保存成功", Data = model });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        [HttpGet]
        public ActionResult DeleteCompleteExample(string IDs)
        {
            try
            {
                string sql = QuerySuite.DeleteSql(IDs, "GeneralExample", "ExampleID");
                var result = SqlHelper.ExecuteSql(sql);
                
                return Json(new { Code = 0, Msg = "删除成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        [HttpPost]
        public FileResult ExportCompleteExample()
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "ExampleName desc");                
                querySuite.Select("select a.*,b.userName,c.departmentName from GeneralExample a left join SysUser b on a.UserPicker=b.UserID left join SysDepartment c on a.OUPicker=c.DepartmentID");
                
                querySuite.AddParam("ExampleName", "like");
                querySuite.AddParam("PlainText", "=");
                querySuite.AddParam("DateTimePicker", ">=");

                DataSet ds = SqlHelper.Query(querySuite.ExportSql, querySuite.Params);

                return null;

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return null;
            }
        }
        #endregion
    }
}