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
    /// Interaction logic for GuestReq.xaml
    /// </summary>
    public partial class GuestReq : Page
    {
        public GuestReq()
        {
            InitializeComponent();
            this.type.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            this.areacb.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));



            startday.BlackoutDates.AddDatesInPast();
            startday.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddMonths(11), DateTime.Now.AddYears(100)));
            endday.BlackoutDates.AddDatesInPast();

        }
        private void Startday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime from = startday.SelectedDate.Value.Date;
            endday.BlackoutDates.Clear();
            endday.BlackoutDates.AddDatesInPast();
            endday.BlackoutDates.Add((new CalendarDateRange(DateTime.Now, from)));

        }

        private void Endday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL _bl = BL.FactoryBL.getBL();//creates an instance of dal

            DateTime from = startday.SelectedDate.Value.Date;
            DateTime to = endday.SelectedDate.Value.Date;
            int numdays = (to - from).Days;
            numofday.Content = numdays - 1 + "-night stay";
        }

        private void Areacb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            areacb.Visibility = Visibility.Visible;
            string val = areacb.SelectedValue.ToString();

            if (val == "DeadSea")
            {
                subareacb.SelectedItem = "{Binding Path=DeadSea,Mode=TwoWay}";
                this.subareacb.ItemsSource = Enum.GetValues(typeof(BE.DeadSea));
            }
            else if (val == "Eilat")
            {
                subareacb.SelectedItem = "{Binding Path=Eilat,Mode=TwoWay}";
                this.subareacb.ItemsSource = Enum.GetValues(typeof(BE.Eilat));
            }
            else if (val == "Jerusalem")
            {
                subareacb.SelectedItem = "{Binding Path=Jerusalem,Mode=TwoWay}";
                this.subareacb.ItemsSource = Enum.GetValues(typeof(BE.Jerusalem));
            }
            else if (val == "North")
            {
                subareacb.SelectedItem = "{Binding Path=North,Mode=TwoWay}";
                this.subareacb.ItemsSource = Enum.GetValues(typeof(BE.North));
            }
            else if (val == "South")
            {
                subareacb.SelectedItem = "{Binding Path=South,Mode=TwoWay}";
                this.subareacb.ItemsSource = Enum.GetValues(typeof(BE.South));
            }
            else if (val == "Center")
            {
                subareacb.SelectedItem = "{Binding Path=Center,Mode=TwoWay}";
                this.subareacb.ItemsSource = Enum.GetValues(typeof(BE.Center));
            }
            else
            {
                subareacb.SelectedItem = "{Binding Path=All,Mode=TwoWay}";
                this.subareacb.ItemsSource = Enum.GetValues(typeof(BE.All));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PName.Text == "")
            {

            }
        }
    }
}
