/***********************
 * BitAdmin2.0框架文件
 ***********************/
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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
    /// 说明：项目根据需要扩展。
    /// 
    /// 需要重构：
    /// 重构1、支持mssql,mysql,oracle；
    /// </summary>
    public class QuerySuite
    {
        string _dbtype = "mssql";        //mysql,oracle

        string _select, _orderby;
        Dictionary<string, string> _filter = new Dictionary<string, string>();

        List<SqlParameter> _sqlParams = new List<SqlParameter>();
        List<MySqlParameter> _mysqlParams = new List<MySqlParameter>();

        Controller _controller;

        /// <summary>
        /// 查询套件封装,与前端querySuite组件对应。
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="defaultOrder">默认排序字段名称</param>
        public QuerySuite(Controller controller, string defaultOrder)
        {
            Init(controller, defaultOrder);
        }
        public QuerySuite(Controller controller, string defaultOrder,string dbtype)
        {
            _dbtype = dbtype;
            Init(controller, defaultOrder);
        }
        private void Init(Controller controller, string defaultOrder)
        {
            if (string.IsNullOrEmpty(defaultOrder))
                throw new Exception("参数 [defaultOrder,默认排序字段] 不能为空");

            _controller = controller;
            _orderby = defaultOrder;

            //添加前端传回的列筛选条件
            AddParam(_controller.HttpContext.Request.Form["column"].FirstOrDefault(), "like", _controller.HttpContext.Request.Form["condition"].FirstOrDefault());
        }
        private string OrderBy
        {
            get
            {
                var fo1= _controller.HttpContext.Request.Form["orderby"].FirstOrDefault();
                var fo2 = _controller.HttpContext.Request.Form["ordertype"].FirstOrDefault();
                if (!string.IsNullOrEmpty(fo1))
                    return string.Format("{0} {1}", fo1, fo2);
                else
                    return _orderby;
            }
        }
        private int Offset => Convert.ToInt32(_controller.HttpContext.Request.Form["offset"].FirstOrDefault() ?? "0");
        private int Limit => Convert.ToInt32(_controller.HttpContext.Request.Form["limit"].FirstOrDefault() ?? "10");
        private int StartRow => Offset + 1;
        private int EndRow => Offset + Limit;

        public void Select(string select)
        {
            _select = select;
        }
        public void AddParam(string sql, SqlParameter para)
        {
            if (_sqlParams.FindAll(x => x.ParameterName == para.ParameterName).Count() > 0) return;
            _filter[para.ParameterName] = sql;
            _sqlParams.Add(para);
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
            if (_sqlParams.FindAll(x => x.ParameterName == parameter).Count() > 0) return;

            string reqfiled = filed.IndexOf(".") > 0 ? filed.Split('.')[1] : filed;
            object value = _controller.HttpContext.Request.Form[reqfiled].FirstOrDefault();
            bool hasValue = !string.IsNullOrEmpty(Convert.ToString(value));
            bool hasParam = para.Length > 0 && !string.IsNullOrEmpty(Convert.ToString(para[0]));

            switch (type)
            {
                case "like":
                    if (hasParam | hasValue)
                    {
                        value = hasParam ? para[0] : value;
                        _filter[parameter] = string.Format(" and {0} {1} '%'+{2}+'%' ", filed, type, parameter);
                        _sqlParams.Add(new SqlParameter(parameter, value));
                    }
                    break;
                case "=":
                case ">":
                case ">=":
                case "<":
                case "<=":
                    if (hasParam | hasValue)
                    {
                        value = hasParam ? para[0] : value;
                        _filter[parameter] = string.Format(" and {0} {1} {2} ", filed, type, parameter);
                        _sqlParams.Add(new SqlParameter(parameter, value));
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

        public string SqlString
        {
            get
            {
                //分页之前的总查询
                switch (_dbtype)
                {
                    case "mssql":
                        Regex regex = new Regex(_select.Trim().Substring(0, 6));
                        string selectSql = regex.Replace(_select, "select row_number()over (order by {0}) rowNumber,", 1);
                        if (!_select.Contains("where")) selectSql += " where 1=1 ";

                        StringBuilder _sbSql = new StringBuilder();
                        _sbSql.AppendFormat(selectSql, OrderBy);
                        foreach (var e in _filter)
                        {
                            _sbSql.AppendLine(e.Value);
                        }
                        return _sbSql.ToString();
                    case "mysql":
                        return "";
                    case "oracle":
                        return "";
                    default:
                        return "";
                }
            }
        }
        public SqlParameter[] Params { get { return _sqlParams.ToArray(); } }

        public string QuerySql
        {
            get
            {
                switch (_dbtype)
                {
                    case "mssql":
                        return string.Format(@"select count(1) from ({0}) t ;select * from ({0}) t where rowNumber between {1} and {2} ",SqlString, StartRow, EndRow);
                    case "mysql":
                        return "";
                    case "oracle":
                        return "";
                    default:
                        return "";
                }
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
        public static string SortSql(string ids, string table, string order, params string[] keys)
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

        public static List<Dictionary<string, object>> ToDictionary(DataTable dt, string filter = "")
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Select(filter))
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic[FormatKey(dc.ColumnName)] = dr[dc.ColumnName] == DBNull.Value ? "" : dr[dc.ColumnName];
                }
                dicList.Add(dic);
            }
            return dicList;
        }
        public static List<Dictionary<string, object>> ToDictionary(DataTable data, string parentKey, string primaryKey, string propertyName = "children")
        {
            return ToDictionary(data, parentKey, "", primaryKey, propertyName);
        }
        public static List<Dictionary<string, object>> ToDictionary(DataTable data, string parentKey, string parentValue, string primaryKey, string propertyName)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

            DataRow[] rows = data.Select(string.Format("{0}='{1}'", parentKey, parentValue));
            if (string.IsNullOrEmpty(parentValue))
                rows = data.Select(string.Format("{0} is null or {0}='' ", parentKey));
            foreach (DataRow dr in rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in data.Columns)
                {
                    dic[FormatKey(dc.ColumnName)] = dr[dc.ColumnName] == DBNull.Value ? "" : dr[dc.ColumnName];
                }
                dic[FormatKey(propertyName)] = ToDictionary(data, parentKey, Convert.ToString(dr[primaryKey]), primaryKey, propertyName);
                dicList.Add(dic);
            }
            return dicList;
        }
        public static List<Dictionary<string, object>> ToDictionary<T>(List<T> data, string parentKey, string primaryKey, string propertyName = "children")
        {
            return ToDictionary(data, parentKey, "", primaryKey, propertyName);
        }
        public static List<Dictionary<string, object>> ToDictionary<T>(List<T> data, string parentKey, string parentValue, string primaryKey, string propertyName)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

            var list = data.FindAll(x => {
                var xplist = x.GetType().GetProperties();
                foreach (var p in xplist)
                {
                    if (p.Name.ToLower() == parentKey.ToLower()) 
                        return Convert.ToString(p.GetValue(x)) == Convert.ToString(parentValue);
                }
                return false;
            });
            foreach (var dr in list)
            {
                var plist = dr.GetType().GetProperties();
                PropertyInfo pkey = null;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (var p in plist)
                {
                    dic[FormatKey(p.Name)] = p.GetValue(dr);
                    if (p.Name.ToLower() == primaryKey.ToLower()) pkey = p;
                }
                dic[FormatKey(propertyName)] = ToDictionary(data, parentKey, Convert.ToString(pkey.GetValue(dr)), primaryKey, propertyName);
                dicList.Add(dic);
            }
            return dicList;
        }
        private static string FormatKey(string msg)
        {

            int lastUpper = 0;
            char[] msgArr = msg.ToCharArray();

            for (int i = 0; i < msgArr.Length; i++)
            {
                bool isUpper = (msgArr[i] >= 'A' && msgArr[i] <= 'Z');
                if (isUpper && lastUpper == i - 1)
                    msgArr[i] = msgArr[i].ToString().ToLower()[0];
                if (isUpper) lastUpper = i;
            }
            if (msgArr.Length > 0) msgArr[0] = msgArr[0].ToString().ToLower()[0];
            return new StringBuilder().Append(msgArr).ToString();

            //char[] msgArr = msg.ToCharArray();
            //char[] result = msg.ToCharArray();
            //for (int i = 0; i < msgArr.Length; i++)
            //{
            //    if ((i == 0 || i == msgArr.Length - 1) && msgArr[i] >= 'A' && msgArr[i] <= 'Z')
            //        result[i] = msgArr[i].ToString().ToLower()[0];
            //    else if (i + 1 < msgArr.Length && msgArr[i] >= 'A' && msgArr[i] <= 'Z' && msgArr[i - 1] >= 'A' && msgArr[i - 1] <= 'Z')
            //        result[i] = msgArr[i].ToString().ToLower()[0];
            //}
            //return new StringBuilder().Append(result).ToString();
        }
    }

    #endregion

    #region FormSuite

    public static class FormSuite
    {
        /// <summary> 
        /// 将表单值赋予对象
        /// </summary> 
        /// <param name="t">实体对象</param> 
        public static void ToModel<T>(this Controller controller, T t)
        {
            Type type = t.GetType();
            PropertyInfo[] pi = type.GetProperties();
            foreach (PropertyInfo p in pi)
            {
                if (!string.IsNullOrEmpty(controller.Request.Form[p.Name].FirstOrDefault()))
                {
                    p.SetValue(t, ChangeType(string.Join("|", controller.Request.Form[p.Name].ToArray()), (Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType)), null);
                }
                else if (controller.Request.Form[p.Name].FirstOrDefault() != null)
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