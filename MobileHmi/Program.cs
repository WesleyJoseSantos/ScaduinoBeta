using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Common;

namespace MobileHmi
{
    static class Program
    {
        static private AppData AppData;
        static private MainForm startScreen;
        static private AppNotifyIcon notifyIcon;

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool instanceCountOne = false;

            using (Mutex mtex = new Mutex(true, "MobileHMI", out instanceCountOne))
            {
                if (instanceCountOne)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    AppData = new AppData();
                    AppData = AppData.LoadDefault() as AppData;
                    notifyIcon = new AppNotifyIcon();

                    ShowStartScreen();
                    Application.Run();
                }
                else
                {
                    MessageBox.Show("Mobile HMI already running!");
                }
            }
        }

        static public void ShowStartScreen()
        {
            if (startScreen == null || startScreen.IsDisposed)
            {
                startScreen = new MainForm(AppData);
                startScreen.NotifyIcon = notifyIcon;
                startScreen.Show();
            }
            startScreen.Show();
            startScreen.BringToFront();
        }
    }
}
