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
            //cbArea.DataContext = System.Enum.GetValues(typeof(BE.VacationArea));
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

    }
}
