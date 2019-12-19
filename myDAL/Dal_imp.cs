using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace myDAL
{
   public class Dal_imp:Idal
    {
        public void AddGuestRequest(GuestRequest gr)
        {
            if (!DataSource.requestCollection.Any(g=>g.GuestRequestKey == gr.GuestRequestKey))
                //throw new DuplicateIdException("request", gr.GuestRequestKey);
           else DataSource.requestCollection.Add(gr);
        }
        public GuestRequest this[int index]
        {
            get
            {
                return DataSource.requestCollection[index];
            }
            set
            {
                DataSource.requestCollection[index] = value;
            }
        }


        public bool UpdateGuestRequest(GuestRequest gr)
        {
            var request = from guest in DataSource.requestCollection
                          where guest.GuestRequestKey == gr.GuestRequestKey
                          select guest;
            if (request[0])
                return false;
            return false;

        }


        public bool AddHostingUnit(HostingUnit hu)
        {
            return false;

        }

        public bool RemoveHostingUnit(HostingUnit hu)
        {
            return false;

        }

        public bool UpdateHostingUnit(HostingUnit hu)
        {
            return false;

        }


        public bool AddOrder(Order o)
        {
            return false;

        }

        public bool UpdateOrder(Order o) //status update
        {
            return false;

        }


        public List<HostingUnit> ListOfHostingUnits()
        {
            return null;

        }

        public List<string> ListOfCustomers()
        {
            return null;

        }

        public List<Order> ListOfOrders()
        {

            return null;

        }

        public List<BankAccount> ListOfBanks()
        {
           
            return Bank;
        }

    }
}
