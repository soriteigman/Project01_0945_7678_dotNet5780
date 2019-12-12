using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace myDAL
{

    public interface Idal
    {
        void AddGuestRequest(GuestRequest gr);

        bool UpdateGuestRequest(GuestRequest gr);


        bool AddHostingUnit(HostingUnit hu);

        bool RemoveHostingUnit(HostingUnit hu);

        bool UpdateHostingUnit(HostingUnit hu);


        bool AddOrder(Order o);

        bool UpdateOrder(Order o); //status update


        List<HostingUnit> ListOfHostingUnits();

        List<string> ListOfCustomers();

        List<Order> ListOfOrders();

        List<BankAccount> ListOfBanks(); 


    }
}
