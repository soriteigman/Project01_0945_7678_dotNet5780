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

        }

        private void Add_HU_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            HU_Button.Foreground = Brushes.Blue;
            //HU_Button.
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
                        this.NavigationService.Navigate(new ManagingPage(int.Parse(ID.Text)));
                    }
                    else
                    {
                        MessageBox.Show("ID not in System");
                    }
                    
                }
                else
                    MessageBox.Show("אנא הכנס תעודת זהות 9 מספרים");
            }
            catch (KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);
                ID.Clear();
            }
            //catch (Exception a)
            //{
            //    MessageBox.Show(a.Message);
            //}
        }
    }
}
