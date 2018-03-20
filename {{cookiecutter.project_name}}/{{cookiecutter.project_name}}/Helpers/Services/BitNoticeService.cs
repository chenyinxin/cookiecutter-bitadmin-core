/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Services
{
    /// <summary>
    /// 基于WebSocket的消息推送服务
    /// </summary>
    public class BitNoticeService
    {
        public static async Task SendStringAsync(Guid? userId, string msg)
        {
            foreach (var user in sockets)
            {
                if (user.Key != userId.ToString()) continue;
                foreach (var socket in user.Value)
                {
                    if (socket.WebSocket.State == WebSocketState.Open)
                        await SendStringAsync(socket.WebSocket, msg, socket.HttpContext.RequestAborted);
                }
            }
        }
        static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            //LogHelper.SaveLog("socket", "send->" + data + ",count->" + sockets.Count);
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    string data = await reader.ReadToEndAsync();
                    //LogHelper.SaveLog("socket", "rev->" + data + ",count->" + sockets.Count);
                    return data;
                }
            }
        }

        static Dictionary<string, List<UserSocket>> sockets = new Dictionary<string, List<UserSocket>>();
        static async Task Acceptor(HttpContext context, Func<Task> n)
        {
            if (!context.WebSockets.IsWebSocketRequest || !SSOClient.IsLogin)
                return;

            CancellationToken ct = context.RequestAborted;
            var currentSocket = await context.WebSockets.AcceptWebSocketAsync();
            string sid = SSOClient.UserId.ToString();
            if (!sockets.ContainsKey(sid))
                sockets.TryAdd(sid, new List<UserSocket>());
            sockets[sid].Add(new UserSocket() { HttpContext = context, WebSocket = currentSocket });
            //LogHelper.SaveLog("socket", "add:sid->" + sid + ",count->" + sockets[sid].Count);

            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                string request = await ReceiveStringAsync(currentSocket, ct);
                if (string.IsNullOrEmpty(request))
                    continue;

                BitNoticeMessage msg = JsonConvert.DeserializeObject<BitNoticeMessage>(request);
                switch (msg.MessageType)
                {
                    case "chat"://回复处理
                        break;
                    default:
                        continue;
                }

            }
        }
        public static void Map(IApplicationBuilder app)
        {
            Timer timer = new Timer(x =>
            {
                //一个用户多个连接，通过定时器清除无效连接
                foreach (var user in sockets)
                {
                    //LogHelper.SaveLog("socket", "remtime:sid->" + user.Key + ",count->" + user.Value.Count);
                    List<UserSocket> rm = new List<UserSocket>();
                    user.Value.ForEach(socket =>
                    {
                        if (socket.WebSocket.State != WebSocketState.Open)
                            rm.Add(socket);
                    });
                    rm.ForEach(socket =>
                    {
                        user.Value.Remove(socket);
                        socket.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", socket.HttpContext.RequestAborted);
                        socket.WebSocket.Dispose();
                        //LogHelper.SaveLog("socket", "remtime:sid->" + user.Key + ",count->" + user.Value.Count);
                    });
                }
            }, null, 0, 5000);
            app.UseWebSockets();
            app.Use(Acceptor);
        }
    }
    public class UserSocket
    {
        public HttpContext HttpContext { get; set; }
        public WebSocket WebSocket { get; set; }
    }
    public class BitNoticeMessage
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string MessageType { get; set; }        
        public string Content { get; set; }
    }
}
