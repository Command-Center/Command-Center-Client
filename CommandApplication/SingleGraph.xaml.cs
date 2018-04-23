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
    /// Interaction logic for SingleGraph.xaml
    /// </summary>
    public partial class SingleGraph : Window
    {
        public SingleGraph(string param)
        {
            InitializeComponent();
            switch (param)
            {
                case "accx":
                    title.Content = "Acceleration X";
                    break;
                case "accy":
                    title.Content = "Acceleration Y";
                    break;
                case "accz":
                    title.Content = "Acceleration Z";
                    break;
                case "pitch":
                    title.Content = "Pitch";
                    break;
                case "roll":
                    title.Content = "Roll";
                    break;
                case "yaw":
                    title.Content = "Yaw";
                    break;
                default:
                    break;
            }
        }
    }
}
