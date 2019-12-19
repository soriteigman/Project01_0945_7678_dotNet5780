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
        static BankAccount B1 = new BankAccount() { BankNumber = 123, BankName = "hapoalim", BranchNumber = 290, BranchAddress = "beitar", BankAccountNumber = 123456 };
        static BankAccount B2 = new BankAccount() { BankNumber = 456, BankName = "pagi", BranchNumber = 291, BranchAddress = "kanfei nesharim", BankAccountNumber = 78910 };
        static BankAccount B3 = new BankAccount() { BankNumber = 789, BankName = "leumi", BranchNumber = 292, BranchAddress = "har nof", BankAccountNumber = 111213 };
        static BankAccount B4 = new BankAccount() { BankNumber = 101, BankName = "mizrahi", BranchNumber = 293, BranchAddress = "beit hadfus", BankAccountNumber = 141516 };
        static BankAccount B5 = new BankAccount() { BankNumber = 202, BankName = "yahav", BranchNumber = 294, BranchAddress = "arieli", BankAccountNumber = 171819 };

        internal static List<BankAccount> BankAccountCollection = new List<BankAccount>() { B1, B2, B3, B4, B5 };

        internal static List<GuestRequest> requestCollection = new List<GuestRequest>()
        {
            new GuestRequest(){PrivateName="Esther", FamilyName="burack", MailAddress="stburack@gmail.com",Status=Status.NotAddressedYet,RegistrationDate=new DateTime(2020, 09, 09), EntryDate=new DateTime(2020, 09, 25), ReleaseDate=new DateTime(2020, 09, 28), Area=VacationArea.Center, SubArea=VacationSubArea.BatYam, Type=VacationType.Hut, Adults=2, Children=3, Pool=Choices.Yes, Jacuzzi=Choices.No, Garden=Choices.DontCare, ChildrensAttractions=Choices.Yes },
            new GuestRequest(){PrivateName="Aviva", FamilyName="Nam", MailAddress="Man@gmail.com",Status=Status.Closed,RegistrationDate=new DateTime(2019, 08, 10), EntryDate=new DateTime(2019, 08, 29), ReleaseDate=new DateTime(2019, 09, 03), Area=VacationArea.East, SubArea=VacationSubArea.Arad, Type=VacationType.LogCabin, Adults=4, Children=17, Pool=Choices.No, Jacuzzi=Choices.Yes, Garden=Choices.Yes, ChildrensAttractions=Choices.DontCare },
            new GuestRequest(){PrivateName="Sara", FamilyName="Teig", MailAddress="Teig@gmail.com",Status=Status.Active,RegistrationDate=new DateTime(2020, 03, 16), EntryDate=new DateTime(2020, 04, 01), ReleaseDate=new DateTime(2020, 04, 04), Area=VacationArea.South, SubArea=VacationSubArea.Netanya, Type=VacationType.Hotel, Adults=1, Children=8, Pool=Choices.Yes, Jacuzzi=Choices.Yes, Garden=Choices.No, ChildrensAttractions=Choices.Yes }
        };

        internal static List<Host> My_Host = new List<Host>
        {
            new Host(){HostKey=12345678, PrivateName="Sori", FamilyName="Teigman", PhoneNumber=0548475920, MailAddress="soriteigman@gmail.com", CollectionClearance=true, BankAccount=B1 },
            new Host(){HostKey=13679829, PrivateName="Esti", FamilyName="Burack", PhoneNumber=0537208407, MailAddress="stburack@gmail.com", CollectionClearance=false, BankAccount=B2 },
            new Host(){HostKey=16785678, PrivateName="Ruth", FamilyName="Miller", PhoneNumber=0583295002, MailAddress="ruthmiller@gmail.com", CollectionClearance=true, BankAccount=B3 },
        };

        internal static List<HostingUnit> HostingUnitCollection = new List<HostingUnit>()
        {
            new HostingUnit(){HostingUnitKey=12345678, HostingUnitName="Esther", Owner=My_Host.First()},
            new HostingUnit(){HostingUnitKey=87654321, HostingUnitName="Sara", Owner=My_Host.First()},
            new HostingUnit(){HostingUnitKey=18272635, HostingUnitName="Elisheva", Owner=My_Host.Last()}

        };

        internal static List<Order> OrderCollection = new List<Order>()
        {
            new Order(){HostingUnitKey=12345678,GuestRequestKey=12345678,OrderKey=12345678,Status=Status.Active, CreateDate=new DateTime(1963, 10, 04), OrderDate=new DateTime(1963, 10, 10)},
            new Order(){HostingUnitKey=98735478,GuestRequestKey=63749587,OrderKey=54637382,Status=Status.Closed, CreateDate=new DateTime(2000, 04, 16), OrderDate=new DateTime(2000, 05, 01)},
            new Order(){HostingUnitKey=65748493,GuestRequestKey=65740912,OrderKey=65019468,Status=Status.NoAnswer, CreateDate=new DateTime(2019, 11, 11), OrderDate=new DateTime(2019, 11, 11)}
        };
    }
}
