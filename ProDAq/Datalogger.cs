using DotNetCom.DataBase;
using DotNetCom.General.Tags;
using DotNetScadaComponents.Trend;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ProDAq
{
    public partial class Datalogger : Component
    {
        [JsonProperty]
        [Category("General")]
        [DisplayName("Directory")]
        [Description("Directory where the data logging files will be saved.")]
        [Editor(typeof(DataloggerDirEditor), typeof(UITypeEditor))]
        public string FilePath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ProDAq\\DAT";

        [JsonProperty]
        [Category("General")]
        [DisplayName("File Max Size")]
        [Description("Max single datalogging file size in Kb.")]
        public int FileMaxSize { get; set; } = 256;

        [JsonProperty]
        [Browsable(false)]
        public int CurrentFileIdx { get; set; } = 0;

        private string currentFileName
        {
            get
            {
                string fileName = FilePath + "\\Dat" + CurrentFileIdx + ".dat";
                return fileName;
            }
        }

        private FileStream fs;
        private long currentSize;
        private bool running = false;

        public Datalogger()
        {
            InitializeComponent();
            Data.TagsDataBase.TagChanged += TagsDataBase_TagChanged;
        }

        public Datalogger(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void Start()
        {
            var finf = new FileInfo(currentFileName);
            if (!finf.Directory.Exists)
            {
                Directory.CreateDirectory(finf.Directory.FullName);
            }
            fs = File.OpenWrite(currentFileName);
            currentSize = finf.Length / 1024;
            running = true;
        }

        public void Stop()
        {
            fs?.Close();
            running = false;
        }

        public void Open()
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "Datalogging file | *.dat",
                Multiselect = true
            };
            if(fileDialog.ShowDialog() == DialogResult.OK) {
                var trend = new Trend()
                {
                    Dock = DockStyle.Fill
                };
                var form = new Form()
                {
                    Text = fileDialog.FileName,
                };
                trend.Open(fileDialog.FileNames);
                trend.Dock = DockStyle.Fill;
                form.Controls.Add(trend);
                form.Show();
            }
        }

        private void TagsDataBase_TagChanged(object sender, EventArgs e)
        {
            if (fs != null && running)
            {
                var tag = sender as Tag;
                var json = TrendPointData.ToJson(tag.Name, tag.Value) + '\n';
                var data = Encoding.UTF8.GetBytes(json);
                var size = data.Length;
                currentSize += size;

                if(currentSize > FileMaxSize * 1024)
                {
                    fs.Close();
                    CurrentFileIdx++;
                    currentSize = 0;
                    fs = File.OpenWrite(currentFileName);
                }
                fs.Write(data, 0, size);
            }
        }
    }

    class DataloggerDirEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    value = folderDialog.SelectedPath;
                }
            }
            return value;
        }
    }
}
