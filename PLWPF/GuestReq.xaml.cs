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
        IBL _bl = BL.FactoryBL.getBL();//creates an instance of dal

        public GuestReq()
        {
            InitializeComponent();
            this.type.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            this.areacb.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.numadult.ItemsSource = Enum.GetValues(typeof(BE.num));
            this.numkid.ItemsSource = Enum.GetValues(typeof(BE.num));
            this.starcb.ItemsSource = Enum.GetValues(typeof(BE.StarRating));



            startday.BlackoutDates.AddDatesInPast();
            startday.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddMonths(11), DateTime.Now.AddYears(100)));
            endday.BlackoutDates.AddDatesInPast();

        }
        private void Startday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            endday.Visibility = Visibility.Visible;
            DateTime from = startday.SelectedDate.Value.Date;
            if(endday.SelectedDate != null)
                endday.SelectedDate = null;
            endday.BlackoutDates.Clear();
            endday.BlackoutDates.AddDatesInPast();
            DateTime today = DateTime.Now;
            endday.BlackoutDates.Add((new CalendarDateRange(today, from)));

        }

        private void Endday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (endday.SelectedDate != null)
            {
                DateTime from = startday.SelectedDate.Value.Date;
                DateTime to = endday.SelectedDate.Value.Date;
                int numdays = (to - from).Days;
                numofday.Content = numdays + "-night stay";
            }
        }

        private void Areacb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            subareacb.Visibility = Visibility.Visible;
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
            bool flag = true;
            bool email_flag = true;
            try
            {
                email_flag=_bl.IsValidEmail(Emailtb.Text);
            }
            catch(Exception a)
            {
                Emailtb.Clear();
                ErrorEmail.Visibility = Visibility.Visible;
                flag = false;
                email_flag = false;
            }

            if (Emailtb.Text=="")
            {
                Emailtb.Clear();
                ErrorEmail.Visibility = Visibility.Visible;
                flag = false;
            }
            else
            {
                ErrorEmail.Visibility = Visibility.Hidden;
            }

            if(PName.Text=="")
            {
                flag = false;
                Errorpname.Visibility = Visibility.Visible;
            }
            else
            {
                Errorpname.Visibility = Visibility.Hidden;

            }

            if (Lname.Text == "")
            {
                flag = false;
                Errorlname.Visibility = Visibility.Visible;
            }
            else
            {
                Errorlname.Visibility = Visibility.Hidden;
            }

            if(subareacb.SelectedValue==null)
            {
                flag = false;
                Errorsubarea.Visibility = Visibility.Visible;
            }
            else
            {
                Errorsubarea.Visibility = Visibility.Hidden;
            }

            if (areacb.SelectedValue == null)
            {
                flag = false;
                Errorarea.Visibility = Visibility.Visible;
            }
            else
            {
                Errorarea.Visibility = Visibility.Hidden;
            }

            if (numkid.SelectedValue == null)
            {
                flag = false;
                ErrorKids.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorKids.Visibility = Visibility.Hidden;
            }

            if (numadult.SelectedValue == null)
            {
                flag = false;
                Erroradult.Visibility = Visibility.Visible;
            }
            else
            {
                Erroradult.Visibility = Visibility.Hidden;
            }

            if (type.SelectedValue == null)
            {
                flag = false;
                Errortype.Visibility = Visibility.Visible;
            }
            else
            {
                Errortype.Visibility = Visibility.Hidden;
            }

            if (starcb.SelectedValue == null)
            {
                flag = false;
                Errorstar.Visibility = Visibility.Visible;
            }
            else
            {
                Errorstar.Visibility = Visibility.Hidden;
            }

            if (endday.SelectedDate == null||startday.SelectedDate==null)
            {
                flag = false;
                Errordates.Visibility = Visibility.Visible;
            }
            else
            {
                Errordates.Visibility = Visibility.Hidden;
            }
            if(flag)
            {
                GuestRequest gr = new GuestRequest();
                gr.MailAddress = Emailtb.Text;
                gr.PrivateName = PName.Text;
                gr.FamilyName = Lname.Text;
                gr.Area = (VacationArea)Enum.Parse(typeof(VacationArea), areacb.SelectedValue.ToString(), true);
                gr.SubArea = subareacb.SelectedValue.ToString();
                gr.Children = (int)numkid.SelectedValue;
                gr.Adults = (int)numadult.SelectedValue;
                gr.Type = (VacationType)Enum.Parse(typeof(VacationType), type.SelectedValue.ToString(), true);
                gr.Stars = (StarRating)Enum.Parse(typeof(StarRating), starcb.SelectedValue.ToString(), true);
                gr.EntryDate= startday.SelectedDate.Value.Date;
                gr.ReleaseDate = endday.SelectedDate.Value.Date;
                gr.RegistrationDate = DateTime.Today;
                gr.Pet = (bool)pet.IsChecked;

                if(((bool)Nwifi.IsChecked && (bool)ywifi.IsChecked)|| !(bool)Nwifi.IsChecked&& !(bool)ywifi.IsChecked)
                    gr.WiFi = Choices.DontCare;
                else if ((bool)Nwifi.IsChecked)
                    gr.WiFi = Choices.No;
                else if ((bool)ywifi.IsChecked)
                    gr.WiFi = Choices.Yes;

                if (((bool)Nparking.IsChecked && (bool)yparking.IsChecked) || !(bool)Nparking.IsChecked && !(bool)yparking.IsChecked)
                    gr.Parking = Choices.DontCare;
                else if ((bool)Nwifi.IsChecked)
                    gr.Parking = Choices.No;
                else if ((bool)ywifi.IsChecked)
                    gr.Parking = Choices.Yes;

                if (((bool)Npool.IsChecked && (bool)ypool.IsChecked) || !(bool)Npool.IsChecked && !(bool)ypool.IsChecked)
                    gr.Pool = Choices.DontCare;
                else if ((bool)Npool.IsChecked)
                    gr.Pool = Choices.No;
                else if ((bool)ypool.IsChecked)
                    gr.Pool = Choices.Yes;

                if (((bool)Nfitness.IsChecked && (bool)yfitness.IsChecked) || !(bool)Nfitness.IsChecked && !(bool)yfitness.IsChecked)
                    gr.FitnessCenter = Choices.DontCare;
                else if ((bool)Njacuzzi.IsChecked)
                    gr.FitnessCenter = Choices.No;
                else if ((bool)yjacuzzi.IsChecked)
                    gr.FitnessCenter = Choices.Yes;

                if (((bool)Ngarden.IsChecked && (bool)ygarden.IsChecked) || !(bool)Ngarden.IsChecked && !(bool)ygarden.IsChecked)
                    gr.Garden = Choices.DontCare;
                else if ((bool)Njacuzzi.IsChecked)
                    gr.Garden = Choices.No;
                else if ((bool)yjacuzzi.IsChecked)
                    gr.Garden = Choices.Yes;

                if (((bool)NChildatt.IsChecked && (bool)yChildatt.IsChecked) || !(bool)NChildatt.IsChecked && !(bool)yChildatt.IsChecked)
                    gr.ChildrensAttractions = Choices.DontCare;
                else if ((bool)Njacuzzi.IsChecked)
                    gr.ChildrensAttractions = Choices.No;
                else if ((bool)yjacuzzi.IsChecked)
                    gr.ChildrensAttractions = Choices.Yes;

                if (((bool)Njacuzzi.IsChecked && (bool)yjacuzzi.IsChecked) || !(bool)Njacuzzi.IsChecked && !(bool)yjacuzzi.IsChecked)
                    gr.Jacuzzi = Choices.DontCare;
                else if ((bool)Njacuzzi.IsChecked)
                    gr.Jacuzzi = Choices.No;
                else if ((bool)yjacuzzi.IsChecked)
                    gr.Jacuzzi = Choices.Yes;


                _bl.addreq(gr);
            }
            

        }

        private void Emailtb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!_bl.IsValidEmail(Emailtb.Text))
            {
                Emailtb.Clear();
                ErrorEmail.Visibility = Visibility.Visible;
            }
            if(_bl.IsValidEmail(Emailtb.Text))
            {
                ErrorEmail.Visibility = Visibility.Hidden;
            }
        }
    }
}
