using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarusVoiceInput
{
    public partial class Form1 : Form
    {
        VoiceRecognizeEngine vrEngine;
        HotKey hotKey;

        public Form1()
        {
            InitializeComponent();
            ConfigManager.Instance.Load();
            vrEngine = new VoiceRecognizeEngine();
            vrEngine.Setup();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //vrEngine.StartBrowser();
            Console.WriteLine($"http://localhost:{ConfigManager.Instance.Data.UsePort}/");
            hotKey = new HotKey(ConfigManager.Instance.Data.HotkeyMODKey, ConfigManager.Instance.Data.HotkeyKey);
            hotKey.HotKeyPush += HotKeyPush;
        }

        private void HotKeyPush(object sender, EventArgs e)
        {
            TogglePause();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            vrEngine.Stop();
            ConfigManager.Instance.Save();
            hotKey.Dispose();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            vrEngine.StartBrowser($"http://localhost:{ConfigManager.Instance.Data.UsePort}/index.html?lang={ConfigManager.Instance.Data.Language}", true);
            buttonPause.Enabled = true;
            buttonStart.Enabled = false;
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            TogglePause();
        }

        private void TogglePause()
        {
            vrEngine.onPause = !vrEngine.onPause;
            if (vrEngine.onPause)
            {
                buttonPause.Text = "再開";
            }
            else
            {
                buttonPause.Text = "一時停止";
            }
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(ConfigManager.Instance.SettingsFilePath);
            vrEngine.StartBrowser($"http://localhost:{ConfigManager.Instance.Data.UsePort}/settings.html", false);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://barusupdateservice.web.app/products.html");
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            if (!vrEngine.runningBrowser)
            {
                buttonStart.Enabled = true;
                buttonPause.Enabled = false;
            }
            else
            {
                buttonStart.Enabled = false;
                buttonPause.Enabled = true;
            }
        }
    }
}
