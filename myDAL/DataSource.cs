using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    class DataSource
    {
        static BankBranch B1 = new BankBranch() { BankNumber = 123, BankName = "hapoalim", BranchNumber = 290, BranchAddress = "beitar", BranchCity = "Tzfat" };
        static BankBranch B2 = new BankBranch() { BankNumber = 456, BankName = "pagi", BranchNumber = 291, BranchAddress = "kanfei nesharim", BranchCity = "Beit Shemesh" };
        static BankBranch B3 = new BankBranch() { BankNumber = 789, BankName = "leumi", BranchNumber = 292, BranchAddress = "har nof", BranchCity = "Tel Aviv" };
        static BankBranch B4 = new BankBranch() { BankNumber = 101, BankName = "mizrahi", BranchNumber = 293, BranchAddress = "beit hadfus", BranchCity = "Jerusalem" };
        static BankBranch B5 = new BankBranch() { BankNumber = 202, BankName = "yahav", BranchNumber = 294, BranchAddress = "arieli", BranchCity="Beitar"};

        internal static List<BankBranch> BankAccountCollection = new List<BankBranch>() { B1, B2, B3, B4, B5 };

        internal static List<GuestRequest> requestCollection = new List<GuestRequest>()
        {
  new GuestRequest
            {
                PrivateName = "wanted",
                FamilyName = "dead or alive",
                RegistrationDate = Configuration.today,
                EntryDate = new DateTime(2020, 7, 03),
                ReleaseDate = new DateTime(2020, 7, 07),
                MailAddress = "stburack@gmail.com",
                Type = VacationType.Hotel,
                Adults = 2,
                Children = 3,
                Pet = false,
                Area = VacationArea.Center,
                SubArea = "beitar",
                ChildrensAttractions = Choices.Yes,
                FitnessCenter = Choices.DontCare,
                Garden = Choices.DontCare,
                Jacuzzi = Choices.Yes,
                Parking = Choices.Yes,
                Pool = Choices.Yes,
                Stars = StarRating.four_star,
                Status = Status.Active,
                WiFi = Choices.Yes
            },
        new GuestRequest
        {
            PrivateName = "wanted",FamilyName = "dead or alive",RegistrationDate = Configuration.today,
            EntryDate = new DateTime(2020, 8, 03),
            ReleaseDate = new DateTime(2020, 8, 05),
            MailAddress = "stburack@gmail.com",
            Type = VacationType.Hotel,
            Adults = 2,
            Children = 3,
            Pet = false,
            Area = VacationArea.South,
            SubArea = "Herzliya",
            ChildrensAttractions = Choices.Yes,
            FitnessCenter = Choices.DontCare,
            Garden = Choices.DontCare,
            Jacuzzi = Choices.Yes,
            Parking = Choices.Yes,
            Pool = Choices.Yes,
            Stars = StarRating.four_star,
            Status = Status.Active,
            WiFi = Choices.Yes
        },
        new GuestRequest
        {
            PrivateName = "sori",
            FamilyName = "teigman",
            RegistrationDate = Configuration.today,
            EntryDate = new DateTime(2020, 9, 05),
            ReleaseDate = new DateTime(2020, 9, 10),
            MailAddress = "soriteigman@gmail.com",
            Type = VacationType.BeachHouse,
            Adults = 2,
            Children = 1,
            Pet = false,
            Area = VacationArea.Jerusalem,
            SubArea = "Tiberias",
            ChildrensAttractions = Choices.No,
            FitnessCenter = Choices.Yes,
            Garden = Choices.No,
            Jacuzzi = Choices.Yes,
            Parking = Choices.Yes,
            Pool = Choices.Yes,
            Stars = StarRating.five_star,
            WiFi = Choices.Yes
        },
        new GuestRequest(){PrivateName="Esther", FamilyName="burack", MailAddress="stburack@gmail.com",Status=Status.NotAddressedYet,RegistrationDate=new DateTime(2020, 09, 09), EntryDate=new DateTime(2020, 09, 25), ReleaseDate=new DateTime(2020, 09, 28), Area=VacationArea.Center, SubArea="BatYam", Type=VacationType.Hut, Adults=2, Children=3, Pool=Choices.Yes, Jacuzzi=Choices.No, Garden=Choices.DontCare, ChildrensAttractions=Choices.Yes, FitnessCenter=Choices.DontCare, Parking=Choices.Yes, GuestRequestKey=12345678, Pet=false, Stars=StarRating.four_star, WiFi=Choices.Yes},
            new GuestRequest(){PrivateName="Mother", FamilyName="Burack", MailAddress="stburack@gmail.com", Status=Status.Active, RegistrationDate=new DateTime(2020, 08, 01), EntryDate=new DateTime(2020, 02, 02), ReleaseDate=new DateTime(2020, 02, 08), Area=VacationArea.North, SubArea="Tiberias", Adults=2, Children=7, ChildrensAttractions=Choices.No, FitnessCenter=Choices.No, Garden=Choices.Yes, GuestRequestKey=20000000, Jacuzzi=Choices.Yes, Parking=Choices.Yes, Pet=false, Pool=Choices.Yes, Stars=StarRating.four_star, Type=VacationType.Hotel, WiFi=Choices.DontCare},
            new GuestRequest(){PrivateName="Aviva", FamilyName="Nam", MailAddress="Man@gmail.com",Status=Status.Closed,RegistrationDate=new DateTime(2019, 08, 10), EntryDate=new DateTime(2019, 08, 29), ReleaseDate=new DateTime(2019, 09, 03), Area=VacationArea.DeadSea, SubArea="Arad", Type=VacationType.LogCabin, Adults=4, Children=17, Pool=Choices.No, Jacuzzi=Choices.Yes, Garden=Choices.Yes, ChildrensAttractions=Choices.DontCare, WiFi=Choices.No, Stars=StarRating.five_star, FitnessCenter=Choices.Yes, GuestRequestKey=12365479, Parking=Choices.Yes, Pet=true},
            new GuestRequest(){PrivateName="Sara", FamilyName="Teig", MailAddress="Teig@gmail.com",Status=Status.Active,RegistrationDate=new DateTime(2020, 03, 16), EntryDate=new DateTime(2020, 04, 01), ReleaseDate=new DateTime(2020, 04, 04), Area=VacationArea.South, SubArea="Netanya", Type=VacationType.Hotel, Adults=1, Children=8, Pool=Choices.Yes, Jacuzzi=Choices.Yes, Garden=Choices.No, ChildrensAttractions=Choices.Yes, Pet=false, Parking=Choices.DontCare, GuestRequestKey=98765432, FitnessCenter=Choices.DontCare, Stars=StarRating.dontCare, WiFi=Choices.Yes }
        };

        internal static List<Host> My_Host = new List<Host>
        {
           new Host
            {
                BankAccountNumber = 12345,
                BankBranchDetails = new BankBranch
                {
                    BankName = "leumi",
                    BankNumber = 5432,
                    BranchAddress = "arlozorov",
                    BranchCity = "telaviv",
                    BranchNumber = 455
                },
                CollectionClearance = false,
                FamilyName = "stark",
                HostKey = 315320945,
                MailAddress = "stark@gmail.com",
                PhoneNumber = 0583215877,
                PrivateName = "goodbye"
            },
           new Host
            {
                BankAccountNumber = 12345,
                BankBranchDetails = new BankBranch
                {
                    BankName = "leumi",
                    BankNumber = 5432,
                    BranchAddress = "arlozorov",
                    BranchCity = "telaviv",
                    BranchNumber = 455
                },
                CollectionClearance = true,
                FamilyName = "stark",
                HostKey = 315320945,
                MailAddress = "stark@gmail.com",
                PhoneNumber = 0583215877,
                PrivateName = "goodbye"
            },
           new Host
        {
            BankAccountNumber = 12345,
            BankBranchDetails = new BankBranch
            {
                BankName = "pagi",
                BankNumber = 12345,
                BranchAddress = "arielli",
                BranchCity = "beitar",
                BranchNumber = 992
            },
            CollectionClearance = true,
            FamilyName = "levi",
            HostKey = 337757678,
            MailAddress = "levi@gmail.com",
            PhoneNumber = 0537208407,
            PrivateName = "tom"
        }
    };

        internal static List<HostingUnit> HostingUnitCollection = new List<HostingUnit>()
        {
            new HostingUnit(){HostingUnitKey=123456789, HostingUnitName="Esther", Owner=My_Host.First(), WiFi=true, Stars=StarRating.three_star, FitnessCenter=false, Area=VacationArea.Jerusalem, Beds=6, ChildrensAttractions=true, Garden=true,Jacuzzi=true, Parking=true, Pet=false, SubArea="Jerusalem", Pool=true, Type=VacationType.Hotel},
            new HostingUnit(){HostingUnitKey=87654321, HostingUnitName="Sara", Owner=My_Host[1], WiFi=true, Stars=StarRating.five_star, FitnessCenter=true, Area=VacationArea.North, Beds=5, ChildrensAttractions=true, Garden=true, Jacuzzi=true, Parking=true, Pet=true, SubArea="Netanya", Pool=true, Type=VacationType.BeachHouse},
            new HostingUnit(){HostingUnitKey=18272635, HostingUnitName="Fanta Sea", Owner=My_Host.Last(), WiFi=false, Stars=StarRating.three_star, FitnessCenter=false, Area=VacationArea.Center, Beds=9, ChildrensAttractions=false, Garden=false, Jacuzzi=false, Parking=false, SubArea="TelAviv", Pool=false, Type=VacationType.Tent, Pet=true},
            new HostingUnit(){HostingUnitKey=20000000, HostingUnitName="thisone", Owner=My_Host[1], WiFi=false, Area=VacationArea.North, SubArea="Tiberias",Beds=9, ChildrensAttractions=false, FitnessCenter=false, Garden=true, Jacuzzi=true, Parking=true, Pet=false, Pool=true, Stars=StarRating.four_star, Type=VacationType.Hotel}

        };

        internal static List<Order> OrderCollection = new List<Order>()
        {
            new Order(){HostingUnitKey=20000000, GuestRequestKey=20000000, OrderKey=20000000, Status=Status.Active, CreateDate=new DateTime(2000, 09, 09), SentEmail=default(DateTime)},
            new Order(){HostingUnitKey=12345678,GuestRequestKey=12345678,OrderKey=12345678,Status=Status.Active, CreateDate=new DateTime(1963, 10, 04), SentEmail=default(DateTime)},
            new Order(){HostingUnitKey=98735478,GuestRequestKey=63749587,OrderKey=54637382,Status=Status.Closed, CreateDate=new DateTime(2000, 04, 16),  SentEmail=new DateTime(2019, 04, 25)},
            new Order(){HostingUnitKey=65748493,GuestRequestKey=65740912,OrderKey=65019468,Status=Status.NoAnswer, CreateDate=new DateTime(2019, 11, 11),  SentEmail=default(DateTime)}
        };
    }
}
