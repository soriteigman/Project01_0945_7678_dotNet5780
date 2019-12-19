using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace myDAL
{
    class DataSource
    {
        internal static List<GuestRequest> requestCollection = new List<GuestRequest>()
        {

        };
        static List<Host> My_Host = new List<Host>
        {
            new Host(){HostKey=12345678, PrivateName="Sori", FamilyName="Teigman", PhoneNumber=0548475920, MailAddress="soriteigman@gmail.com", CollectionClearance=true },
            new Host(){HostKey=13679829, PrivateName="Esti", FamilyName="Burack", PhoneNumber=0537208407, MailAddress="stburack@gmail.com", CollectionClearance=true },
            new Host(){HostKey=16785678, PrivateName="Ruth", FamilyName="Miller", PhoneNumber=0583295002, MailAddress="ruthmiller@gmail.com", CollectionClearance=true },
        };

        internal static List<HostingUnit> HostingUnitCollection = new List<HostingUnit>()
        {
            new HostingUnit(){HostingUnitKey=12345678, HostingUnitName="Esther", Owner=My_Host.First()},
            new HostingUnit(){HostingUnitKey=87654321, HostingUnitName="Sara", Owner=My_Host.First()},
            new HostingUnit(){HostingUnitKey=18272635, HostingUnitName="Elisheva", Owner=My_Host.Last()}

        };

        internal static List<Order> OrderCollection = new List<Order>()
        {
            new Order(){HostingUnitKey=12345678,GuestRequestKey=12345678,OrderKey=12345678,Status=BE.Status.Active, CreateDate=new DateTime(1963, 10, 04), OrderDate=new DateTime(1963, 10, 10)},
            new Order(){HostingUnitKey=98735478,GuestRequestKey=63749587,OrderKey=54637382,Status=BE.Status.Closed, CreateDate=new DateTime(2000, 04, 16), OrderDate=new DateTime(2000, 05, 01)},
            new Order(){HostingUnitKey=65748493,GuestRequestKey=65740912,OrderKey=65019468,Status=BE.Status.NoAnswer, CreateDate=new DateTime(2019, 11, 11), OrderDate=new DateTime(2019, 11, 11)}
        };


        

        List<BankAccount> Bank = new List<BankAccount>
            {
                new BankAccount() {BankNumber=123,BankName="hapoalim" ,BranchNumber=290,BranchAddress="beitar",BankAccountNumber=123456},
                new BankAccount() {BankNumber=456,BankName="pagi" ,BranchNumber=291,BranchAddress="kanfei nesharim",BankAccountNumber=78910},
                new BankAccount() {BankNumber=789,BankName="leumi" ,BranchNumber=292,BranchAddress="har nof",BankAccountNumber=111213},
                new BankAccount() {BankNumber=101,BankName="mizrahi" ,BranchNumber=293,BranchAddress="beit hadfus",BankAccountNumber=141516},
                new BankAccount() {BankNumber=202,BankName="yahav" ,BranchNumber=294,BranchAddress="arieli",BankAccountNumber=171819}
            };
    }
}
