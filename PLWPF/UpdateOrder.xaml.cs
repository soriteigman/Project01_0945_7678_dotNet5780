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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for UpdateOrder.xaml
    /// </summary>
    public partial class UpdateOrder : Window
    {
        IBL myBL = BL.FactoryBL.getBL();//creates an instance of BL
        Order currentOrder;
        DataGrid sourceDG;

        public UpdateOrder(Order o, DataGrid dg)
        {
            InitializeComponent();
            currentOrder = o;
            sourceDG = dg;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            update.IsEnabled = true;
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            currentOrder.Status = Status.Closed;
            myBL.UpdateOrder(currentOrder);
            MessageBox.Show("Your order status was successfully updated to closed.", "Successful Update", MessageBoxButton.OK, MessageBoxImage.Information);
            //sourceDG.ItemsSource=
            
            Close();
        }

    }
}
