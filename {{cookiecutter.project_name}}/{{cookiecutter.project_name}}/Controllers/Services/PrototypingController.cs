/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace {{cookiecutter.project_name}}.Controllers
{
    /// <summary>
    /// 快速原型数据（数据放prototyping文件夹）
    /// </summary>
    public class PrototypingController: Controller
    {
        string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "prototyping");
        public JsonResult Query(string module,string page)
        {
            try
            {
                string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "prototyping", module + ".xlsx");
                DataTable dt = ExcelHelper.ExcelToDataTable(file, page);
                dt.Columns.Add("module");
                dt.Columns.Add("page");
                dt.Columns.Add("key");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    row["module"] = module;
                    row["page"] = page;
                    row["key"] = Convert.ToString(row[0]) + i;
                }
                if (dt.Rows.Count > 2)
                {
                    dt.Rows.RemoveAt(0);
                    dt.Rows.RemoveAt(0);
                }
                return Json(new { Code = 0, Total = dt.Rows.Count, Data = QuerySuite.ToDictionary(dt) });
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
        public JsonResult Load(string module, string page, string key)
        {
            try
            {
                string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "prototyping", module + ".xlsx");
                DataTable dt = ExcelHelper.ExcelToDataTable(file, page);
                dt.Columns.Add("module");
                dt.Columns.Add("page");
                dt.Columns.Add("key");
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    DataRow row = dt.Rows[i];
                    row["module"] = module;
                    row["page"] = page;
                    row["key"] = Convert.ToString(row[0]) + i;
                }
                return Json(new { Code = 0, Total = dt.Rows.Count, Data = QuerySuite.ToDictionary(dt, "key='" + key + "'")[0] });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        public JsonResult Save(Guid? UserID, string userCode)
        {
            try
            {
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
        public JsonResult Delete(string ids)
        {
            try
            {
                return Json(new { Code = 0, Msg = "删除成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        /// <summary>
        /// 生成页面模板
        /// http://localhost:51671/prototyping/code
        /// </summary>
        /// <returns></returns>
        public JsonResult Code()
        {
            try
            {
                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "prototyping");
                foreach (var file in Directory.GetFiles(folder))
                {
                    if (!file.ToLower().EndsWith(".txt")) continue;

                    Dictionary<string, string> row = new Dictionary<string, string>();
                    var lines = System.IO.File.ReadAllLines(file);
                    string line = lines[0];

                    foreach (var val in line.Split("\t"))
                    {
                        row["c" + (row.Count + 1)] = val;
                    }
                    string sign = Path.GetFileNameWithoutExtension(file); 

                    //生成代码
                }
                
                return Json(new { Code = 0, Msg = "生成成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }
}
