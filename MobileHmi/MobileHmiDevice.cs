using DotNetCom.General;
using DotNetCom.General.Tags;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace MobileHmi
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class MobileHmiDevice : Component
    {
        [Browsable(false)]
        public string ProjectFolder { get; set; }

        public MobileHmiDevice()
        {
            InitializeComponent();
        }

        public MobileHmiDevice(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void UpdateExplorer(TreeNode treeNode, string path)
        {
            var dirInfo = new DirectoryInfo(path);
            ProjectFolder = path;
            treeNode.Nodes.Clear();

            foreach (var item in dirInfo.GetFiles())
            {
                if(item.Extension == ".html" || item.Extension == ".htm" || item.Extension == ".js" || item.Extension == ".css")
                {
                    var it = treeNode.Nodes.Add(item.Name);
                    it.Tag = item.FullName;
                    switch (item.Extension)
                    {
                        case ".html":
                            it.ImageIndex = 6;
                            it.SelectedImageIndex = 6;
                            break;
                        case ".htm":
                            it.ImageIndex = 6;
                            it.SelectedImageIndex = 6;
                            break;
                        case ".js":
                            it.ImageIndex = 7;
                            it.SelectedImageIndex = 7;
                            break;
                        case ".css":
                            it.ImageIndex = 8;
                            it.SelectedImageIndex = 8;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

}
