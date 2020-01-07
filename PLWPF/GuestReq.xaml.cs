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

            startday.BlackoutDates.AddDatesInPast();
            startday.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddMonths(11),DateTime.Now.AddYears(100)));
            endday.BlackoutDates.AddDatesInPast();

        }
        private void Startday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        { 
            DateTime from = startday.SelectedDate.Value.Date;
            endday.BlackoutDates.Clear();
            endday.BlackoutDates.AddDatesInPast();
            endday.BlackoutDates.Add((new CalendarDateRange(DateTime.Now,from)));

        }

        private void Endday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
