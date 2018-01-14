using Google.Maps;
using Google.Maps.StaticMaps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            GoogleSigned.AssignAllServices(new GoogleSigned("AIzaSyDE-w-vFJxyw5W4HGqK435n46mSdNGUEys"));
            InitializeComponent();
            string curDir = Directory.GetCurrentDirectory();
            //mapNavi.Navigate(new Uri(String.Format("file:///{0}/Views/openlayermap.html", curDir)));
            //var map = new StaticMapRequest();
            //map.Center = new Location("1600 Pennsylvania Ave NW, Washington, DC 20500");
            //map.Size = new System.Drawing.Size(400, 400);
            //map.Zoom = 14;
            
            //map.Scale = 2;
            //mapImage.Source = new BitmapImage(map.ToUri());

            

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var video = new VideoWindow();
            video.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var sensor = new SensorWindow();
            sensor.Show();
        }
    }
}
