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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using BE;
using BL;
using System.Threading;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl

        private IEnumerable<Order> orders { get; set; }
        private IEnumerable<GuestRequest> gr { get; set; }
        private IEnumerable<HostingUnit> hu { get; set; }

        public Admin()
        {
            InitializeComponent();

            Thread thread = new Thread(DailyUpdate);
            thread.Start();

            #region orders
            IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl
            orders = _bl.GetsOpenOrders();
            this.MyOrderstab.DataGrid.ItemsSource = orders;
            this.MyOrderstab.DataGrid.DisplayMemberPath = "orders";
            this.MyOrderstab.DataGrid.SelectedIndex = 0;
            this.MyOrderstab.Larea.Visibility = Visibility.Hidden;
            this.MyOrderstab.Lkey.Visibility = Visibility.Hidden;
            this.MyOrderstab.Lname.Visibility = Visibility.Hidden;
            this.MyOrderstab.Lstar.Visibility = Visibility.Hidden;
            this.MyOrderstab.AddButton.Visibility = Visibility.Hidden;
            this.MyOrderstab.updateButton.Visibility = Visibility.Hidden;
            this.MyOrderstab.RemoveButton.Visibility = Visibility.Hidden;
            this.MyOrderstab.refreshButton.Visibility = Visibility.Hidden;
            this.MyOrderstab.FilterName.Visibility = Visibility.Hidden;
            this.MyOrderstab.FilterKey.Visibility = Visibility.Hidden;
            this.MyOrderstab.FilterStar.Visibility = Visibility.Hidden;
            this.MyOrderstab.FilterArea.Visibility = Visibility.Hidden;
            this.MyOrderstab.FilterType.SelectionChanged += OApplyFiltering;
            this.MyOrderstab.ResetFiltersButton.Click += OResetFilters;
            this.MyOrderstab.DataGrid.AutoGeneratingColumn += OWayOfView;
            this.MyOrderstab.commissionLabel.Visibility = Visibility.Visible;
            this.MyOrderstab.totalCommissionText.Visibility = Visibility.Visible;
            this.MyOrderstab.totalCommissionText.Text = (_bl.TotalCommissionCalculator()).ToString();

            this.MyOrderstab.Ltype.Content = "Filter Status";
            this.MyOrderstab.FilterType.ItemsSource = Enum.GetValues(typeof(BE.Status));
            this.MyOrderstab.FilterType.SelectedItem = "{Binding Path=UnitPath,Mode=TwoWay}";
            #endregion

            #region gr
            gr = _bl.listGR();
            this.MyRequeststab.DataGrid.ItemsSource = gr;
            this.MyRequeststab.DataGrid.DisplayMemberPath = "guestrequest";
            this.MyRequeststab.DataGrid.SelectedIndex = 0;
            this.MyRequeststab.AddButton.Visibility = Visibility.Hidden;
            this.MyRequeststab.updateButton.Visibility = Visibility.Hidden;
            this.MyRequeststab.RemoveButton.Visibility = Visibility.Hidden;
            this.MyRequeststab.refreshButton.Visibility = Visibility.Hidden;
            this.MyRequeststab.Lkey.Visibility = Visibility.Hidden;
            this.MyRequeststab.FilterKey.Visibility = Visibility.Hidden;
            this.MyRequeststab.FilterName.TextChanged += GRApplyFiltering;
            this.MyRequeststab.FilterKey.SelectionChanged += GRApplyFiltering;
            this.MyRequeststab.FilterStar.SelectionChanged += GRApplyFiltering;
            this.MyRequeststab.FilterArea.SelectionChanged += GRApplyFiltering;
            this.MyRequeststab.FilterType.SelectionChanged += GRApplyFiltering;
            this.MyRequeststab.ResetFiltersButton.Click += GRResetFilters;
            this.MyRequeststab.DataGrid.AutoGeneratingColumn += GRWayOfView;


            this.MyRequeststab.FilterStar.ItemsSource = Enum.GetValues(typeof(BE.StarRating));
            this.MyRequeststab.FilterStar.SelectedItem = "{Binding Path=UnitStar,Mode=TwoWay}";

            this.MyRequeststab.FilterArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.MyRequeststab.FilterArea.SelectedItem = "{Binding Path=UnitArea,Mode=TwoWay}";

            this.MyRequeststab.FilterType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            this.MyRequeststab.FilterType.SelectedItem = "{Binding Path=UnitPath,Mode=TwoWay}";
            #endregion

            #region hu
            hu = _bl.listHU();
            this.MyUnitstab.DataGrid.ItemsSource = hu;
            this.MyUnitstab.DataGrid.DisplayMemberPath = "hostingunit";
            this.MyUnitstab.DataGrid.SelectedIndex = 0;
            this.MyUnitstab.AddButton.Visibility = Visibility.Hidden;
            this.MyUnitstab.updateButton.Visibility = Visibility.Hidden;
            this.MyUnitstab.RemoveButton.Visibility = Visibility.Hidden;
            this.MyUnitstab.refreshButton.Visibility = Visibility.Hidden;
            this.MyUnitstab.FilterName.TextChanged += HUApplyFiltering;
            this.MyUnitstab.Lkey.Visibility = Visibility.Hidden;
            this.MyUnitstab.FilterKey.Visibility = Visibility.Hidden;
            this.MyUnitstab.FilterKey.SelectionChanged += HUApplyFiltering;
            this.MyUnitstab.FilterStar.SelectionChanged += HUApplyFiltering;
            this.MyUnitstab.FilterArea.SelectionChanged += HUApplyFiltering;
            this.MyUnitstab.FilterType.SelectionChanged += HUApplyFiltering;
            this.MyUnitstab.ResetFiltersButton.Click += HUResetFilters;
            this.MyUnitstab.DataGrid.AutoGeneratingColumn += HUWayOfView;


            this.MyUnitstab.FilterStar.ItemsSource = Enum.GetValues(typeof(BE.StarRating));
            this.MyUnitstab.FilterStar.SelectedItem = "{Binding Path=UnitStar,Mode=TwoWay}";

            this.MyUnitstab.FilterArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.MyUnitstab.FilterArea.SelectedItem = "{Binding Path=UnitArea,Mode=TwoWay}";

            this.MyUnitstab.FilterType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            this.MyUnitstab.FilterType.SelectedItem = "{Binding Path=UnitPath,Mode=TwoWay}";
            #endregion

        }
        #region daily
        private void DailyUpdate()
        {
            while (true)
            {
                if (Configuration._DateLastRun < DateTime.Now.Date)
                {
                    OrderDailyMethod();
                    ReqDailyMethod();
                    _bl.UpdateConfig("_DateLastRun", DateTime.Now.Date);
                    Thread.Sleep(86400000);//sleep for 24 hours
                }
            }
        }

        private void ReqDailyMethod()
        {
            IEnumerable<GuestRequest> listOfreq = _bl.DaysPassedOnReq(31);
            List<GuestRequest> g = new List<GuestRequest>();
            foreach (GuestRequest o in listOfreq)
            {
                g.Add(o);
            }
            g.ForEach(element => element.Status = Status.Closed);
            g.ForEach(element => _bl.Updategr(element));
        }

        private void OrderDailyMethod()
        {
            IEnumerable<Order> listOfOrder = _bl.DaysPassedOnOrders(31);
            List<Order> ord=new List<Order>();
            foreach(Order o in listOfOrder)
            {
                ord.Add(o);
            }
            ord.ForEach(element => element.Status = Status.Closed);
            ord.ForEach(element => _bl.UpdateOrder(element));
        }
        #endregion

        #region hu func
        private void HUApplyFiltering(object sender, RoutedEventArgs e)
        {
            this.MyUnitstab.DataGrid.ItemsSource = from item in _bl.GetAllUnits(
                                                    this.MyUnitstab.FilterName.Text,
                                                    this.MyUnitstab.FilterKey.SelectedItem,
                                                    this.MyUnitstab.FilterStar.SelectedItem as BE.StarRating?,
                                                    this.MyUnitstab.FilterArea.SelectedItem as BE.VacationArea?,
                                                    this.MyUnitstab.FilterType.SelectedItem as BE.VacationType?)
                                                   orderby item.HostingUnitName, item.HostingUnitKey
                                                   select new
                                                   {
                                                       item.HostingUnitKey,
                                                       item.HostingUnitName,
                                                       item.Owner,
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

        private void HUResetFilters(object sender, RoutedEventArgs e)
        {
            this.MyUnitstab.FilterName.Text = "";
            this.MyUnitstab.FilterKey.SelectedItem = null;
            this.MyUnitstab.FilterStar.SelectedItem = null;
            this.MyUnitstab.FilterArea.SelectedItem = null;
            this.MyUnitstab.FilterType.SelectedItem = null;
            this.MyUnitstab.DataGrid.ItemsSource = hu;
            this.MyUnitstab.DataGrid.DisplayMemberPath = "hostingunit";
            this.MyUnitstab.DataGrid.SelectedIndex = 0;
        }

        private void HUWayOfView(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            switch (e.PropertyName)
            {
                case "HostingUnitKey":
                    e.Column.Header = "Property Key";
                    break;
                case "HostingUnitName":
                    e.Column.Header = "Property Name";
                    break;
                case "Owner":
                    //e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "Area":
                    e.Column.Header = "Area";
                    break;
                case "SubArea":
                    e.Column.Header = "Sub-Area";
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
                    e.Column.Header = "Childrens\nAttractions";
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

        #region gr func
        private void GRApplyFiltering(object sender, RoutedEventArgs e)
        {
            this.MyRequeststab.DataGrid.ItemsSource = from item in _bl.GetAllReq(
                                                    this.MyRequeststab.FilterName.Text,
                                                    this.MyRequeststab.FilterStar.SelectedItem as BE.StarRating?,
                                                    this.MyRequeststab.FilterArea.SelectedItem as BE.VacationArea?,
                                                    this.MyRequeststab.FilterType.SelectedItem as BE.VacationType?)
                                                      orderby item.PrivateName, item.FamilyName
                                                      select new
                                                      {
                                                          item.GuestRequestKey,
                                                          item.PrivateName,
                                                          item.FamilyName,
                                                          item.MailAddress,
                                                          item.Status,
                                                          item.RegistrationDate,
                                                          item.EntryDate,
                                                          item.ReleaseDate,
                                                          item.Area,
                                                          item.Type,
                                                          item.Adults,
                                                          item.Children,
                                                          item.Pool,
                                                          item.Jacuzzi,
                                                          item.Garden,
                                                          item.ChildrensAttractions,
                                                          item.Pet,
                                                          item.FitnessCenter,
                                                          item.Stars,
                                                          item.WiFi,
                                                          item.Parking,
                                                          item.SubArea,

                                                      };
        }
        private void GRResetFilters(object sender, RoutedEventArgs e)
        {
            this.MyRequeststab.FilterName.Text = "";
            this.MyRequeststab.FilterStar.SelectedItem = null;
            this.MyRequeststab.FilterArea.SelectedItem = null;
            this.MyRequeststab.FilterType.SelectedItem = null;
            this.MyRequeststab.DataGrid.ItemsSource = gr;
            this.MyRequeststab.DataGrid.DisplayMemberPath = "my req";
            this.MyRequeststab.DataGrid.SelectedIndex = 0;
        }
        private void GRWayOfView(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            switch (e.PropertyName)
            {
                case "GuestRequestKey":
                    e.Column.Header = "Request Key";
                    break;
                case "PrivateName":
                    e.Column.Header = "First Name";
                    break;
                case "FamilyName":
                    e.Column.Header = "Last Name";
                    break;
                case "MailAddress":
                    e.Column.Header = "Email Address";
                    break;
                case "Status":
                    e.Column.Header = "Guest Request Status";
                    break;
                case "RegistrationDate":
                    e.Column.Header = "Registration Date";
                    break;
                case "EntryDate":
                    e.Column.Header = "Entry Date";
                    break;
                case "ReleaseDate":
                    e.Column.Header = "Release Date";
                    break;
                case "Area":
                    e.Column.Header = "Area";
                    break;
                case "SubArea":
                    e.Column.Header = "Sub-Area";
                    break;
                case "Type":
                    e.Column.Header = "Vacation Type";
                    break;
                case "Adults":
                    e.Column.Header = "Adults";
                    break;
                case "Children":
                    e.Column.Header = "Children";
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
                    e.Column.Header = "Childrens\nAttractions";
                    break;
                case "FitnessCenter":
                    e.Column.Header = "Fitness Center";
                    break;
                case "Stars":
                    e.Column.Header = "Rating";
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region order func
        private void OApplyFiltering(object sender, RoutedEventArgs e)
        {
            this.MyOrderstab.DataGrid.ItemsSource = from item in _bl.GetAllOrders(
                                                    this.MyOrderstab.FilterType.SelectedItem as BE.Status?)
                                                    orderby item.CreateDate, item.Status
                                                    select new
                                                    {

                                                        item.HostingUnitKey,
                                                        item.GuestRequestKey,
                                                        item.OrderKey,
                                                        item.CreateDate,
                                                        item.Status,
                                                        item.SentEmail,
                                                    };
        }
        private void OResetFilters(object sender, RoutedEventArgs e)
        {
            this.MyOrderstab.FilterName.Text = "";
            this.MyOrderstab.FilterType.SelectedItem = null;
            this.MyOrderstab.DataGrid.ItemsSource = orders;
            this.MyOrderstab.DataGrid.DisplayMemberPath = "orders";
            this.MyOrderstab.DataGrid.SelectedIndex = 0;
        }
        private void OWayOfView(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            switch (e.PropertyName)
            {
                case "GuestRequestKey":
                    e.Column.Header = "Guest Request Key";
                    break;
                case "HostingUnitKey":
                    e.Column.Header = "Hosting Unit Key";
                    break;
                case "OrderKey":
                    e.Column.Header = "Order Key";
                    break;
                case "Status":
                    e.Column.Header = "Request Status";
                    break;
                case "CreateDate":
                    e.Column.Header = "Create Date";
                    break;
                case "SentEmail":
                    e.Column.Header = "Email Date";
                    break;
                default:
                    break;

            }
        }
        #endregion



    }
}
