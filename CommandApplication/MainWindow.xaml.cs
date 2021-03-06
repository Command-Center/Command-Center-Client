﻿using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using CommandApplication.Model;
using CommandApplication.ViewModel;

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel mvm;
           
        public MainWindow()
        {
            //GoogleSigned.AssignAllServices(new GoogleSigned("AIzaSyDE-w-vFJxyw5W4HGqK435n46mSdNGUEys"));
            InitializeComponent();
            //string curDir = Directory.GetCurrentDirectory();
            
            //mapImage.Source = new BitmapImage(map.ToUri());
            //media.Source = new Uri("http://129.242.174.142:8081/");
            //BrowserCam.Address = new Uri("http://129.242.174.142:8081/").ToString();
            //BrowserCam1.Address = new Uri("http://" + Constants.ServerAddressDemo + ":8084").ToString();
            //BrowserCam2.Address = new Uri("http://" + Constants.ServerAddressDemo + ":8083").ToString();

            DataContext = mvm = new MainViewModel();

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SensorWindow sensorWindow = new SensorWindow();
            if (Screen.AllScreens.Length > 1)
            {
                Screen s2 = Screen.AllScreens[0];
                Rectangle r2 = s2.WorkingArea;
                sensorWindow.WindowState = WindowState.Normal;
                sensorWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                sensorWindow.Top = r2.Top;
                sensorWindow.Left = r2.Left;
                sensorWindow.Show();
                sensorWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                sensorWindow.Show();
                //Screen s1 = Screen.AllScreens[0];
                //Rectangle r1 = s1.WorkingArea;
                //videoWindow.WindowState = WindowState.Normal;
                //videoWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                //videoWindow.Top = r1.Top;
                //videoWindow.Left = r1.Left;
                //videoWindow.Show();
                //videoWindow.WindowState = WindowState.Maximized;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MapWindow mapWindow = new MapWindow(); //TODO CefSharp trenger 32bit. Må ses på.
            if (Screen.AllScreens.Length > 1)
            {
                Screen s2 = Screen.AllScreens[1];
                Rectangle r2 = s2.WorkingArea;
                mapWindow.WindowState = WindowState.Normal;
                mapWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                mapWindow.Top = r2.Top;
                mapWindow.Left = r2.Left;
                mapWindow.Show();
                mapWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                mapWindow.Show();
                //Screen s1 = Screen.AllScreens[0];
                //Rectangle r1 = s1.WorkingArea;
                //sensorWindow.WindowState = WindowState.Normal;
                //sensorWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                //sensorWindow.Top = r1.Top;
                //sensorWindow.Left = r1.Left;
                //sensorWindow.Show();
                //sensorWindow.WindowState = WindowState.Maximized;
            }
        }
        private void Show_Menu_Button(object sender, RoutedEventArgs e)
        {
            (sender as System.Windows.Controls.Button).ContextMenu.IsEnabled = true;
            (sender as System.Windows.Controls.Button).ContextMenu.PlacementTarget = (sender as System.Windows.Controls.Button);
            (sender as System.Windows.Controls.Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as System.Windows.Controls.Button).ContextMenu.IsOpen = true;
        }
        private void Show_Menu_Button2(object sender, RoutedEventArgs e)
        {
            (sender as System.Windows.Controls.Button).ContextMenu.IsEnabled = true;
            (sender as System.Windows.Controls.Button).ContextMenu.PlacementTarget = (sender as System.Windows.Controls.Button);
            (sender as System.Windows.Controls.Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as System.Windows.Controls.Button).ContextMenu.IsOpen = true;
        }
        private void GotoXacc(object sender, RoutedEventArgs e)
        {
            SingleGraphWindow graphWindow = new SingleGraphWindow(Topic.XAccTopic);
            graphWindow.Show();
        }
        private void GotoYacc(object sender, RoutedEventArgs e)
        {
            SingleGraphWindow graphWindow = new SingleGraphWindow(Topic.YAccTopic);
            graphWindow.Show();
        }
        private void GotoZacc(object sender, RoutedEventArgs e)
        {
            SingleGraphWindow graphWindow = new SingleGraphWindow(Topic.ZAccTopic);
            graphWindow.Show();
        }
        private void GotoRoll(object sender, RoutedEventArgs e)
        {
            SingleGraphWindow graphWindow = new SingleGraphWindow(Topic.RollTopic);
            graphWindow.Show();
        }
        private void GotoPitch(object sender, RoutedEventArgs e)
        {
            SingleGraphWindow graphWindow = new SingleGraphWindow(Topic.PitchTopic);
            graphWindow.Show();
        }
        private void GotoYaw(object sender, RoutedEventArgs e)
        {
            SingleGraphWindow graphWindow = new SingleGraphWindow(Topic.YawTopic);
            graphWindow.Show();
        }
        private void Show_Cam(object sender, RoutedEventArgs e)
        {
            CamWindow camWindow = new CamWindow();
            camWindow.Show();
        }

    }
}
