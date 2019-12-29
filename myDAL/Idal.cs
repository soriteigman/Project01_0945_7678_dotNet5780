using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{

    public interface IDal
    {

        #region GuestRequest
        void AddGuestRequest(GuestRequest gr);
        void UpdateGuestRequest(GuestRequest gr);
        IEnumerable<GuestRequest> ListOfCustomers();
        GuestRequest searchGRbyID(int key);
        bool GRexist(int key);

        #endregion


        #region HostingUnit
        void AddHostingUnit(HostingUnit hu);

        void RemoveHostingUnit(HostingUnit hu);

        void UpdateHostingUnit(HostingUnit hu);

        IEnumerable<HostingUnit> ListOfHostingUnits();
        HostingUnit SearchHUbyID(int key);
        bool HUexist(int key);


        #endregion


        #region Order
        void AddOrder(Order o);

        void UpdateOrder(Order o); //status update

        IEnumerable<Order> ListOfOrders();

        Order SearchOrbyID(int key);
        bool ORexist(int key);


        #endregion


        #region Bank
        IEnumerable<BankBranch> ListOfBanks();

        #endregion


    }
}
