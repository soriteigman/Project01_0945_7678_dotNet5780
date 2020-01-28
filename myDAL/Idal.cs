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
        IEnumerable<GuestRequest> GetAllRequests(Func<GuestRequest, bool> predicate = null);

        void AddGuestRequest(GuestRequest gr);
        void UpdateGuestRequest(GuestRequest gr);
        IEnumerable<GuestRequest> ListOfCustomers();
        GuestRequest searchGRbyID(int key);
        bool GRexist(int key);

        #endregion


        #region HostingUnit
        IEnumerable<HostingUnit> GetAllUnits(Func<HostingUnit, bool> predicate = null);

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

        IEnumerable<Order> GetAllOrders(Func<Order, bool> predicate = null);

        #endregion


        #region Bank
        IEnumerable<BankBranch> ListOfBanks();
        IEnumerable<BankBranch> GetAllbanks(Func<BankBranch, bool> condition = null);//gets all requests that fit the condition

        #endregion
        //void UpdateConfig(DateTime dt);

    }
}
