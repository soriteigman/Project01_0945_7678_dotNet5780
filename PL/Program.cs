using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BE;

namespace PL
{
    public class Program
    {
        static void Main(string[] args)
        {
            IBL ibl = FactoryBL.getBL();
            GuestRequest gr = new GuestRequest
            {
                //GuestRequestKey = Configuration.GuestRequest_s++,
                PrivateName = "wanted",
                FamilyName = "dead or alive",
                RegistrationDate = Configuration.today,
                EntryDate = new DateTime(2019, 12, 31),
                ReleaseDate = new DateTime(2020, 01, 05),
                MailAddress = "stburack@gmail.com",
                Type = VacationType.Hotel,
                Adults = 2,
                Children = 3,
                Pet = false,
                Area = VacationArea.East,
                SubArea = VacationSubArea.Netanya,
                ChildrensAttractions = Choices.Yes,
                FitnessCenter = Choices.DontCare,
                Garden = Choices.DontCare,
                Jacuzzi = Choices.Yes,
                Parking = Choices.Yes,
                Pool = Choices.Yes,
                Stars = StarRating.four_star,
                Status = Status.Active,
                WiFi = Choices.Yes
            };
            GuestRequest gr1 = new GuestRequest
            {
                //GuestRequestKey = Configuration.GuestRequest_s++,
                PrivateName = "esti",
                FamilyName = "burack",
                RegistrationDate = Configuration.today,
                EntryDate = new DateTime(2019, 12, 31),
                ReleaseDate = new DateTime(2020, 01, 05),
                MailAddress = "stburack@gmail.com",
                Type = VacationType.Hotel,
                Adults = 2,
                Children = 3,
                Pet = false,
                Area = VacationArea.East,
                SubArea = VacationSubArea.BatYam,
                ChildrensAttractions = Choices.Yes,
                FitnessCenter = Choices.DontCare,
                Garden = Choices.DontCare,
                Jacuzzi = Choices.Yes,
                Parking = Choices.Yes,
                Pool = Choices.Yes,
                Stars = StarRating.four_star,
                Status = Status.Active,
                WiFi = Choices.Yes
            };

            HostingUnit hu = new HostingUnit
            {
                //HostingUnitKey = Configuration.HostingUnitKey_s++,
                HostingUnitName = "Fanta Sea",
                Owner = new Host
                {
                    BankAccountNumber = 12345,
                    BankBranchDetails = new BankBranch
                    {
                        BankName = "pag",
                        BankNumber = 12345,
                        BranchAddress = "gda",
                        BranchCity = "hhu",
                        BranchNumber = 2345
                    },
                    CollectionClearance = false,
                    FamilyName = "hjfyjf",
                    HostKey = 54257570,
                    MailAddress = "mail@mail.com",
                    PhoneNumber = 8765432,
                    PrivateName = "hello"
                },
                Pet = false,
                Area = VacationArea.East,
                SubArea = VacationSubArea.Netanya,
                ChildrensAttractions = true,
                FitnessCenter = false,
                Garden = true,
                Jacuzzi = true,
                Parking = true,
                Pool = true,
                Stars = StarRating.four_star,
                WiFi = true,
                Beds = 5,
                Type = VacationType.Hotel
            };
            try
            {
                ibl.addreq(gr);
                ibl.addreq(gr1);
                ibl.AddHostingUnit(hu);
                ibl.UpdateHostingUnit(hu);

            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }

            Host esti = new Host
            {
                BankAccountNumber = 12345,
                BankBranchDetails = new BankBranch
                {
                    BankName = "pag",
                    BankNumber = 12345,
                    BranchAddress = "gda",
                    BranchCity = "hhu",
                    BranchNumber = 2345
                },
                CollectionClearance = false,
                FamilyName = "hjfyjf",
                HostKey = 54257570,
                MailAddress = "mail@mail.com",
                PhoneNumber = 8765432,
                PrivateName = "hello"
            };
           
            IEnumerable<GuestRequest> myRequests = ibl.AllRequestsThatMatch(ibl.BuildPredicate(hu));
            foreach(GuestRequest item in myRequests)
            {
                ibl.AddOrder(ibl.CreateOrder(hu.HostingUnitKey, item.GuestRequestKey));
            }


        }
    }
}
