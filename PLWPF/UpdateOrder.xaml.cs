using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<Order> myOrders = new ObservableCollection<Order>();
        IList<int> keys = new List<int>();//all his hukeys
        public IEnumerable<HostingUnit> units { get; set; }//all his units
        IList<GuestRequest> req;//list of all requests that match any of the units
        IEnumerable<Order> ord;//all his orders

        public UpdateOrder(Order or, DataGrid dg, int id)
        {
            InitializeComponent();
            units = myBL.searchHUbyOwner(id);//list of all units for this host
            ord = myBL.GetsOpenOrders().ToList();

            currentOrder = or;
            sourceDG = dg;
            foreach (HostingUnit h in units)
            {
                keys.Add(h.HostingUnitKey);
                if (myBL.AllRequestsThatMatch(myBL.BuildPredicate(h)).Count() > 0)
                    if (req == null)
                        req = myBL.AllRequestsThatMatch(myBL.BuildPredicate(h)).ToList();
                    else req.Concat(myBL.AllRequestsThatMatch(myBL.BuildPredicate(h)));
            }
            foreach (int key in keys)
            {
                foreach (Order o in ord)
                {
                    if (o.HostingUnitKey == key)
                        myOrders.Add(o);
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            update.IsEnabled = true;
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            currentOrder.Status = Status.Booked;
            try
            {
                myBL.UpdateOrder(currentOrder);
                MessageBox.Show("Your order status was successfully updated to booked.", "Successful Update", MessageBoxButton.OK, MessageBoxImage.Information);
                sourceDG.ItemsSource = myOrders;

                Close();
            }
            catch(Exception a)
            {
                MessageBox.Show(a.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
