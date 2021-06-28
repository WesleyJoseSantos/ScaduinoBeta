
namespace ProDAq
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Datalogger");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("OPC DA");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Text Interface");
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "opc.png");
            this.imageList.Images.SetKeyName(1, "Text Interface.ico");
            this.imageList.Images.SetKeyName(2, "group.ico");
            this.imageList.Images.SetKeyName(3, "rs-232-male.png");
            this.imageList.Images.SetKeyName(4, "item.png");
            this.imageList.Images.SetKeyName(5, "square-wave.png");
            this.imageList.Images.SetKeyName(6, "analog.png");
            this.imageList.Images.SetKeyName(7, "datalogging.ico");
            // 
            // AppTree
            // 
            this.ImageIndex = 0;
            this.ImageList = this.imageList;
            this.LineColor = System.Drawing.Color.Black;
            treeNode1.Name = "datalogger";
            treeNode1.Text = "Datalogger";
            treeNode2.Name = "opcDa";
            treeNode2.Text = "OPC DA";
            treeNode3.Name = "textInterface";
            treeNode3.Text = "Text Interface";
            this.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.SelectedImageIndex = 0;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
    }
}
