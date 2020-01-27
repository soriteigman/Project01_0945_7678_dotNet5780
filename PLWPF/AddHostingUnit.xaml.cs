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
    /// Interaction logic for AddHostingUnit.xaml..
    /// </summary>
    public partial class AddHostingUnit : Page
    {
        int intRate = 0;
        int intCount = 1;
        int Rate = 0;
        IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl

        public AddHostingUnit()
        {
            InitializeComponent();

            LoadImages();
            lblRating.Text = intRate.ToString();

            this.cbArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            this.cbType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));

            #region banks
            this.BankInfoTabUserControl.AddButton.Visibility=Visibility.Hidden;
            this.BankInfoTabUserControl.RemoveButton.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.refreshButton.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.updateButton.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.ResetFiltersButton.Style = (Style)FindResource("RoundCorner");
            this.BankInfoTabUserControl.ResetFiltersButton.Content = "DONE";

            this.BankInfoTabUserControl.Lname.Content = "Search";
            this.BankInfoTabUserControl.Lkey.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.Lstar.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.Larea.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.Ltype.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.FilterArea.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.FilterKey.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.FilterStar.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.FilterType.Visibility = Visibility.Hidden;
            this.BankInfoTabUserControl.DataGrid.ItemsSource = _bl.ListOfBanks();


            //this.BankInfoTabUserControl.FilterName.TextChanged += ApplyFiltering;
            //this.BankInfoTabUserControl.FilterKey.SelectionChanged += ApplyFiltering;
            //this.BankInfoTabUserControl.FilterStar.SelectionChanged += ApplyFiltering;
            //this.BankInfoTabUserControl.FilterArea.SelectionChanged += ApplyFiltering;
            //this.BankInfoTabUserControl.FilterType.SelectionChanged += ApplyFiltering;
            //this.BankInfoTabUserControl.ResetFiltersButton.Click += ResetFilters;
            //this.BankInfoTabUserControl.DataGrid.SelectionChanged += ShowButtons;
            //this.BankInfoTabUserControl.AddButton.Click += addOrder;
            //this.BankInfoTabUserControl.updateButton.Click += updateHu;
            //this.BankInfoTabUserControl.RemoveButton.Click += remove_Click;

            //this.OrdersTabUserControl.DataGrid.MouseDoubleClick += DataGridRow_MouseDoubleClick;

            //this.OrdersTabUserControl.refreshButton.Visibility = Visibility.Hidden;

            //this.OrdersTabUserControl.FilterKey.ItemsSource = keys;
            //this.OrdersTabUserControl.FilterKey.SelectedItem = "{Binding Path=UnitKey,Mode=TwoWay}";

            //this.OrdersTabUserControl.FilterStar.ItemsSource = Enum.GetValues(typeof(BE.StarRating));
            //this.OrdersTabUserControl.FilterStar.SelectedItem = "{Binding Path=UnitStar,Mode=TwoWay}";

            //this.OrdersTabUserControl.FilterArea.ItemsSource = Enum.GetValues(typeof(BE.VacationArea));
            //this.OrdersTabUserControl.FilterArea.SelectedItem = "{Binding Path=UnitArea,Mode=TwoWay}";

            //this.OrdersTabUserControl.FilterType.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            //this.OrdersTabUserControl.FilterType.SelectedItem = "{Binding Path=UnitPath,Mode=TwoWay}";

            //this.OrdersTabUserControl.DataGrid.ItemsSource = units;
            //this.OrdersTabUserControl.DataGrid.DisplayMemberPath = "units";
            //this.OrdersTabUserControl.DataGrid.SelectedIndex = 0;
            //this.OrdersTabUserControl.DataGrid.AutoGeneratingColumn += WayOfView;
            //this.OrdersTabUserControl.DataGrid.UnselectAll();


            #endregion

        }


        #region rating
        private void LoadImages()
        {
            for (int i = 1; i <= 5; i++)
            {
                Image img = new Image();
                img.Name = "imgRate" + i;
                img.Stretch = Stretch.UniformToFill;
                img.Height = 25;
                img.Width = 25;
                img.Source = new BitmapImage(new Uri(@"\Images\MinusRate.png", UriKind.Relative));
                img.MouseEnter += imgRateMinus_MouseEnter;
                pnlMinus.Children.Add(img);

                Image img1 = new Image();
                img1.Name = "imgRate" + i + i;
                img1.Stretch = Stretch.UniformToFill;
                img1.Height = 25;
                img1.Width = 25;
                img1.Visibility = Visibility.Hidden;
                img1.Source = new BitmapImage(new Uri(@"\Images\PlusRate.png", UriKind.Relative));
                img1.MouseEnter += imgRatePlus_MouseEnter;
                img1.MouseLeave += imgRatePlus_MouseLeave;
                img1.MouseLeftButtonUp += imgRatePlus_MouseLeftButtonUp;
                pnlPlus.Children.Add(img1);
            }
        }

        private void imgRateMinus_MouseEnter(object sender, MouseEventArgs e)
        {
            GetData(sender, Visibility.Visible, Visibility.Hidden);
        }

        private void imgRatePlus_MouseEnter(object sender, MouseEventArgs e)
        {
            GetData(sender, Visibility.Visible, Visibility.Hidden);
        }

        private void imgRatePlus_MouseLeave(object sender, MouseEventArgs e)
        {
            GetData(sender, Visibility.Hidden, Visibility.Visible);
        }

        private void gdRating_MouseLeave(object sender, MouseEventArgs e)
        {
            SetImage(Rate, Visibility.Visible, Visibility.Hidden);
        }

        private void GetData(object sender, Visibility imgYellowVisibility, Visibility imgGrayVisibility)
        {
            GetRating(sender as Image);
            SetImage(intRate, imgYellowVisibility, imgGrayVisibility);
        }

        private void SetImage(int intRate, Visibility imgYellowVisibility, Visibility imgGrayVisibility)
        {
            foreach (Image imgItem in pnlPlus.Children.OfType<Image>())
            {
                if (intCount <= intRate)
                    imgItem.Visibility = imgYellowVisibility;
                else
                    imgItem.Visibility = imgGrayVisibility;
                intCount++;
            }
            intCount = 1;
        }

        private void imgRatePlus_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetRating(sender as Image);
            Rate = intRate;
            lblRating.Text = intRate.ToString();
            if (intRate > 0)
                errorStars.Visibility = Visibility.Hidden;
        }

        private void GetRating(Image Img)
        {
            string strImgName = Img.Name;
            intRate = Convert.ToInt32(strImgName.Substring(strImgName.Length - 1, 1));
        }

        #endregion


        private void id_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (ID.Text == "Enter Your ID")
                ID.Clear();
        }
        private void Fname_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Fname.Text == "Enter Your First Name")
                Fname.Clear();
        }
        private void Lname_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Lname.Text == "Enter Your Last Name")
                Lname.Clear();
        }
        private void Phonenum_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Phonenum.Text == "Enter Your Phone Number")
                Phonenum.Clear();
        }
        private void Email_PreviewMouseDown(object sender, MouseButtonEventArgs e)//cleaning the text box so you can write the ID 
        {
            if (Email.Text == "Enter Your Email Address")
                Email.Clear();
        }
      
        private void HUname_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (HUname.Text == "Give Your Property a Name")
                HUname.Clear();
        }

        private void BankAcctNum_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (BankAcctNum.Text == "Enter Your Bank Account Number")
                BankAcctNum.Clear();
        }

        private void Beds_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Beds.Text == "Amount of Beds Your Property has")
                Beds.Clear();
        }

        private void ID_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ID.Text == "")
                ID.Text = "Enter Your ID";
        }
        private void Fname_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Fname.Text == "")
                Fname.Text = "Enter Your First Name";
        }
        private void Lname_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Lname.Text == "")
                Lname.Text = "Enter Your Last Name";
        }

        private void Phonenum_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Phonenum.Text == "")
                Phonenum.Text = "Enter Your Phone Number";
        }

        private void Email_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Email.Text == "")
                Email.Text = "Enter Your Email Address";
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl
            HostingUnit hu = new HostingUnit();
            Host h = new Host();
            h.PrivateName = Fname.Text;
            h.FamilyName = Lname.Text;
            h.PhoneNumber = Convert.ToInt32(Phonenum.Text);
            h.MailAddress = Email.Text;

            //_bl.AddHostingUnit();
        }
        private void HUname_MouseLeave(object sender, MouseEventArgs e)
        {
            if (HUname.Text == "")
                HUname.Text = "Give Your Property a Name";
        }

        private void BankAcctNum_MouseLeave(object sender, MouseEventArgs e)
        {
            if (BankAcctNum.Text == "")
                BankAcctNum.Text = "Enter the City your Branch is Located in";
        }

        private void Beds_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Beds.Text == "")
                Beds.Text = "Amount of Beds Your Property has";
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = TC.SelectedIndex + 1;
            if (newIndex >= TC.Items.Count)
                newIndex = 0;
            TC.SelectedIndex = newIndex;
        }

        private void prev_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = TC.SelectedIndex - 1;
            if (newIndex < 0)
                newIndex = TC.Items.Count - 1;
            TC.SelectedIndex = newIndex;
        }

        private void done_Click(object sender, RoutedEventArgs e)//creates a hosting unit out of the owners info
        {
            IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl
            int temp;

            bool flag = true;
            bool email_flag = true;
            try
            {
                email_flag = _bl.IsValidEmail(Email.Text);
            }
            catch// (Exception a)
            {
                Email.Clear();
                Email.Text = "Enter Your Email Address";
                Email.BorderBrush = Brushes.Red;
                flag = false;
                email_flag = false;
            }

            if (ID.Text == "Enter Your ID")
            {
                flag = false;
                ID.BorderBrush = Brushes.Red;
            }
            else if(!Int32.TryParse(ID.Text,out temp))
            {
                flag = false;
                ID.BorderBrush = Brushes.Red;
            }

            if (Fname.Text == "Enter Your First Name")
            {
                flag = false;
                Fname.BorderBrush = Brushes.Red;
            }

            if (Lname.Text == "Enter Your Last Name")
            {
                flag = false;
                Lname.BorderBrush = Brushes.Red;
            }

            if (HUname.Text == "Give Your Property a Name")
            {
                flag = false;
                HUname.BorderBrush = Brushes.Red;
            }


            if (cbSubArea.SelectedValue == null)
            {
                flag = false;
                cbSubArea.Foreground = Brushes.Red;
            }


            if (cbArea.SelectedValue == null)
            {
                flag = false;
                cbArea.Foreground = Brushes.Red;
            }


            if (Beds.Text == "Amount of Beds Your Property has")
            {
                flag = false;
                Beds.BorderBrush = Brushes.Red;
            }
            else if (!Int32.TryParse(Beds.Text, out temp))
            {
                flag = false;
                Beds.BorderBrush = Brushes.Red;
            }

            if (Phonenum.Text == "Enter Your Phone Number")
            {
                flag = false;
                Phonenum.BorderBrush = Brushes.Red;
            }
            else if (!Int32.TryParse(Phonenum.Text, out temp))
            {
                flag = false;
                Phonenum.BorderBrush = Brushes.Red;
            }

            if (BankAcctNum.Text == "Enter Your Bank Account Number")
            {
                flag = false;
                BankAcctNum.BorderBrush = Brushes.Red;
            }
            else if (!Int32.TryParse(BankAcctNum.Text, out temp))
            {
                flag = false;
                BankAcctNum.BorderBrush = Brushes.Red;
            }

            if (cbType.SelectedValue == null)
            {
                flag = false;
                cbType.Foreground = Brushes.Red;
            }

            if (lblRating.Text.ToString() == "0")
            {
                flag = false;
                errorStars.Visibility = Visibility.Visible;
            }
            else
            {
                errorStars.Visibility = Visibility.Hidden;
            }


            if (flag)
            {
                HostingUnit hu = new HostingUnit();
                Host h = new Host();
                BankBranch b = new BankBranch
                {
                    //BankNumber = Convert.ToInt32(Banknum.Text),
                    //BankName = Bankname.Text,
                    //BranchNumber = Convert.ToInt32(Branchnum.Text),
                    //BranchAddress = Bankaddress.Text,
                    //BranchCity = Bankcity.Text
                };

                h.HostKey = Convert.ToInt32(ID.Text);
                h.PrivateName = Fname.Text;
                h.FamilyName = Lname.Text;
                h.PhoneNumber = Convert.ToInt32(Phonenum.Text);
                h.MailAddress = Email.Text;
                h.BankBranchDetails = b;
                h.CollectionClearance = (bool)CollectionClearance.IsChecked;
                h.BankAccountNumber = Convert.ToInt32(BankAcctNum.Text);

                hu.HostingUnitName = HUname.Text;
                hu.Owner = h;
                hu.Area = (VacationArea)Enum.Parse(typeof(VacationArea), cbArea.SelectedValue.ToString(), true);
                hu.SubArea = cbSubArea.SelectedValue.ToString();
                hu.Type = (VacationType)Enum.Parse(typeof(VacationType), cbType.SelectedValue.ToString(), true);
                hu.Beds = Convert.ToInt32(Beds.Text);
                hu.Pet = (bool)Pets.IsChecked;
                hu.WiFi = (bool)Wifi.IsChecked;
                hu.Parking = (bool)Parking.IsChecked;
                hu.Pool = (bool)Pool.IsChecked;
                hu.Jacuzzi = (bool)Jacuzzi.IsChecked;
                hu.Garden = (bool)Garden.IsChecked;
                hu.ChildrensAttractions = (bool)chiAttract.IsChecked;
                hu.FitnessCenter = (bool)FitnessCenter.IsChecked;
                hu.Stars = (StarRating)Enum.Parse(typeof(StarRating), lblRating.Text.ToString(), true);
                try
                {

                    _bl.AddHostingUnit(hu);
                    MessageBox.Show("Here is your new property key code. Please save for future reference.\n Your Key: " + hu.HostingUnitKey, "Important Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.NavigationService.Navigate(new MainPage());
                }
                catch(Exception a)
                {
                    MessageBox.Show(a.Message , "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("Missing important information, please fill everything out to continue.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void cbArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbSubArea.Visibility = Visibility.Visible;
            string val = cbArea.SelectedValue.ToString();
            cbArea.Foreground = Brushes.Black;

            if (val == "DeadSea")
            {
                cbSubArea.SelectedItem = "{Binding Path=DeadSea,Mode=TwoWay}";
                this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.DeadSea));
            }
            else if (val == "Eilat")
            {
                cbSubArea.SelectedItem = "{Binding Path=Eilat,Mode=TwoWay}";
                this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.Eilat));
            }
            else if (val == "Jerusalem")
            {
                cbSubArea.SelectedItem = "{Binding Path=Jerusalem,Mode=TwoWay}";
                this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.Jerusalem));
            }
            else if (val == "North")
            {
                cbSubArea.SelectedItem = "{Binding Path=North,Mode=TwoWay}";
                this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.North));
            }
            else if (val == "South")
            {
                cbSubArea.SelectedItem = "{Binding Path=South,Mode=TwoWay}";
                this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.South));
            }
            else if (val == "Center")
            {
                cbSubArea.SelectedItem = "{Binding Path=Center,Mode=TwoWay}";
                this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.Center));
            }
            else
            {
                cbSubArea.SelectedItem = "{Binding Path=All,Mode=TwoWay}";
                this.cbSubArea.ItemsSource = Enum.GetValues(typeof(BE.All));
            }

        }

        private void Fname_TextChanged(object sender, TextChangedEventArgs e)
        {
            Fname.BorderBrush = Brushes.Black;
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            Email.BorderBrush = Brushes.Black;

        }
        
        private void ID_TextChanged(object sender, TextChangedEventArgs e)
        {
            ID.BorderBrush = Brushes.Black;
        }

        private void Lname_TextChanged(object sender, TextChangedEventArgs e)
        {
            Lname.BorderBrush = Brushes.Black;
        }

        private void Phonenum_TextChanged(object sender, TextChangedEventArgs e)
        {
            Phonenum.BorderBrush = Brushes.Black;
        }

        private void BankAcctNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            BankAcctNum.BorderBrush = Brushes.Black;
        }

        private void Beds_TextChanged(object sender, TextChangedEventArgs e)
        {
            Beds.BorderBrush = Brushes.Black;
        }

       

        private void HUname_TextChanged(object sender, TextChangedEventArgs e)
        {
            HUname.BorderBrush = Brushes.Black;

        }

        private void cbArea_DropDownOpened(object sender, EventArgs e)
        {
            cbArea.Foreground = Brushes.Black;

        }

        private void cbType_DropDownOpened(object sender, EventArgs e)
        {
            cbType.Foreground = Brushes.Black;
        }

        private void cbSubArea_DropDownOpened(object sender, EventArgs e)
        {
            cbSubArea.Foreground = Brushes.Black;
        }

        #region banks

        private void ShowButtons(object sender, SelectionChangedEventArgs e)
        {
            if (this.BankInfoTabUserControl.DataGrid.CurrentItem != null)//something was selected
            {
                this.BankInfoTabUserControl.ResetFiltersButton.IsEnabled = true;//allows button clicks
            }
            else
            {
                this.BankInfoTabUserControl.ResetFiltersButton.IsEnabled = false;//otherwise disables them
            }
        }

        #endregion
    }
}
