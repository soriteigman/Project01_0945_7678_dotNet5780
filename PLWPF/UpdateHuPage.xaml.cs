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
                    e.Column.Header = "מסקסימום\nטסטים לשבוע";
                    break;
                default:
                    break;
            }

            /* private void TraineesDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case "ID":
                        e.Column.Header = "תעודת זהות";
                        break;
                    case "FirstName":
                        e.Column.Header = "שם פרטי";
                        break;
                    case "LastName":
                        e.Column.Header = "שם משפחה";
                        break;
                    case "BirthDate":
                        e.Column.Header = "תאריך לידה";
                        break;
                    case "Gender":
                        e.Column.Header = "מין";
                        break;
                    case "PhoneNumber":
                        e.Column.Header = "מספר טלפון";
                        break;
                    case "MailAddress":
                        e.Column.Header = "כתובת מייל";
                        break;
                    case "Address":
                        e.Column.Header = "כתובת";
                        break;
                    case "Vehicle":
                        e.Column.Header = "סוג רשיון";
                        break;
                    case "GearBoxType":
                        e.Column.Header = "תיבת\nהילוכים";
                        break;
                    case "DrivingSchoolName":
                        e.Column.Header = "שם\nבית הספר";
                        break;
                    case "TeacherName":
                        e.Column.Header = "שם\nהמורה";
                        break;
                    case "NumOfDrivingLessons":
                        e.Column.Header = "מספר\nשיעורים";
                        break;
                    case "Experience":
                        e.Column.Header = "שנות נסיון";
                        break;
                    case "MaxTestsInWeek":
                        e.Column.Header = "מסקסימום\nטסטים לשבוע";
                        break;
                    case "MaxDistanceInMeters":
                        e.Column.Header = "מרחק\nמקסימלי";
                        break;
                    case "TestID":
                        e.Column.Header = "מספר טסט";
                        break;
                    case "TesterID":
                        e.Column.Header = "תעודת זהות\nבוחן";
                        break;
                    case "TraineeID":
                        e.Column.Header = "תעודת זהות\nתלמיד";
                        break;
                    case "Passed":
                        e.Column.Header = "עבר";
                        break;
                    case "WorkHours":
                        e.Cancel = true;
                        break;
                    case "Time":
                        e.Column.Header = "זמן";
                        break;
                    case "TesterNotes":
                        e.Column.Header = "הערות הבוחן";
                        break;
                    case "AppealTest":
                        e.Column.Header = "ערעור";
                        break;
                    case "Indices":
                        e.Cancel = true;
                        break;
                    case "RemeinderEmailSent":
                        e.Cancel = true;
                        break;
                    case "SummaryEmailSent":
                        e.Cancel = true;
                        break;
                    case "OnlyMyGender":
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
                */
        }
    }
}
