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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddHostingUnit.xaml
    /// </summary>
    public partial class AddHostingUnit : Page
    {
        public AddHostingUnit()
        {
            InitializeComponent();
            //cbArea.DataContext = System.Enum.GetValues(typeof(BE.VacationArea));
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void fillCBArea_Click(object sender, RoutedEventArgs e)
        {
            cbArea.DataSource = Enum.GetValues(typeof(Weekdays));
            ComboBox c=new ComboBox
            {
                dat
            }
        }

        private void comBox_FilledWithEnumeration_SelectionChangeCommitted(object sender, EventArgs e)
        {
            lbl_EnumerationValue.Text = "Selected day is: " + comBox_FilledWithEnumeration.SelectedValue.ToString();
        }
    }
}
