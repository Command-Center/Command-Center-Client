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
    public partial class SingleGraphWindow : Window
    {
        SingleGraphViewModel singleGraphViewModel;
        public SingleGraphWindow(string identifier)
        {
            InitializeComponent();
            DataContext = singleGraphViewModel = new SingleGraphViewModel(this, identifier);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            singleGraphViewModel.Unsubscribe();
        }
    }
}
