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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void HU_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HostingUnitPage());

        }

        private void OWNER_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow(this);
            lw.Show();

            if(!lw.IsActive)
              this.NavigationService.Navigate(new Admin());

        }

        private void GR_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new GuestReq());

        }
    }
}
