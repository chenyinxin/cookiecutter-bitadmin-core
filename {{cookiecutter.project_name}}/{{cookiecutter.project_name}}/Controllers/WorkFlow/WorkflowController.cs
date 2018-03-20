/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsieJavaScriptEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace {{cookiecutter.project_name}}.Controllers
{

    [BitAuthorize]
    public class WorkflowController : Controller
    {
        #region 注释

        /*
         * 1.流程状态
         * 审批中：run
         * 已结束：end
         * 作废：void
         * 自动运行：Auto
         * 待办：ToDo
         * 已办：Done
         * 待阅：ToRead
         * 已阅：ReadEnd
         * 
         * 2.审批结果
         * 同意：1
         * 退回：10
         * 转交：50
         * 
         * 3.流程按钮
         * 发起按钮：0
         * 关闭按钮：100
         * 
         * 4.流程环节类型
         * 开始环节：1
         * 结束环节：100
         * 
         * 5.后续步骤启动方式[auto,select]
         * 自动运行：auto
         * 基于选择：select
         * 
         * 6.短信模版 关键字替换
         * 工单编号：$WorkOrderCode$
         * 工单名称：$WorkOrderName$
         * 流程名称：$FlowName$
         * 环节名称：$StepName$
         * 提交人名称：$UserName$
         * 提交人帐号：$UserCode$
         * 提交时间：$StartTime$
         */

        #endregion

        WorkflowContext dbContext = new WorkflowContext();

        #region 流程配置管理

        /// <summary>
        /// 获取流程主表列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFlowMainList()
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "Code ASC");
                querySuite.Select("select * from FlowMain");
                querySuite.AddParam("Name", "like");
                querySuite.AddParam("Code", "like");
                DataSet ds = SqlHelper.Query(querySuite.QuerySql, querySuite.Params);
                return Json(new { Code = 0, Total = ds.Tables[0].Rows[0][0], Data = QuerySuite.ToDictionary(ds.Tables[1]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        public ActionResult LoadFlowMain(Guid? Id)
        {
            try
            {
                if (!Id.HasValue)
                    return Json(new { Code = 0, Msg = "" });

                var model = dbContext.FlowMain.Find(Id.Value);

                return Json(new { Code = 0, Data = model });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });

            }
        }
        /// <summary>
        /// 新增或修改主表数据
        /// </summary>
        /// <param name="id">流程主表主件</param>
        /// <param name="ApplicationID"></param>
        /// <returns></returns>
        public ActionResult NewOrEidtFlowMain(Guid? Id, string Code)
        {
            try
            {
                List<FlowMain> models = dbContext.FlowMain.Where(x => x.Id == Id || x.Code == Code).ToList();
                FlowMain model = null;

                //部门编号唯一性验证
                if (models.FirstOrDefault(x => x.Id != Id && x.Code == Code) != null)
                    return Json(new { Code = 1, Msg = "流程标识已经存在，请重新输入！" });

                model = models.FirstOrDefault(x => x.Id == Id);
                FormSuite formSuite = new FormSuite(this);
                if (model != null)
                {
                    //修改
                    formSuite.ToModel(model);
                }
                else
                {
                    //新增
                    model = new FlowMain();
                    formSuite.ToModel(model);
                    Id = Guid.NewGuid();
                    model.Id = Id.Value;
                    dbContext.FlowMain.Add(model);
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
        /// 删除主表数据
        /// </summary>
        /// <param name="id">流程主表主件</param>
        /// <returns></returns>
        public JsonResult DelFlowMain(string IDs)
        {
            try
            {
                Dictionary<string, SqlParameter[]> dicsql = new Dictionary<string, SqlParameter[]>();
                string delTxt = string.Empty;
                int i = 0;
                foreach (string id in IDs.Split(','))
                {
                    string paramName = "@ID" + i++;
                    SqlParameter[] sqlpara = new SqlParameter[] { new SqlParameter(paramName, id) };

                    delTxt = "delete FlowMain where Id = " + paramName;
                    dicsql.Add(delTxt, sqlpara);
                    delTxt = "delete FlowStep where mainId = " + paramName;
                    dicsql.Add(delTxt, sqlpara);
                    delTxt = "delete FlowStepPath where mainId = " + paramName;
                    dicsql.Add(delTxt, sqlpara);
                }
                SqlHelper.ExecuteSqlTran(dicsql);
                return Json(new { Code = 0, Msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取流程的步骤与关联
        /// </summary>
        /// <param name="mainId">流程主表主键</param>
        /// <returns></returns>
        public ActionResult GetStepAndPath(string mainId)
        {
            try
            {
                if (string.IsNullOrEmpty(mainId))
                    return Json(new { Code = 0, Msg = "" });

                List<StepAndPath> sapList = null;
                if (!string.IsNullOrWhiteSpace(mainId))
                {
                    //获取该流程主表所关联的所有步骤
                    List<FlowStep> fsList = dbContext.Set<FlowStep>().Where(o => o.MainId.ToLower() == mainId.ToLower()).ToList();
                    if (fsList != null && fsList.Count > 0)
                    {
                        //获取步骤表数据里的 “配置界面的元素信息”字段，转换为 StepAndPath 类，输出到页面
                        foreach (FlowStep s in fsList)
                        {
                            if (!string.IsNullOrWhiteSpace(s.DeployInfo))
                            {
                                //json字符串转为类
                                StepAndPath sap = JsonConvert.DeserializeObject<StepAndPath>(s.DeployInfo);

                                if (sap != null && !string.IsNullOrWhiteSpace(sap.id))
                                {
                                    if (sapList == null)
                                    {
                                        sapList = new List<StepAndPath>();
                                    }
                                    sapList.Add(sap);
                                }
                            }
                        }
                    }
                }
                return Json(new { Code = 0, Data = sapList });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取Roles表所有数据的主键与名字
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleAll()
        {
            try
            {
                var result = dbContext.SysRole.Select(a => new
                {
                    Id = a.Id,
                    Name = a.RoleName
                });
                return Json(new { Code = 0, Data = result });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取DGUser表所有数据的主键与名字
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserAll()
        {
            try
            {
                var result = dbContext.SysUser.Select(a => new
                {
                    Id = a.UserId,
                    Name = a.UserName
                });
                return Json(new { Code = 0, Data = result });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetFlowConfigs()
        {
            try
            {
                var configs = dbContext.FlowMain.ToList();
                return Json(new { Code = 0, Data = configs });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 保存流程的步骤与关联
        /// </summary>
        /// <param name="stepList">流程步骤与关联的对象数组</param>
        /// <returns></returns>
        public ActionResult SaveStepAndPath(List<StepAndPath> stepList)
        {
            try
            {
                if (stepList == null || stepList.Count == 0 || string.IsNullOrWhiteSpace(stepList[0].MainId))
                {
                    return Json(new { Code = 0, Msg = "保存成功" });
                }

                string mainId = stepList[0].MainId.ToLower();
                //获取该流传已有的所有关联，先删除
                List<FlowStepPath> fspList = dbContext.FlowStepPath.Where(o => o.MainId.ToLower() == mainId).ToList();
                if (fspList != null && fspList.Count > 0)
                {
                    foreach (FlowStepPath sp in fspList)
                    {
                        dbContext.FlowStepPath.Remove(sp);
                    }
                }
                //获取该流传已有的所有关联，先删除
                List<FlowStep> fsList = dbContext.FlowStep.Where(o => o.MainId.ToLower() == mainId).ToList();
                if (fsList != null && fsList.Count > 0)
                {
                    foreach (FlowStep s in fsList)
                    {
                        dbContext.FlowStep.Remove(s);
                    }
                }
                dbContext.SaveChanges();
                foreach (StepAndPath sap in stepList)
                {
                    //给null的process_to属性赋空值
                    if (string.IsNullOrWhiteSpace(sap.process_to))
                    {
                        sap.process_to = "";
                    }
                    FlowStep fs = new FlowStep
                        {
                            Id = Guid.Parse(sap.id),
                            MainId = sap.MainId,
                            AuditId = sap.AuditId,
                            AuditNorm = sap.AuditNorm,
                            AuditLinkCode = sap.RelationStepKey,
                            Auditors = sap.Auditors,
                            DeployInfo =JsonConvert.SerializeObject(sap), //把类转为json字符串
                            Description = sap.Description,
                            StepName = sap.StepName,
                            StepStatus = sap.StepStatus,
                            LinkCode = sap.LinkCode,
                            RunMode = sap.RunMode,
                            Function = sap.Function,
                            ShowTabIndex = sap.ShowTabIndex,
                            Circularize = sap.Circularize,
                            ReminderTimeout = sap.ReminderTimeout,
                            SmsTemplateToDo = sap.SMSTemplateToDo,
                            SmsTemplateRead = sap.SMSTemplateRead,
                            AuditIdRead = sap.AuditIdRead,
                            AuditNormRead = sap.AuditNormRead
                        };
                    dbContext.FlowStep.Add(fs);
                    //获取该步骤的所有下步骤
                    if (!string.IsNullOrWhiteSpace(sap.process_to) && !string.IsNullOrWhiteSpace(sap.conditions))
                    {
                        string[] pList = sap.process_to.Split(',');
                        string[] cList = sap.conditions.Split(',');
                        //把步骤关联加入数据表
                        foreach (string p in pList)
                        {
                            foreach (string c in cList)
                            {
                                string[] condi = c.Split(':');
                                if (condi.Length > 1 && condi[0].ToLower() == p.ToLower() && !string.IsNullOrWhiteSpace(condi[1]))
                                {
                                    Guid nextId;
                                    if (!string.IsNullOrWhiteSpace(p) && Guid.TryParse(p, out nextId))
                                    {
                                        FlowStepPath fsp = new FlowStepPath
                                        {
                                            Id = Guid.NewGuid(),
                                            MainId = sap.MainId,
                                            UpStep = sap.id,
                                            NextStep = nextId.ToString(),
                                            Description = "",
                                            Condition = int.Parse(condi[1]),
                                            Expression = condi[3],
                                            BtnName = condi[2]
                                        };
                                        dbContext.FlowStepPath.Add(fsp);
                                    }
                                }
                            }
                        }
                    }
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
        #endregion

        #region 流程审批相关信息

        /// <summary>
        /// 获取当前审批环节
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult GetNowStep(string taskUserId, string mainCode, string linkCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(taskUserId))
                {
                    linkCode = GetStepLinkCodeByBillsReUserId(taskUserId);
                }
                if (string.IsNullOrEmpty(linkCode))
                {
                    linkCode = GetStepLinkCodeByMainCode(mainCode);
                }
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Code", mainCode));
                list.Add(new SqlParameter("@linkCode", linkCode));
                SqlParameter[] sqlParam = list.ToArray();
                string strSql = @"select t2.runMode from FlowMain as t1
                                left join FlowStep as t2 on t1.Id=t2.mainId
                                where t1.Code=@Code and t2.linkCode=@linkCode ";
                string runMode = Convert.ToString(SqlHelper.GetSingle(strSql, sqlParam));
                bool nowIsAgainHandleStep = false;
                //if (Data.RunMode == "auto")
                if (!string.IsNullOrEmpty(taskUserId))
                {
                    list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@taskUserId", taskUserId));
                    sqlParam = list.ToArray();
                    strSql = @"select COUNT(1) from FlowBillsRecordUser as t1 
                            left join FlowBillsRecord as t2 on t1.BillsRecordId=t2.Id
                            left join FlowBillsRecord as t3 on t2.BillsId=t3.BillsId and t3.NextStep=t1.stepId
                            where t1.Id=@taskUserId";
                    nowIsAgainHandleStep = Convert.ToInt32(SqlHelper.GetSingle(strSql, sqlParam)) > 1;
                }
                return Json(new { Code = 0, Data = new { runMode = runMode, NowIsAgainHandleStep = nowIsAgainHandleStep } });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        /// <summary>
        /// 根据待办Id 获取当前环节linkCode
        /// </summary>
        /// <param name="billsRecordUserId"></param>
        /// <returns></returns>
        private string GetStepLinkCodeByBillsReUserId(string billsRecordUserId)
        {
            string strSql = @"select t2.linkCode from FlowBillsRecordUser as t1
                            left join FlowStep as t2 on t1.stepId=t2.Id
                            where t1.Id=@billsRecordUserId ";
            return Convert.ToString(SqlHelper.GetSingle(strSql, new SqlParameter("@billsRecordUserId", billsRecordUserId)));
        }
        /// <summary>
        /// 根据mainCode 获取开始环节linkCode
        /// </summary>
        /// <param name="mainCode"></param>
        /// <returns></returns>
        private string GetStepLinkCodeByMainCode(string mainCode)
        {
            string strSql = string.Format(@"select t2.linkCode from FlowMain as t1
                            left join FlowStep as t2 on t1.Id=t2.mainId
                            where t2.stepStatus='{0}' and t1.Code=@mainCode ", 1);
            return Convert.ToString(SqlHelper.GetSingle(strSql, new SqlParameter("@mainCode", mainCode)));
        }

        /// <summary>
        /// 获取下一审批环节
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult GetNextStep(string taskUserId, string mainCode, string linkCode, List<DataParameter> expression, int condition)
        {
            try
            {
                if (!string.IsNullOrEmpty(taskUserId))
                {
                    linkCode = GetStepLinkCodeByBillsReUserId(taskUserId);
                }
                if (string.IsNullOrEmpty(linkCode))
                {
                    linkCode = GetStepLinkCodeByMainCode(mainCode);
                }
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Code", mainCode));
                list.Add(new SqlParameter("@linkCode", linkCode));
                list.Add(new SqlParameter("@condition", condition));
                SqlParameter[] sqlParam = list.ToArray();
                string strSql = @"select t1.Id as 'mainId',t1.Code as 'mainCode',t1.Name as 'mainName',t4.Id as 'stepId',t4.stepName,t4.linkCode,t4.stepStatus,t3.expression,t4.circularize
                from FlowMain as t1 
                left join FlowStep as t2 on t1.Id=t2.mainId
                left join FlowStepPath as t3 on t2.Id=t3.UpStep
                left join FlowStep as t4 on t3.NextStep=t4.Id
                where t1.Code=@Code and t2.linkCode=@linkCode and t3.condition=@condition ";
                var nextSteps = dbContext.GetNextStepOutParam.FromSql(strSql, sqlParam).ToList();
                List<GetNextStepOutParam> Data = new List<GetNextStepOutParam>();
                foreach (GetNextStepOutParam item in nextSteps)
                {
                    if (!string.IsNullOrEmpty(item.expression) && !RunJScript(item.expression, expression))
                    {
                        continue;
                    }
                    Data.Add(item);
                }
                return Json(new { Code = 0, Data = Data });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取审批环节处理人
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult GetStepUsers(Guid stepId, string taskUserId, string condition, string Type, int PageIndex, int PageSize)
        {
            try
            {
                DataTable Data = null;
                int Total = 0;
                DataSet ds = null;
                FlowStep oModel = dbContext.FlowStep.FirstOrDefault(t => t.Id == stepId);
                if (oModel.StepStatus == 100)
                {
                    Data = new DataTable();
                    Total = 0;
                    return Json(new { Code = 0, Data = QuerySuite.ToDictionary(Data), Total = Total });
                }
                if (condition == "10")
                {
                    ds = GetStepUsersByReturn(taskUserId, stepId);
                    Data = ds.Tables[0];
                    Total = 0;
                    return Json(new { Code = 0, Data = QuerySuite.ToDictionary(Data), Total = Total });
                }
                if (oModel != null)
                {
                    string AuditNorm = "";
                    string AuditId = "";
                    switch (Type)
                    {
                        case "Read":
                            AuditNorm = oModel.AuditNormRead;
                            AuditId = oModel.AuditIdRead;
                            break;
                        default:
                            AuditNorm = oModel.AuditNorm;
                            AuditId = oModel.AuditId;
                            break;
                    }
                    switch (AuditNorm)
                    {
                        case "Users":
                            ds = GetStepUsersByUsers(AuditId, stepId);
                            Data = ds.Tables[0];
                            Total = 0;
                            break;
                        case "Roles":
                            ds = GetStepUsersByRoles(AuditId, PageIndex, PageSize, stepId);
                            Data = ds.Tables[1];
                            Total = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            break;
                        case "OUDetail":
                            ds = GetStepUsersByOUDetail(AuditId, PageIndex, PageSize, stepId);
                            Data = ds.Tables[1];
                            Total = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            break;
                        case "ParentOUDetail":
                            ds = GetStepUsersByParentOUDetail(AuditId, PageIndex, PageSize, stepId);
                            Data = ds.Tables[1];
                            Total = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            break;
                        case "startuser":
                            ds = GetStepUsersByStartuser(taskUserId, PageIndex, PageSize, stepId);
                            Data = ds.Tables[1];
                            Total = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            break;
                        case "all":
                            ds = GetStepUsersByAll(AuditId, PageIndex, PageSize, stepId);
                            Data = ds.Tables[1];
                            Total = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            break;
                        default:

                            break;
                    }
                }
                return Json(new { Code = 0, Data = Data == null ? null : QuerySuite.ToDictionary(Data), Total = Total });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        #region 获取环节处理人

        /// <summary>
        /// 获取审批环节处理人   参与者：Users   用户
        /// </summary>
        public DataSet GetStepUsersByUsers(string userId, Guid stepId)
        {
            List<SqlParameter> list = new List<SqlParameter>();//添加查询条件
            list.Add(new SqlParameter("@userId", userId));

            string strSql = string.Format("select t1.userId,t1.userCode,t1.userName,'{0}' stepId from SysUser as t1 where t1.UserID = @userId", stepId);
            SqlParameter[] param = list.ToArray();
            return SqlHelper.Query(strSql, param);
        }

        /// <summary>
        /// 获取审批环节处理人   参与者：Roles   角色
        /// </summary>
        public DataSet GetStepUsersByRoles(string RoleId, int PageIndex, int PageSize, Guid stepId)
        {
            List<SqlParameter> list = new List<SqlParameter>();//添加查询条件
            list.Add(new SqlParameter("@RoleId", RoleId));

            string strSql = string.Format(@"select ROW_NUMBER()OVER (order by {0} {1}) RowNumber,t3.userId,t3.userCode,t3.userName,'{2}' stepId from SysRole as t1
                                left join SysRoleUser as t2 on t1.ID=t2.roleId
                                join SysUser t3 on t2.userId=t3.userId
                                where t1.ID=@RoleId"
                , "t3.userName", "asc", stepId);

            strSql = string.Format(@"select count(1) from ({0})t ;
                                     select * from ({0})t where RowNumber between {1} and {2} ", strSql, (PageIndex - 1) * PageSize + 1, PageIndex * PageSize);

            SqlParameter[] param = list.ToArray();
            return SqlHelper.Query(strSql, param);
        }

        /// <summary>
        /// 获取审批环节处理人   参与者：OUDetail    本级领导
        /// </summary>
        public DataSet GetStepUsersByOUDetail(string userId, int PageIndex, int PageSize, Guid stepId)
        {
            List<SqlParameter> list = new List<SqlParameter>();//添加查询条件
            list.Add(new SqlParameter("@userId", userId));

            string strSql = string.Format(@"select ROW_NUMBER()OVER (order by {0} {1}) RowNumber,t3.userId,t3.userName,t3.userCode,'{2}' stepId from SysUser as t1 
                                            left join SysDepartment as dp on dp.DepartmentID=t1.DepartmentID
                                            join SysLeader as t2 on dp.DepartmentCode=t2.DepartmentCode
                                            left join SysUser as t3 on t2.userCode=t3.userCode
                                            where t1.UserID=@userId"
                                            , "t3.userCode", "asc", stepId);

            strSql = string.Format(@"select count(1) from ({0})t ;
                                     select * from ({0})t where RowNumber between {1} and {2} ", strSql, (PageIndex - 1) * PageSize + 1, PageIndex * PageSize);

            SqlParameter[] param = list.ToArray();
            return SqlHelper.Query(strSql, param);
        }

        /// <summary>
        /// 获取审批环节处理人   参与者：ParentOUDetail    上级领导
        /// </summary>
        public DataSet GetStepUsersByParentOUDetail(string userId, int PageIndex, int PageSize, Guid stepId)
        {
            List<SqlParameter> list = new List<SqlParameter>();//添加查询条件
            list.Add(new SqlParameter("@userId", userId));

            string strSql = string.Format(@"select ROW_NUMBER()OVER (order by {0} {1}) RowNumber,t3.userId,t3.userName,t3.userCode,'{2}' stepId from SysUser as t1 
                                            left join SysDepartment as DP on t1.DepartmentID=dp.DepartmentID
                                            left join SysDepartment as parentDP on parentDP.DepartmentID=DP.ParentID
                                            join SysLeader as t2 on parentDP.DepartmentCode=t2.DepartmentCode
                                            left join SysUser as t3 on t2.userCode=t3.userCode
                                            where t1.UserID=@userId"
                                            , "t3.userName", "asc", stepId);

            strSql = string.Format(@"select count(1) from ({0})t ;
                                     select * from ({0})t where RowNumber between {1} and {2} ", strSql, (PageIndex - 1) * PageSize + 1, PageIndex * PageSize);

            SqlParameter[] param = list.ToArray();
            return SqlHelper.Query(strSql, param);
        }

        /// <summary>
        /// 获取审批环节处理人   参与者：startuser    发起人
        /// </summary>
        public DataSet GetStepUsersByStartuser(string taskUserId, int PageIndex, int PageSize, Guid stepId)
        {
            List<SqlParameter> list = new List<SqlParameter>();//添加查询条件
            var bru = dbContext.FlowBillsRecordUser.FirstOrDefault(t => t.Id.ToString() == taskUserId);
            if (bru == null)
            {
                return null;
            }
            list.Add(new SqlParameter("@BillsId", bru.BillsRecord.BillsId));
            string strSql = string.Format(@"select ROW_NUMBER()OVER (order by {0} {1}) RowNumber,t2.userId,t2.userName,t2.userCode,'{2}' stepId from FlowBills as t1
                                        left join SysUser as t2 on t1.SubmitUser=t2.UserID
                                        where t1.Id=@BillsId"
                                        , "t2.userName", "asc", stepId);
            strSql = string.Format(@"select count(1) from ({0})t ;
                                     select * from ({0})t where RowNumber between {1} and {2} ", strSql, (PageIndex - 1) * PageSize + 1, PageIndex * PageSize);
            SqlParameter[] param = list.ToArray();
            return SqlHelper.Query(strSql, param);
        }

        /// <summary>
        /// 获取审批环节处理人   参与者：all    全部人
        /// </summary>
        public DataSet GetStepUsersByAll(string userId, int PageIndex, int PageSize, Guid stepId)
        {
            //List<SqlParameter> list = new List<SqlParameter>();//添加查询条件
            //list.Add(new SqlParameter("@userId", userId));

            string strSql = string.Format(@"select ROW_NUMBER()OVER (order by {0} {1}) RowNumber,t1.userId,t1.userName,t1.userCode,'{2}' stepId from SysUser as t1 "
                , "t1.userName", "asc", stepId);

            strSql = string.Format(@"select count(1) from ({0})t;
                                     select * from ({0})t where RowNumber between {1} and {2} ", strSql, (PageIndex - 1) * PageSize + 1, PageIndex * PageSize);

            //SqlParameter[] param = list.ToArray();
            return SqlHelper.Query(strSql);
        }

        /// <summary>
        /// 获取审批环节处理人   回退操作
        /// </summary>
        /// <returns></returns>
        public DataSet GetStepUsersByReturn(string taskUserId, Guid stepId)
        {
            List<SqlParameter> list = new List<SqlParameter>();//添加查询条件

            list.Add(new SqlParameter("@taskUserId", taskUserId));
            string strSql = string.Format(@"select t3.userId,t4.userName,t4.userCode,'{1}' stepId from FlowBillsRecordUser as t1 
                                        left join FlowBillsRecord as t2 on t1.BillsRecordId=t2.Id
                                        left join FlowBillsRecordUser as t3 on t2.UpBillsRecordId=T3.BillsRecordId and t3.state='{0}'
                                        left join SysUser as t4 on t3.userId=t4.UserID
                                        where t1.Id=@taskUserId"
                                        , "Done", stepId);
            SqlParameter[] param = list.ToArray();
            DataSet ds = SqlHelper.Query(strSql, param);
            return ds;
        }

        #endregion

        /// <summary>
        /// 流程发起
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult StartFlow(string mainCode, Guid? parentId, string billsCode, string billsType, string workOrderName, string workOrderCode, string description, List<SelectNextStep> nextStepList_To, List<SelectNextStepUser> stepUserList_To, List<SelectNextStepUser> circulationUserList_To)
        {
            try
            {
                if (dbContext.FlowBills.Where(t => t.BillsCode == billsCode).Count() > 0)
                {
                    return Json(new { Code = 1, Msg = "当前工单已经存在，不能重复发起！" });
                }
                FlowMain flow_Main = dbContext.FlowMain.Where(t => t.Code == mainCode).FirstOrDefault();
                //流程主表  添加信息
                FlowBills flow_bl = new FlowBills();
                flow_bl.Id = Guid.NewGuid();
                flow_bl.MainId = flow_Main.Id.ToString();
                flow_bl.ParentId = parentId;
                flow_bl.Version = 1;
                flow_bl.UpdateTime = DateTime.Now;
                flow_bl.SubmitUser = SSOClient.UserId;
                flow_bl.State = "run";
                flow_bl.Sort = 1;
                flow_bl.StepId = "";
                flow_bl.Description = description;
                flow_bl.CreateTime = DateTime.Now;
                flow_bl.BillsCode = billsCode;
                flow_bl.BillsType = billsType;
                flow_bl.WorkOrderName = workOrderName;
                flow_bl.WorkOrderCode = workOrderCode;
                dbContext.Set<FlowBills>().Add(flow_bl);
                //流程任务表     发起环节 添加信息
                FlowBillsRecord flow_blr = new FlowBillsRecord();
                flow_blr.Id = Guid.NewGuid();
                flow_blr.BillsId = flow_bl.Id;
                flow_blr.Sort = 1;
                flow_blr.StartTime = DateTime.Now;
                flow_blr.State = "end";
                flow_blr.Type = 0;
                flow_blr.UpStep = "0";//上一环节
                flow_blr.NextStep = dbContext.Set<FlowStep>().Where(t => t.StepStatus == 1 && t.MainId == flow_Main.Id.ToString()).FirstOrDefault().Id.ToString();//当前环节
                flow_blr.UserId = Convert.ToString(SSOClient.UserId);//当前环节处理人
                flow_blr.EndTime = DateTime.Now;
                flow_blr.Description = description;
                flow_blr.Condition = 1;
                flow_blr.Choice = "1";
                flow_blr.Batch = 0;
                flow_blr.AuditDate = DateTime.Now;
                dbContext.Set<FlowBillsRecord>().Add(flow_blr);
                //流程任务待办表   发起环节 添加信息
                FlowBillsRecordUser flow_bru = new FlowBillsRecordUser();
                flow_bru.Id = Guid.NewGuid();
                flow_bru.BillsRecordId = flow_blr.Id;
                flow_bru.BillsRecordOutId = flow_blr.Id.ToString();
                flow_bru.Choice = "1";
                flow_bru.Condition = "1";
                flow_bru.CreateTime = DateTime.Now;
                flow_bru.DisplayState = "Done";
                flow_bru.State = "Done";
                flow_bru.EndTime = DateTime.Now;
                flow_bru.Opinion = description;
                flow_bru.RunTime = DateTime.Now;
                flow_bru.StartTime = DateTime.Now;
                flow_bru.StepId = flow_blr.NextStep;//当前环节
                flow_bru.Type = 1;
                flow_bru.UserId = flow_blr.UserId;
                dbContext.Set<FlowBillsRecordUser>().Add(flow_bru);
                //流程任务表     发起步骤之后的第一步骤 添加信息
                FlowBillsRecord flow_blrOne = new FlowBillsRecord();
                flow_blrOne.Id = Guid.NewGuid();
                flow_blrOne.BillsId = flow_bl.Id;
                flow_blrOne.Sort = 2;
                flow_blrOne.StartTime = flow_blr.EndTime;
                flow_blrOne.State = "run";
                flow_blrOne.Type = 0;
                flow_blrOne.UpBillsRecordId = flow_blr.Id.ToString();
                flow_blrOne.UpStep = flow_blr.NextStep;//上一环节
                foreach (SelectNextStep nextStep in nextStepList_To)
                {
                    FlowStep nextStepItem = dbContext.FlowStep.Find(Guid.Parse(nextStep.StepId));
                    flow_blrOne.NextStep = nextStep.StepId;//当前步骤
                    flow_blrOne.Batch = 0;
                    dbContext.Set<FlowBillsRecord>().Add(flow_blrOne);
                    //流程任务待办表     发起步骤之后的第一步骤处理人 添加信息
                    List<SelectNextStepUser> nextStepUsers = stepUserList_To.Where(t => t.StepId == nextStep.StepId).ToList();
                    foreach (SelectNextStepUser nextStepUser in nextStepUsers)
                    {
                        FlowBillsRecordUser flow_bruOne = new FlowBillsRecordUser();
                        flow_bruOne.Id = Guid.NewGuid();
                        flow_bruOne.BillsRecordId = flow_blrOne.Id;
                        flow_bruOne.BillsRecordOutId = flow_blrOne.Id.ToString();
                        flow_bruOne.CreateTime = DateTime.Now.AddSeconds(0.1);
                        flow_bruOne.DisplayState = "ToDo";
                        flow_bruOne.State = "ToDo";
                        flow_bruOne.RunTime = flow_bruOne.CreateTime;
                        flow_bruOne.StartTime = flow_bruOne.CreateTime;
                        flow_bruOne.StepId = flow_blrOne.NextStep;//当前环节
                        flow_bruOne.UserId = nextStepUser.UserId;
                        dbContext.Set<FlowBillsRecordUser>().Add(flow_bruOne);
                        ////添加代办短信
                        //AddSms(nextStepItem.SMSTemplateToDo, flow_bruOne.Id.ToString(), flow_bl.WorkOrderCode, flow_bl.WorkOrderName, flow_Main.Name, nextStepItem.stepName, flow_bruOne.StartTime, flow_bruOne.UserId);
                        ////添加Portal待办
                        //AddPendingJob(mainCode, flow_bl.BillsCode, flow_bruOne.Id.ToString(), "ToDo", nextStepUser.UserCode, SSOClient.User.userName, WorkOrderName, flow_bruOne.CreateTime.Value);
                    }
                    //流程任务待办表     发起步骤之后的第一步骤待阅 添加信息
                    if (circulationUserList_To != null && circulationUserList_To.Count > 0)
                    {
                        List<SelectNextStepUser> nextStepCirculanizeUsers = circulationUserList_To.Where(t => t.StepId == nextStep.StepId).ToList();
                        foreach (SelectNextStepUser nextStepCirculanizeUser in nextStepCirculanizeUsers)
                        {
                            FlowBillsRecordUser flow_bruOne = new FlowBillsRecordUser();
                            flow_bruOne.Id = Guid.NewGuid();
                            flow_bruOne.BillsRecordId = flow_blrOne.Id;
                            flow_bruOne.BillsRecordOutId = flow_blrOne.Id.ToString();
                            flow_bruOne.CreateTime = DateTime.Now.AddSeconds(0.1);
                            flow_bruOne.DisplayState = "ToRead";
                            flow_bruOne.State = "ToRead";
                            flow_bruOne.RunTime = flow_bruOne.CreateTime;
                            flow_bruOne.StartTime = flow_bruOne.CreateTime;
                            flow_bruOne.StepId = flow_blrOne.NextStep;//当前环节
                            flow_bruOne.UserId = nextStepCirculanizeUser.UserId;
                            dbContext.Set<FlowBillsRecordUser>().Add(flow_bruOne);
                            ////添加代阅短信
                            //AddSms(nextStepItem.SMSTemplateRead, flow_bruOne.Id.ToString(), flow_bl.WorkOrderCode, flow_bl.WorkOrderName, flow_Main.Name, nextStepItem.StepName, flow_bruOne.StartTime, flow_bruOne.UserId);
                            ////添加Portal待阅
                            //AddPendingJob(mainCode, flow_bl.BillsCode, flow_bruOne.Id.ToString(), "ToRead", nextStepCirculanizeUser.UserCode, SSOClient.User.userName, WorkOrderName, flow_bruOne.CreateTime.Value);
                        }
                    }
                }
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "流程发起成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 流程审批
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult SubmitNextStep(Guid taskUserId, int Agree, string description, List<SelectNextStep> nextStepList_To, List<SelectNextStepUser> stepUserList_To, List<SelectNextStepUser> circulationUserList_To)
        {
            try
            {
                FlowBillsRecordUser flow_bru = dbContext.Set<FlowBillsRecordUser>().Where(t => t.Id == taskUserId).FirstOrDefault();
                FlowBillsRecord flow_br = dbContext.FlowBillsRecord.Find(Guid.Parse(flow_bru.BillsRecordOutId));
                FlowBills flow_bl = dbContext.FlowBills.Find(flow_br.BillsId);
                FlowMain flow_Main = dbContext.FlowMain.Find(Guid.Parse(flow_bl.MainId));
                //当前待办结束
                flow_bru.Choice = Agree.ToString();
                flow_bru.Condition = Agree.ToString();
                flow_bru.EndTime = DateTime.Now;
                flow_bru.Opinion = description;
                flow_bru.Type = Agree;
                flow_bru.DisplayState = "Done";
                flow_bru.State = "Done";
                //添加Portal已办
                //AddPendEndJob(flow_Main.Code, flow_bl.BillsCode, flow_bru.Id.ToString(), "Done", flow_bru.EndTime.Value);
                var flow_brus = dbContext.Set<FlowBillsRecordUser>().Where(t => t.BillsRecordId == flow_bru.BillsRecordId && t.Id != flow_bru.Id && t.State == "ToDo").ToList();
                foreach (FlowBillsRecordUser flow_bruEx in flow_brus)
                {
                    flow_bruEx.Choice = Agree.ToString();
                    flow_bruEx.Condition = Agree.ToString();
                    flow_bruEx.EndTime = DateTime.Now;
                    flow_bruEx.Opinion = description;
                    flow_bruEx.Type = Agree;
                    flow_bruEx.DisplayState = "Auto";
                    flow_bruEx.State = "Auto";
                    //添加Portal已办
                    //AddPendEndJob(flow_Main.Code, flow_bl.BillsCode, flow_bruEx.Id.ToString(), "Done", flow_bruEx.EndTime.Value);
                }
                //当前任务结束
                flow_br.UserId = flow_bru.UserId;//当前环节处理人
                flow_br.EndTime = DateTime.Now;
                flow_br.Description = description;
                flow_br.Condition = Agree;
                flow_br.Choice = Agree.ToString();
                flow_br.State = "end";
                flow_br.AuditDate = DateTime.Now;

                
                int billsRecordCount = dbContext.Set<FlowBillsRecord>().Where(t => t.BillsId == flow_br.BillsId && t.State == "run").Count();//流程审批分支个数
                //下一任务开始
                foreach (SelectNextStep nextStep in nextStepList_To)
                {
                    FlowStep nextStepItem = dbContext.Set<FlowStep>().Where(t => t.Id.ToString() == nextStep.StepId).FirstOrDefault();
                    FlowBillsRecord flow_blrOne = new FlowBillsRecord();
                    flow_blrOne.Id = Guid.NewGuid();
                    flow_blrOne.BillsId = flow_br.BillsId;
                    flow_blrOne.Sort = flow_br.Sort + 1;
                    flow_blrOne.StartTime = DateTime.Now;
                    flow_blrOne.State = "run";
                    flow_blrOne.Type = 0;
                    flow_blrOne.UpStep = flow_br.NextStep;//上一环节
                    flow_blrOne.NextStep = nextStep.StepId;//当前步骤
                    flow_blrOne.Batch = 0;
                    flow_blrOne.UpBillsRecordId = flow_br.Id.ToString();
                    if (nextStepItem.StepStatus == 100)
                    {
                        //结束环节 任务结束
                        flow_blrOne.UserId = Convert.ToString(SSOClient.UserId);//当前环节处理人
                        flow_blrOne.EndTime = DateTime.Now;
                        flow_blrOne.Description = "";
                        flow_blrOne.Condition = 1;
                        flow_blrOne.Choice = "1";
                        flow_blrOne.State = "end";
                        flow_blrOne.AuditDate = DateTime.Now;
                        flow_br.Bills.State = "end";
                    }
                    if (nextStepItem.StepStatus != 100 || nextStepItem.StepStatus == 100 && billsRecordCount < 2)
                        dbContext.Set<FlowBillsRecord>().Add(flow_blrOne);
                    //下一任务待办处理人
                    List<SelectNextStepUser> nextStepUsers = stepUserList_To == null ? null : stepUserList_To.Where(t => t.StepId.Equals(nextStep.StepId)).ToList();
                    if (nextStepUsers != null && nextStepUsers.Count > 0)
                    {
                        foreach (SelectNextStepUser nextStepUser in nextStepUsers)
                        {
                            FlowBillsRecordUser flow_bruOne = new FlowBillsRecordUser();
                            flow_bruOne.Id = Guid.NewGuid();
                            flow_bruOne.BillsRecordId = flow_blrOne.Id;
                            flow_bruOne.BillsRecordOutId = flow_blrOne.Id.ToString();
                            flow_bruOne.CreateTime = DateTime.Now;
                            flow_bruOne.DisplayState = "ToDo";
                            flow_bruOne.State = "ToDo";
                            flow_bruOne.RunTime = DateTime.Now;
                            flow_bruOne.StartTime = DateTime.Now;
                            flow_bruOne.StepId = flow_blrOne.NextStep;//当前环节
                            flow_bruOne.UserId = nextStepUser.UserId;
                            dbContext.Set<FlowBillsRecordUser>().Add(flow_bruOne);
                            if (nextStepItem.StepStatus == 100)
                            {
                                //结束环节 待办结束
                                flow_bruOne.Choice = "1";
                                flow_bruOne.Condition = "1";
                                flow_bruOne.EndTime = DateTime.Now;
                                flow_bruOne.Opinion = string.Empty;
                                flow_bruOne.Type = 1;
                                flow_bruOne.DisplayState = "Done";
                                flow_bruOne.State = "Done";
                                flow_bruOne.UserId = Convert.ToString(SSOClient.UserId);
                            }
                            //添加代办短信
                            //AddSms(nextStepItem.SMSTemplateToDo, flow_bruOne.Id.ToString(), flow_bl.WorkOrderCode, flow_bl.WorkOrderName, flow_Main.Name, nextStepItem.StepName, flow_bruOne.StartTime, flow_bruOne.UserId);
                            //添加Portal待办
                            //AddPendingJob(flow_Main.Code, flow_bl.BillsCode, flow_bruOne.Id.ToString(), "ToDo", nextStepUser.UserCode, SSOClient.User.UserName, flow_bl.WorkOrderName, flow_bruOne.CreateTime.Value);
                        }
                    }
                    //下一任务待阅处理人
                    if (circulationUserList_To != null && circulationUserList_To.Count > 0)
                    {
                        List<SelectNextStepUser> nextStepCirculanizeUsers = circulationUserList_To.Where(t => t.StepId.Equals(nextStep.StepId)).ToList();
                        if (nextStepCirculanizeUsers != null && nextStepCirculanizeUsers.Count > 0)
                        {
                            foreach (SelectNextStepUser nextStepCirculanizeUser in nextStepCirculanizeUsers)
                            {
                                FlowBillsRecordUser flow_bruOne = new FlowBillsRecordUser();
                                flow_bruOne.Id = Guid.NewGuid();
                                flow_bruOne.BillsRecordId = flow_blrOne.Id;
                                flow_bruOne.BillsRecordOutId = flow_blrOne.Id.ToString();
                                flow_bruOne.CreateTime = DateTime.Now;
                                flow_bruOne.DisplayState = "ToRead";
                                flow_bruOne.State = "ToRead";
                                flow_bruOne.RunTime = DateTime.Now;
                                flow_bruOne.StartTime = DateTime.Now;
                                flow_bruOne.StepId = flow_blrOne.NextStep;//当前环节
                                flow_bruOne.UserId = nextStepCirculanizeUser.UserId;
                                dbContext.Set<FlowBillsRecordUser>().Add(flow_bruOne);
                                //添加代阅短信
                                //AddSms(nextStepItem.SMSTemplateRead, flow_bruOne.Id.ToString(), flow_bl.WorkOrderCode, flow_bl.WorkOrderName, flow_Main.Name, nextStepItem.StepName, flow_bruOne.StartTime, flow_bruOne.UserId);
                                //添加Portal待阅
                                //AddPendingJob(flow_Main.Code, flow_bl.BillsCode, flow_bruOne.Id.ToString(), "ToRead", nextStepCirculanizeUser.UserCode, SSOClient.User.UserName, flow_bl.WorkOrderName, flow_bruOne.CreateTime.Value);
                            }
                        }
                    }
                }
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "流程审批成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取审批历史
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult GetHistory(Guid? taskUserId)
        {
            try
            {
                DataTable Data = new DataTable();
                string strSql = @"declare @BillId nvarchar(50)
                                select @BillId=t2.BillsId from FlowBillsRecordUser t1
                                left join FlowBillsRecord t2 on t1.BillsRecordId=t2.Id
                                where t1.Id=@taskUserId

                                select t4.stepName,t5.userName,t5.userCode,t3.choice,t3.Opinion as 'description',t3.state,t3.startTime,t3.endTime,
                                dbo.fn_ComputationTimeDifference(t3.startTime,t3.endTime,'hh:mm:ss') 'Timespace'
                                from FlowBills as t1
                                left join FlowBillsRecord as t2 on t1.Id=t2.BillsId
                                left join FlowBillsRecordUser as t3 on t2.Id=t3.BillsRecordId
                                left join FlowStep as t4 on t2.NextStep=t4.Id
                                left join SysUser as t5 on t3.userId=t5.UserID
                                where t1.Id=@BillId
                                order by isnull(t3.startTime,t2.endTime) asc";
                Data = SqlHelper.Query(strSql, new SqlParameter("@taskUserId", taskUserId)).Tables[0];
                return Json(new { Code = 0, Total = 0, Data = QuerySuite.ToDictionary(Data) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取流程配置按钮
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult GetFlowBtn(string billsCode, string taskUserId, string mainCode, List<DataParameter> btnAuths)
        {
            try
            {
                DataTable data = new DataTable();
                data.Columns.Add("btnName", Type.GetType("System.String"));
                data.Columns.Add("condition", Type.GetType("System.Int32"));

                if (string.IsNullOrEmpty(taskUserId))
                {
                    if (dbContext.FlowBills.Where(t => t.BillsCode == billsCode).Count() == 0)
                        data.Rows.Add(new object[] { "发起流程", 0 });
                    data.Rows.Add(new object[] { "保存草稿", 2 });
                }
                else
                {
                    //审批
                    List<SqlParameter> list = new List<SqlParameter>();
                    string strSql = string.Format(@"select '{0}' 'btnName',{1} 'condition' from FlowBillsRecordUser as t1 
                        left join FlowStep as t2 on t1.StepId=t2.Id
                        where t1.Id=@taskUserId and t2.[Function]='Agency'
                        union
                        select t2.BtnName,t2.condition from FlowBillsRecordUser as t1
                        left join FlowStepPath as t2 on t1.stepId=t2.UpStep
                        where t1.Id=@taskUserId
                        group by t2.BtnName,t2.condition
                        order by condition", "转交", 50);
                    list.Add(new SqlParameter("@taskUserId", taskUserId));
                    data = SqlHelper.Query(strSql, list.ToArray()).Tables[0];
                }
                //是否显示提交按钮
                if (btnAuths != null && btnAuths.Count(t => t.Key == "1" && t.Value == "hide") > 0)
                {
                    for (int i = data.Rows.Count - 1; i >= 0; i--)
                    {
                        if (data.Rows[i]["condition"].ToString() == "1")
                        {
                            data.Rows.Remove(data.Rows[i]);
                            break;
                        }
                    }
                }
                //是否显示保存按钮
                if (btnAuths != null && btnAuths.Count(t => t.Key == "2" && t.Value == "hide") <= 0)
                    data.Rows.Add(new object[] { "保存", 2 });

                data.Rows.Add(new object[] { "返回", 100 });
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(data) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取转交人员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public virtual ActionResult GetUsers(string Key, Guid? stepId, int PageIndex, int PageSize)
        {
            try
            {
                string sqlWhere = string.Empty;
                List<SqlParameter> list = new List<SqlParameter>();//添加查询条件
                if (!string.IsNullOrEmpty(Key))
                {
                    sqlWhere += " and (userCode like @val Or mobile like @val Or userName like @val)";
                    list.Add(new SqlParameter("@val", string.Format("%{0}%", Key)));
                }
                string strSql = string.Format(@"select ROW_NUMBER()OVER (order by {0} {1}) RowNumber,userId,userCode,userName,mobile,'{2}' as 'stepId'  from SysUser where 1=1 {3}", "userCode", "asc", stepId, sqlWhere);
                strSql = string.Format(@"select count(1) from ({0})t ;
                                     select * from ({0})t where RowNumber between {1} and {2} ", strSql, (PageIndex - 1) * PageSize + 1, PageIndex * PageSize);
                SqlParameter[] param = list.ToArray();
                DataSet ds = SqlHelper.Query(strSql, param);
                return Json(new { Code = 0, Total = ds.Tables[0].Rows[0][0], Data = QuerySuite.ToDictionary(ds.Tables[1]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 流程转交
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult TransferNextStep(Guid taskUserId, string description, string userId)
        {
            try
            {
                //当前待办结束
                FlowBillsRecordUser flow_bru = dbContext.FlowBillsRecordUser.Find(taskUserId);
                flow_bru.Choice = "50";
                flow_bru.Condition = "50";
                flow_bru.EndTime = DateTime.Now;
                flow_bru.Opinion = description;
                flow_bru.Type = 50;
                flow_bru.DisplayState = "Done";
                flow_bru.State = "Done";
                //被转交人 待办开始
                FlowBillsRecordUser flow_bruNew = new FlowBillsRecordUser();
                flow_bruNew.Id = Guid.NewGuid();
                flow_bruNew.BillsRecordId = flow_bru.BillsRecordId;
                flow_bruNew.BillsRecordOutId = flow_bru.BillsRecordOutId;
                flow_bruNew.CreateTime = DateTime.Now;
                flow_bruNew.DisplayState = "ToDo";
                flow_bruNew.State = "ToDo";
                flow_bruNew.RunTime = DateTime.Now;
                flow_bruNew.StartTime = DateTime.Now;
                flow_bruNew.StepId = flow_bru.StepId;//当前环节
                flow_bruNew.UserId = userId;
                dbContext.Set<FlowBillsRecordUser>().Add(flow_bruNew);
                dbContext.SaveChanges();

                //添加代办短信
                //DataRow dr = SqlHelper.Query(@"select t4.SMSTemplateToDo,t2.WorkOrderCode,t2.WorkOrderName,t2.BillsCode,t3.Name,t3.Code 'mainCode',t4.StepName from FlowBillsRecord t1
                //                            join FlowBills t2 on t1.BillsId=t2.Id
                //                            join FlowMain t3 on t2.MainId=t3.Id
                //                            join FlowStep t4 on t1.NextStep=t4.Id
                //                            where t1.Id='" + flow_bru.BillsRecordId + "'").Tables[0].Rows[0];
                //AddSms(Convert.ToString(dr["SMSTemplateToDo"]), flow_bruNew.Id.ToString(), Convert.ToString(dr["WorkOrderCode"]), Convert.ToString(dr["WorkOrderName"])
                //    , Convert.ToString(dr["Name"]), Convert.ToString(dr["StepName"]), flow_bruNew.StartTime, flow_bruNew.userId);

                //添加Portal已办
                //AddPendEndJob(Convert.ToString(dr["mainCode"]), Convert.ToString(dr["BillsCode"]), flow_bru.Id.ToString(), "Done", flow_bru.EndTime.Value);
                //添加Portal待办
                //SysUser userItem = dbContext.SysUsers.Find(Guid.Parse(UserId));
                //AddPendingJob(Convert.ToString(dr["mainCode"]), Convert.ToString(dr["BillsCode"]), flow_bruNew.Id.ToString(), "ToDo", userItem.UserCode, SSOClient.User.UserName, Convert.ToString(dr["WorkOrderName"]), flow_bruNew.CreateTime.Value);

                return Json(new { Code = 0, Msg = "流程转交成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 修改状态 为 已阅
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public ActionResult UpdateStateReadEnd(Guid taskUserId)
        {
            try
            {
                FlowBillsRecordUser flow_bru = dbContext.FlowBillsRecordUser.Find(taskUserId);

                if (flow_bru == null)
                    return Json(new { Code = 1, Msg = "对象不存在，请联系管理员" });

                flow_bru.State = "ReadEnd";
                flow_bru.DisplayState = "ToRead";
                flow_bru.EndTime = DateTime.Now;
                dbContext.SaveChanges();
                //添加Portal已阅
                //DataRow dr = SqlHelper.Query(@"select t2.BillsCode,t3.Name,t3.Code 'mainCode' from FlowBillsRecord t1
                //                            join FlowBills t2 on t1.BillsId=t2.Id
                //                            join FlowMain t3 on t2.MainId=t3.Id
                //                            where t1.Id='" + flow_bru.BillsRecordId + "'").Tables[0].Rows[0];
                //AddPendEndJob(Convert.ToString(dr["mainCode"]), Convert.ToString(dr["BillsCode"]), flow_bru.Id.ToString(), "ReadEnd", flow_bru.EndTime.Value);

                return Json(new { Code = 0, Msg = "阅读成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        #endregion

        #region
        /// <summary>
        /// 获取工作台列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkbenchList(string name, string stepName, string state, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                QuerySuite querySuite = new QuerySuite(this, "BRU.endTime DESC");
                querySuite.Select(@"select Bills.*, UR.userName,UR.userCode,Main.name,Step.stepName,BRU.startTime,BRU.Id as 'taskUserId',Main.Code as 'mainCode',BRU.endTime,Step.linkCode
                                    from FlowBills as Bills
                                    left join FlowBillsRecord as BR on BR.BillsId=Bills.Id 
                                    left join FlowBillsRecordUser as BRU on BRU.BillsRecordId=BR.Id
                                    left join FlowMain as Main on Main.Id=Bills.mainId 
                                    left join FlowStep as Step on BR.NextStep=Step.Id
                                    left join SysUser as UR on UR.UserID=BRU.userId");

                if (string.IsNullOrEmpty(name))
                {
                    querySuite.AddParam(" and Main.name like @name ", new SqlParameter("@name", string.Format("%{0}%", name)));
                }
                if (string.IsNullOrEmpty(stepName))
                {
                    querySuite.AddParam(" and Step.stepName like @stepName ", new SqlParameter("@stepName", string.Format("%{0}%", stepName)));
                }
                string TimeField = "";
                if (state == "ToDo" || state == "ToRead")
                    TimeField = "BRU.startTime";
                else
                    TimeField = "BRU.endTime";
                if (StartDate != null)
                {
                    querySuite.AddParam(string.Format(" and {0} >= @StartDate ", TimeField), new SqlParameter("@StartDate", StartDate.Value));
                }
                if (EndDate != null)
                {
                    querySuite.AddParam(string.Format(" and {0} <= @EndDate ", TimeField), new SqlParameter("@EndDate", EndDate.Value.AddDays(1)));
                }
                querySuite.AddParam(" and UR.UserID = @userId ", new SqlParameter("@userId", SSOClient.UserId));
                querySuite.AddParam(" and BRU.state = @state ", new SqlParameter("@state", state));

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
        /// 获取Tab数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTabSource(Guid? taskUserId)
        {
            try
            {
                if (!taskUserId.HasValue)
                    return Json(new { Code = 1, Msg = "代办Id不允许为空，请联系管理员" });

                string strSql = @"select t3.stepName,t3.id 'stepId',t3.linkCode,t3.showTabIndex,t2.Id 'currentStepId' from FlowBillsRecordUser t1
                                left join FlowStep t2 on t1.stepId=t2.Id
                                left join FlowStep t3 on t2.mainId=t3.mainId
                                where t1.Id=@taskUserId and t3.linkCode<>'100'";
                DataSet ds = SqlHelper.Query(strSql, new SqlParameter("@taskUserId", taskUserId));
                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(ds.Tables[0]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取环节处理人
        /// </summary>
        /// <returns></returns>
        public ActionResult IsEnableFlowTab(string taskUserId, string linkCode)
        {
            try
            {
                string strSql = @"select COUNT(1) from FlowBillsRecordUser as t1 
                                join FlowStep as t2 on t1.stepId=t2.Id
                                where t1.Id=@taskUserId and t1.state='ToDo' and t2.linkCode=@linkCode and t1.UserId=@userId";
                List<SqlParameter> paramlist = new List<SqlParameter>();
                paramlist.Add(new SqlParameter("@taskUserId", taskUserId));
                paramlist.Add(new SqlParameter("@linkCode", linkCode));
                paramlist.Add(new SqlParameter("@userId", SSOClient.UserId));
                bool data = Convert.ToInt32(SqlHelper.GetSingle(strSql, paramlist.ToArray())) > 0;
                return Json(new { Code = 0, Data = data });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        #region 创建工单编号

        /// <summary>
        /// 创建工单编号
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOrderCode(string pinyin)
        {
            try
            {
                string Day = DateTime.Now.ToString("yyyyMMdd");

                int Index =(int)SqlHelper.GetSingle("select ISNULL(MAX([Index]),0)+1 from FlowOrderCodes where pinyin=@pinyin and Day='" + Day + "'", new SqlParameter("@pinyin", pinyin));

                FlowOrderCodes addModel = new FlowOrderCodes();
                addModel.Pinyin = pinyin;
                addModel.Day = Day;
                addModel.Index = Index;
                addModel.Code = pinyin + Day + Index.ToString().PadLeft(4, '0');
                dbContext.FlowOrderCodes.Add(addModel);
                dbContext.SaveChanges();

                return Json(new { Code = 0, Data = addModel.Code });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion

        ///// <summary>
        ///// 添加发送短信
        ///// </summary>
        //private void AddSms(string SMSTemplate, string RelationCode, string WorkOrderCode, string WorkOrderName, string FlowName, string StepName, Nullable<DateTime> StartTime, string ToUserId)
        //{
        //    if (string.IsNullOrEmpty(SMSTemplate))
        //        return;

        //    SysUser userItem = dbContext.SysUsers.Find(Guid.Parse(ToUserId));

        //    SysSmsSend sms = new SysSmsSend();
        //    sms.RelationCode = RelationCode;

        //    Dictionary<string, string> dics = new Dictionary<string, string>();
        //    dics.Add("$WorkOrderCode$", WorkOrderCode);
        //    dics.Add("$WorkOrderName$", WorkOrderName);
        //    dics.Add("$FlowName$", FlowName);
        //    dics.Add("$StepName$", StepName);
        //    dics.Add("$UserName$", SSOClient.User.UserName);
        //    dics.Add("$UserCode$", SSOClient.User.UserCode);
        //    dics.Add("$StartTime$", string.Format("{0:yyyy-MM-dd hh:mm:ss}", StartTime));
        //    sms.Content = ReplaceList(SMSTemplate, dics);
        //    sms.Type = "Flow";
        //    sms.State = 0;
        //    sms.CreateTime = DateTime.Now;
        //    sms.SendUser = SSOClient.User.UserCode;
        //    sms.SendMobile = SSOClient.User.Mobile;
        //    sms.ToUser = userItem.UserCode;
        //    sms.ToMobile = userItem.Mobile;
        //    dbContext.SysSmsSends.Add(sms);
        //    dbContext.SaveChanges();
        //}

        ///// <summary>
        ///// 添加Portal待办待阅、已办已阅
        ///// </summary>
        //private void AddPendingJob(string mainCode, string orderId, string taskUserId, string PendingType, string Owner, string ParentOwner, string JobName, DateTime IssuedTime)
        //{
        //    PendingJob model = new PendingJob();
        //    model.BillsRecordUserID = taskUserId;
        //    model.PendingType = PendingType;
        //    model.Owner = Owner;
        //    model.ParentOwner = ParentOwner;
        //    model.JobName = JobName;
        //    model.URL = GetPortalUrl(mainCode, orderId, PendingType, taskUserId);
        //    model.IssuedTime = IssuedTime;
        //    model.State = 0;
        //    model.Msg = null;
        //    model.ActionType = 1;
        //    model.PendingJobID = null;
        //    switch (PendingType)
        //    {
        //        case "ToDo":
        //            model.JobType = "待办";
        //            break;
        //        case "ToRead":
        //            model.JobType = "待阅";
        //            break;
        //    }
        //    dbContext.PendingJobs.Add(model);
        //    dbContext.SaveChanges();
        //}

        ///// <summary>
        ///// 添加Portal已办已阅
        ///// </summary>
        //private void AddPendEndJob(string mainCode, string orderId, string taskUserId, string PendingType, DateTime IssuedTime)
        //{
        //    PendingJob model = new PendingJob();
        //    model.BillsRecordUserID = taskUserId;
        //    model.PendingType = PendingType;
        //    model.Owner = null;
        //    model.ParentOwner = null;
        //    model.JobName = null;
        //    model.URL = GetPortalUrl(mainCode, orderId, PendingType, taskUserId);
        //    model.IssuedTime = IssuedTime;
        //    model.State = 0;
        //    model.Msg = null;
        //    model.ActionType = 3;
        //    var pendingJob = dbContext.PendingJobs.FirstOrDefault(t => t.BillsRecordUserID == taskUserId);
        //    if (pendingJob != null)
        //        model.PendingJobID = pendingJob.ID;
        //    switch (PendingType)
        //    {
        //        case "Done":
        //            model.JobType = "已办";
        //            break;
        //        case "ReadEnd":
        //            model.JobType = "已阅";
        //            break;
        //    }
        //    dbContext.PendingJobs.Add(model);
        //    dbContext.SaveChanges();
        //}
        #endregion

        #region Helper

            /// <summary>
            /// 
            /// </summary>
            /// <param name="expression">@a=3&&@a=5||@b=x</param>
            /// <param name="parameters">[{@a:3},{@b:5}]</param>
            /// <returns></returns>
        private bool RunJScript(string expression, List<DataParameter> parameters)
        {
            expression = expression.Replace("‘", "'").Replace("’", "'");
            bool IsSuccess = false;
            Regex regex = new Regex(@"\$\w*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match match = regex.Match(expression);
            while (match.Success && parameters != null && parameters.Count > 0)
            {
                string key = match.Value.Replace("$", "");
                DataParameter parameter = parameters.FirstOrDefault(P => P.Key == key);
                if (parameter == null)
                {
                    IsSuccess = false;
                    break;
                }
                expression = regex.Replace(expression, string.Format("'{0}'", parameter.Value), 1);
                match = match.NextMatch();
                IsSuccess = true;
            }
            var jsEngine = new MsieJsEngine();            
            return IsSuccess ? jsEngine.Evaluate<bool>(expression) : false;
        }
        // 字符串替换
        private string ReplaceList(string strVal, Dictionary<string, string> dics)
        {
            foreach (var dic in dics)
                strVal = strVal.Replace(dic.Key, dic.Value);

            return strVal;
        }
        private string GetPortalUrl(string mainCode, string orderId, string state, string taskUserId)
        {
            string PortalUrl = null;
            switch (mainCode)
            {
                case "999":
                    PortalUrl = GetRedirect("/WorkflowExample/ExampleTab", orderId, state, taskUserId);
                    break;
            }
            return PortalUrl;
        }
        private string GetRedirect(string url, string orderId, string state, string taskUserId)
        {
            return "../../Pages/Shared/LayoutFlowAprove.html?page=" + WebUtility.HtmlEncode(url + "?orderId=" + orderId + "&state=" + state + "&taskUserId=" + taskUserId) + "&sign=whitelist";
        }
        #endregion

        public bool SaveFirstStepUser1()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return false;
            }
        }
    }


    public class StepAndPath
    {
        //步骤主键
        public string id { get; set; }
        //步骤名
        public string StepName { get; set; }
        //步骤名
        public string process_name { get; set; }
        //步骤参数
        public string conditions { get; set; }
        //下步骤号
        public string process_to { get; set; }
        //审核类型
        public string AuditNorm { get; set; }
        //样式
        public string style { get; set; }
        //审核人
        public string AuditId { get; set; }
        //步骤状态
        public int StepStatus { get; set; }
        //备注
        public string Description { get; set; }
        public string RelationStepKey { get; set; }
        //额定审核人数
        public int Auditors { get; set; }
        //主表主键
        public string MainId { get; set; }

        private string _OpenChoices = "open";

        public string OpenChoices
        {
            get { return _OpenChoices; }
            set { _OpenChoices = value; }
        }

        private string _Power = "readonly";

        public string Power
        {
            get { return _Power; }
            set { _Power = value; }
        }

        private string _RunMode = "auto";

        public string RunMode
        {
            get { return _RunMode; }
            set { _RunMode = value; }
        }


        private string _JoinMode = "select";

        public string JoinMode
        {
            get { return _JoinMode; }
            set { _JoinMode = value; }
        }

        private string _ExamineMode = "onlyone";

        public string ExamineMode
        {
            get { return _ExamineMode; }
            set { _ExamineMode = value; }
        }

        public string Percentage { get; set; }

        public string Function { get; set; }

        //黎泽金
        public string LinkCode { get; set; }
        public string ShowTabIndex { get; set; }

        public string Circularize { get; set; }
        public Nullable<Int64> ReminderTimeout { get; set; }
        public string SMSTemplateToDo { get; set; }
        public string SMSTemplateRead { get; set; }
        //审核类型
        public string AuditNormRead { get; set; }
        //审核人
        public string AuditIdRead { get; set; }
    }
    public class PostParam
    {
        public List<StepAndPath> stepList { get; set; }
    }
    public class DataParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class GetNextStepOutParam
    {
        public Guid MainId { get; set; }
        public string mainCode { get; set; }
        public string MainName { get; set; }
        [Key]
        public Guid StepId { get; set; }
        public string StepName { get; set; }
        public string LinkCode { get; set; }
        public int StepStatus { get; set; }
        public string expression { get; set; }
        public string Circularize { get; set; }
    }
    public class SelectNextStep
    {
        /// <summary>
        /// 环节Id
        /// </summary>
        public string StepId { get; set; }
        /// <summary>
        /// 环节名称
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// 环节处理人数量
        /// </summary>
        public int UserCount { get; set; }
    }
    public class SelectNextStepUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string StepId { get; set; }
    }
    public class WorkflowContext : DataContext
    {
        public DbSet<GetNextStepOutParam> GetNextStepOutParam { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            

            builder.Entity<GetNextStepOutParam>()
                .HasKey(x => new { x.StepId });

        }

    }
}