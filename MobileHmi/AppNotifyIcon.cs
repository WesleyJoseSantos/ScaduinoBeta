using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MobileHmi
{
    public partial class AppNotifyIcon : Component
    {
        public AppNotifyIcon()
        {
            InitializeComponent();
            Init();
        }

        public AppNotifyIcon(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            Init();
        }

        private void Init()
        {
            tsmiStop.Click += TsmiStop_Click;
        }

        private void TsmiStop_Click(object sender, EventArgs e)
        {
            var diag = MessageBox.Show("Mobile HMI communication will be interrupted. Confirm?", "Confirm", MessageBoxButtons.OKCancel);
            if (diag == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void Notify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Program.ShowStartScreen();
        }
    }
}
