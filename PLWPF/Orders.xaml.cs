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
        IEnumerable<GuestRequest> gr;
        public IEnumerable<HostingUnit> units { get; set; }//all his units
        IEnumerable<GuestRequest> req;//list of all requests that match any of the units
        IEnumerable<Order> ord;//all his orders
        IList<int> keys = new List<int>();//all his keys
        IList<Order> myOrders = new List<Order>();
        int id;
        public Orders(int ID)
        {
            id = ID;
            InitializeComponent();
            units = _bl.searchHUbyOwner(id);//list of all units for this host
            ord = _bl.GetsOpenOrders();
            #region איתחול
            foreach (HostingUnit h in units)
            {
                keys.Add(h.HostingUnitKey);
                if (_bl.AllRequestsThatMatch(_bl.BuildPredicate(h)).Count() > 0)
                    if (req == null)
                        req = _bl.AllRequestsThatMatch(_bl.BuildPredicate(h));
                    else req.Concat(_bl.AllRequestsThatMatch(_bl.BuildPredicate(h)));
            }
            foreach (int key in keys)
            {
                foreach (Order o in ord)
                {
                    if (o.HostingUnitKey == key)
                        myOrders.Add(o);
                }
            }
            #endregion

            #region units

            this.OrdersTabUserControl.FilterName.TextChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterKey.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterStar.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterArea.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterType.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.ResetFiltersButton.Click += ResetFilters;
            this.OrdersTabUserControl.DataGrid.SelectionChanged += ShowButtons;
            this.OrdersTabUserControl.AddButton.Click += addOrder;
            this.OrdersTabUserControl.updateButton.Click += updateHu;
            this.OrdersTabUserControl.DataGrid.MouseDoubleClick += DataGridRow_MouseDoubleClick;



            this.OrdersTabUserControl.FilterKey.ItemsSource = keys;
            this.OrdersTabUserControl.FilterKey.SelectedItem = "{Binding Path=UnitKey,Mode=TwoWay}";

            this.OrdersTabUserControl.FilterStar.ItemsSource = Enum.GetValues(typeof(BE.StarRating));
            this.OrdersTabUserControl.FilterStar.SelectedItem = "{Binding Path=UnitStar,Mode=TwoWay}";

            this.OrdersTabUserControl.FilterArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.OrdersTabUserControl.FilterArea.SelectedItem = "{Binding Path=UnitArea,Mode=TwoWay}";

            this.OrdersTabUserControl.FilterType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            this.OrdersTabUserControl.FilterType.SelectedItem = "{Binding Path=UnitPath,Mode=TwoWay}";

            this.OrdersTabUserControl.DataGrid.ItemsSource = units;
            this.OrdersTabUserControl.DataGrid.DisplayMemberPath = "units";
            this.OrdersTabUserControl.DataGrid.SelectedIndex = 0;
            this.OrdersTabUserControl.DataGrid.AutoGeneratingColumn += WayOfView;
            this.OrdersTabUserControl.DataGrid.UnselectAll();
            #endregion

            #region my req
            HostingUnit hu = (HostingUnit)this.OrdersTabUserControl.DataGrid.SelectedItem;
            this.newOrderTabUserControl.RemoveButton.Visibility = Visibility.Hidden;
            this.newOrderTabUserControl.updateButton.Visibility = Visibility.Hidden;
            this.newOrderTabUserControl.FilterName.TextChanged += ApplyFilteringgr;
            this.newOrderTabUserControl.FilterKey.Visibility = Visibility.Hidden;
            this.newOrderTabUserControl.FilterStar.SelectionChanged += ApplyFilteringgr;
            this.newOrderTabUserControl.FilterArea.SelectionChanged += ApplyFilteringgr;
            this.newOrderTabUserControl.FilterType.SelectionChanged += ApplyFilteringgr;
            this.newOrderTabUserControl.ResetFiltersButton.Click += ResetFiltersgr;
            this.newOrderTabUserControl.DataGrid.SelectionChanged += ShowButtonsreq;
            this.newOrderTabUserControl.AddButton.Click += createOrder;
            #region אתחול
            if (gr.Count() == 0)
            {
                this.newOrderTabUserControl.DataGrid.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.empty.Visibility = Visibility.Visible;
            }
            else
            {
                this.newOrderTabUserControl.DataGrid.ItemsSource = gr;
                this.newOrderTabUserControl.DataGrid.DisplayMemberPath = "GuestReq";
                this.newOrderTabUserControl.DataGrid.SelectedIndex = 0;
                this.newOrderTabUserControl.DataGrid.AutoGeneratingColumn += WayOfView;
            }
            #endregion
            this.newOrderTabUserControl.FilterStar.ItemsSource = Enum.GetValues(typeof(BE.StarRating));
            this.newOrderTabUserControl.FilterStar.SelectedItem = "{Binding Path=UnitStar,Mode=TwoWay}";

            this.newOrderTabUserControl.FilterArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.newOrderTabUserControl.FilterArea.SelectedItem = "{Binding Path=UnitArea,Mode=TwoWay}";

            this.newOrderTabUserControl.FilterType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            this.newOrderTabUserControl.FilterType.SelectedItem = "{Binding Path=UnitPath,Mode=TwoWay}";

            this.OrdersTabUserControl.DataGrid.ItemsSource = gr;
            this.OrdersTabUserControl.DataGrid.DisplayMemberPath = "myreq";
            this.OrdersTabUserControl.DataGrid.SelectedIndex = 0;
            this.OrdersTabUserControl.DataGrid.AutoGeneratingColumn += WayOfView;
            this.OrdersTabUserControl.DataGrid.UnselectAll();
            #endregion
        }
        #region units
        private void addOrder(object sender, RoutedEventArgs e)
        {
            HostingUnit HU = (HostingUnit)this.OrdersTabUserControl.DataGrid.SelectedItem;
            if (HU == null)
                return;
            IEnumerable<GuestRequest> gr = _bl.AllRequestsThatMatch(_bl.BuildPredicate(HU));

            
           // this.NavigationService.Navigate(new NewOrderPage(HU));
        }

        private void updateHu(object sender, RoutedEventArgs e)
        {
            if (OrdersTabUserControl.DataGrid.SelectedItem != null && OrdersTabUserControl.DataGrid.SelectedItem is HostingUnit)
            {
                new UpdateHuWindow((HostingUnit)OrdersTabUserControl.DataGrid.SelectedItem).Show();
                //closeOpenMain = false;//don't open main after closing
                //this.Close();
            }
        }//sends to unit information with data of current row to bind to

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersTabUserControl.DataGrid.SelectedItem != null && OrdersTabUserControl.DataGrid.SelectedItem is HostingUnit)
            {
                new UpdateHuWindow((HostingUnit)OrdersTabUserControl.DataGrid.SelectedItem).Show();
                //closeOpenMain = false;//don't open main after closing
                //this.Close();
            }
        }//sends to unit information with data of current row to bind to

        private void ShowButtons(object sender, SelectionChangedEventArgs e)
        {
            if (this.OrdersTabUserControl.DataGrid.CurrentItem != null)//something was selected
            {
                this.OrdersTabUserControl.updateButton.IsEnabled = true;//allows button clicks
                this.OrdersTabUserControl.RemoveButton.IsEnabled = true;
                if (((HostingUnit)this.OrdersTabUserControl.DataGrid.CurrentItem).Owner.CollectionClearance)//if he has collection clearance
                    this.OrdersTabUserControl.AddButton.IsEnabled = true;
                else
                {
                    this.OrdersTabUserControl.AddButton.IsEnabled = false;//otherwise disables them
                    MessageBox.Show("Cannot create orders without collection clearance.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                this.OrdersTabUserControl.AddButton.IsEnabled = false;//otherwise disables them
                this.OrdersTabUserControl.updateButton.IsEnabled = false;
                this.OrdersTabUserControl.RemoveButton.IsEnabled = false;
            }
        }

        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            this.OrdersTabUserControl.FilterName.Text = "";
            this.OrdersTabUserControl.FilterKey.SelectedItem = null;
            this.OrdersTabUserControl.FilterStar.SelectedItem = null;
            this.OrdersTabUserControl.FilterArea.SelectedItem = null;
            this.OrdersTabUserControl.FilterType.SelectedItem = null;
            this.OrdersTabUserControl.DataGrid.ItemsSource = units;
            this.OrdersTabUserControl.DataGrid.DisplayMemberPath = "units";
            this.OrdersTabUserControl.DataGrid.SelectedIndex = 0;
        }


        private void ApplyFiltering(object sender, RoutedEventArgs e)
        {
            this.OrdersTabUserControl.DataGrid.ItemsSource = from item in _bl.GetAllTUnits(
                                                    this.OrdersTabUserControl.FilterName.Text,
                                                    this.OrdersTabUserControl.FilterKey.SelectedItem,
                                                    this.OrdersTabUserControl.FilterStar.SelectedItem as BE.StarRating?,
                                                    this.OrdersTabUserControl.FilterArea.SelectedItem as BE.VacationArea?,
                                                    this.OrdersTabUserControl.FilterType.SelectedItem as BE.VacationType?)
                                                             orderby item.HostingUnitName, item.HostingUnitKey
                                                             select new
                                                             {
                                                                 item.HostingUnitKey,
                                                                 item.HostingUnitName,
                                                                 item.Area,
                                                                 item.SubArea,
                                                                 item.Type,
                                                                 item.Pet,
                                                                 item.WiFi,
                                                                 item.Parking,
                                                                 item.Pool,
                                                                 item.Jacuzzi,
                                                                 item.Garden,
                                                                 item.ChildrensAttractions,
                                                                 item.FitnessCenter,
                                                                 item.Stars,
                                                                 item.Beds,
                                                             };
        }
        private void WayOfView(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "HostingUnitKey":
                    e.Column.Header = "Property Key";
                    break;
                case "Owner":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "Area":
                    e.Column.Header = "Area";
                    break;
                case "SubArea":
                    e.Column.Header = "Sub Area";
                    break;
                case "Type":
                    e.Column.Header = "Property Type";
                    break;
                case "Pet":
                    e.Column.Header = "Pet";
                    break;
                case "WiFi":
                    e.Column.Header = "Wifi";
                    break;
                case "Parking":
                    e.Column.Header = "Parking";
                    break;
                case "Pool":
                    e.Column.Header = "Pool";
                    break;
                case "Jacuzzi":
                    e.Column.Header = "Jacuzzi";
                    break;
                case "Garden":
                    e.Column.Header = "Garden";
                    break;
                case "ChildrensAttractions":
                    e.Column.Header = "Childrens\n Attractions";
                    break;
                case "FitnessCenter":
                    e.Column.Header = "Fitness Center";
                    break;
                case "Stars":
                    e.Column.Header = "Rating";
                    break;
                case "Beds":
                    e.Column.Header = "Beds";
                    break;
                case "Diary":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }



        }
        #endregion

        #region myreq
        private void ShowButtonsreq(object sender, SelectionChangedEventArgs e)
        {
            if (this.newOrderTabUserControl.DataGrid.CurrentItem != null)//something was selected
            {
                this.newOrderTabUserControl.updateButton.IsEnabled = true;//allows button clicks
                this.newOrderTabUserControl.RemoveButton.IsEnabled = true;
                if (((GuestRequest)this.newOrderTabUserControl.DataGrid.CurrentItem).Status != Status.Closed)//if he has collection clearance
                    this.newOrderTabUserControl.AddButton.IsEnabled = true;
                else
                {
                    this.newOrderTabUserControl.AddButton.IsEnabled = false;//otherwise disables them
                    MessageBox.Show("Cannot create orders with a closed request", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                this.newOrderTabUserControl.AddButton.IsEnabled = false;//otherwise disables them
                this.newOrderTabUserControl.updateButton.IsEnabled = false;
                this.newOrderTabUserControl.RemoveButton.IsEnabled = false;
            }
        }
        private void ApplyFilteringgr(object sender, RoutedEventArgs e)
        {
            this.newOrderTabUserControl.DataGrid.ItemsSource = from item in _bl.GetAllTUnits(
                                                    this.OrdersTabUserControl.FilterName.Text,
                                                    this.OrdersTabUserControl.FilterKey.SelectedItem,
                                                    this.OrdersTabUserControl.FilterStar.SelectedItem as BE.StarRating?,
                                                    this.OrdersTabUserControl.FilterArea.SelectedItem as BE.VacationArea?,
                                                    this.OrdersTabUserControl.FilterType.SelectedItem as BE.VacationType?)
                                                             orderby item.HostingUnitName, item.HostingUnitKey
                                                             select new
                                                             {
                                                                 item.HostingUnitKey,
                                                                 item.HostingUnitName,
                                                                 item.Area,
                                                                 item.SubArea,
                                                                 item.Type,
                                                                 item.Pet,
                                                                 item.WiFi,
                                                                 item.Parking,
                                                                 item.Pool,
                                                                 item.Jacuzzi,
                                                                 item.Garden,
                                                                 item.ChildrensAttractions,
                                                                 item.FitnessCenter,
                                                                 item.Stars,
                                                                 item.Beds,
                                                             };
        }
        private void ShowButtonsreq(object sender, RoutedEventArgs e)
        {

        }
        private void ResetFiltersgr(object sender, RoutedEventArgs e)
        {

        }
        private void createOrder(object sender, RoutedEventArgs e)
        {

        }
        #endregion

    }
}

