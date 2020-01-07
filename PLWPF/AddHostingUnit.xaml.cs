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
    /// Interaction logic for AddHostingUnit.xaml
    /// </summary>
    public partial class AddHostingUnit : Page
    {
        public AddHostingUnit()
        {
            InitializeComponent();

            this.cbArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.VacationSubArea));
            this.cbType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



 
        private void id_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (ID.Text == "Enter Your ID")
                ID.Clear();
        }
        private void Fname_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Fname.Text == "Enter Your First Name")
                Fname.Clear();
        }
        private void Lname_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Lname.Text == "Enter Your Last Name")
                Lname.Clear();
        }
        private void Phonenum_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Phonenum.Text == "Enter Your Phone Number")
                Phonenum.Clear();
        }
        private void Email_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Email.Text == "Enter Your Email Address")
                Email.Clear();
        }
        private void Bankname_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Bankname.Text == "Enter Your Bank Name")
                Bankname.Clear();
        }
        private void Banknum_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Banknum.Text == "Enter Your Bank's Number")
                Banknum.Clear();
        }
        private void BankBranch_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Branchnum.Text == "Enter Your Branch's Number")
                Branchnum.Clear();
        }
        private void BankAddress_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Bankaddress.Text == "Enter Your Branch's Address")
                Bankaddress.Clear();
        }
        private void Bankcity_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Bankcity.Text == "Enter the City your Branch is Located in")
                Bankcity.Clear();
        }

        private void HUname_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (HUname.Text == "Give Your Property a Name ")
                HUname.Clear();
        }

        private void BankAcctNum_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (BankAcctNum.Text == "Enter Your Bank Account Number")
                BankAcctNum.Clear();
        }

        private void ID_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ID.Text == "")
                ID.Text = "Enter Your ID";
        }
        private void Fname_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Fname.Text == "")
                Fname.Text = "Enter Your First Name";
        }
        private void Lname_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Lname.Text == "")
                Lname.Text = "Enter Your Last Name";
        }

        private void Phonenum_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Phonenum.Text == "")
                Phonenum.Text = "Enter Your Phone Number";
        }

        private void Email_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Email.Text == "")
                Email.Text = "Enter Your Email Address";
        }

        private void Bankname_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Bankname.Text == "")
                Bankname.Text = "Enter Your Bank Name";
        }

        private void Banknum_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Banknum.Text == "")
                Banknum.Text = "Enter Your Bank's Number";
        }

        private void Branchnum_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Branchnum.Text == "")
                Branchnum.Text = "Enter Your Branch's Number";
        }

        private void Bankaddress_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Bankaddress.Text == "")
                Bankaddress.Text = "Enter Your Branch's Address";
        }

        private void Bankcity_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Bankcity.Text == "")
                Bankcity.Text = "Enter the City your Branch is Located in";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl
            HostingUnit hu = new HostingUnit();
            Host h = new Host();
            h.PrivateName = Fname.Text;
            h.FamilyName = Lname.Text;
            h.PhoneNumber = Convert.ToInt32(Phonenum.Text);
            h.MailAddress = Email.Text;

            //_bl.AddHostingUnit();
        }
            private void HUname_MouseLeave(object sender, MouseEventArgs e)
            {
                if (HUname.Text == "")
                    HUname.Text = "Enter the City your Branch is Located in";
            }

            private void BankAcctNum_MouseLeave(object sender, MouseEventArgs e)
        {
            if (BankAcctNum.Text == "")
                BankAcctNum.Text = "Enter the City your Branch is Located in";
        }
    }
}
