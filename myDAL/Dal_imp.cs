using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
   public class Dal_imp:Idal
   {
        //Singleton
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

        

        public void AddGuestRequest(GuestRequest gr)
        {
            try
            {
                if (!DataSource.requestCollection.Any(g => g.GuestRequestKey == gr.GuestRequestKey))
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


                var request = from guest in DataSource.requestCollection
                              where guest.GuestRequestKey == gr.GuestRequestKey
                              select guest;

                if (request != null)
                {
                    DataSource.requestCollection.Remove(request.First());
                }
                else throw new KeyNotFoundException("request to update doesn't exist");
            }
            catch (KeyNotFoundException a)
            {
                throw a;//deal with this
            }
        }

        public void AddHostingUnit(HostingUnit hu)
        {
            try
            {
                if (!DataSource.HostingUnitCollection.Any(h => h.HostingUnitKey == hu.HostingUnitKey))
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
            var Unit = from unit in DataSource.HostingUnitCollection
                          where unit.HostingUnitKey == hu.HostingUnitKey
                          select unit;

            if (Unit != null)
                DataSource.HostingUnitCollection.Remove(hu);
           
            else throw new KeyNotFoundException("request to remove doesn't exist");

        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            try
            {


                var Unit = from unit in DataSource.HostingUnitCollection
                              where unit.HostingUnitKey == hu.HostingUnitKey
                              select unit;

                if (Unit != null)
                {
                    DataSource.HostingUnitCollection.Remove(Unit.First());
                }
                else throw new KeyNotFoundException("HostingUnit to update doesn't exist");
            }
            catch (KeyNotFoundException a)
            {
                throw a;//deal with this
            }
        }


        public void AddOrder(Order o)
        {
            try
            {

                if (!DataSource.OrderCollection.Any(or => or.OrderKey == o.OrderKey))
                    throw new DuplicateWaitObjectException("order already exists");
                else DataSource.OrderCollection.Add(o);
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


                var Ord = from ord in DataSource.OrderCollection
                              where ord.OrderKey == o.OrderKey
                              select ord;

                if (Ord != null)
                {
                    DataSource.OrderCollection.Remove(Ord.First());
                }
                else throw new KeyNotFoundException("Order to update doesn't exist");
            }
            catch (KeyNotFoundException a)
            {
                throw a;//deal with this
            }
        }


        public IEnumerable <HostingUnit> ListOfHostingUnits()
        {
            return from hostingUnit in DataSource.HostingUnitCollection
                   select hostingUnit.Clone();

        }
 

        public IEnumerable<GuestRequest> ListOfCustomers()
        {
            return from guestRequest in DataSource.requestCollection
                   select guestRequest.Clone();
        }

        public IEnumerable<Order> ListOfOrders()
        {
            return from order in DataSource.OrderCollection
                   select order.Clone();
        }

        public IEnumerable<BankBranch> ListOfBanks()
        {
            return from bankAccount in DataSource.BankAccountCollection
                   select bankAccount.Clone();
        }

    }
}
