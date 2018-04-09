using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Controllers
{
    public class BitServiceController : Controller
    {

        /// <summary>
        /// 查询本服务器WebSockets连接
        /// </summary>
        /// <returns></returns>
        public ActionResult WebSockets()
        {
            try
            {
                string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                BitNoticeService.ServerIps[ip] = DateTime.Now;
                LogHelper.SaveLog("WebSocketsService", "WebSockets查询：" + ip);
                return Json(new { Code = 0, Users = BitNoticeService.SocketUsers });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 向本服务器用户发送消息
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> SendWebSocketString()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.Default, true, 1024, true))
                {
                    var text = reader.ReadToEnd();
                    LogHelper.SaveLog("SendWebSocketString", text);
                    var msg = JsonConvert.DeserializeObject<BitNoticeMessage>(text);
                    await BitNoticeService.SendStringAsync(msg, true);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        /// <summary>
        /// 在线服务器和用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Online()
        {
            try
            {
                 
                List<Dictionary<string, string>> sockets = new List<Dictionary<string, string>>();
                foreach(var user in BitNoticeService.Sockets)
                {
                    foreach (var socket in user.Value)
                    {
                        Dictionary<string, string> item = new Dictionary<string, string>();
                        item["UserId"] = user.Key;
                        item["IsRemote"] = socket.IsRemote.ToString();
                        item["IsOpen"] = socket.IsRemote ? "" : (socket.WebSocket.State == WebSocketState.Open).ToString();
                        item["RemoteIp"] = socket.IsRemote ? socket.RemoteIp : "";
                        item["RemoteTime"] = socket.IsRemote ? socket.RemoteTime.ToString("yyyy-MM-dd HH:mm:ss") : "";
                        sockets.Add(item);
                    }
                }

                return Json(new { Server = BitNoticeService.ServerIps, Sockets = sockets });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }
}
