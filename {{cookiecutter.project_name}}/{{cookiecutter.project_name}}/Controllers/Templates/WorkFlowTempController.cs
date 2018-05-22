/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

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
        public ActionResult SaveCompleteExample(Guid? ExampleID)
        {
            try
            {

                GeneralExample model = new GeneralExample();
                model = dbContext.GeneralExample.FirstOrDefault(x => x.ExampleId == ExampleID);
                if (model != null)
                {
                    //修改操作
                    this.ToModel(model);
                }
                else
                {
                    model = new GeneralExample();
                    this.ToModel(model);
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
        public ActionResult ImportCompleteExample()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("import/{0:yyyyMMdd}/{1}", DateTime.Now, Guid.NewGuid()), file.FileName);
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                using (var stream = System.IO.File.Create(fileName))
                    file.CopyTo(stream);

                var dt = ExcelHelper.ExcelToDataTable(fileName, "");

                //这里写入库代码

                return Json(new { Code = 0, Msg = "导入成功", Data = QuerySuite.ToDictionary(dt) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public ActionResult ExportCompleteExample()
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "ExampleName desc");                
                querySuite.Select("select a.*,b.userName,c.departmentName from GeneralExample a left join SysUser b on a.UserPicker=b.UserID left join SysDepartment c on a.OUPicker=c.DepartmentID");
                
                querySuite.AddParam("ExampleName", "like");
                querySuite.AddParam("PlainText", "=");
                querySuite.AddParam("DateTimePicker", ">=");

                DataSet ds = SqlHelper.Query(querySuite.ExportSql, querySuite.Params);

                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("export/{0:yyyyMMdd}/{1}.xlsx", DateTime.Now, Guid.NewGuid()));
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                ExcelHelper.DataTableToExcel(ds.Tables[0], fileName, "导出Sheet");
                FileStream fs = new FileStream(fileName, FileMode.Open);
                return File(fs, "application/vnd.ms-excel", "export.xlsx");
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