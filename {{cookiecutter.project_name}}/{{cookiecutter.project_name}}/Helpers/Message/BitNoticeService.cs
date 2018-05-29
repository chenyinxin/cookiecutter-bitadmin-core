/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Services
{
    /*
     * 基于WebSocket的消息推送服务
     * 分布式后台通讯端口1899，部署多台服务器时，需要能相互访问。
     */
    public class BitNoticeService
    {
        public static int SocketCount => sockets.Count;
        public static List<string> SocketUsers
        {
            get
            {
                List<string> user = new List<string>();
                foreach (var socket in sockets)
                {
                    if (socket.Value.Count(x => !x.IsRemote) > 0)
                        user.Add(socket.Key);
                }
                return user;
            }
        }
        public static Dictionary<string, List<UserSocket>> Sockets => sockets;

        public static async Task SendStringAsync(BitNoticeMessage message,bool isLocal=false)
        {
            foreach (var user in sockets)
            {
                if (user.Key != message.Receiver) continue;
                foreach (var socket in user.Value)
                {
                    //LogHelper.SaveLog("WebSockets", string.Format("发送消息：{0}:{1}:{2}:{3}", socket.IsRemote, socket.RemoteIp, socket.RemoteTime, JsonConvert.SerializeObject(message)));
                    if (socket.IsRemote && !isLocal)
                    {//远程通道
                        try
                        {
                            WebClient wc = new WebClient();
                            byte[] sendByte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
                            var url = string.Format("http://{0}:1899/BitService/SendWebSocketString", socket.RemoteIp);
                            //LogHelper.SaveLog("WebSockets", string.Format("发送远程消息：{0}", url));
                            var result = wc.UploadData(url, sendByte);
                            var strResult = Encoding.Default.GetString(result);
                        }
                        catch (Exception ex) { LogHelper.SaveLog(ex); }
                    }
                    else if (socket.WebSocket.State == WebSocketState.Open)
                    {//本地通道
                        await SendStringAsync(socket.WebSocket, JsonConvert.SerializeObject(message), socket.HttpContext.RequestAborted);
                    }
                }
            }
        }
        static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
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
                    //LogHelper.SaveLog("WebSockets", "rev->" + data + ",count->" + sockets.Count);
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
            sockets[sid].Add(new UserSocket() { HttpContext = context, WebSocket = currentSocket, IsRemote = false });

            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                string request = await ReceiveStringAsync(currentSocket, ct);
                if (string.IsNullOrEmpty(request))
                    continue;

                BitNoticeMessage msg = JsonConvert.DeserializeObject<BitNoticeMessage>(request);
                msg.Sender = SSOClient.UserId.ToString();
                switch (msg.MessageType)
                {
                    case "notice":  //通知消息
                        LogHelper.SaveLog("WebSockets", "接收到通知消息：" + request);
                        break;
                    case "chat":    //聊天消息
                        await SendStringAsync(msg);
                        LogHelper.SaveLog("WebSockets", "接收到聊天消息：" + request);
                        break;
                    default:
                        continue;
                }

            }
        }
        static Timer timer, timer2;
        public static void Map(IApplicationBuilder app)
        {
            //清除无效(本地)连接(每30秒轮询服务)
            timer = new Timer(x =>
            {
                foreach (var user in sockets)
                {
                    List<UserSocket> rm = new List<UserSocket>();
                    user.Value.ForEach(socket =>
                    {
                        if (!socket.IsRemote && socket.WebSocket.State != WebSocketState.Open)
                            rm.Add(socket);
                    });
                    rm.ForEach(socket =>
                    {
                        user.Value.Remove(socket);
                        socket.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", socket.HttpContext.RequestAborted);
                        socket.WebSocket.Dispose();
                        //LogHelper.SaveLog("WebSockets", "remtime:sid->" + user.Key + ",count->" + user.Value.Count);
                    });
                }
            }, null, 0, 30000);
            app.UseWebSockets();
            app.Use(Acceptor);
             
            //分布式支持，开放1899端口作为后台通讯(每30秒轮询服务)。
            //注册本地IP
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            DataContext dbContext = new DataContext();
            List<string> iplist = new List<string>();
            for (int i = 0; i < addressList.Length; i++)
            {
                iplist.Add(addressList[i].ToString());
                var item = dbContext.SysServer.Find(addressList[i].ToString());
                if (item != null) continue;

                SysServer sysServer = new SysServer() { ServerIp = addressList[i].ToString(), CreateTime = DateTime.Now, UpdateTime = DateTime.Now };
                dbContext.SysServer.Add(sysServer);
                dbContext.SaveChanges();
            }

            //查询所有非本地IP
           dbContext.SysServer.ToList().ForEach(x=> {
               if (!iplist.Contains(x.ServerIp))
                   ServerIPAddress[x.ServerIp] = DateTime.MinValue;
            });
            //LogHelper.SaveLog("WebSockets", "ServerIps:" + JsonConvert.SerializeObject(ServerIPAddress));

            //轮询所有非本地IP
            timer2 = new Timer(x =>
            { 
                foreach(var ip in ServerIPAddress)
                {
                    new Thread(new ThreadStart(delegate {
                        try 
                        {
                            WebClient wc = new WebClient();
                            string result = wc.DownloadString(string.Format("http://{0}:1899/BitService/WebSockets", ip.Key));
                            var json = JObject.Parse(result);
                            ServerIPAddress[ip.Key] = DateTime.Now;

                            //LogHelper.SaveLog("WebSockets", "WebSocketsResult:" + result); 

                            var users = (JArray)json["users"];
                            foreach(var user in users)
                            {
                                string sid = (string)user;
                                if (!sockets.ContainsKey(sid))
                                    sockets.TryAdd(sid, new List<UserSocket>());

                                if (sockets[sid].Count(y => y.RemoteIp == ip.Key) == 0)
                                    sockets[sid].Add(new UserSocket() { IsRemote = true, RemoteIp = ip.Key, RemoteTime = DateTime.Now });
                                else
                                    sockets[sid].Find(y => y.RemoteIp == ip.Key).RemoteTime = DateTime.Now;                                
                            }
                        }
                        catch { }
                    })).Start();
                }
            }, null, 0, 30000);
        }
        public static Dictionary<string, DateTime> ServerIPAddress = new Dictionary<string, DateTime>();
    }
    public class UserSocket
    {
        public HttpContext HttpContext { get; set; }
        public WebSocket WebSocket { get; set; }
        public bool IsRemote { get; set; }
        public string RemoteIp { get; set; }
        public DateTime RemoteTime { get; set; }
    }
    public class BitNoticeMessage
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string MessageType { get; set; }        
        public string Content { get; set; }
    }
}
