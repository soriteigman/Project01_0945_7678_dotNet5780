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
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (usernameBox.Password.ToString() != "Estiburack" || usernameBox.Password.ToString() != "estiburack" || usernameBox.Password.ToString() != "Soriteigman" || usernameBox.Password.ToString() != "soriteigman")
                flag = false;


        }
    }
}
