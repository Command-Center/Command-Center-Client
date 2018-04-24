using CommandApplication.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for CamWindow.xaml
    /// </summary>
    public partial class CamWindow : Window
    {
        public CamWindow()
        {
            InitializeComponent();
            BrowserCam1.Address = new Uri("http://" + Constants.ServerAddressDemo + ":8084").ToString();
            //BrowserCam2.Address = new Uri("http://" + Constants.ServerAddressDemo + ":8083").ToString();
        }
    }
}
