using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class Dal_imp : IDal
    {
        #region Singleton
        private static Dal_imp instance;

        public static Dal_imp Instance
        {
            get
            {
                if (instance == null)
                    instance = new Dal_imp();
                return instance;
            }
        }

        private Dal_imp() { }
        #endregion

        //----------------------------------------------------------------------------------------------
        #region GuestRequest
        public IEnumerable<BE.GuestRequest> GetAllRequests(Func<GuestRequest, bool> predicate = null)
        {
            IEnumerable<GuestRequest> LHu = ListOfCustomers();
            if (predicate == null)
                return LHu.Select(t => t.Clone());
            return LHu.Where(predicate).Select(t => t.Clone());
        }
        public void AddGuestRequest(GuestRequest gr)
        {
            try
            {
                gr.GuestRequestKey = Configuration.GuestRequest_s++;
                if (GRexist(gr.GuestRequestKey))
                    throw new DuplicateWaitObjectException("request already exists");
                else DataSource.requestCollection.Add(gr.Clone());
            }
            catch (DuplicateWaitObjectException a)
            {
                throw a;
            }
        }

        public void UpdateGuestRequest(GuestRequest gr)
        {
            try
            {
                GuestRequest request;
                if (GRexist(gr.GuestRequestKey))
                {
                    request = searchGRbyID(gr.GuestRequestKey).Clone();
                    DataSource.requestCollection.Remove(request);
                }
                DataSource.requestCollection.Add(gr);
            }
            catch (KeyNotFoundException a)
            {
                throw a;//deal with this
            }
        }

        public GuestRequest searchGRbyID(int key)
        {
            var request = from req in DataSource.requestCollection
                          where req.GuestRequestKey == key
                          select req;
            return request.FirstOrDefault();
        }

        public bool GRexist(int key)
        {
            var request = from req in DataSource.requestCollection
                          where req.GuestRequestKey == key
                          select req;
            return request.FirstOrDefault() != null;
        }


        public IEnumerable<GuestRequest> ListOfCustomers()
        {
            return from guestRequest in DataSource.requestCollection
                   select guestRequest.Clone();
        }
        #endregion


        #region HostingUnit
        public IEnumerable<BE.HostingUnit> GetAllUnits(Func<BE.HostingUnit, bool> predicate = null)
        {
            IEnumerable<HostingUnit> LHu = ListOfHostingUnits();
            if (predicate == null)
                return LHu.Select(t => t.Clone());
            return LHu.Where(predicate).Select(t => t.Clone());
        }
        public void AddHostingUnit(HostingUnit hu)
        {
            try
            {
                hu.HostingUnitKey = Configuration.HostingUnitKey_s++;
                if (HUexist(hu.HostingUnitKey))
                    throw new DuplicateWaitObjectException("unit already exists");
                else DataSource.HostingUnitCollection.Add(hu);
            }
            catch (DuplicateWaitObjectException a)
            {
                throw a;
            }
        }

        public void RemoveHostingUnit(HostingUnit hu)
        {
            if (HUexist(hu.HostingUnitKey))
            {
                DataSource.HostingUnitCollection.RemoveAll(h => h.HostingUnitKey == hu.HostingUnitKey);//removes all occurances of unit with hu.key(will remove one unit only)
            }
            else throw new KeyNotFoundException("Property to remove doesn't exist");

        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            try
            {
                HostingUnit unit=new HostingUnit();
                if (HUexist(hu.HostingUnitKey))
                {
                    unit = SearchHUbyID(hu.HostingUnitKey);
                    DataSource.HostingUnitCollection.Remove(unit);
                }
                else
                {
                    throw new KeyNotFoundException("property to update does not exist");
                }
                DataSource.HostingUnitCollection.Add(hu);
            }
            catch (KeyNotFoundException a)
            {
                throw a;
            }
        }

        public IEnumerable<HostingUnit> ListOfHostingUnits()
        {
            return from hostingUnit in DataSource.HostingUnitCollection
                   select hostingUnit.Clone();
        }

        public HostingUnit SearchHUbyID(int key)
        {
            var units = from unit in DataSource.HostingUnitCollection
                        where unit.HostingUnitKey == key
                        select unit;
            return units.FirstOrDefault();
        }

        public bool HUexist(int key)
        {
            var units = from unit in DataSource.HostingUnitCollection
                        where unit.HostingUnitKey == key
                        select unit;
            return units.FirstOrDefault() != null;
        }

        #endregion


        #region Order

        public void AddOrder(Order o)
        {
            try
            {
                if (ORexist(o.OrderKey))
                    throw new DuplicateWaitObjectException("order already exists");
                else DataSource.OrderCollection.Add(o.Clone());
            }
            catch (DuplicateWaitObjectException a)
            {
                throw a;
            }
        }

        public void UpdateOrder(Order o) //status update
        {
            try
            {
                Order ord;
                if (ORexist(o.OrderKey))
                {
                    ord = SearchOrbyID(o.OrderKey);//.Clone();//cant do it with clone because order isnt icomparible
                    DataSource.OrderCollection.RemoveAll(or=>or.OrderKey==ord.OrderKey);
                }
                DataSource.OrderCollection.Add(o);
            }
            catch (KeyNotFoundException a)
            {
                throw a;//deal with this
            }
        }

        public IEnumerable<Order> ListOfOrders()
        {
            IEnumerable<Order> o = DataSource.OrderCollection.Select(item => (Order)item.Clone()).ToList();
            return o;
        }
        public IEnumerable<BE.Order> GetAllOrders(Func<BE.Order, bool> predicate = null)
        {
            IEnumerable<Order> ord = ListOfOrders();
            if (predicate == null)
                return ord.AsEnumerable().Select(t => t.Clone());
            return ord.Where(predicate).Select(t => t.Clone());
        }
        public Order SearchOrbyID(int key)
        {
            var orders = from order in DataSource.OrderCollection
                        where order.OrderKey == key
                        select order;
            return orders.FirstOrDefault();
        }
        public bool ORexist(int key)
        {
            var orders = from order in DataSource.OrderCollection
                         where order.OrderKey == key
                         select order;
            return orders.FirstOrDefault() != null;
        }

        #endregion


        #region Bank
        public IEnumerable<BankBranch> ListOfBanks()
        {
            return from bankAccount in DataSource.BankAccountCollection
                   select bankAccount.Clone();
        }

        public IEnumerable<BankBranch> GetAllbanks(Func<BankBranch, bool> condition = null)
        {
            throw new NotImplementedException();
        }


        #endregion

        public void UpdateConfig(string original, DateTime dt)
        {
            throw new NotImplementedException();
        }
    }
}
