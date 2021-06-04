using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace BarusVoiceInput
{
    class VoiceRecognizeEngine
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        SimpleWebServer webServer;
        public bool onPause = false;
        public bool runningBrowser { get; protected set; }

        class TransferJsonObj
        {
            public string head { get; set; }
            public Newtonsoft.Json.Linq.JObject body { get; set; }
        }

        public void Setup()
        {
            webServer = new SimpleWebServer();
            webServer.Start($"http://localhost:{ConfigManager.Instance.Data.UsePort}/");
            webServer.OnReceiveMessage += OnReceiveMessage;
        }

        private void OnReceiveMessage(System.Net.WebSockets.WebSocket webSocket, string message)
        {
            Console.WriteLine(message);
            if (message.Length < 2) { return; }
            int msCode = int.Parse(message[0].ToString());
            string msBody = message.Substring(1);
            if (msCode == 0 && !onPause)
            {
                Thread thread = new Thread(delegate () {
                    try
                    {
                        //object tempData = Clipboard.GetDataObject();
                        Clipboard.SetText(ReplaceText(msBody));
                        keybd_event(17, 0, 0, 0);//down Ctrl key
                        keybd_event(86, 0, 0, 0);//down V key
                        keybd_event(86, 0, 2, 0);//up V key
                        keybd_event(17, 0, 2, 0);//up Ctrl key
                        //Thread.Sleep(500);//少し待ってみる
                        //Clipboard.SetDataObject(tempData, true);
                    }catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex}");
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }else if (msCode == 1 && msBody == "close")
            {
                runningBrowser = false;
                Console.WriteLine($"browser closed. runnnigFlag:{runningBrowser}");
            }else if (msCode == 5 && msBody == "getSettingsJson")
            {
                string json_text = "{\"head\": \"settings_json\", \"body\": "+ConfigManager.Instance.GetJsonText()+"}";
                webServer.SendMessage(webSocket, "6"+json_text);
            }else if (msCode == 6)
            {
                TransferJsonObj jsonObj = JsonConvert.DeserializeObject<TransferJsonObj>(msBody);
                if (jsonObj.head == "settings_json")
                {
                    ConfigData configData = jsonObj.body.ToObject<ConfigData>();
                    ConfigManager.Instance.SetData(configData);
                }
            }
        }

        string ReplaceText(string text)
        {
            if (ConfigManager.Instance.Data.ReplaceTable.ContainsKey(text))
            {
                return ConfigManager.Instance.Data.ReplaceTable[text];
            }
            else
            {
                return text;
            }
        }

        public void StartBrowser(string url, bool changeRunningFlag)
        {
            string chromePath = ConfigManager.Instance.Data.ChromePath;
            //string hostname = $"localhost:{ConfigManager.Instance.Data.UsePort}";
            if (!File.Exists(chromePath))
            {
                Console.WriteLine("chromeのパスが間違っているか、存在しません。");
                Console.WriteLine(chromePath);
                return;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("--incognito --disable-extensions ");
            stringBuilder.Append($"--app={url} ");
            stringBuilder.Append($"--unsafely-treat-insecure-origin-as-secure={url}");
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = chromePath,
                Arguments = stringBuilder.ToString(),
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process process = Process.Start(startInfo);
            process.EnableRaisingEvents = true;
            process.Exited += delegate
            {
                process.Dispose();
            };
            if (changeRunningFlag)
            {
                runningBrowser = true;
            }
        }

        public void CloseBrowser()
        {

        }

        public void Stop()
        {
            webServer.Stop();
        }
    }
}
