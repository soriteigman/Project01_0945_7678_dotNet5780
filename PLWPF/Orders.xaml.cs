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
        int id;
        public Orders(int ID)
        {
            id = ID;
            InitializeComponent();
            this.OrdersTabUserControl.StatusCB.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.SearchTextBox.TextChanged += ApplyFiltering;
            this.OrdersTabUserControl.ResetFiltersButton.Click += ResetFilters;
            this.OrdersTabUserControl.StatusCB.ItemsSource = Enum.GetValues(typeof(BE.Status));


            units = _bl.searchHUbyOwner(id);//list of all units for this host
            this.OrdersTabUserControl.DataGrid.ItemsSource = units;
            this.OrdersTabUserControl.DataGrid.DisplayMemberPath = "orders";
            this.OrdersTabUserControl.DataGrid.SelectedIndex = 0;


            IEnumerable<GuestRequest> req = null;//list of all requests that match any of the units
            //List<string> keys = null;//list of all names of host
            foreach (HostingUnit hu in units)
            {
                //if (keys == null)
                //    keys[0] = hu.HostingUnitName;
                //keys.Add(hu.HostingUnitName);
                if (_bl.AllRequestsThatMatch(_bl.BuildPredicate(hu)).Count() > 0)
                    req.Concat(_bl.AllRequestsThatMatch(_bl.BuildPredicate(hu)));
            }
        }
        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            this.OrdersTabUserControl.SearchTextBox.Text = null;
            this.OrdersTabUserControl.SearchTextBox.Text = null;
            this.OrdersTabUserControl.StatusCB.SelectedItem = null;
            ApplyFiltering(this, new RoutedEventArgs());
        }

        private void ApplyFiltering(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OrdersTabUserControl.DataGrid.ItemsSource = from item in _bl.GetAllOrders(
                      this.OrdersTabUserControl.SearchTextBox.Text,
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

