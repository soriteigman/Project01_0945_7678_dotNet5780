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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl
        public IEnumerable<HostingUnit> units { get; set; }
        IEnumerable<GuestRequest> req;//list of all requests that match any of the units
        IEnumerable<Order> ord;
        IList<Order> myOrders = new List<Order>();
        int id;
        public Orders(int ID)
        {
            id = ID;
            InitializeComponent();
            IList<int> keys = new List<int>();
            units = _bl.searchHUbyOwner(id);//list of all units for this host
            ord = _bl.GetsOpenOrders();
            foreach (HostingUnit hu in units)
            {
                keys.Add(hu.HostingUnitKey);
                if (_bl.AllRequestsThatMatch(_bl.BuildPredicate(hu)).Count() > 0)
                    if (req == null)
                        req = _bl.AllRequestsThatMatch(_bl.BuildPredicate(hu));
                    else req.Concat(_bl.AllRequestsThatMatch(_bl.BuildPredicate(hu)));
            }
            foreach(int key in keys)
            {
                foreach(Order o in ord)
                {
                    if (o.HostingUnitKey == key)
                        myOrders.Add(o);

                }
            }



            this.OrdersTabUserControl.SearchBox.ItemsSource = keys;
            this.OrdersTabUserControl.StatusCB.SelectedItem = "{Binding Path=OrderStatus,Mode=TwoWay}";
            this.OrdersTabUserControl.StatusCB.ItemsSource = Enum.GetValues(typeof(BE.Status));

            this.OrdersTabUserControl.StatusCB.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.SearchBox.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.ResetFiltersButton.Click += ResetFilters;
            this.OrdersTabUserControl.AddButton.Click += Addorder;
            this.OrdersTabUserControl.StatusCB.ItemsSource = Enum.GetValues(typeof(BE.Status));

            

            this.OrdersTabUserControl.DataGrid.ItemsSource = myOrders;
            this.OrdersTabUserControl.DataGrid.DisplayMemberPath = "orders";
            this.OrdersTabUserControl.DataGrid.SelectedIndex = 0;



        }
        private void Addorder(object sender, RoutedEventArgs e)
        {

        }
        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            this.OrdersTabUserControl.SearchBox.Text = null;
            this.OrdersTabUserControl.SearchBox.Text = null;
            this.OrdersTabUserControl.StatusCB.SelectedItem = null;
            ApplyFiltering(this, new RoutedEventArgs());
        }

        private void ApplyFiltering(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OrdersTabUserControl.DataGrid.ItemsSource = from item in _bl.GetAllOrders(
                      Convert.ToInt32(this.OrdersTabUserControl.SearchBox.Text),
                      this.OrdersTabUserControl.StatusCB.SelectedItem as BE.Status?
                      )
                      orderby item.HostingUnitKey
                      select new
                      {
                          item.HostingUnitKey,
                          item.GuestRequestKey,
                          item.CreateDate,
                          item.SentEmail,
                          item.Status,
                          item.OrderKey
                      };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
        }
       
    }
}

