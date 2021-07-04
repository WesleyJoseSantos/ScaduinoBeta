using DotNetCom.OpcDa;
using DotNetCom.SerialInterface;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Common;
using System.IO;
using System.Linq;
using FastColoredTextBoxNS;
using CefSharp.WinForms;

namespace MobileHmi
{
    public partial class MainForm : Form, IMainForm
    {
        private AppData appData;

        public IAppData AppData { get => appData; set => appData = value as AppData; }

        public AppNotifyIcon NotifyIcon { get; set; }

        public AppDataFileDialog FileDialog { get; set; }

        private ChromiumWebBrowser webBrowser;

        //private Form localHmi;

        public MainForm(AppData appData)
        {
            InitializeComponent();
            AppData = appData;
            FileDialog = new AppDataFileDialog();
            webBrowser = new ChromiumWebBrowser("", null);
        }

        private void StartScreen_Load(object sender, EventArgs e)
        {
            if(appData != null)
            {
                appData.MobileHmi.UpdateExplorer(treeView.Nodes[1], appData.HmiWebServer.ServerRoot);
            }
            splitContainer1.Panel2.Controls.Add(webBrowser);
            webBrowser.BringToFront();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppData = FileDialog.New(AppData) as AppData;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppData = FileDialog.Open(AppData) as AppData;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDialog.Save(AppData);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileDialog.SaveDefault(AppData);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Text)
            {
                case "OPC Client":
                    propertyGrid.SelectedObject = appData.OpcClient;
                    break;
                case "USB Tags Server":
                    propertyGrid.SelectedObject = appData.UsbTagsServer;
                    break;
                case "Http Tags Server":
                    propertyGrid.SelectedObject = appData.HttpTagsServer;
                    break;
                case "HMI Web Server":
                    propertyGrid.SelectedObject = appData.HmiWebServer;
                    break;
                case "Mobile HMI":
                    appData.MobileHmi.UpdateExplorer(treeView.Nodes[1], appData.HmiWebServer.ServerRoot);
                    break;
                default:
                    if(e.Node.Tag is string)
                    {
                        var filePath = e.Node.Tag as string;
                        if (File.Exists(filePath))
                        {
                            var previousFile = codeEditor.Tag as string;
                            if (File.Exists(previousFile)){
                                if (codeEditor.IsChanged)
                                {
                                    codeEditor.SaveToFile(previousFile, System.Text.Encoding.UTF8);
                                }
                            }
                            codeEditor.Language = GetLanguage(filePath);
                            codeEditor.Tag = filePath;
                            codeEditor.OpenFile(filePath);
                            splitContainer1.Panel2Collapsed = true;
                            if (filePath.Contains(".htm"))
                            {
                                splitContainer1.Panel2Collapsed = false;
                                webBrowser.Load(filePath);
                            }
                        }
                    }
                    break;
            }
        }

        public Language GetLanguage(string file)
        {
            var ext = file.Split('.').ToList().Last();
            switch (ext)
            {
                case "js":
                    return Language.JS;
                case "css":
                    return Language.JS;
                default:
                    return Language.HTML;
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartServer();
            splitContainer1.Panel2Collapsed = false;
            webBrowser.Load(appData.HmiWebServer.Url);
        }

        private void runHMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartServer();
            StartLocalHmi();
        }

        private void StartServer()
        {
            if (appData.OpcClient.Enabled) appData.OpcClient.Connect();
            if (appData.UsbTagsServer.Enabled) appData.UsbTagsServer.Begin();
            if (appData.HttpTagsServer.Enabled) appData.HttpTagsServer.Start();
            if (appData.HmiWebServer.Enabled) appData.HmiWebServer.Start();
            UpdateNotifyStatus();
        }

        private void StartLocalHmi()
        {
            var localHmi = new Form();
            var newBrowser = new ChromiumWebBrowser("", null);

            localHmi.FormBorderStyle = FormBorderStyle.None;
            localHmi.WindowState = FormWindowState.Maximized;
            localHmi.Controls.Clear();
            newBrowser.Dock = DockStyle.Fill;

            localHmi.Controls.Add(newBrowser);
            newBrowser.Load(appData.HmiWebServer.Url);
            localHmi.Show();
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appData.OpcClient.Disconnect();
            appData.UsbTagsServer.End();
            appData.HttpTagsServer.Stop();
            appData.HmiWebServer.Stop();
            UpdateNotifyStatus();
        }

        private void Server_StatusChanged(object sender, EventArgs e)
        {
            UpdateNotifyStatus();
        }

        private void UpdateNotifyStatus()
        {
            var opcOk = appData.OpcClient.State == ClientState.Connected;
            var usbOk = appData.UsbTagsServer.Running;
            var httpOk = appData.HttpTagsServer.Status == DotNetCom.Web.WebServerStatus.Running;
            var webOk = appData.HmiWebServer.Status == DotNetCom.Web.WebServerStatus.Running;
            var text = opcOk ? "OPC Server Is Running!\n" : "OPC Server Is Stopped!\n";
            text += usbOk ? "USB Tags Server Is Running!\n" : "USB Tags Server Is Stopped!\n";
            text += httpOk ? "Http Tags Server Is Running!\n" : "Http Tags Server Is Stopped!\n";
            text += webOk ? "HMI Web Server Is Running!" : "HMI Web Server Is Stopped!";
            NotifyIcon.Notify.BalloonTipTitle = "HMI Mobile Status:";
            NotifyIcon.Notify.BalloonTipText = text;
            NotifyIcon.Notify.ShowBalloonTip(5000);
        }

        private void projectExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainContainer.Panel1Collapsed = !mainContainer.Panel1Collapsed;
        }

        private void propertyGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //workspaceContainer.Panel2Collapsed = !workspaceContainer.Panel2Collapsed;
        }

        private void itemViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //workspaceContainer.Panel1Collapsed = !workspaceContainer.Panel1Collapsed;
        }

        private void codeEditor_CustomAction(object sender, CustomActionEventArgs e)
        {
            var file = treeView.SelectedNode.Tag as string;
            if (File.Exists(file))
            {
                if (codeEditor.IsChanged)
                {
                    codeEditor.SaveToFile(file, System.Text.Encoding.UTF8);
                    webBrowser.Load(file);
                }
            }
        }
    }
}
