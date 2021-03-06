﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
    /// Interaction logic for UpdateHuWindow.xaml
    /// </summary>
    public partial class UpdateHuWindow : Window
    {
        IBL myBL = FactoryBL.getBL();
        HostingUnit hu1 = new HostingUnit();
        bool updated = false;

        public UpdateHuWindow(HostingUnit hu )
        {
            InitializeComponent();
            hu1 = hu;
            this.cbArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.cbType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            this.cbBank.ItemsSource = myBL.ListOfBanks();
           
            this.DataContext = hu;
            this.cbBank.Text = (hu.Owner.BankBranchDetails).ToString();
            this.Closing += Window_Closing;

        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            HostingUnit hu2 = new HostingUnit();
            hu2 = hu1;
            bool flag = true;
            bool email_flag = true;
            try
            {
                email_flag = myBL.IsValidEmail(Email.Text);
            }
            catch// (Exception a)
            {
                Email.Clear();
                Email.Text = "Enter Your Email Address";
                Email.BorderBrush = Brushes.Red;
                flag = false;
                email_flag = false;
            }

            if (!myBL.AllowedToChangeCommission(hu2))
            {
                if (!(bool)CollectionClearance.IsChecked && hu2.Owner.CollectionClearance)
                {
                    flag = false;
                    MessageBox.Show("cannot change collection clearance when property has booked orders");
                }

            }
                if (flag)
                {
                    hu2.Owner.PhoneNumber = Convert.ToInt32(Phonenum.Text);
                    hu2.Owner.MailAddress = Email.Text;
                    hu2.Owner.BankAccountNumber = Convert.ToInt32(BankAcctNum.Text);
                    hu2.Owner.CollectionClearance = (bool)CollectionClearance.IsChecked;
                    hu2.Owner.BankBranchDetails = (BankBranch)cbBank.SelectedItem;
                    hu2.HostingUnitName = HUname.Text;
                    hu2.Beds = Convert.ToInt32(Beds.Text);
                    hu2.Pet = (bool)Pets.IsChecked;
                    hu2.WiFi = (bool)Wifi.IsChecked;
                    hu2.Parking = (bool)Parking.IsChecked;
                    hu2.Pool = (bool)Pool.IsChecked;
                    hu2.Jacuzzi = (bool)Jacuzzi.IsChecked;
                    hu2.Garden = (bool)Garden.IsChecked;
                    hu2.ChildrensAttractions = (bool)chiAttract.IsChecked;
                    hu2.FitnessCenter = (bool)FitnessCenter.IsChecked;

                    try
                    {
                        // explicit update of each property of the hosting unit
                        BindingExpression be = HUname.GetBindingExpression(TextBox.TextProperty);
                        be.UpdateSource();
                        be = Phonenum.GetBindingExpression(TextBox.TextProperty);
                        be.UpdateSource();
                        be = BankAcctNum.GetBindingExpression(TextBox.TextProperty);
                        be.UpdateSource();
                        be = Email.GetBindingExpression(TextBox.TextProperty);
                        be.UpdateSource();
                        be = Beds.GetBindingExpression(TextBox.TextProperty);
                        be.UpdateSource();

                        be = cbBank.GetBindingExpression(ComboBox.TextProperty);
                        if (be != null)
                        {
                            be.UpdateSource();
                        }
                        be = Pool.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();
                        be = Jacuzzi.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();
                        be = Pets.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();
                        be = Garden.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();
                        be = Parking.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();
                        be = Wifi.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();
                        be = chiAttract.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();
                        be = FitnessCenter.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();
                        be = CollectionClearance.GetBindingExpression(CheckBox.IsCheckedProperty);
                        be.UpdateSource();

                        myBL.UpdateHostingUnit(hu2);
                        updated = true;
                        Close();
                    }
                    catch (Exception a)
                    {
                        MessageBox.Show(a.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


            

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult myResult= MessageBox.Show("Do you want to save changes before you leave?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);
            if (myResult == MessageBoxResult.Yes)
            {
                update_Click(sender, e);
            }
            if (myResult == MessageBoxResult.No)
            {
                Close();
            }

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!updated)
            {
                MessageBoxResult myResult = MessageBox.Show("Are you sure you want to quit without saving changes?", "", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (myResult == MessageBoxResult.No)
                    e.Cancel = true;
                if (myResult == MessageBoxResult.Yes)
                    this.Closing -= Window_Closing;
                updated = false;
            }

        }
    }
}
