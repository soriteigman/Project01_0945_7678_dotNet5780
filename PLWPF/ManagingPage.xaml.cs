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
    /// Interaction logic for ManagingPage.xaml
    /// </summary>
    public partial class ManagingPage : Page
    {
        public ManagingPage()
        {
            InitializeComponent();
        }

        public ManagingPage(int ID)
        {

        }

        private void CbHostingUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.NavigationService.Navigate(new HostingUnitPage());

        }

        private void go_Click(object sender, RoutedEventArgs e)
        {
            if (cbHostingUnit.SelectedValue.ToString() == "Update Hosting Unit")
                ;//this.NavigationService.Navigate(new update());
            //else remove property with key of remove text box
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
    }
}
