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
        public UpdateHuWindow(HostingUnit hu )
        {
            InitializeComponent();

            this.cbArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.cbType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            //if (cbArea.SelectedItem.ToString() == "DeadSea")
            //{
            //    cbSubArea.SelectedItem = "{Binding Path=DeadSea,Mode=TwoWay}";
            //    this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.DeadSea));
            //}
            //else if (cbArea.SelectedItem.ToString() == "Eilat")
            //{
            //    cbSubArea.SelectedItem = "{Binding Path=Eilat,Mode=TwoWay}";
            //    this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.Eilat));
            //}
            //else if (cbArea.SelectedItem.ToString() == "Jerusalem")
            //{
            //    cbSubArea.SelectedItem = "{Binding Path=Jerusalem,Mode=TwoWay}";
            //    this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.Jerusalem));
            //}
            //else if (cbArea.SelectedItem.ToString() == "North")
            //{
            //    cbSubArea.SelectedItem = "{Binding Path=North,Mode=TwoWay}";
            //    this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.North));
            //}
            //else if (cbArea.SelectedItem.ToString() == "South")
            //{
            //    cbSubArea.SelectedItem = "{Binding Path=South,Mode=TwoWay}";
            //    this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.South));
            //}
            //else if (cbArea.SelectedItem.ToString() == "Center")
            //{
            //    cbSubArea.SelectedItem = "{Binding Path=Center,Mode=TwoWay}";
            //    this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.Center));
            //}
            //else
            //{
            //    cbSubArea.SelectedItem = "{Binding Path=All,Mode=TwoWay}";
            //    this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.All));
            //}

           this.DataContext = hu;
           ID.Text = (hu.Owner.HostKey).ToString();
           Fname.Text = (hu.Owner.PrivateName).ToString();
           Lname.Text = (hu.Owner.FamilyName).ToString();
           Fname.Text = (hu.Owner.PrivateName).ToString();
           Phonenum.Text = (hu.Owner.PhoneNumber).ToString();
           Email.Text = (hu.Owner.MailAddress).ToString();
           BankAcctNum.Text = (hu.Owner.BankAccountNumber).ToString(); 

            cbArea.SelectedValue = hu.Area;
        }

        
    }
}
