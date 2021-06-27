using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetCom.General.Controls
{
    public class ModuleStatus
    {
        private ToolStripStatusLabel speedLabel;

        private ToolStripStatusLabel connectionStatusLabel;

        public ModuleStatus(ToolStripStatusLabel speedLabel, ToolStripStatusLabel connectionStatusLabel)
        {
            this.speedLabel = speedLabel;
            this.connectionStatusLabel = connectionStatusLabel;
        }

        public string Speed
        {
            get => speedLabel.Text;
            set => speedLabel.Text = value;
        }

        public string ConnectionStatus
        {
            get => connectionStatusLabel.Text;
            set => connectionStatusLabel.Text = value;
        }
    }
}
