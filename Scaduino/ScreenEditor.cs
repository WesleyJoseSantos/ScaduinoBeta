using CefSharp.WinForms;
using System;
using System.Windows.Forms;

namespace Scaduino
{
    public partial class ScreenEditor : UserControl, IEditor
    {
        public ChromiumWebBrowser Browser { get; set; }
        public bool IsChanged { get => codeEditor.IsChanged; }
        public string FileName { get => codeEditor.FileName; set => codeEditor.FileName = value; }

        public ScreenEditor()
        {
            InitializeComponent();
            Browser = new ChromiumWebBrowser("");
            Browser.Dock = DockStyle.Fill;
            
            splitContainer.Panel2.Controls.Add(Browser);
            codeEditor.FileSaved += CodeEditor_FileSaved;
        }

        private void CodeEditor_FileSaved(object sender, EventArgs e)
        {
            var code = sender as CodeEditor;
            Browser.Load(code.FileName);
        }

        public void ShowBroser()
        {
            splitContainer.Panel2Collapsed = false;
        }

        public void HideBrowser()
        {
            splitContainer.Panel2Collapsed = false;
        }

        public void LoadFile()
        {
            codeEditor.LoadFile();
            Browser.Load(FileName);
        }

        public void SaveFile()
        {
            codeEditor.SaveFile();
        }
    }
}
