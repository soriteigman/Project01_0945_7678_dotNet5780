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
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostingUnitPage.xaml
    /// </summary>
    public partial class HostingUnitPage : Page
    {
        IBL myBL = BL.FactoryBL.getBL();

        public HostingUnitPage()
        {
            InitializeComponent();
        }

        private void Add_HU_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddHostingUnit());

        }

        private void ID_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (ID.Text == "Enter your ID")
                ID.Clear();
        }
        private void Add_HU_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            HU_Button.Foreground = Brushes.Red;
        }

        private void Add_HU_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            HU_Button.Foreground = Brushes.Black;
        }

        private void ID_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID.Text.Count() == 9)
                {
                    if (myBL.HExists(int.Parse(ID.Text)))
                    {
                        this.NavigationService.Navigate(new Orders(int.Parse(ID.Text)));
                    }
                    else
                    {
                        MessageBox.Show("ID not in System");
                    }
                    
                }
                else
                    MessageBox.Show("please enter a 9 digit ID");
            }
            catch (KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);
                ID.Clear();
            }
           
        }

        private void ID_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ID.Text == "")
                ID.Text = "Enter your ID";
        }
    }
}
