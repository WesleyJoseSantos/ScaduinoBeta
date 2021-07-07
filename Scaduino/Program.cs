using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Scaduino
{
    static class Program
    {
        static private AppData appData = new AppData();

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            appData = appData.Load() as AppData;
            Application.Run(new MainForm(appData));
        }
    }
}
