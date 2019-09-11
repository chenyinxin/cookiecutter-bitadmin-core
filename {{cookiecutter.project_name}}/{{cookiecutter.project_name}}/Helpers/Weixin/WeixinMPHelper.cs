/***********************
 * BitAdmin2.0框架文件
 * 微信公众号
 ***********************/
using Senparc.NeuChar.Entities;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class WeixinMPHelper
    {
        private static string AppId => AccessTokenContainer.GetFirstOrDefaultAppId(PlatformType.MP);

        #region 发送文本
        public static void SendText(Guid? userId, string text)
        {
            SendText(new List<Guid?>() { userId }, text);
        }
        public static void SendText(List<Guid?> userIds, string text)
        {
            QueryOpenId(userIds).ForEach(openId => CustomApi.SendText(AppId, openId, text));
        }
        public static void SendText(string openId, string text)
        {
            CustomApi.SendText(AppId, openId, text);
        }
        #endregion

        #region 发送图片
        public static void SendImage(Guid? userId, string text)
        {
            SendImage(new List<Guid?>() { userId }, text);
        }
        public static void SendImage(List<Guid?> userIds, string text)
        {
            QueryOpenId(userIds).ForEach(openId => CustomApi.SendImage(AppId, openId, text));
        }
        #endregion

        #region 发送图文
        public static void SendNews(Guid? userId, List<Article> articles)
        {
            SendNews(new List<Guid?>() { userId }, articles);
        }
        public static void SendNews(List<Guid?> userIds, List<Article> articles)
        {
            QueryOpenId(userIds).ForEach(openId => CustomApi.SendNews(AppId, openId, articles));
        } 
        #endregion

        /**
         * 发送模板信息（需要先在后台进行配置）
         * */
        public static void SendTemplate(Guid? userId, string templateId, string linkUrl)
        {
            SendTemplate(new List<Guid?>() { userId }, templateId, linkUrl);
        }
        public static void SendTemplate(List<Guid?> userIds, string templateId, string linkUrl)
        {
            QueryOpenId(userIds).ForEach(openId => {
                var templateData = new TemplateData()
                {
                    Title = new TemplateDataItem("您好，您的订单已支付成功！", "#000000"),
                    Place = new TemplateDataItem("广州-北京", "#000000"),
                    Price = new TemplateDataItem("100元", "#000000"),
                    Time = new TemplateDataItem("2111-11-11 11:11:11", "#000000"),
                    Remark = new TemplateDataItem("感谢您的购买~", "#000000")
                };
                SendTemplateMessageResult sendResult = TemplateApi.SendTemplateMessage(AppId, openId, templateId, linkUrl, templateData, null);
            });
        }

        public class TemplateData
        {
            public TemplateDataItem Title { get; set; }
            public TemplateDataItem Place { get; set; }
            public TemplateDataItem Price { get; set; }
            public TemplateDataItem Time { get; set; }
            public TemplateDataItem Remark { get; set; }
        }               

        private static List<string> QueryOpenId(List<Guid?> userIds)
        {
            var list = userIds.ConvertAll<string>(x => x.ToString()).ToArray();
            string sql = "select clientId from SysUserClientId where userId in('" + string.Join("','", list) + "')";
            var rows = SqlHelper.Query(sql).Tables[0].Rows;

            List<string> clientIds = new List<string>();
            foreach (DataRow row in rows)
                clientIds.Add(Convert.ToString(row[0]));

            return clientIds;
        }
    }
}
