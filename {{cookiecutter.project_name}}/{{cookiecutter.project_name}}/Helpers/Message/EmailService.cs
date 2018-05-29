/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class EmailService
    {
        /*
         * 邮件一般用来发送通知，示例用QQ邮件。
         * 登陆qq邮箱时，密码是授权码（设置->账户->生成授权码）
         */
        public static void SendMail(string subject, string html, string tomail, string displayName = "")
        {
            Dictionary<string, string> tolist = new Dictionary<string, string>();
            tolist.Add(tomail, displayName);
            SendMail(subject, html, tolist);
        }
        public static void SendMail(string subject, string html, Dictionary<string, string> tomail)
        {
            string fromMail = "bit@bitdao.cn";

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(fromMail,"{{cookiecutter.project_name}}")
            };
            foreach (var item in tomail)
            {
                if (string.IsNullOrEmpty(item.Value))
                    mailMessage.To.Add(new MailAddress(item.Key));
                else
                    mailMessage.To.Add(new MailAddress(item.Key, item.Value));
            }
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Subject = subject;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.Body = html;
            mailMessage.IsBodyHtml = true;

            //发送图片附件等，请参照：https://www.cnblogs.com/phasd/p/7439696.html

            SmtpClient sendClient = new SmtpClient("smtp.qq.com", 25)
            {
                Credentials = new NetworkCredential(fromMail, "vaiiskehnyptbjcj")
            };

            sendClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            sendClient.EnableSsl = true;
            sendClient.Send(mailMessage);
        }
    }
}
