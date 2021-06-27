using System;
using System.Windows.Forms;
using Common;
using DotNetCom.General;
using DotNetCom.General.Tags;
using DotNetCom.OpcDa;
using DotNetCom.Text;

namespace ProDAq
{
    public partial class MainForm : Form, IMainForm
    {
        private AppData appData;
        private TreeNode lastNode;

        public IAppData AppData { get => appData; set => appData = value as AppData; }

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
            LoadModules();
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
            FileDialog.SaveDefault(AppData);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeModules.SelectedNode == null)
            {
                MessageBox.Show("Please, select an valid module group.");
                return;
            }
            TreeNode parentNode = new TreeNode();
            object newModule;
            switch (treeModules.SelectedNode.Text)
            {
                case "OPC DA":
                    newModule = new OpcClient();
                    parentNode = treeModules.Nodes[0];
                    AddModuleToNode(parentNode, newModule as IComModule, 7);
                    appData.AddModule(newModule as OpcClient);
                    break;
                case "Text Interface":
                    newModule = new SerialText();
                    parentNode = treeModules.Nodes[1];
                    AddModuleToNode(parentNode, newModule as IComModule, 8);
                    appData.AddModule(newModule as SerialText);
                    break;
                default:
                    break;
            }
            parentNode.Expand();
        }

        private TreeNode AddModuleToNode(TreeNode parentNode, IComModule newModule, int imgIdx)
        {
            var newNode = new TreeNode();
            newNode.Text = $"{newModule.Name}";
            newModule.Name = newNode.Text;
            newNode.Tag = newModule;
            newNode.ImageIndex = imgIdx;
            newNode.SelectedImageIndex = imgIdx;
            parentNode.Nodes.Add(newNode);
            parentNode.Expand();
            return newNode;
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
            if(e.ChangedItem.Label == "Name")
            {
                if(lastNode != null)
                {
                    lastNode.Text = e.ChangedItem.Value as string;
                }
            }
        }

        private void LoadModules()
        {
            int nodeIdx = 0;
            foreach (var item in appData.ComModules)
            {
                var node = treeModules.Nodes[nodeIdx];
                AddModulesToNode(node, item.Value, item.Key);
                nodeIdx++;
            }
        }

        private void AddModulesToNode(TreeNode node, IComModule[] modules, int imgIdx)
        {
            foreach (var item in modules)
            {
                AddModuleToNode(node, item, imgIdx);
            }
        }

        private void LoadSignals()
        {

        }
    }
}
