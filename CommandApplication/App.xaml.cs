using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        //For opening windows in multiple screens
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();
            
            System.Diagnostics.Trace.WriteLine(Screen.AllScreens.Length);
            if (Screen.AllScreens.Length > 1)
            {
                Screen s2 = Screen.AllScreens[2];
                Rectangle r2 = s2.WorkingArea;
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                mainWindow.Top = r2.Top;
                mainWindow.Left = r2.Left;
                mainWindow.Show();
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Screen s1 = Screen.AllScreens[0];
                Rectangle r1 = s1.WorkingArea;
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                mainWindow.Top = r1.Top;
                mainWindow.Left = r1.Left;
                mainWindow.Show();
                mainWindow.WindowState = WindowState.Maximized;
            }

        }
    }
}
