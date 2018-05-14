using {{cookiecutter.project_name}}.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace {{cookiecutter.project_name}}.Controllers
{
    public class GeneratingController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public GeneratingController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public JsonResult One(string tablename)
        {
            try
            {
                string where = string.IsNullOrEmpty(tablename) ? "" : "where d.name='" + tablename + "'";
                string strSql = string.Format(@"SELECT 
                                    表名       = case when a.colorder=1 then d.name else '' end,
                                    表说明     = case when a.colorder=1 then isnull(f.value,'') else '' end,
                                    字段序号   = a.colorder,
                                    字段名     = a.name,
                                    标识       = case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,
                                    主键       = case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                                                     SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then '√' else '' end,
                                    类型       = b.name,
                                    占用字节数 = a.length,
                                    长度       = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
                                    小数位数   = isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
                                    非空     = case when a.isnullable=1 then ''else '√' end,
                                    默认值     = isnull(e.text,''),
                                    字段说明   = isnull(g.[value],'')
                                FROM syscolumns a left join systypes b on a.xusertype=b.xusertype
                                inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
                                left join syscomments e on a.cdefault=e.id
                                left join sys.extended_properties g on a.id=G.major_id and a.colid=g.minor_id  
                                left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0
                                {0}
                                order by d.name,a.colorder", where);

                SqlDataAdapter sda = new SqlDataAdapter(strSql, SqlHelper.ConnectionString);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                string[] extable = new string[] { "Example","FlowBills", "FlowBillsRecord", "FlowBillsRecordUser", "FlowMain", "FlowOrderCodes", "FlowStep", "FlowStepPath", "GeneralExample",
                    "SysAttachment", "SysDictionary", "SysLog", "SysModule", "SysModulePage", "SysOperation", "SysPageOperation", "SysRole", "SysRoleOperatePower", "SysRoleUser",
                    "SysServer",  "SysUserClientId", "SysUserOpenId" };

                List<Table> tables = new List<Table>();
                Table table = new Table();
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToString(row["字段序号"]) == "1" && extable.Count(x => x == Convert.ToString(row["表名"])) == 0)
                    {
                        table = new Table
                        {
                            Name = Convert.ToString(row["表名"]),
                            Remark = Convert.ToString(row["表说明"]),
                            Columns = new List<Column>()
                        };
                        tables.Add(table);
                    }
                    if (string.IsNullOrEmpty(tablename)) continue;
                    table.Columns.Add(new Column
                    {
                        Order = Convert.ToString(row["字段序号"]),
                        Label = Convert.ToString(row["字段说明"]),
                        Name = Convert.ToString(row["字段名"]),
                        IsKey = Convert.ToString(row["主键"]),
                        Length = Convert.ToString(row["长度"]),
                        Type = Convert.ToString(row["类型"]),
                        Scale = Convert.ToString(row["小数位数"]),
                        Value = Convert.ToString(row["默认值"]),
                        IsNull = Convert.ToString(row["非空"])
                    });
                }
                return Json(new { Code = 0, Data = tables });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public class Table
        {
            public string Name { get; set; }
            public string Remark { get; set; }
            public List<Column> Columns { get; set; }
        }

        public class Column
        {
            public string Order { get; set; }
            public string Label { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Length { get; set; }
            public string IsKey { get; set; }
            public string IsNull { get; set; }
            public string Scale { get; set; }
            public string Value { get; set; }

        }
        public JsonResult HtmlTemplates(string name,string title)
        {
            try
            {
                string html = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\codes", "default.html");
                var result = System.IO.File.ReadAllText(html);
                result = result.Replace("{name}", name).Replace("{title}", title);

                return Json(new { Code = 0, Data = HttpUtility.HtmlEncode(result) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public JsonResult CSharpTemplates(string name, string title)
        {
            try
            {
                string html = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\codes", "default_cs");
                var result = System.IO.File.ReadAllText(html);
                result = result.Replace("{name}", name).Replace("{title}", title);

                return Json(new { Code = 0, Data = HttpUtility.HtmlEncode(result) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public JsonResult FieldTemplates()
        {
            try
            {
                string fields = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\codes\\fields");
                var files = Directory.GetFiles(fields);
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach(var file in files)
                {
                    result.Add(Path.GetFileNameWithoutExtension(file).Replace("-", ""), HttpUtility.HtmlEncode(System.IO.File.ReadAllText(file)));
                }
                return Json(new { Code = 0, Data = result });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }
}
