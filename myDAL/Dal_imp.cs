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
            if(request[0])

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
            List<BankAccount> Bank = new List<BankAccount>
            {
                new BankAccount() {BankNumber=123,BankName="hapoalim" ,BranchNumber=290,BranchAddress="beitar",BankAccountNumber=123456},
                new BankAccount() {BankNumber=456,BankName="pagi" ,BranchNumber=291,BranchAddress="kanfei nesharim",BankAccountNumber=78910},
                new BankAccount() {BankNumber=789,BankName="leumi" ,BranchNumber=292,BranchAddress="har nof",BankAccountNumber=111213},
                new BankAccount() {BankNumber=101,BankName="mizrahi" ,BranchNumber=293,BranchAddress="beit hadfus",BankAccountNumber=141516},
                new BankAccount() {BankNumber=202,BankName="yahav" ,BranchNumber=294,BranchAddress="arieli",BankAccountNumber=171819}
            };
            return Bank;
        }

    }
}
