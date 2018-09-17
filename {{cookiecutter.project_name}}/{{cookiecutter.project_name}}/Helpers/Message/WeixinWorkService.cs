/***********************
 * BitAdmin2.0框架文件
 * 微信企业号
 ***********************/
using Senparc.NeuChar.Entities;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace {{cookiecutter.project_name}}.Helpers
{
    
    public class WeixinWorkService
    {
        private static string AgentId = "1000002";
        private static string CorpID = "ww991092455544b3f4";
        private static string Secret = "J2ZSqRn0BPHkbJeQVcORU1rP7-JJP945vvjiGmhnEw4";

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
            Register();
            userCodes.ForEach(userCode => MassApi.SendText(GetToken(), string.IsNullOrEmpty(agentId) ? AgentId : agentId, text, userCode));
        }

        //发送图片
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
            Register();
            userCodes.ForEach(userCode => MassApi.SendImage(GetToken(), agentId?? AgentId, mediaId, userCode));
        }

        //发送图文
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
        public static void SendNews(string agentId,List<string> userCodes, List<Article> articles)
        {
            Register();
            userCodes.ForEach(userCode => MassApi.SendNews(GetToken(), agentId?? AgentId, articles, userCode));
        }

        private static void Register()
        {
            if (!AccessTokenContainer.CheckRegistered(CorpID))
                AccessTokenContainer.Register(CorpID, Secret);
        }

        private static string GetToken()
        {
            Register();
            return AccessTokenContainer.TryGetToken(CorpID, Secret);
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
