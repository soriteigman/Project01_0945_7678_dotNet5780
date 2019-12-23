using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{

    public interface Idal
    {
        void AddGuestRequest(GuestRequest gr);

        void UpdateGuestRequest(GuestRequest gr);


        void AddHostingUnit(HostingUnit hu);

        void RemoveHostingUnit(HostingUnit hu);

        void UpdateHostingUnit(HostingUnit hu);


        void AddOrder(Order o);

        void UpdateOrder(Order o); //status update


        IEnumerable<HostingUnit> ListOfHostingUnits();

        IEnumerable<GuestRequest> ListOfCustomers();

        IEnumerable<Order> ListOfOrders();

        IEnumerable<BankBranch> ListOfBanks(); 


    }
}
