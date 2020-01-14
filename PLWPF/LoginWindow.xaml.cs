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
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public Page p ;

        public LoginWindow(Page goTo)
        {
            p = goTo;
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Please Enter Admin Password", "This Page is Private", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            bool flag = true;
            if (usernameBox.Text != "Estiburack" && usernameBox.Text != "estiburack" && usernameBox.Text != "Soriteigman" && usernameBox.Text != "soriteigman")
            {
                flag = false;

            }
            if (flag&&passwordBox.Password.ToString()!="7777")
            {
                flag = false;
            }
            if(!flag)
            {
               MessageBoxResult myResult= MessageBox.Show("Incorrect username or password", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
               if(myResult==MessageBoxResult.OK)
                {
                    usernameBox.Clear();
                    passwordBox.Clear();
                }
                if (myResult == MessageBoxResult.Cancel)
                    Close();
            }
            if (flag)
            {
                p.NavigationService.Navigate(new Admin());

                Close();

            }


        }
    }
}
