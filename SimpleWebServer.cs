using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.WebSockets;
using System.IO;

namespace BarusVoiceInput
{
    delegate void WSReceiveMessageHandler(WebSocket webSocket, string message);

    class SimpleWebServer
    {
        HttpListener httpListener;
        List<WebSocket> wsConnections;
        public event WSReceiveMessageHandler OnReceiveMessage;

        public void Start(params string[] prefixes)
        {
            httpListener = new HttpListener();
            wsConnections = new List<WebSocket>();
            foreach (string prefix in prefixes)
            {
                httpListener.Prefixes.Add(prefix);
            }
            httpListener.Start();
            httpListener.BeginGetContext(OnRequested, null);
        }

        void OnRequested(IAsyncResult ar)
        {
            if (!httpListener.IsListening)
            {
                return;
            }
            HttpListenerContext context = httpListener.EndGetContext(ar);
            httpListener.BeginGetContext(OnRequested, httpListener);
            try
            {
                if (ProcessGetRequest(context))
                {
                    return;
                }
                if (ProcessWebSocketRequest(context))
                {
                    return;
                }
            }catch (Exception ex)
            {
                ReturnInternalError(context.Response, ex);
            }
        }

        bool CanAccept(HttpMethod expected, string requested)
        {
            return string.Equals(expected.Method, requested, StringComparison.CurrentCultureIgnoreCase);
        }

        bool ProcessGetRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            if (!CanAccept(HttpMethod.Get, request.HttpMethod) || request.IsWebSocketRequest) { return false; }
            response.StatusCode = (int)HttpStatusCode.OK;
            using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
            {
                string file = request.RawUrl.Split('?')[0];
                //writer.Write(Properties.Resources.ResourceManager.GetObject(""));
                if (file == "/" || file == "/index.html")
                {
                    response.ContentType = "text/html";
                    writer.Write(Properties.Resources.html);
                }else if (file == "/main.js")
                {
                    response.ContentType = "text/javascript";
                    writer.Write(Properties.Resources.javascript);
                }else if (file == "/settings.html")
                {
                    response.ContentType = "text/html";
                    writer.Write(Properties.Resources.settings_html);
                }else if (file == "/settings_js.js")
                {
                    response.ContentType = "text/javascript";
                    writer.Write(Properties.Resources.settings_js);
                }
                else
                {
                    writer.WriteLine($"you have sent headers:\n{request.Headers}");
                    writer.WriteLine($"your requested uri: {request.RawUrl}");
                }
            }
            response.Close();
            return true;
        }

        bool ProcessWebSocketRequest(HttpListenerContext context)
        {
            if (!context.Request.IsWebSocketRequest) { return false; }
            WebSocket webSocket = context.AcceptWebSocketAsync(null).Result.WebSocket;
            wsConnections.Add(webSocket);
            ProcessReceivedMessage(webSocket, OnReceiveMessage);
            return true;
        }

        async void ProcessReceivedMessage(WebSocket webSocket, WSReceiveMessageHandler onReceiveMessage)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(buffer, System.Threading.CancellationToken.None);
                if (receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer.Take(receiveResult.Count).ToArray());
                    onReceiveMessage(webSocket, message);
                }
            }
            wsConnections.Remove(webSocket);
        }

        public void SendMessageToAll(string message)
        {
            foreach (WebSocket ws in wsConnections)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                ws.SendAsync(buffer, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
            }
        }

        public void SendMessage(WebSocket webSocket, string message)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }

        void ReturnInternalError(HttpListenerResponse response, Exception cause)
        {
            Console.WriteLine(cause);
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "text/plain";
            try
            {
                using (var writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
                {
                    writer.Write(cause.ToString());
                }
                response.Close();
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                response.Abort();
            }
        }

        public void Stop()
        {
            httpListener.Stop();
            httpListener.Close();
        }
    }
}
