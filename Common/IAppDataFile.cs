using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public class AppDataFileDialog
    {
        public IAppData New(IAppData appData)
        {
            var msg = MessageBox.Show("Overwrite current configuration?", "Confirm", MessageBoxButtons.YesNo);
            if (msg == DialogResult.Yes)
            {
                return appData.New();
            }
            return appData;
        }

        public IAppData Open(IAppData appData)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Json file | *.json"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return appData.Load(dialog.FileName);
            }

            return appData;
        }

        public void Save(IAppData appData)
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "Json file | *.json"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                appData.Save(dialog.FileName);
            }
        }

        public void SaveDefault(IAppData appData)
        {
            appData.SaveDefault();
        }
    }
}
