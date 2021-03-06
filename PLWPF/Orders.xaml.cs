﻿using System;
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
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl
        HostingUnit HU = null;
        IList<GuestRequest> gr;//all his requests
        public IEnumerable<HostingUnit> units { get; set; }//all his units
        IList<GuestRequest> req;//list of all requests that match any of the units
        IEnumerable<Order> ord;//all his orders
        //List<Order> myOrds;
        IList<int> keys = new List<int>();//all his hukeys
        ObservableCollection<Order> myOrders = new ObservableCollection<Order>();
        int id;
        public Orders(int ID)
        {
            id = ID;
            InitializeComponent();
            units = _bl.searchHUbyOwner(id);//list of all units for this host
            ord = _bl.GetsOpenOrders().ToList();
            #region איתחול
            foreach (HostingUnit h in units)
            {
                keys.Add(h.HostingUnitKey);
                if (_bl.AllRequestsThatMatch(_bl.BuildPredicate(h)).Count() > 0)
                    if (req == null)
                        req = _bl.AllRequestsThatMatch(_bl.BuildPredicate(h)).ToList();
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
            this.OrdersTabUserControl.AddButton.Content = "show requests that match this unit";
            this.OrdersTabUserControl.FilterName.TextChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterKey.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterStar.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterArea.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.FilterType.SelectionChanged += ApplyFiltering;
            this.OrdersTabUserControl.ResetFiltersButton.Click += ResetFilters;
            this.OrdersTabUserControl.DataGrid.SelectionChanged += ShowButtons;
            this.OrdersTabUserControl.AddButton.Click += addOrder;
            this.OrdersTabUserControl.updateButton.Click += updateHu;
            this.OrdersTabUserControl.RemoveButton.Click += remove_Click;

            this.OrdersTabUserControl.DataGrid.MouseDoubleClick += DataGridRow_MouseDoubleClick;

            this.OrdersTabUserControl.refreshButton.Visibility = Visibility.Hidden;

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
            this.newOrderTabUserControl.RemoveButton.Visibility = Visibility.Hidden;
            this.newOrderTabUserControl.updateButton.Visibility = Visibility.Hidden;
            this.newOrderTabUserControl.FilterKey.Visibility = Visibility.Hidden;
            this.newOrderTabUserControl.FilterName.TextChanged += ApplyFilteringgr;
            this.newOrderTabUserControl.FilterStar.SelectionChanged += ApplyFilteringgr;
            this.newOrderTabUserControl.FilterArea.SelectionChanged += ApplyFilteringgr;
            this.newOrderTabUserControl.FilterType.SelectionChanged += ApplyFilteringgr;
            this.newOrderTabUserControl.ResetFiltersButton.Click += ResetFiltersgr;
            this.newOrderTabUserControl.DataGrid.SelectionChanged += ShowButtonsreq;
            this.newOrderTabUserControl.DataGrid.AutoGeneratingColumn += WayOfViewgr;
            this.newOrderTabUserControl.AddButton.Click += createOrder;

            this.newOrderTabUserControl.refreshButton.Visibility = Visibility.Hidden;

            this.newOrderTabUserControl.FilterStar.ItemsSource = Enum.GetValues(typeof(BE.StarRating));
            this.newOrderTabUserControl.FilterStar.SelectedItem = "{Binding Path=UnitStar,Mode=TwoWay}";

            this.newOrderTabUserControl.FilterArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.newOrderTabUserControl.FilterArea.SelectedItem = "{Binding Path=UnitArea,Mode=TwoWay}";

            this.newOrderTabUserControl.FilterType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            this.newOrderTabUserControl.FilterType.SelectedItem = "{Binding Path=UnitPath,Mode=TwoWay}";

            this.newOrderTabUserControl.DataGrid.DisplayMemberPath = "myreq";
            this.newOrderTabUserControl.DataGrid.SelectedIndex = 0;
            this.newOrderTabUserControl.DataGrid.UnselectAll();
            #endregion

            #region all requests
            this.AllRequeststab.DataGrid.ItemsSource = req;
            this.AllRequeststab.DataGrid.AutoGeneratingColumn += WayOfViewgr;
            this.AllRequeststab.updateButton.Visibility = Visibility.Hidden;
            this.AllRequeststab.RemoveButton.Visibility = Visibility.Hidden;
            this.AllRequeststab.refreshButton.Visibility = Visibility.Hidden;

            #endregion

            #region my orders
            // myOrds.RemoveAll(o => _bl.SearchHUbyID_bl(o.HostingUnitKey).Owner.HostKey != id);//removes all orders not connected to current host
            this.MyRequeststab.DataGrid.ItemsSource = myOrders;
            this.MyRequeststab.updateButton.IsEnabled = true;
            this.MyRequeststab.updateButton.Content = "Update Order";
            this.MyRequeststab.updateButton.Click += UpdateOrder_Click;
            this.MyRequeststab.refreshButton.Click += RefreshButton_Click;
            this.MyRequeststab.DataGrid.AutoGeneratingColumn += WayOfViewOr;
            this.MyRequeststab.DataGrid.SelectionChanged += ShowOrderButtons;
            this.MyRequeststab.DataGrid.UnselectAll();
            this.MyRequeststab.updateButton.IsEnabled = false;
            this.MyRequeststab.RemoveButton.Visibility = Visibility.Hidden;
            this.MyRequeststab.AddButton.Visibility = Visibility.Hidden;


            #endregion
        }

        #region units
        private bool Condition(int k)//removes requests that were delt with
        {
            foreach (Order o in myOrders)
            {
                if (o.GuestRequestKey == k)
                    return true;
            }
            return false;
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

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            HostingUnit myunit = (HostingUnit)this.OrdersTabUserControl.DataGrid.SelectedItem;//gets selected row of the datagrid
            int id = myunit.Owner.HostKey;
            int key = myunit.HostingUnitKey;
            MessageBoxResult mbResult;
            mbResult = MessageBox.Show("Are you sure you want to delete the following property:\n" +
                "Name: " + myunit.HostingUnitName + "\nKey: " + myunit.HostingUnitKey +
                " \nThis action cannot be undone.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (myunit == null)
                return;
            try
            {
                if (mbResult == MessageBoxResult.Yes)
                {
                    _bl.RemoveHostingUnit(myunit);//remove unit
                    units = _bl.searchHUbyOwner(id);//list of all units for this host
                    this.OrdersTabUserControl.DataGrid.ItemsSource = units;
                    if (!_bl.HExists(key))
                    {
                        MessageBox.Show("You have successfully removed property\n" +
                        "Name: " + myunit.HostingUnitName + "\nKey: " + myunit.HostingUnitKey, "Successfulyy removed", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
            this.OrdersTabUserControl.DataGrid.ItemsSource = from item in _bl.GetAllUnits(
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
        private void WayOfView(object sender, DataGridAutoGeneratingColumnEventArgs e)//hosting unit header display
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
                    e.Column.Visibility = Visibility.Collapsed;
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
            this.newOrderTabUserControl.DataGrid.ItemsSource = from item in _bl.GetAllReq(
                                                    this.OrdersTabUserControl.FilterName.Text,
                                                    this.OrdersTabUserControl.FilterStar.SelectedItem as BE.StarRating?,
                                                    this.OrdersTabUserControl.FilterArea.SelectedItem as BE.VacationArea?,
                                                    this.OrdersTabUserControl.FilterType.SelectedItem as BE.VacationType?)
                                                               orderby item.PrivateName, item.FamilyName
                                                               select new
                                                               {
                                                                   item.PrivateName,
                                                                   item.FamilyName,
                                                                   item.MailAddress,
                                                                   item.Status,
                                                                   item.RegistrationDate,
                                                                   item.EntryDate,
                                                                   item.ReleaseDate,
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
        private void ResetFiltersgr(object sender, RoutedEventArgs e)
        {
            this.newOrderTabUserControl.FilterName.Text = "";
            this.newOrderTabUserControl.FilterStar.SelectedItem = null;
            this.newOrderTabUserControl.FilterArea.SelectedItem = null;
            this.newOrderTabUserControl.FilterType.SelectedItem = null;
            this.newOrderTabUserControl.DataGrid.ItemsSource = gr;
            this.newOrderTabUserControl.DataGrid.DisplayMemberPath = "my req";
            this.newOrderTabUserControl.DataGrid.SelectedIndex = 0;
        }

        private void createOrder(object sender, RoutedEventArgs e)//to do
        {
            try
            {
                GuestRequest GR = (GuestRequest)this.newOrderTabUserControl.DataGrid.SelectedItem;
                Order newOrd = _bl.CreateOrder(HU.HostingUnitKey, GR.GuestRequestKey);
                _bl.AddOrder(newOrd);
                MessageBox.Show("Order created succesfully", "Order", MessageBoxButton.OK, MessageBoxImage.Information);
                ord = _bl.GetsOpenOrders().ToList();
                myOrders.Clear();
                foreach (int key in keys)
                {
                    foreach (Order o in ord)
                    {
                        if (o.HostingUnitKey == key)
                            myOrders.Add(o);
                    }
                }
                this.MyRequeststab.DataGrid.ItemsSource = myOrders;
                this.AllRequeststab.DataGrid.ItemsSource = req;
                TC.SelectedIndex = 0;
                this.myRequests.Visibility = Visibility.Hidden;
                this.OrdersTabUserControl.DataGrid.UnselectAll();
                this.OrdersTabUserControl.AddButton.IsEnabled = false;
                this.OrdersTabUserControl.RemoveButton.IsEnabled = false;
                this.OrdersTabUserControl.updateButton.IsEnabled = false;
                List<GuestRequest> list = new List<GuestRequest>();
                foreach (GuestRequest i in req)
                {
                    list.Add(i);
                }
                list.RemoveAll(g => AllCondition(g.GuestRequestKey));
                req.Clear();
                foreach (GuestRequest guest in list)
                {
                    req.Add(guest);
                }
                this.newOrderTabUserControl.DataGrid.ItemsSource = gr;
            }
            catch(Exception a)
            {
                MessageBox.Show(a.Message);
            }

        }
        private bool AllCondition(int k)//removes requests that were delt with
        {
            ord = _bl.GetsOpenOrders().ToList();
            foreach (Order o in ord)
            {
                if (o.GuestRequestKey == k)
                    return true;
            }
            return false;
        }
        private void WayOfViewgr(object sender, DataGridAutoGeneratingColumnEventArgs e)//gr header display
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

        #region orders

        private void addOrder(object sender, RoutedEventArgs e)
        {
            HU = (HostingUnit)this.OrdersTabUserControl.DataGrid.SelectedItem;
            if (HU == null)
                return;
            gr = _bl.AllRequestsThatMatch(_bl.BuildPredicate(HU)).ToList();
            #region אתחול
            if (gr == null || gr.Count() == 0)//doesnt have requests that match this unit
            {
                this.newOrderTabUserControl.FilterKey.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.FilterName.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.FilterStar.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.FilterArea.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.FilterType.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.ResetFiltersButton.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.AddButton.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.DataGrid.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.empty.Visibility = Visibility.Visible;
            }
            else//has requests that match his unit
            {
                this.newOrderTabUserControl.DataGrid.ItemsSource = gr;
                this.newOrderTabUserControl.DataGrid.DisplayMemberPath = "GuestReq";
                this.newOrderTabUserControl.DataGrid.SelectedIndex = 0;
                this.newOrderTabUserControl.DataGrid.AutoGeneratingColumn += WayOfView;
                this.newOrderTabUserControl.FilterName.Visibility = Visibility.Visible;
                this.newOrderTabUserControl.FilterStar.Visibility = Visibility.Visible;
                this.newOrderTabUserControl.FilterArea.Visibility = Visibility.Visible;
                this.newOrderTabUserControl.FilterType.Visibility = Visibility.Visible;
                this.newOrderTabUserControl.ResetFiltersButton.Visibility = Visibility.Visible;
                this.newOrderTabUserControl.AddButton.Visibility = Visibility.Visible;
                this.newOrderTabUserControl.DataGrid.Visibility = Visibility.Visible;
                this.newOrderTabUserControl.empty.Visibility = Visibility.Hidden;
                this.newOrderTabUserControl.DataGrid.IsEnabled = true;
            }
            #endregion
            this.myRequests.Visibility = Visibility.Visible;
            TC.SelectedIndex = 3;
            List<GuestRequest> list = new List<GuestRequest>();
            foreach (GuestRequest i in gr)
            {
                list.Add(i);
            }
            list.RemoveAll(g => Condition(g.GuestRequestKey));
            gr.Clear();
            foreach (GuestRequest guest in list)
            {
                gr.Add(guest);
            }
            this.newOrderTabUserControl.DataGrid.ItemsSource = gr;
            this.newOrderTabUserControl.DataGrid.UnselectAll();


        }


        private void WayOfViewOr(object sender, DataGridAutoGeneratingColumnEventArgs e)//order header display
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

        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrder uo = new UpdateOrder((Order)MyRequeststab.DataGrid.SelectedItem, this.MyRequeststab.DataGrid,id);
            this.MyRequeststab.DataGrid.UnselectAll();
            this.MyRequeststab.updateButton.IsEnabled = false;
            uo.Show();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ord = _bl.GetsOpenOrders().ToList();
            myOrders.Clear();
            foreach (int key in keys)
            {
                foreach (Order o in ord)
                {
                    if (o.HostingUnitKey == key)
                        myOrders.Add(o);
                }
            }
            this.MyRequeststab.DataGrid.ItemsSource = myOrders;
        }

        private void ShowOrderButtons(object sender, SelectionChangedEventArgs e)
        {
            if (this.MyRequeststab.DataGrid.CurrentItem != null)//something was selected
            {
                this.MyRequeststab.updateButton.IsEnabled = true;//allows button clicks
            }
            else
            {
                this.OrdersTabUserControl.updateButton.IsEnabled = false;
            }
        }

        #endregion
    }    
}

