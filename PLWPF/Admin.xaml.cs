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
using BE;
using BL;

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
            this.MyOrderstab.FilterName.Visibility=Visibility.Hidden;
            this.MyOrderstab.FilterKey.Visibility = Visibility.Hidden;
            this.MyOrderstab.FilterStar.Visibility = Visibility.Hidden;
            this.MyOrderstab.FilterArea.Visibility = Visibility.Hidden;
            this.MyOrderstab.FilterType.SelectionChanged += OApplyFiltering;
            this.MyOrderstab.ResetFiltersButton.Click += OResetFilters;
            this.MyOrderstab.DataGrid.AutoGeneratingColumn += OWayOfView;

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
                                                                   item.PrivateName,
                                                                   item.FamilyName,
                                                                   item.MailAddress,
                                                                   item.Status,
                                                                   item.RegistrationDate,
                                                                   EntryDate = item.EntryDate.ToShortDateString(),
                                                                   ReleaseDate = item.ReleaseDate.ToShortDateString(),
                                                                   item.Area,
                                                                   item.SubArea,
                                                                   item.Type,
                                                                   item.Adults,
                                                                   item.Children,
                                                                   item.Pet,
                                                                   item.WiFi,
                                                                   item.Parking,
                                                                   item.Pool,
                                                                   item.Jacuzzi,
                                                                   item.Garden,
                                                                   item.ChildrensAttractions,
                                                                   item.FitnessCenter,
                                                                   item.Stars,
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
            switch (e.PropertyName)
            {
                case "GuestRequestKey":
                    e.Column.Header = "guest Request Key";
                    break;
                case "PrivateName":
                    e.Column.Header = "First Name";
                    break;
                case "FamilyName":
                    e.Column.Header = "Last Name";
                    break;
                case "MailAddress":
                    e.Column.Header = "Email";
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
                                                       SentEmail = item.SentEmail.ToShortDateString(),
                                                       item.Status,
                                                       CreateDate = item.CreateDate.ToShortDateString(),
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
            switch (e.PropertyName)
            {
                case "GuestRequestKey":
                    e.Column.Header = "guest Request Key";
                    break;
                case "PrivateName":
                    e.Column.Header = "First Name";
                    break;
                case "FamilyName":
                    e.Column.Header = "Last Name";
                    break;
                case "MailAddress":
                    e.Column.Header = "Email";
                    break;
                case "RegistrationDate":
                    e.Column.Header = "Registration Date";
                    break;
                case "entryDate":
                    e.Column.Header = "Entry Date";
                    break;
                case "releaseDate":
                    e.Column.Header = "Release Date";
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

                default:
                    break;

            }
        }


        #endregion
    }



}
