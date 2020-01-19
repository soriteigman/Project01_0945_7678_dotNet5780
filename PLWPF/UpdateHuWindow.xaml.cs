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

        public UpdateHuWindow(HostingUnit hu )
        {
            InitializeComponent();
            hu1 = hu;
            this.cbArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.cbType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));

           this.DataContext = hu;

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
            //if(Convert.ToInt32(Beds)<=0)
            //{
            //    flag = false;
            //    Beds.BorderBrush = Brushes.Red;
            //}

            if (flag)
            {
                hu2.Owner.PhoneNumber = Convert.ToInt32(Phonenum.Text);
                hu2.Owner.MailAddress = Email.Text;
                hu2.Owner.BankAccountNumber = Convert.ToInt32(BankAcctNum.Text);
                hu2.Owner.CollectionClearance = (bool)CollectionClearance.IsChecked;
                //hu2.Owner.BankBranchDetails=(BankBranch)cbBank.SelectedItem;
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
                    myBL.UpdateHostingUnit(hu2);
                    Close();
                }
                catch(Exception a)
                {
                    MessageBox.Show(a.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }

        }
    }
}
