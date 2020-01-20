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
    /// Interaction logic for NewOrderPage.xaml
    /// </summary>
    public partial class NewOrderPage : Page
    {
        IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl
        public NewOrderPage(HostingUnit hu)
        {
            InitializeComponent();

        }
        private void WayOfView(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "guestRequestKey":
                    e.Column.Header = "Guest Request Key";
                    break;
                case "privateName":
                    e.Column.Header = "First Name";
                    break;
                case "familyName":
                    e.Column.Header = "Last Name";
                    break;
                case "mailAddress":
                    e.Column.Header = "Email";
                    break;
                case "registrationDate":
                    e.Column.Header = "Registration Date";
                    break;
                case "entryDate":
                    e.Column.Header = "Check in";
                    break;
                case "releaseDate":
                    e.Column.Header = "Check out";
                    break;
                case "area":
                    e.Column.Header = "Area";
                    break;
                case "subArea":
                    e.Column.Header = "sub Area";
                    break;
                case "Type":
                    e.Column.Header = "Property Type";
                    break;
                case "adults":
                    e.Column.Header = "Adults";
                    break;
                case "children":
                    e.Column.Header = "Children";
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
                    e.Column.Header = "Childrens\n Attractions";
                    break;
                case "FitnessCenter":
                    e.Column.Header = "Fitness Center";
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
        }
       
        }
}
