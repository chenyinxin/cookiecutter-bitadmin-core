/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace {{cookiecutter.project_name}}.Helpers
{
    /// <summary>
    /// 个推平台消息推送
    /// </summary>
    public class IgetuiService
    {        
        private static String host = "http://sdk.open.api.igexin.com/apiex.htm";
        
        private static String appId = "JcuCBhKQl9AGcye9vcZzR7";
        private static String appKey = "eRg6sHBJ6788trdMtVW5V3";
        private static String masterSecret = "NXjvNatE3j9m7wnEazd7q4";
        
        public static void Push(Guid? userId, NotificationTemplate template)
        {
            Push(new List<Guid?>() { userId }, template);
        }
        public static void Push(List<Guid?> userIds, NotificationTemplate template)
        {
            var list = userIds.ConvertAll<string>(x => x.ToString()).ToArray();
            string sql = "select clientId from SysUserClientId where userId in('" + string.Join("','", list) + "')";
            var rows = SqlHelper.Query(sql).Tables[0].Rows;

            List<string> clientIds = new List<string>();
            foreach (DataRow row in rows)
                clientIds.Add(Convert.ToString(row[0]));

            Push(clientIds, template);
        }
        public static void Push(string clientId, NotificationTemplate template)
        {
            Push(new List<string>() { clientId }, template);
        }
        public static void Push(List<string> clientIds ,NotificationTemplate template)
        {
            IGtPush push = new IGtPush(host, appKey, masterSecret);            
            ListMessage message = new ListMessage() { Data = template };

            List<Target> targetList = new List<Target>();
            clientIds.ForEach(clientId => targetList.Add(new Target() { appId = appId, clientId = clientId }));

            string contentId = push.getContentId(message);
            string pushResult = push.pushMessageToList(contentId, targetList);
        }


        /// <summary>
        /// 创建消息模板
        /// </summary>
        /// <param name="content">json格式字符串</param>
        /// <returns></returns>
        public static NotificationTemplate Create(string title, string text, string logo, string logourl, string content)
        {
            NotificationTemplate template = new NotificationTemplate();
            template.AppId = appId;
            template.AppKey = appKey;
            //通知栏标题
            template.Title = title;
            //通知栏内容     
            template.Text = text;
            //通知栏显示本地图片
            template.Logo = logo;
            //通知栏显示网络图标
            template.LogoURL = logourl;
            //应用启动类型，1：强制应用启动  2：等待应用启动
            template.TransmissionType = "1";
            //透传内容  
            template.TransmissionContent = content;
            //接收到消息是否响铃，true：响铃 false：不响铃   
            template.IsRing = true;
            //接收到消息是否震动，true：震动 false：不震动   
            template.IsVibrate = true;
            //接收到消息是否可清除，true：可清除 false：不可清除    
            template.IsClearable = true;
            //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
            string begin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string end = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
            template.setDuration(begin, end);

            return template;
        }
    }
}
