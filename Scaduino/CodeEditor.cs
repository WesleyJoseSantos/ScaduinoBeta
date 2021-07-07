using FastColoredTextBoxNS;
using System;
using System.IO;
using System.Text;

namespace Scaduino
{
    public class CodeEditor : FastColoredTextBox, IEditor
    {
        public string FileName { get; set; }

        public event EventHandler FileSaved;

        public void LoadFile()
        {
            var file = new FileInfo(FileName);
            if (file.Exists)
            {
                try
                {
                    switch (file.Extension)
                    {
                        case ".js":
                            Language = Language.JS;
                            break;
                        case ".jso":
                            Language = Language.JS;
                            break;
                        case ".json":
                            Language = Language.JS;
                            break;
                        case ".htm":
                            Language = Language.HTML;
                            break;
                        case ".html":
                            Language = Language.HTML;
                            break;
                        case ".cs":
                            Language = Language.CSharp;
                            break;
                        default:
                            break;
                    }
                    OpenFile(file.FullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeEditor));
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // CodeEditor
            // 
            this.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Hotkeys = resources.GetString("$this.Hotkeys");
            this.Name = "CodeEditor";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        public void SaveFile()
        {
            SaveToFile(FileName, Encoding.UTF8);
            FileSaved?.Invoke(this, null);
        }
    }
}
