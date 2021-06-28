using System;
using System.Windows.Forms;
using Common;
using DotNetCom.DataBase;
using DotNetCom.General;
using DotNetCom.General.Tags;
using DotNetCom.OpcDa;
using DotNetCom.Text;
using DotNetScadaComponents.Trend;

namespace ProDAq
{
    public partial class MainForm : Form, IMainForm
    {
        private AppData appData;
        private TreeNode lastNode;
        private bool running;

        public IAppData AppData
        {
            get => appData;
            set
            {
                appData = value as AppData;
                appTree.AddModules(appData.ComModules);
                appTree.AppNodes.Datalogging.Tag = appData.Datalogger;
            }
        }

        public AppNotifyIcon NotifyIcon { get; set; }

        public AppDataFileDialog FileDialog { get; set; }

        public MainForm(AppData appData)
        {
            InitializeComponent();
            AppData = appData;
            FileDialog = new AppDataFileDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTrends();
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
            FileDialog.Save(AppData);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            FileDialog.SaveDefault(AppData);
            
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            foreach (var module in appData.ComModules)
            {
                if (module.Enabled) module.Start();
            }
            foreach (var trend in appData.Trends)
            {
                trend.Start();
            }
            appData.Datalogger.Start();

            LoadSignals();
            propertyGrid.Enabled = false || propertyGrid.SelectedObject is TrendChartSettings;
            running = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            foreach (var module in appData.ComModules)
            {
                module.Stop();
            }
            foreach (var trend in appData.Trends)
            {
                trend.Stop();
            }
            appData.Datalogger.Stop();

            propertyGrid.Enabled = true;
            running = false;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(appTree.SelectedNode == null)
            {
                MessageBox.Show("Please, select an valid module group.");
                return;
            }
            TreeNode parentNode = new TreeNode();
            object newModule;
            switch (appTree.SelectedNode.Text)
            {
                case "OPC DA":
                    newModule = new OpcClient();
                    parentNode = appTree.AppNodes.OpcDa;
                    appTree.AddModule(newModule as OpcClient);
                    appData.AddModule(newModule as OpcClient);
                    break;
                case "Text Interface":
                    newModule = new SerialText();
                    parentNode = appTree.AppNodes.TextInterface;
                    appTree.AddModule(newModule as SerialText);
                    appData.AddModule(newModule as SerialText);
                    break;
                default:
                    break;
            }
            parentNode.Expand();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteNode(lastNode);
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = e.Node.Tag;
            lastNode = e.Node;
        }

        private void treeModules_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                var tree = sender as TreeView;
                deleteNode(tree.SelectedNode);
            }
        }

        private void deleteNode(TreeNode node)
        {
            if (node != null && node.Tag != null)
            {
                var dialog = MessageBox.Show("This operation is irreversible. Confirm module delete?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    appData.RemoveItem(node.Tag);
                    node.Remove();
                }
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var pGrid = s as PropertyGrid;
            var obj = pGrid.SelectedObject;
            if(e.ChangedItem.Label == "Name" && obj is IComModule)
            {
                if(lastNode != null)
                {
                    lastNode.Text = e.ChangedItem.Value as string;
                }
            }
            if (e.ChangedItem.Label == "Items" && obj is IComModule)
            {
                LoadSignals();
            }
            if (e.ChangedItem.Label == "Name" && obj is TrendChartSettings)
            {
                tabControl1.SelectedTab.Text = e.ChangedItem.Value as string;
            }
        }

        private void LoadSignals()
        {
            var parent = treeSignals.Nodes[0];
            parent.Nodes.Clear();
            foreach (var item in Data.TagsDataBase.Tags)
            {
                var tag = item.Value;
                if (tag.Value == null) continue;
                var imgIdx = tag.Value is bool ? 10 : 11;
                AddSignalToNode(parent, tag, imgIdx);
            }
        }   

        private void AddSignalToNode(TreeNode node, Tag tag, int imgIdx)
        {
            var newNode = new TreeNode(tag.Name);
            newNode.Tag = tag;
            newNode.ImageIndex = imgIdx;
            newNode.SelectedImageIndex = imgIdx;
            node.Nodes.Add(newNode);
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tab = sender as TabControl;
            if(tab.SelectedIndex == 1)
            {
                LoadSignals();
            }
        }

        private void treeSignals_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var tree = sender as TreeView;
            tree.SelectedNode = e.Item as TreeNode;
            var tag = tree.SelectedNode.Tag;
            tree.DoDragDrop(tag, DragDropEffects.Copy);
        }

        private void trend_DragEnter(object sender, DragEventArgs e)
        {
            var tag = e.Data.GetData(typeof(Tag)) as Tag;
            if(tag is Tag)
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void trend_DragDrop(object sender, DragEventArgs e)
        {
            var tag = e.Data.GetData(typeof(Tag)) as Tag;
            if (tag is Tag)
            {
                if(sender is Trend)
                {
                    var trend = sender as Trend;
                    trend.Add(tag);
                }
                else
                {
                    var trend = new Trend();
                    AddTrend(trend);
                    trend.Add(tag);
                    appData.AddTrend(trend);
                }
            }
        }

        private void trend_Click(object sender, EventArgs e)
        {
            var trend = sender as Trend;
            propertyGrid.SelectedObject = trend.Settings;
        }

        private void AddTrend(Trend trend)
        {
            var tab = new TabPage();

            trend.Dock = DockStyle.Fill;
            trend.Click += trend_Click;
            trend.AllowDrop = true;
            splitContainer1.AllowDrop = false;
            trend.DragEnter += trend_DragEnter;
            trend.DragDrop += trend_DragDrop;

            tab.Controls.Add(trend);
            tab.Text = trend.Settings.Name;

            tabControl1.TabPages.Add(tab);

            if (running) trend.Start();
        }


        private void LoadTrends()
        {
            foreach (var trend in appData.Trends)
            {
                AddTrend(trend);
                trend.Initialize();
            }
        }

        private void propertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var pGrid = sender as PropertyGrid;
            var obj = pGrid.SelectedObject;
            if(obj is TrendChartSettings)
            {
                pGrid.Enabled = true;
            }
            else
            {
                pGrid.Enabled = !running;
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appData.Datalogger.Open();
        }
    }
}
