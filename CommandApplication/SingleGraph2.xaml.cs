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
    /// Interaction logic for SingleGraph2.xaml
    /// </summary>
    public partial class SingleGraph2 : Window
    {
        SingleGraphViewModel singleGraphViewModel;
        public SingleGraph2(string identifier)
        {
            InitializeComponent();
            DataContext = singleGraphViewModel = new SingleGraphViewModel(this, identifier);
        }
    }
}
