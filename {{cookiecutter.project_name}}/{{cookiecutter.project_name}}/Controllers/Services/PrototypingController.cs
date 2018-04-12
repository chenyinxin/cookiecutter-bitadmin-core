using {{cookiecutter.project_name}}.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers
{
    /// <summary>
    /// 快速原型数据（数据放Prototyping文件夹）
    /// </summary>
    public class PrototypingController: Controller
    {
        string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prototyping");
        public JsonResult Query(string sign)
        {
            try
            {
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prototyping", sign + ".txt");
                var lines = System.IO.File.ReadAllLines(file, Encoding.GetEncoding("gb2312"));
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    Dictionary<string, string> row = new Dictionary<string, string>();
                    var items = line.Split("\t");
                    foreach (var val in items)
                    {
                        row["c" + (row.Count + 1)] = val;
                    }
                    row["sign"] = sign;
                    result.Add(row);
                }

                return Json(new { Code = 0, Total = result.Count, Data = result });
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
        public JsonResult Load(string sign,string c1)
        {
            try
            {
                string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prototyping", sign + ".txt");
                var lines = System.IO.File.ReadAllLines(file, Encoding.GetEncoding("gb2312"));
                Dictionary<string, string> row = new Dictionary<string, string>();

                foreach (var line in lines)
                {
                    var items = line.Split("\t");
                    if (items[0] != c1)
                        continue;

                    foreach (var val in items)
                    {
                        row["c" + (row.Count + 1)] = val;
                    }
                }
                row["sign"] = sign;
                return Json(new { Code = 0, Data = row });
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
                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prototyping");
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
