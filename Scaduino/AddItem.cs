using DotNetCom.OpcDa;
using DotNetCom.SerialInterface;
using DotNetCom.Text;
using DotNetCom.Web;
using ScaduinoDevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Scaduino
{
    public partial class AddItem : Form
    {
        public enum RootNode : int
        {
            DataAquisition,
            DataServer,
            Devices,
            Files
        }

        public dynamic ItemToAdd { get; set; }

        public string ItemName { get => tbName.Text; }

        public static string Directory { get; set; }

        private ListViewItem[] dataAquisitionItems =
        {
            new ListViewItem()
            {
                Text = "OPC DA", 
                Name = "OPC DA",
                ImageIndex = (int)AppTree.Image.OpcDa,
                Tag = new OpcClient()
            },
            new ListViewItem()
            {
                Text = "Serial Json",
                Name = "Serial Json",
                ImageIndex = (int)AppTree.Image.SerialJson,
                Tag = new SerialText()
            },
        };

        private ListViewItem[] dataServerItems =
        {
            new ListViewItem()
            {
                Text = "Http Server",
                Name = "Http Server",
                ImageIndex = (int)AppTree.Image.Http,
                Tag = new HttpTagsServer()
            },
            new ListViewItem()
            {
                Text = "Serial",
                Name = "Serial",
                ImageIndex = (int)AppTree.Image.Serial,
                Tag = new SerialTagsServer()
            },
        };

        private ListViewItem[] devicesItems =
        {
            new ListViewItem()
            {
                Text = "USB WiFi HMI Server",
                Name = "USB WiFi HMI Server",
                ImageIndex = (int)AppTree.Image.UsbWifiHmiServer,
                Tag = new UsbWifiHmiServer()
            },
            new ListViewItem()
            {
                Text = "HMI Web Server Station",
                Name = "HMI Web Server Station",
                ImageIndex = (int)AppTree.Image.HmiServerStation,
                Tag = new HmiServerStation()
            },
        };

        private ListViewItem[] filesItems =
        {
            new ListViewItem()
            {
                Text = "Screen File",
                Name = "NewScreen.html",
                ImageIndex = (int)AppTree.Image.Html,
                Tag = new FileInfo(Directory + "\\NewScreen.html")
            },
            new ListViewItem()
            {
                Text = "Style File",
                Name = "NewStyle.css",
                ImageIndex = (int)AppTree.Image.Css,
                Tag = new FileInfo(Directory + "\\NewStyle.css")
            },
            new ListViewItem()
            {
                Text = "Script File",
                Name = "NewScript.js",
                ImageIndex = (int)AppTree.Image.Javascript,
                Tag = new FileInfo(Directory + "\\NewScript.js")
            },
            new ListViewItem()
            {
                Text = "Data File",
                Name = "NewData.json",
                ImageIndex = (int)AppTree.Image.Json,
                Tag = new FileInfo(Directory + "\\NewData.json")
            },
        };

        private List<ListViewItem[]> listViewItems;

        public AddItem()
        {
            InitializeComponent();
            listViewItems = new List<ListViewItem[]>();
            listViewItems.Add(dataAquisitionItems);
            listViewItems.Add(dataServerItems);
            listViewItems.Add(devicesItems);
            listViewItems.Add(filesItems);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int idx = e.Node.Index;
            listView.Clear();
            listView.Items.AddRange(listViewItems[idx]);
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = sender as ListView;
            if (list == null || list.SelectedItems.Count == 0) return;
            var it = list.SelectedItems[0];
            tbName.Text = it.Name;
            ItemToAdd = it.Tag;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if(ItemToAdd != null)
            {
                if(!(ItemToAdd is FileInfo))
                {
                    ItemToAdd.Name = tb.Text;
                }
                else
                {
                    ItemToAdd = new FileInfo(Directory + "\\" + tb.Text);
                }
            }
        }
    }

}
