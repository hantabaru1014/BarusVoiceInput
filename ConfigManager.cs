using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace BarusVoiceInput
{
    public class ConfigData
    {
        public string ChromePath;
        public int UsePort;
        public string Language;
        public Dictionary<string, string> ReplaceTable;
        public MOD_KEY HotkeyMODKey;
        public System.Windows.Forms.Keys HotkeyKey;

        public ConfigData()
        {
            ChromePath = "";
            UsePort = 51015;
            Language = "ja-JP";
            ReplaceTable = new Dictionary<string, string>();
            ReplaceTable.Add("改行", "\n");
            HotkeyMODKey = MOD_KEY.CONTROL;
            HotkeyKey = System.Windows.Forms.Keys.Space;
        }
    }

    public class ConfigManager
    {
        public static ConfigManager Instance { get; protected set; }
        public ConfigData Data { get; protected set; }
        public string SettingsFilePath;
        static ConfigManager()
        {
            Instance = new ConfigManager();
        }
        private ConfigManager()
        {
            Data = new ConfigData();
            SettingsFilePath = "settings.json";
        }
        public void Load()
        {
            if (!File.Exists(SettingsFilePath))
            {
                InitSettings();
                Save();
                return;
            }
            using (StreamReader sr = new StreamReader(SettingsFilePath, Encoding.UTF8))
            {
                string data = sr.ReadToEnd();
                Data = JsonConvert.DeserializeObject<ConfigData>(data);
            }
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(SettingsFilePath, false, Encoding.UTF8))
            {
                sw.Write(json);
            }
        }
        public string GetJsonText()
        {
            return JsonConvert.SerializeObject(Data, Formatting.None);
        }
        public void SetData(ConfigData configData)
        {
            Data = configData;
        }
        public void InitSettings()
        {
            Data = new ConfigData();
            SearchChromePath();
        }
        public void SearchChromePath()
        {
            foreach (string path in new List<string>
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Chrome\\Application\\chrome.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google\\Chrome\\Application\\chrome.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Google\\Chrome\\Application\\chrome.exe")
            })
            {
                if (File.Exists(path))
                {
                    Data.ChromePath = path;
                    break;
                }
            }
        }
    }
}
