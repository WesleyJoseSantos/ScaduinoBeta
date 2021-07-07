using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scaduino
{
    interface IEditor
    {
        string FileName { get; set; }

        bool IsChanged { get; }

        void LoadFile();

        void SaveFile();

        DockStyle Dock { get; set; }
    }
}
