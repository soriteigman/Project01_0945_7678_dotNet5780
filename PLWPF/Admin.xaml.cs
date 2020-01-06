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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        private IEnumerable<Order> orders { get; set; }
        private IEnumerable<GuestRequest> gr { get; set; }
        private IEnumerable<HostingUnit> hu { get; set; }

        public Admin()
        {
            InitializeComponent();

            IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl

            orders = _bl.GetsOpenOrders();
            ListOfOrders.ItemsSource = orders; ;
            ListOfOrders.DisplayMemberPath = "orders";
            ListOfOrders.SelectedIndex = 0;

            gr = _bl.listGR();
            guestreq.ItemsSource = gr; ;
            guestreq.DisplayMemberPath = "guestrequest";
            guestreq.SelectedIndex = 0;


            hu = _bl.listHU();
            hostingunit.ItemsSource = hu; ;
            hostingunit.DisplayMemberPath = "hostingunit";
            hostingunit.SelectedIndex = 0;

            //DataContext = _bl.GetsOpenOrders();
        }

    }



}
