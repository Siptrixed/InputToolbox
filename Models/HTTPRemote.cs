using InputToolbox.Import;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace InputToolbox.Models
{
    internal class HTTPRemote : IDisposable
    {
        private readonly HttpListener _listener;

        public WebSocketServer webSocketServer { get; }

        public HTTPRemote()
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:80/");
            _listener.Start();
            ThreadPool.QueueUserWorkItem(o =>
            {
                //Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            HttpListenerContext? ctx = c as HttpListenerContext;
                            try
                            {
                                if (ctx == null)
                                {
                                    return;
                                }

                                var rstr = Request(ctx.Request);
                                var buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch
                            {
                                // ignored
                            }
                            finally
                            {
                                if (ctx != null)
                                {
                                    ctx.Response.OutputStream.Close();
                                }
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            });
            webSocketServer = new("ws://0.0.0.0:8181");
            webSocketServer.AddWebSocketService<WebSockServ>("/sendBack");
            webSocketServer.Start();
        }
        private string Request(HttpListenerRequest x)
        {
            return Properties.Resources.ResourceManager.GetString("Remote");
        }
        public void Dispose()
        {
            _listener.Stop();
            _listener.Close();
            webSocketServer.Stop();
            GC.SuppressFinalize(this);
        }
    }
    public class WebSockServ : WebSocketBehavior
    {
        public WebSockServ()
        {

        }
        protected override void OnMessage(MessageEventArgs e)
        {
            try
            {
                string[] ts = e.Data.Split('|');
                int x = int.Parse(ts[0]) / 10;
                int y = int.Parse(ts[1]) / 10;
                var mp = WinApi.GetCursorPosition();
                WinApi.SetCursorPos(mp.X + x, mp.Y + y);
            }
            catch
            {
                Debug.WriteLine(e.Data);
            }
        }
    }
}
