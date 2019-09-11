/***********************
 * BitAdmin2.0框架文件
 * 微信企业号
 ***********************/
using Senparc.NeuChar.Entities;
using Senparc.Weixin;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace {{cookiecutter.project_name}}.Helpers
{
    
    public class WeixinWorkHelper
    {
        public static string CorpId = "ww991092455544b3f4";
        public static readonly Dictionary<string, string> AgentSecrets = new Dictionary<string, string>();
        static WeixinWorkHelper()
        {
            AgentSecrets["1000001"] = "GVDVK4i8qUYhAOtpe1K2IR8ywZIhVAegpavmIeOmzNk";//通讯录
            AgentSecrets["1000002"] = "6vmf1zKvXGJR6TF7ht1JESMiaEiTZaRw_JYnJal4NSM";//框架测试
        }

        #region 发送文本
        public static void SendText(string agentId, Guid? userId, string text)
        {
            SendText(agentId, new List<Guid?>() { userId }, text);
        }
        public static void SendText(string agentId, List<Guid?> userIds, string text)
        {
            SendText(agentId, QueryUserCode(userIds), text);
        }
        public static void SendText(string agentId, string userCode, string text)
        {
            SendText(agentId, new List<string>() { userCode }, text);
        }
        public static void SendText(string agentId, List<string> userCodes, string text)
        {
            userCodes.ForEach(userCode => MassApi.SendText(GetToken(agentId), agentId, text, userCode));
        }
        #endregion
        
        #region 发送图片
        public static void SendImage(string agentId, Guid? userId, string mediaId)
        {
            SendImage(agentId, new List<Guid?>() { userId }, mediaId);
        }
        public static void SendImage(string agentId, List<Guid?> userIds, string mediaId)
        {
            SendImage(agentId, QueryUserCode(userIds), mediaId);
        }
        public static void SendImage(string agentId, string userCode, string mediaId)
        {
            SendImage(agentId, new List<string>() { userCode }, mediaId);
        }
        public static void SendImage(string agentId, List<string> userCodes, string mediaId)
        {
            userCodes.ForEach(userCode => MassApi.SendImage(GetToken(agentId), agentId, mediaId, userCode));
        }
        #endregion

        #region 发送图文
        public static void SendNews(string agentId, Guid? userId, List<Article> articles)
        {
            SendNews(agentId, new List<Guid?>() { userId }, articles);
        }
        public static void SendNews(string agentId, List<Guid?> userIds, List<Article> articles)
        {
            SendNews(agentId, QueryUserCode(userIds), articles);
        }
        public static void SendNews(string agentId, string userCode, List<Article> articles)
        {
            SendNews(agentId, new List<string>() { userCode }, articles);
        }
        public static void SendNews(string agentId, List<string> userCodes, List<Article> articles)
        {
            userCodes.ForEach(userCode => MassApi.SendNews(GetToken(agentId), agentId, articles, userCode));
        } 
        #endregion

        public static string GetToken(string agentId= "1000001")
        {
            return AccessTokenContainer.TryGetToken(CorpId, AgentSecrets[agentId]);
        }
        private static List<string> QueryUserCode(List<Guid?> userIds)
        {
            var list = userIds.ConvertAll<string>(x => x.ToString()).ToArray();
            string sql = "select userCode from SysUser where userId in('" + string.Join("','", list) + "')";
            var rows = SqlHelper.Query(sql).Tables[0].Rows;

            List<string> clientIds = new List<string>();
            foreach (DataRow row in rows)
                clientIds.Add(Convert.ToString(row[0]));

            return clientIds;
        }
    }
}
