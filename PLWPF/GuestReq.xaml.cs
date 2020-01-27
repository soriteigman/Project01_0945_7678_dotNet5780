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
            //if((int.Parse(numadult.Text)==0)/* && int.Parse(numadult.Text)==0)*/)
            //{
            //    Erroradult.Visibility = Visibility.Visible;
            //    ErrorKids.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    Erroradult.Visibility = Visibility.Hidden;
            //    ErrorKids.Visibility = Visibility.Hidden;
            //}
            if (PName.Text=="")
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

            if (numkid.Text== null)
            {
                flag = false;
                ErrorKids.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorKids.Visibility = Visibility.Hidden;
            }

            if (numadult.Text == null)
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
            if (endday.SelectedDate != null && startday.SelectedDate != null)//if both dates were selected, make sure they are valid
            {
                if(flag)//if there is no a problem in the input yet, then check
                {
                    flag = _bl.DateOK(startday.SelectedDate.Value.Date, endday.SelectedDate.Value.Date);
                    if(!flag)//if dates arent valid
                    {
                        Errordates.Visibility = Visibility.Visible;
                        Errordates.Content="Please select valid dates";
                    }
                    else Errordates.Visibility = Visibility.Hidden;
                }
            }

            if (endday.SelectedDate == null||startday.SelectedDate==null)//if didnt select both dates
            {
                flag = false;
                Errordates.Visibility = Visibility.Visible;
                Errordates.Content = "Please select dates";
            }
            else Errordates.Visibility = Visibility.Hidden;
            if(flag)
            {
                GuestRequest gr = new GuestRequest();
                gr.MailAddress = Emailtb.Text;
                gr.PrivateName = PName.Text;
                gr.FamilyName = Lname.Text;
                gr.Area = (VacationArea)Enum.Parse(typeof(VacationArea), areacb.SelectedValue.ToString(), true);
                gr.SubArea = subareacb.SelectedValue.ToString();
                gr.Children = Convert.ToInt32(numkid.Text);
                gr.Adults = Convert.ToInt32(numadult.Text);
                gr.Type = (VacationType)Enum.Parse(typeof(VacationType), type.SelectedValue.ToString(), true);
                gr.Stars = (StarRating)Enum.Parse(typeof(StarRating), starcb.SelectedValue.ToString(), true);
                gr.EntryDate= startday.SelectedDate.Value.Date;
                gr.ReleaseDate = endday.SelectedDate.Value.Date;
                gr.RegistrationDate = DateTime.Today;
                gr.Pet = (bool)pet.IsChecked;

                if(((bool)Nwifi.IsChecked && (bool)ywifi.IsChecked)|| !((bool)Nwifi.IsChecked)&& (!(bool)ywifi.IsChecked))
                    gr.WiFi = Choices.DontCare;
                else if ((bool)Nwifi.IsChecked)
                    gr.WiFi = Choices.No;
                else if ((bool)ywifi.IsChecked)
                    gr.WiFi = Choices.Yes;

                if (((bool)Nparking.IsChecked && (bool)yparking.IsChecked) || !((bool)Nparking.IsChecked) && (!(bool)yparking.IsChecked))
                    gr.Parking = Choices.DontCare;
                else if ((bool)Nparking.IsChecked)
                    gr.Parking = Choices.No;
                else if ((bool)yparking.IsChecked)
                    gr.Parking = Choices.Yes;

                if (((bool)Npool.IsChecked && (bool)ypool.IsChecked) || (!(bool)Npool.IsChecked) && (!(bool)ypool.IsChecked))
                    gr.Pool = Choices.DontCare;
                else if ((bool)Npool.IsChecked)
                    gr.Pool = Choices.No;
                else if ((bool)ypool.IsChecked)
                    gr.Pool = Choices.Yes;

                if (((bool)Nfitness.IsChecked && (bool)yfitness.IsChecked) || (!(bool)Nfitness.IsChecked) && (!(bool)yfitness.IsChecked))
                    gr.FitnessCenter = Choices.DontCare;
                else if ((bool)Nfitness.IsChecked)
                    gr.FitnessCenter = Choices.No;
                else if ((bool)yfitness.IsChecked)
                    gr.FitnessCenter = Choices.Yes;

                if (((bool)Ngarden.IsChecked && (bool)ygarden.IsChecked) || !(bool)Ngarden.IsChecked && !(bool)ygarden.IsChecked)
                    gr.Garden = Choices.DontCare;
                else if ((bool)Ngarden.IsChecked)
                    gr.Garden = Choices.No;
                else if ((bool)ygarden.IsChecked)
                    gr.Garden = Choices.Yes;

                if (((bool)NChildatt.IsChecked && (bool)yChildatt.IsChecked) || !(bool)NChildatt.IsChecked && !(bool)yChildatt.IsChecked)
                    gr.ChildrensAttractions = Choices.DontCare;
                else if ((bool)NChildatt.IsChecked)
                    gr.ChildrensAttractions = Choices.No;
                else if ((bool)yChildatt.IsChecked)
                    gr.ChildrensAttractions = Choices.Yes;

                if (((bool)Njacuzzi.IsChecked && (bool)yjacuzzi.IsChecked) || !(bool)Njacuzzi.IsChecked && !(bool)yjacuzzi.IsChecked)
                    gr.Jacuzzi = Choices.DontCare;
                else if ((bool)Njacuzzi.IsChecked)
                    gr.Jacuzzi = Choices.No;
                else if ((bool)yjacuzzi.IsChecked)
                    gr.Jacuzzi = Choices.Yes;


                try
                {

                    _bl.addreq(gr);
                    MessageBox.Show("Hi "+gr.PrivateName+".\nThank You for your interest in our properties! Your request was succesfully added into our system. We will be in touch with you shortly. " , "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.NavigationService.Navigate(new MainPage());
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

                }
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

        private void lessadult_Click(object sender, RoutedEventArgs e)
        {
            adultTB.Text = (Convert.ToInt32(adultTB.Text) - 1).ToString();
            numadult.Text = adultTB.Text;
            if (adultTB.Text == "0")
                lessadult.IsEnabled = false;          
        }

        private void addadult_Click(object sender, RoutedEventArgs e)
        {
            adultTB.Text = (Convert.ToInt32(adultTB.Text) + 1).ToString();
            numadult.Text=adultTB.Text;
            if (adultTB.Text != "0")
                lessadult.IsEnabled = true;
        }

        private void numkid_DropDownClosed(object sender, EventArgs e)
        {
            numkid.Text = kidTB.Text;
        }

        private void numadult_DropDownClosed(object sender, EventArgs e)
        {
            numadult.Text = adultTB.Text;
        }

        private void lesskid_Click(object sender, RoutedEventArgs e)
        {
            kidTB.Text = (Convert.ToInt32(kidTB.Text) - 1).ToString();
            numkid.Text = kidTB.Text;
            if (kidTB.Text == "0")
                lesskid.IsEnabled = false;
        }

        private void addkid_Click(object sender, RoutedEventArgs e)
        {
            kidTB.Text = (Convert.ToInt32(kidTB.Text) + 1).ToString();
            numkid.Text = kidTB.Text;
            if (kidTB.Text != "0")
                lesskid.IsEnabled = true;
        }
    }
}
