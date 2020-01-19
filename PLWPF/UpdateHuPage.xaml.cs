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
    /// Interaction logic for UpdateHuPage.xaml
    /// </summary>
    public partial class UpdateHuPage : Page
    {
        private IEnumerable<Order> orders { get; set; }
        private IEnumerable<GuestRequest> gr { get; set; }
        private IEnumerable<HostingUnit> hu { get; set; }

        public UpdateHuPage()
        {
            InitializeComponent();

            IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl

            this.UpdateHUGrid.DataGrid.AutoGeneratingColumn += UpdateHUGrid_AutoGeneratingColumn;


            hu = _bl.listHU();
            UpdateHUGrid.DataGrid.ItemsSource = hu;
            UpdateHUGrid.DataGrid.DisplayMemberPath = "hostingunit";
            UpdateHUGrid.DataGrid.SelectedIndex = 0;
        }

        private void UpdateHUGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "HostingUnitKey":
                    e.Column.Header = "Property Key";
                    break;
                case "Owner":
                    e.Column.Header = "Owner";
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "Area":
                    e.Column.Header = "Area";
                    break;
                case "SubArea":
                    e.Column.Header = "Sub Area";
                    break;
                case "Type":
                    e.Column.Header = "Property Type";
                    break;
                case "Pet":
                    e.Column.Header = "Pet";
                    break;
                case "WiFi":
                    e.Column.Header = "Wifi";
                    break;
                case "Parking":
                    e.Column.Header = "Parking";
                    break;
                case "Pool":
                    e.Column.Header = "Pool";
                    break;
                case "Jacuzzi":
                    e.Column.Header = "Jacuzzi";
                    break;
                case "Garden":
                    e.Column.Header = "Garden";
                    break;
                case "ChildrensAttractions":
                    e.Column.Header = "Childrens Attractions";
                    break;
                case "FitnessCenter":
                    e.Column.Header = "FitnessCenter";
                    break;
                case "Stars":
                    e.Column.Header = "Rating";
                    break;
                case "Beds":
                    e.Column.Header = "Beds";
                    break;
                case "Diary":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }

            /* private void ApplyTestersFiltering(object sender, RoutedEventArgs e)
        {
            this.TestersTabUserControl.DataGrid.ItemsSource = from item in bl.GetAllTesters(
                                        this.TestersTabUserControl.SearchTextBox.Text,
                                        this.TestersTabUserControl.genderComboBox.SelectedItem as BE.Gender?,
                                        this.TestersTabUserControl.gearBoxTypeComboBox.SelectedItem as BE.GearBoxType?,
                                        this.TestersTabUserControl.vehicleComboBox.SelectedItem as BE.Vehicle?,
                                        this.TestersTabUserControl.FromTimeDatePicker.SelectedDate,
                                        this.TestersTabUserControl.ToTimeDatePicker.SelectedDate)
                                                              orderby item.FirstName, item.LastName
                                                              select new
                                                              {
                                                                  item.FirstName,
                                                                  item.LastName,
                                                                  item.ID,
                                                                  item.Gender,
                                                                  BirthDate = item.BirthDate.ToShortDateString(),
                                                                  item.PhoneNumber,
                                                                  item.Address,
                                                                  item.MailAddress,
                                                                  item.Vehicle,
                                                                  item.GearBoxType,
                                                                  item.Experience,
                                                                  item.MaxTestsInWeek,
                                                                  item.WorkHours,
                                                                  item.MaxDistanceInMeters
                                                              };
        }
                */

         private void UpdateButton_Click(object sender, RoutedEventArgs e)
         {
             if (!BE.Tools.IsInternetAvailable())
                MessageBox.Show("בדוק את החיבור שלך לרשת", "אין חיבור לרשת", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
             else
             {
                if (new UpdateTrainee().ShowDialog() == true)
                        AddNotification("התלמיד " + selectedTrainees[0].FirstName + ' ' + selectedTrainees[0].LastName + " עודכן בהצלחה");
                ApplyTraineesFiltering(this, new RoutedEventArgs());
             }
        }
        }
    }
}
