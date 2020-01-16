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
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        IBL _bl = BL.FactoryBL.getBL();//creates an instance of bl
        public IEnumerable<HostingUnit> units { get; set; }
        int id;
        public Orders(int ID)
        {
            id = ID;
            InitializeComponent();
            this.ordegrid.updateButton.Content = "craete order";
            units = _bl.searchHUbyOwner(id);
            IEnumerable<GuestRequest> req = null;
            List<int> keys = null;
            foreach (HostingUnit hu in units)
            {
                else keys.Add(hu.HostingUnitKey);
                if(_bl.AllRequestsThatMatch(_bl.BuildPredicate(hu)).Count()>0)
                    req.Concat(_bl.AllRequestsThatMatch(_bl.BuildPredicate(hu)));
            }

        }



    }
}
