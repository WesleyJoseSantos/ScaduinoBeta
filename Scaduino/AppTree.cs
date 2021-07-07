using DotNetCom.General;
using DotNetCom.OpcDa;
using DotNetCom.SerialInterface;
using DotNetCom.Text;
using DotNetCom.Web;
using ScaduinoDevices;
using System.IO;
using System.Windows.Forms;

namespace Scaduino
{
    public partial class AppTree : TreeView
    {
        private PropertyGrid propertyGrid;

        public enum Image : int
        {
            DataLogger,
            DataAquisition,
            DataServer,
            Devices,
            Application,
            Screens,
            Styles,
            Scripts,
            Data,
            Resources,
            OpcDa,
            SerialJson,
            Http,
            Serial,
            UsbWifiHmiServer,
            HmiServerStation,
            Html,
            Css,
            Javascript,
            Json,
            Resx
        }

        public enum RootNode : int
        {
            Datalogger,
            DataAquisition,
            DataServer,
            Devices,
            Application
        }

        public AppTreeRoot Root { get; set; }

        public PropertyGrid PropertyGrid
        {
            get => propertyGrid;
            set
            {
                if (propertyGrid != null) propertyGrid.PropertyValueChanged -= PropertyGrid_PropertyValueChanged;
                propertyGrid = value;
                if (propertyGrid != null) propertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
            }
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if(SelectedNode != null && SelectedNode.Tag != null)
            {
                if(e.ChangedItem.PropertyDescriptor.Name == "Name")
                {
                    dynamic selectedObj = SelectedNode.Tag;
                    SelectedNode.Text = selectedObj.Name;
                }
            }
        }

        public AppTree()
        {
            InitializeComponent();
        }

        public void AddAppData(AppData appData)
        {
            Root = new AppTreeRoot(this);

            Root.Datalogger.Tag = appData.Datalogger;

            foreach (var obj in appData.DaqModules)
            {
                int img = 0;
                if (obj is OpcClient) img = (int)Image.OpcDa;
                if (obj is SerialText) img = (int)Image.SerialJson;
                AddObjectToNode(Root.DataAquisition, obj, obj.Name, img);
            }
            foreach (var obj in appData.DasModules)
            {
                int img = 0;
                if (obj is HttpTagsServer) img = (int)Image.Http;
                if (obj is SerialTagsServer) img = (int)Image.Serial;
                AddObjectToNode(Root.DataServer, obj, obj.Name, img);
            }
            foreach (var obj in appData.Devices)
            {
                int img = 0;
                if (obj is UsbWifiHmiServer) img = (int)Image.UsbWifiHmiServer;
                if (obj is HmiServerStation) img = (int)Image.HmiServerStation;
                AddObjectToNode(Root.Devices, obj, obj.Name, img);
            }
            foreach (var file in appData.Files)
            {
                AddFile(file);
            }
        }

        public void AddFile(FileInfo file)
        {
            if (file.Extension == ".html") AddObjectToNode(Root.Application.Screens, file, file.Name, (int)Image.Html);
            if (file.Extension == ".css") AddObjectToNode(Root.Application.Styles, file, file.Name, (int)Image.Css);
            if (file.Extension == ".js") AddObjectToNode(Root.Application.Scripts, file, file.Name, (int)Image.Javascript);
            if (file.Extension == ".json") AddObjectToNode(Root.Application.Data, file, file.Name, (int)Image.Data);
            if (file.Extension == ".svg" ||
                file.Extension == ".png" ||
                file.Extension == ".jpg" ||
                file.Extension == ".ico")
            {
                AddObjectToNode(Root.Application.Resources, file, file.Name, (int)Image.Resx);
            }
        }

        public TreeNode AddObjectToNode(TreeNode parentNode, object obj, string name, int imgIdx)
        {
            var newNode = new TreeNode();
            newNode.Text = $"{name}";
            newNode.Tag = obj;
            newNode.ImageIndex = imgIdx;
            newNode.SelectedImageIndex = imgIdx;
            parentNode.Nodes.Add(newNode);
            parentNode.Expand();
            return newNode;
        }

        private void AppTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (PropertyGrid != null)
            {
                PropertyGrid.SelectedObject = e.Node.Tag;
            }
        }
    }

    public class AppTreeRoot
    {
        public AppTree AppTree { get; set; }

        public AppTreeRoot(AppTree appTree)
        {
            AppTree = appTree;
        }

        public TreeNode Datalogger { get => AppTree.Nodes[(int)AppTree.RootNode.Datalogger]; }

        public TreeNode DataAquisition { get => AppTree.Nodes[(int)AppTree.RootNode.DataAquisition]; }

        public TreeNode DataServer { get => AppTree.Nodes[(int)AppTree.RootNode.DataServer]; }

        public TreeNode Devices { get => AppTree.Nodes[(int)AppTree.RootNode.Devices]; }

        public AppApplicationNode Application
        {
            get
            {
                var node = new AppApplicationNode();
                node.Root = AppTree.Nodes[(int)AppTree.RootNode.Application];
                return node;
            }
        }
    }

    public class AppApplicationNode
    {
        public enum ApplicationNode : int
        {
            Screens,
            Styles,
            Scripts,
            Data,
            Resources
        }

        public TreeNode Root { get; set; }

        public TreeNode Screens
        {
            get => Root.Nodes[(int)ApplicationNode.Screens];
        }

        public TreeNode Styles
        {
            get => Root.Nodes[(int)ApplicationNode.Styles];
        }

        public TreeNode Scripts
        {
            get => Root.Nodes[(int)ApplicationNode.Scripts];
        }

        public TreeNode Data
        {
            get => Root.Nodes[(int)ApplicationNode.Data];
        }

        public TreeNode Resources
        {
            get => Root.Nodes[(int)ApplicationNode.Resources];
        }
    }
}
