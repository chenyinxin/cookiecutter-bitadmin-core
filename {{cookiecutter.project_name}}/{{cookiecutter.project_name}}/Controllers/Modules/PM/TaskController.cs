using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers
{
    [BitAuthorize]
    public class TaskController : Controller
    {
        DataContext dbContext = new DataContext();
        #region 项目信息
        /// <summary>
        /// 获取项目信息数据
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryTaskData()
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "CreateTime desc");
                querySuite.Select("select * from PmTask");

                querySuite.AddParam("TaskName", "like");

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
        /// 保存项目信息(添加 修改)
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveTaskData(Guid TaskId)
        {
            try
            {
                PmTask model = dbContext.PmTask.FirstOrDefault(s => s.TaskId == TaskId);

                if (model == null)
                {
                    model = new PmTask();
                    this.ToModel(model);
                    model.TaskId = Guid.NewGuid();
                    dbContext.PmTask.Add(model);
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
        /// 加载项目信息
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadTaskData(Guid TaskId)
        {
            try
            {
                var model = dbContext.PmTask.FirstOrDefault(s => s.TaskId == TaskId);
                return Json(new { Code = 0, Data = model });

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 删除项目信息
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteTaskData(string IDs)
        {
            try
            {
                var result = SqlHelper.ExecuteSql(QuerySuite.DeleteSql(IDs, "PmTask", "TaskId"));
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
