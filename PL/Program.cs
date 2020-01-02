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
            Creator us = new Creator
            {
                Username = "st",
                Password = "vip123",
                TotalCommission = 0
            };
            Host sori = new Host
            {
                BankAccountNumber = 12345,
                BankBranchDetails = new BankBranch
                {
                    BankName = "pagi",
                    BankNumber = 12345,
                    BranchAddress = "kanfei",
                    BranchCity = "harnof",
                    BranchNumber = 455
                },
                CollectionClearance = true,
                FamilyName = "teigman",
                HostKey = 54257570,
                MailAddress = "soriteigman@gmail.com",
                PhoneNumber = 0583215876,
                PrivateName = "sori"

            };
            Host esti = new Host
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
                FamilyName = "burack",
                HostKey = 315320967,
                MailAddress = "stburack@gmail.com",
                PhoneNumber = 0537208407,
                PrivateName = "esther"
            };
            IBL ibl = FactoryBL.getBL();
            GuestRequest gr = new GuestRequest
            {
                //GuestRequestKey = Configuration.GuestRequest_s++,
                PrivateName = "wanted",
                FamilyName = "dead or alive",
                RegistrationDate = Configuration.today,
                EntryDate = new DateTime(2020, 01, 03),
                ReleaseDate = new DateTime(2020, 01, 07),
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
                PrivateName = "wanted",
                FamilyName = "dead or alive",
                RegistrationDate = Configuration.today,
                EntryDate = new DateTime(2020, 01, 03),
                ReleaseDate = new DateTime(2020, 01, 05),
                MailAddress = "stburack@gmail.com",
                Type = VacationType.Hotel,
                Adults = 2,
                Children = 3,
                Pet = false,
                Area = VacationArea.East,
                SubArea = VacationSubArea.Herzliya,
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
            GuestRequest gr2 = new GuestRequest
            {
                PrivateName = "sori",
                FamilyName = "teigman",
                RegistrationDate = Configuration.today,
                EntryDate = new DateTime(2020, 01, 05),
                ReleaseDate = new DateTime(2020, 01, 10),
                MailAddress = "soriteigman@gmail.com",
                Type = VacationType.BeachHouse,
                Adults = 2,
                Children = 1,
                Pet = false,
                Area = VacationArea.West,
                SubArea = VacationSubArea.Tiberias,
                ChildrensAttractions = Choices.No,
                FitnessCenter = Choices.Yes,
                Garden = Choices.No,
                Jacuzzi = Choices.Yes,
                Parking = Choices.Yes,
                Pool = Choices.Yes,
                Stars = StarRating.five_star,
                WiFi = Choices.Yes
            };


            HostingUnit hu = new HostingUnit
            {
                HostingUnitName = "sleep",
                Owner = esti,
                Pet = false,
                Area = VacationArea.West,
                SubArea = VacationSubArea.Tiberias,
                ChildrensAttractions = false,
                FitnessCenter = true,
                Garden = false,
                Jacuzzi = true,
                Parking = true,
                Pool = true,
                Stars = StarRating.five_star,
                WiFi = true,
                Beds = 3,
                Type = VacationType.BeachHouse
            };
            HostingUnit hu1 = new HostingUnit
            {
                //HostingUnitKey = Configuration.HostingUnitKey_s++,
                HostingUnitName = "Fanta Sea",
                Owner = sori,
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
            HostingUnit hu2 = new HostingUnit
            {
                //HostingUnitKey = Configuration.HostingUnitKey_s++,
                HostingUnitName = "Fixed",

                Owner = sori,
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
                ibl.addreq(gr2);
                ibl.AddHostingUnit(hu);
                //hu.HostingUnitName = "fix";
                //ibl.UpdateHostingUnit(hu);
                ibl.AddHostingUnit(hu1);
                ibl.AddHostingUnit(hu2);

            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }

            IEnumerable<GuestRequest> myRequests;
            IEnumerable<IGrouping<Host, HostingUnit>> myUnits = ibl.GroupHUByHosts();
            foreach (IGrouping<Host, HostingUnit> h in myUnits)
            {
                foreach (HostingUnit hUnit in h)
                {
                    myRequests = ibl.AllRequestsThatMatch(ibl.BuildPredicate(hUnit));
                    foreach (GuestRequest item in myRequests)
                    {
                        try
                        {
                            ibl.AddOrder(ibl.CreateOrder(hUnit.HostingUnitKey, item.GuestRequestKey));
                        }
                        catch (Exception A)
                        {
                            Console.WriteLine(A.Message);
                        }
                    }
                }
            }
            IEnumerable<Order> orders = ibl.GetsOpenOrders();
            ibl.ChangeRequestStatus(orders.Last());
            IEnumerable<Order> orders2 = ibl.GetsOpenOrders();
            Console.WriteLine("\n");
            foreach (Order item in orders2)
            {
                Console.WriteLine(item);
                Console.WriteLine("\n");
            }

            ibl.TotalCommissionCalculator(us);
            Console.WriteLine(us.TotalCommission);
            //ibl.GroupByNumOfUnits();
        }
    }
}
