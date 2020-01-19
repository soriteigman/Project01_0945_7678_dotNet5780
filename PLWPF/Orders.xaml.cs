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
            foreach (HostingUnit hu in units)
            {
                keys.Add(hu.HostingUnitKey);
                if (_bl.AllRequestsThatMatch(_bl.BuildPredicate(hu)).Count() > 0)
                    if (req == null)
                        req = _bl.AllRequestsThatMatch(_bl.BuildPredicate(hu));
                    else req.Concat(_bl.AllRequestsThatMatch(_bl.BuildPredicate(hu)));
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

            this.OrdersTabUserControl.FilterName.TextChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterKey.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterStar.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterArea.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterType.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.ResetFiltersButton.Click += ResetFilters;
            this.OrdersTabUserControl.DataGrid.SelectionChanged += ShowButtons;
            this.OrdersTabUserControl.AddButton.Click += addOrder;



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

        }
        private void addOrder(object sender, RoutedEventArgs e)
        {
                this.NavigationService.Navigate(new NewOrderPage(((HostingUnit)this.OrdersTabUserControl.DataGrid.CurrentItem).HostingUnitKey));
        }
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
                    //let him kmow why he cant create a order
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
    }
}

