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
    /// Interaction logic for ManagingPage.xaml
    /// </summary>
    public partial class ManagingPage : Page
    {
        IBL myBL = BL.FactoryBL.getBL();

        public ManagingPage()
        {
            InitializeComponent();
        }
        private void go_Click(object sender, RoutedEventArgs e)
        {
            if (cbHostingUnit.Text == "Update Hosting Unit")
            {
                //this.NavigationService.Navigate(new update());
            }
            if (cbHostingUnit.Text == "Create and view orders")
            {
                this.NavigationService.Navigate(new Orders());
            }
            else 
            {
                removeID.Visibility = Visibility.Visible;
                remove.Visibility = Visibility.Visible;
            }
        }

        private void removeID_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (removeID.Text == "Enter the Key Code of the Property you would like to Remove")
                removeID.Clear();
        }

        private void removeID_MouseLeave(object sender, MouseEventArgs e)
        {
            if (removeID.Text == "")
                removeID.Text = "Enter the Key Code of the Property you would like to Remove";
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult;
            mbResult = MessageBox.Show("Are you sure you want to delete this property? This action is irreversible.", "", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No);

            if (mbResult == MessageBoxResult.Yes)
            {
                myBL.RemoveHostingUnit(myBL.SearchHUbyID_bl(Convert.ToInt32(removeID.Text)));
            }
        }
    }
}
