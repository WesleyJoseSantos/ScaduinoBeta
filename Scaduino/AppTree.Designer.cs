
namespace Scaduino
{
    partial class AppTree
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppTree));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Datalogger", 0, 0);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Data Aquisition", 1, 1);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Data Server", 2, 2);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Devices", 3, 3);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Screens", 5, 5);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Styles", 6, 6);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Scripts", 7, 7);
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Data", 8, 8);
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Resources", 9, 9);
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Application", 4, 4, new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            this.images = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // images
            // 
            this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
            this.images.TransparentColor = System.Drawing.Color.Transparent;
            this.images.Images.SetKeyName(0, "dataLogger.ico");
            this.images.Images.SetKeyName(1, "dataAquisition.ico");
            this.images.Images.SetKeyName(2, "dataServer.ico");
            this.images.Images.SetKeyName(3, "devices.ico");
            this.images.Images.SetKeyName(4, "application.ico");
            this.images.Images.SetKeyName(5, "screens.ico");
            this.images.Images.SetKeyName(6, "styles.ico");
            this.images.Images.SetKeyName(7, "scripts.ico");
            this.images.Images.SetKeyName(8, "data.ico");
            this.images.Images.SetKeyName(9, "resources.ico");
            this.images.Images.SetKeyName(10, "opcDa.png");
            this.images.Images.SetKeyName(11, "serialJson.png");
            this.images.Images.SetKeyName(12, "http.ico");
            this.images.Images.SetKeyName(13, "serial.png");
            this.images.Images.SetKeyName(14, "usbWifiHmiServer.ico");
            this.images.Images.SetKeyName(15, "hmiServerStation.ico");
            this.images.Images.SetKeyName(16, "html.png");
            this.images.Images.SetKeyName(17, "css.png");
            this.images.Images.SetKeyName(18, "js.png");
            this.images.Images.SetKeyName(19, "json.png");
            this.images.Images.SetKeyName(20, "resx.ico");
            // 
            // AppTree
            // 
            this.ImageIndex = 0;
            this.ImageList = this.images;
            this.LineColor = System.Drawing.Color.Black;
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "datalogger";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Text = "Datalogger";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "dataAquisition";
            treeNode2.SelectedImageIndex = 1;
            treeNode2.Text = "Data Aquisition";
            treeNode3.ImageIndex = 2;
            treeNode3.Name = "dataServer";
            treeNode3.SelectedImageIndex = 2;
            treeNode3.Text = "Data Server";
            treeNode4.ImageIndex = 3;
            treeNode4.Name = "devices";
            treeNode4.SelectedImageIndex = 3;
            treeNode4.Text = "Devices";
            treeNode5.ImageIndex = 5;
            treeNode5.Name = "screens";
            treeNode5.SelectedImageIndex = 5;
            treeNode5.Text = "Screens";
            treeNode6.ImageIndex = 6;
            treeNode6.Name = "styles";
            treeNode6.SelectedImageIndex = 6;
            treeNode6.Text = "Styles";
            treeNode7.ImageIndex = 7;
            treeNode7.Name = "scripts";
            treeNode7.SelectedImageIndex = 7;
            treeNode7.Text = "Scripts";
            treeNode8.ImageIndex = 8;
            treeNode8.Name = "data";
            treeNode8.SelectedImageIndex = 8;
            treeNode8.Text = "Data";
            treeNode9.ImageIndex = 9;
            treeNode9.Name = "resources";
            treeNode9.SelectedImageIndex = 9;
            treeNode9.Text = "Resources";
            treeNode10.ImageIndex = 4;
            treeNode10.Name = "application";
            treeNode10.SelectedImageIndex = 4;
            treeNode10.Text = "Application";
            this.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode10});
            this.SelectedImageIndex = 0;
            this.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AppTree_AfterSelect);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList images;
    }
}
