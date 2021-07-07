using Common;
using DotNetCom.OpcDa;
using DotNetCom.SerialInterface;
using DotNetCom.Text;
using DotNetCom.Web;
using ScaduinoDevices;
using System;
using System.IO;
using System.Windows.Forms;
using CefSharp.WinForms;

namespace Scaduino
{
    public partial class MainForm : Form, IMainForm
    {
        private AppData appData;
        private Form localHmi;

        public AppDataFileDialog FileDialog { get; set; }

        public IAppData AppData
        {
            get => appData;
            set
            {
                appData = value as AppData;
                appTree.AddAppData(appData);
                appTree.Root.Application.Root.Tag = appData.Settings;
            }
        }

        public MainForm(AppData appData)
        {
            InitializeComponent();
            AppData = appData;
            FileDialog = new AppDataFileDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppData = FileDialog.New(AppData);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppData = FileDialog.Open(AppData);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppData.IsDefaultFile)
            {
                FileDialog.SaveAs(AppData);
            }
            else
            {
                AppData.Save();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDialog.SaveAs(AppData);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Stop();
            FileDialog.SaveDefault(AppData);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var module in appData.DaqModules)
            {
                module.Start();
            }
            foreach (var module in appData.DasModules)
            {
                module.Start();
            }
            foreach (var device in appData.Devices)
            {
                device.Start();
            }
            if (appData.Settings.StartLocalHmi)
            {
                StartLocalHmi();
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var module in appData.DaqModules)
            {
                module.Stop();
            }
            foreach (var module in appData.DasModules)
            {
                module.Stop();
            }
            foreach (var device in appData.Devices)
            {
                device.Stop();
            }
            if (appData.Settings.StartLocalHmi)
            {
                localHmi?.Close();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItem.Directory = appData.Directory;
            var addItemDialog = new AddItem();
            var r = addItemDialog.ShowDialog();
            dynamic obj = addItemDialog.ItemToAdd;
            TreeNode node;

            if(r == DialogResult.OK)
            {
                appData.Add(obj);

                ///DaqModules
                node = appTree.Root.DataAquisition;
                if (addItemDialog.ItemToAdd is OpcClient)
                {
                    obj.Name = addItemDialog.ItemName;
                    appTree.AddObjectToNode(node, obj, obj.Name, (int)AppTree.Image.OpcDa);
                }
                if (addItemDialog.ItemToAdd is SerialText)
                {
                    obj.Name = addItemDialog.ItemName;
                    appTree.AddObjectToNode(node, obj, obj.Name, (int)AppTree.Image.SerialJson);
                }

                ///DasModules
                node = appTree.Root.DataServer;
                if (addItemDialog.ItemToAdd is HttpTagsServer && !(addItemDialog.ItemToAdd is IDevice))
                {
                    obj.Name = addItemDialog.ItemName;
                    appTree.AddObjectToNode(node, obj, obj.Name, (int)AppTree.Image.Http);
                }
                if (addItemDialog.ItemToAdd is SerialTagsServer && !(addItemDialog.ItemToAdd is IDevice))
                {
                    obj.Name = addItemDialog.ItemName;
                    appTree.AddObjectToNode(node, obj, obj.Name, (int)AppTree.Image.Serial);
                }

                ///Devices
                node = appTree.Root.Devices;
                if (addItemDialog.ItemToAdd is UsbWifiHmiServer)
                {
                    obj.Name = addItemDialog.ItemName;
                    appTree.AddObjectToNode(node, obj, obj.Name, (int)AppTree.Image.UsbWifiHmiServer);
                }
                if (addItemDialog.ItemToAdd is HmiServerStation)
                {
                    obj.Name = addItemDialog.ItemName;
                    appTree.AddObjectToNode(node, obj, obj.Name, (int)AppTree.Image.HmiServerStation);
                }

                ///Files
                if (addItemDialog.ItemToAdd is FileInfo)
                {
                    appTree.AddFile(obj);
                }
            }
        }

