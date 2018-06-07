using CommandApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
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

namespace CommandApplication.Views
{
    /// <summary>
    /// Interaction logic for ManageConnectionsWindow.xaml
    /// </summary>
    public partial class ManageConnectionsWindow : Window
    {
        IConnectionAddressRepository irep;
        //private DataContext context = new DataContext();
        bool isInsert = false;
        public ManageConnectionsWindow()
        {
            irep = new ConnectionAddressRepository();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var res = irep.ListAllAddresses().ToList();
            //connectionAddressTable.ItemsSource = new ObservableCollection<ConnectionAddress>(res);
            connectionAddressTable.ItemsSource = res;
        }

        

        private void connectionAddressTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            ConnectionAddress conAddressNew = new ConnectionAddress();
            ConnectionAddress conAddressFromRow = e.Row.DataContext as ConnectionAddress;
            
            if (conAddressFromRow != null)
            {
                if(conAddressFromRow.Id > 0)
                {
                    isInsert = false;
                }
                else
                {
                    isInsert = true;
                }
            }
            if (isInsert)
            {
                var insertRecord = MessageBox.Show("Do you want to add " + conAddressFromRow.Address + "?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(insertRecord == MessageBoxResult.Yes)
                {
                    conAddressNew.Address = conAddressFromRow.Address;
                    conAddressNew.Comment = conAddressFromRow.Comment;
                    irep.AddAddress(conAddressNew);
                }
                else
                {
                    //show all?
                }
            }
            else
            {
                var editRecord = MessageBox.Show("Do you want to edit " + conAddressFromRow.Address + "?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (editRecord == MessageBoxResult.Yes)
                {
                    irep.EditAddress(conAddressFromRow); //Edited conAddress sent to method.
                }
                else
                {
                    //show all?
                }
                
            }
            //context.SaveChanges();
        }

        private void makeActiveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
