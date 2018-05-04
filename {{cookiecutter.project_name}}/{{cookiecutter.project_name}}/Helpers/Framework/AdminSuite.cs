/***********************
 * BitAdmin2.0框架文件
 ***********************/
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace {{cookiecutter.project_name}}.Helpers
{
    #region QuerySuite

    /// <summary>
    /// 查询套件封装,与前端querySuite组件对应。
    /// 说明：项目根据需要扩展
    /// </summary>
    public class QuerySuite
    {
        Controller _controller;
        
        string _select = string.Empty;
        Dictionary<string, string> _filter = new Dictionary<string, string>();
        List<SqlParameter> _params = new List<SqlParameter>();

        string _ordertype, _orderby;

        /// <summary>
        /// 查询套件封装,与前端querySuite组件对应。
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="defaultOrder">默认排序字段名称</param>
        public QuerySuite(Controller controller, string defaultOrder)
        {
            if (string.IsNullOrEmpty(defaultOrder))
                throw new Exception("参数 [defaultOrder,默认排序字段] 不能为空");

            _controller = controller;
            if (defaultOrder.Contains(" "))
            {
                _orderby = defaultOrder.Split(' ')[0];
                _ordertype = defaultOrder.Split(' ')[1];
            }
            else
            {
                _orderby = defaultOrder;
                _ordertype = "asc";
            }
        }
        public string OrderBy=> _controller.HttpContext.Request.Form["orderby"].FirstOrDefault() ?? _orderby;
        public string OrderType=> _controller.HttpContext.Request.Form["ordertype"].FirstOrDefault() ?? _ordertype;
        public int Offset=> Convert.ToInt32(_controller.HttpContext.Request.Form["offset"].FirstOrDefault() ?? "0");
        public int Limit=> Convert.ToInt32(_controller.HttpContext.Request.Form["limit"].FirstOrDefault() ?? "10");
        public int StartRow=> Offset + 1;
        public int EndRow => Offset + Limit;

        public void Select(string select)
        {
            _select = select;
        }
        public void AddParam(string sql, SqlParameter para)
        {
            if (_params.FindAll(x => x.ParameterName == para.ParameterName).Count() > 0) return;
            _filter[para.ParameterName] = sql;
            _params.Add(para);
        }
        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="type"></param>
        /// <param name="para"></param>
        public void AddParam(string filed, string type, params object[] para)
        {
            //字段为空
            if (string.IsNullOrEmpty(filed)) return;

            //条件已经存在
            string parameter = "@" + filed.Replace(".", "");
            if (_params.FindAll(x => x.ParameterName == parameter).Count() > 0) return;

            string reqfiled = filed.IndexOf(".") > 0 ? filed.Split('.')[1] : filed;
            object value = _controller.HttpContext.Request.Form[reqfiled].FirstOrDefault();
            bool hasValue = !string.IsNullOrEmpty(Convert.ToString(value));
            bool hasPara = para.Length > 0 && !string.IsNullOrEmpty(Convert.ToString(para[0]));

            switch (type)
            {
                case "like":
                    if (hasPara | hasValue)
                    {
                        value = hasPara ? para[0] : value;
                        _filter[parameter] = string.Format(" and {0} {1} '%'+{2}+'%' ", filed, type, parameter);
                        _params.Add(new SqlParameter(parameter, value));
                    }
                    break;
                case "=":
                case ">":
                case ">=":
                case "<":
                case "<=":
                    if (hasPara | hasValue)
                    {
                        value = hasPara ? para[0] : value;
                        _filter[parameter] = string.Format(" and {0} {1} {2} ", filed, type, parameter);
                        _params.Add(new SqlParameter(parameter, value));
                    }
                    break;
                case "between":
                    break;
                case "in":
                    break;
                default:
                    break;

            }
        }


        bool isAdd = false;
        public string SqlString
        {
            get
            {
                if (!isAdd)
                {
                    AddParam(_controller.HttpContext.Request.Form["column"].FirstOrDefault(), "like", _controller.HttpContext.Request.Form["condition"].FirstOrDefault());
                    isAdd = true;
                }

                StringBuilder _sql = new StringBuilder();
                var sel = _select.Trim().Substring(0, 6);
                Regex regex = new Regex(sel);
                string selectSql = regex.Replace(_select, "select row_number()over (order by {0} {1}) rowNumber,", 1);
                if (!_select.Contains("where")) selectSql += " where 1=1 ";
                _sql.AppendFormat(selectSql, OrderBy, OrderType);

                foreach (var e in _filter)
                {
                    _sql.AppendLine(e.Value);
                }

                return _sql.ToString();
            }
        }
        public SqlParameter[] Params { get { return _params.ToArray(); } }

        /// <summary>
        /// 返回【总数】【数据】
        /// </summary>
        public string QuerySql
        {
            get
            {
                return string.Format(@"select count(1) from ({0}) t ;
                                       select * from ({0}) t where rowNumber between {1} and {2} ",
                                              SqlString, StartRow, EndRow);
            }
        }
        /// <summary>
        /// 返回【总数】
        /// </summary>
        public string QuerySqlTotal
        {
            get
            {
                return string.Format(@"select count(1) from ({0}) t ", SqlString);
            }
        }
        /// <summary>
        /// 返回【数据】
        /// </summary>
        public string QuerySqlTable
        {
            get
            {
                return string.Format(@"select * from ({0}) t where RowNumber between {1} and {2} ",
                                              SqlString, StartRow, EndRow);
            }
        }
        public string ExportSql { get { return SqlString; } }
        public static string DeleteSql(string ids, string table, params string[] keys)
        {
            List<string> list = new List<string>();
            foreach (string primary in ids.Split(','))
            {
                string[] value = primary.Split('_');

                List<string> filter = new List<string>();
                for (int i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    filter.Add(string.Format("{0}='{1}'", key, value[i]));
                }
                string sql = string.Format(" delete from {0} where {1} ", table, string.Join(" and ", filter.ToArray()));
                list.Add(sql);
            }
            return string.Join(";", list.ToArray());
        }
        public static string SortSql(string ids, string table,string order, params string[] keys)
        {
            List<string> list = new List<string>();
            foreach (string primary in ids.Split(','))
            {
                string[] value = primary.Split('_');

                List<string> filter = new List<string>();
                for (int i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    filter.Add(string.Format("{0}='{1}'", key, value[i]));
                }
                string sql = string.Format(" update {0} set {1}={2} where {3} ", table, order, list.Count + 1, string.Join(" and ", filter.ToArray()));
                list.Add(sql);
            }
            return string.Join(";", list.ToArray());
        }

        public static List<Dictionary<string, object>> ToDictionary(DataTable dt)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic[dc.ColumnName] = dr[dc.ColumnName] == DBNull.Value ? "" : dr[dc.ColumnName];
                }
                dicList.Add(dic);
            }
            return dicList;
        }
        public static List<Dictionary<string, object>> ToDictionary(DataTable data, string parentKey, string primaryKey, string propertyName = "children") {
            return ToDictionary(data, parentKey, "", primaryKey, propertyName);
        }
        public static List<Dictionary<string, object>> ToDictionary(DataTable data, string parentKey, string parentValue, string primaryKey, string propertyName)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

            DataRow[] rows = data.Select(string.Format("{0}='{1}'", parentKey, parentValue));
            if (string.IsNullOrEmpty(parentValue))
                rows = data.Select(string.Format("{0} is null", parentKey));
            foreach (DataRow dr in rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in data.Columns)
                {
                    dic[dc.ColumnName] = dr[dc.ColumnName] == DBNull.Value ? "" : dr[dc.ColumnName];
                }
                dic[propertyName] = ToDictionary(data, parentKey, Convert.ToString(dr[primaryKey]), primaryKey, propertyName);
                dicList.Add(dic);
            }
            if (dicList.Count == 0)
                return null;
            return dicList;
        }
    }

    #endregion

    #region FormSuite

    public class FormSuite
    {
        Controller _controller;
        public FormSuite(Controller controller)
        {
            _controller = controller;
        }

        public bool HasValue(string key)
        {
            return !string.IsNullOrEmpty(_controller.HttpContext.Request.Form[key].FirstOrDefault());
        }
        public string Value(string key)
        {
            return _controller.HttpContext.Request.Form[key].FirstOrDefault();
        }
        //public T Value<T>(string key)
        //{
        //    Type t = T.GetType();
        //    return ChangeType(_controller.Request[key], );
        //}
        public string FirstSql(string table, params string[] keys)
        {
            List<string> filter = new List<string>();
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                var value = _controller.HttpContext.Request.Form[key].FirstOrDefault();
                filter.Add(string.Format("{0}='{1}'", key, value));
            }
            return string.Format(" select * from {0} where {1} ", table, string.Join(" and ", filter.ToArray()));
        }
        public void ToModel<T>(T t) where T : class, new()
        {
            SetFormToModel(t, _controller.Request.Form);
        }

        /// <summary> 
        /// 将表单赋予对对象 
        /// </summary> 
        /// <param name="t">实体对象</param> 
        /// <param name="form">表单集合</param> 
        /// <param name="Updateby">最后更新时间</param>
        public static void SetFormToModel<T>(T t, IFormCollection form)
        {
            Type type = t.GetType();
            PropertyInfo[] pi = type.GetProperties();
            foreach (PropertyInfo p in pi)
            {
                if (!string.IsNullOrEmpty(form[p.Name].FirstOrDefault()))
                {
                    p.SetValue(t, ChangeType(form[p.Name].FirstOrDefault(), (Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType)), null);
                }
                else if (form[p.Name].FirstOrDefault() != null)
                {
                    p.SetValue(t, null);
                }
            }
        }

        private static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
    }
    #endregion
}