        //public void AddFile(FileInfo fileInfo)
        //{
        //    TreeNode node;
        //    if (!fileInfo.Directory.Exists) fileInfo.Directory.Create();
        //    File.Create(fileInfo.FullName);
        //    appTree.Root.Application.Root.Expand();
        //    if (fileInfo.Extension == ".html")
        //    {
        //        node = appTree.Root.Application.Screens;
        //        appTree.AddObjectToNode(node, fileInfo, fileInfo.Name, (int)AppTree.Image.Html);
        //    }
        //    if (fileInfo.Extension == ".css")
        //    {
        //        node = appTree.Root.Application.Styles;
        //        appTree.AddObjectToNode(node, fileInfo, fileInfo.Name, (int)AppTree.Image.Css);
        //    }
        //    if (fileInfo.Extension == ".js")
        //    {
        //        node = appTree.Root.Application.Scripts;
        //        appTree.AddObjectToNode(node, fileInfo, fileInfo.Name, (int)AppTree.Image.Javascript);
        //    }
        //    if (fileInfo.Extension == ".json")
        //    {
        //        node = appTree.Root.Application.Data;
        //        appTree.AddObjectToNode(node, fileInfo, fileInfo.Name, (int)AppTree.Image.Json);
        //    }
        //}

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            appData.Save();
        }

        private void appTree_KeyDown(object sender, KeyEventArgs e)
        {
            var tree = sender as TreeView;
            var node = tree.SelectedNode;
            dynamic obj = node.Tag;
            if(e.KeyCode == Keys.Delete && obj != null)
            {
                var r = MessageBox.Show("Do you really want to delete this item?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(r == DialogResult.Yes)
                {
                    node.Remove();
                    appData.Remove(obj);
                }
            }
        }

        private void appTree_DoubleClick(object sender, EventArgs e)
        {
            var tree = sender as AppTree;
            if(tree.SelectedNode?.Tag is FileInfo)
            {
                var file = tree.SelectedNode.Tag as FileInfo;
                if(tabs.TabPages.Count > 0)
                {
                    foreach (TabPage item in tabs.TabPages)
                    {
                        if(item.Text == file.Name)
                        {
                            tabs.SelectedTab = item;
                            return;
                        }
                    }
                }
                var tab = new TabPage();
                IEditor editor;

                if (file.Name.Contains(".htm"))
                {
                    editor = new ScreenEditor();
                }
                else
                {
                    editor = new CodeEditor();
                }

                tab.Tag = editor;
                editor.FileName = file.FullName;
                editor.LoadFile();
                editor.Dock = DockStyle.Fill;
                tab.Text = file.Name;
                tab.Controls.Add(editor as Control);
                tabs.TabPages.Add(tab);
                tabs.SelectedTab = tab;
            }
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(tabs.SelectedTab != null)
            {
                var editor = tabs.SelectedTab.Tag as IEditor;
                if (editor.IsChanged)
                {
                    var r = MessageBox.Show("Save changes?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes) editor.SaveFile();
                    if (r == DialogResult.No) tabs.TabPages.Remove(tabs.SelectedTab);
                }
                tabs.TabPages.Remove(tabs.SelectedTab);
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(tabs.SelectedTab != null)
            {
                var editor = tabs.SelectedTab.Tag as IEditor;
                editor.SaveFile();
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var importFile = new OpenFileDialog()
            {
                Filter = "All Files | *.*",
                Multiselect = true
            };

            var r = importFile.ShowDialog();

            if(r == DialogResult.OK)
            {
                foreach (string file in importFile.FileNames)
                {
                    var oInfo = new FileInfo(file);
                    string origin = file;
                    string destiny = appData.Directory + "\\" + oInfo.Name;
                    var dInfo = new FileInfo(destiny);

                    File.Copy(origin, destiny);
                    appTree.AddFile(dInfo);
                    appData.Add(dInfo);
                }
            }
        }

        private void StartLocalHmi()
        {
            var form = new Form();
            var newBrowser = new ChromiumWebBrowser("");

            form.FormBorderStyle = appData.Settings.WindowMode == WindowMode.FullScreen ? FormBorderStyle.None : FormBorderStyle.Sizable;
            form.WindowState = FormWindowState.Maximized;
            form.Controls.Clear();
            newBrowser.Dock = DockStyle.Fill;

            form.Controls.Add(newBrowser);
            newBrowser.Load(appData.Settings.StartupUrl);
            form.Show();
            localHmi = form;
        }
    }

}
