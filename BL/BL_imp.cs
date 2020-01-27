using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using System.Net.Mail;
using System.Threading;

namespace BL
{

    public class BL_imp : IBL
    {
        IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
        #region Singleton
        private static BL_imp instance;

        public static BL_imp Instance
        {
            get
            {
                if (instance == null)
                    instance = new BL_imp();
                return instance;
            }
        }

        private BL_imp() { }
        #endregion

        //----------------------------------------------------------------------------------------------

        #region grouping
        public IEnumerable<IGrouping<VacationArea, GuestRequest>> Group_GR_ByArea()//groups the requests by area of choice
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<GuestRequest> requests = dal_bl.ListOfCustomers();//gets the list of requests
            IEnumerable<IGrouping<VacationArea, GuestRequest>> result = from req in requests
                                                                        group req by req.Area into r1
                                                                        select r1;

            return result;
        }
        public IEnumerable<IGrouping<int, GuestRequest>> GroupByNumOfGuests()//groups by number of guests
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<GuestRequest> requests = dal_bl.ListOfCustomers();//gets the list of requests
            IEnumerable<IGrouping<int, GuestRequest>> result = from req in requests
                                                               group req by (req.Adults + req.Children) into r1
                                                               select r1;

            return result;
        }
        public IEnumerable<IGrouping<int, HostingUnit>> GroupHUByHosts()
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<HostingUnit> units = dal_bl.ListOfHostingUnits();//gets the list of units

            var result = from unit in units
                         group unit by unit.Owner.HostKey into g1
                         select g1;
            return result;
        }
        public IEnumerable<IGrouping<int, Host>> GroupByNumOfUnits()//groups by number of hosting units the hosts own
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<HostingUnit> units = dal_bl.ListOfHostingUnits();//gets the list of requests

            var result = from unit in units
                         group unit by unit.Owner into g1
                         select g1;
            IEnumerable<IGrouping<int, Host>> result1 = from re in result
                                                        group re.Key by re.Count();

            return result1;
        }
        public IEnumerable<IGrouping<VacationArea, HostingUnit>> Group_HU_ByArea()//groups the units by area of choice
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<HostingUnit> units = dal_bl.ListOfHostingUnits();//gets the list of requests
            IEnumerable<IGrouping<VacationArea, HostingUnit>> result = from ho in units
                                                                       group ho by ho.Area into r1
                                                                       select r1;

            return result;
        }
        #endregion

        //----------------------------------------------------------------------------------------------

        #region hosting units
        public HostingUnit SearchHUbyID(int key)
        {
            return dal_bl.SearchHUbyID(key);
        }
        public bool HExists(int id)
        {
            IEnumerable<HostingUnit> units = dal_bl.ListOfHostingUnits();//gets the list of guest units

            var result = from unit in units
                         where unit.Owner.HostKey == id
                         select unit;
            if (result.Count() == 0)
                return false;
            return true;// if there is at least one unit with that host returns true
        }
        public IEnumerable<HostingUnit> listHU()
        {
            return dal_bl.ListOfHostingUnits();//returns the list of orders
        }
        public IEnumerable<HostingUnit> searchHUbyOwner(int key)//filters from all orders based on parameters recieved
        {
            IEnumerable<HostingUnit> hostingUnits = dal_bl.ListOfHostingUnits();
            Func<HostingUnit, bool> condition = null;//conditions to filter with
            IEnumerable<HostingUnit> huToReturn = null;//list of filtered units

            condition = hu => hu.Owner.HostKey == key;
            huToReturn = from hu in hostingUnits
                         let p = condition(hu)
                         where p
                         select hu;
            return huToReturn.ToList();
        }
        public IEnumerable<HostingUnit> GetAllUnits(string searchString, object key, StarRating? star, VacationArea? area, VacationType? type)
        {
            return dal_bl.GetAllUnits(t =>
                (t.HostingUnitName.Contains(searchString)
                && (key == null || key.ToString() == t.HostingUnitKey.ToString())

                && (star == null || star == t.Stars)

                && (area == null || area == t.Area)

                && (type == null || t.Type == type)));
        }
        public void UpdateHostingUnit(HostingUnit hu)
        {
            try
            {
                dal_bl.UpdateHostingUnit(hu);
            }
            catch (KeyNotFoundException a)
            {
                throw a;
            }
        }
        public void AddHostingUnit(HostingUnit hu)
        {
            try
            {
                dal_bl.AddHostingUnit(hu);
            }
            catch (DuplicateWaitObjectException a)
            {
                throw a;
            }

        }
        public void RemoveHostingUnit(HostingUnit hu)
        {
            try
            {
                if (!RemoveUnitCheck(hu))
                    throw new InvalidOperationException("cannot delete this hosting unit, there are active orders connected to it");

                dal_bl.RemoveHostingUnit(hu);
            }
            catch (InvalidOperationException a)
            {
                throw a;
            }
        }
        public HostingUnit SearchHUbyID_bl(int key)//finds hosting unit by its key
        {
            HostingUnit hu = new HostingUnit();
            hu = dal_bl.SearchHUbyID(key);

            return hu;
        }
        public bool RemoveUnitCheck(HostingUnit hu)//checks to see if there are any active reservations for that unit before removing it
        {
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of guest requests

            if (orders.Any(x => x.HostingUnitKey == hu.HostingUnitKey && x.Status != Status.Closed))//checks if any orders connected to the hosting unit are not closed
                return false;
            return true;
        }
        public IEnumerable<HostingUnit> AvailableUnits(DateTime startDate, int numOfDays)//returns all available hosting units for the dates requested
        {
            DateTime end = startDate.AddDays(numOfDays);
            IEnumerable<HostingUnit> hostingUnits = dal_bl.ListOfHostingUnits();//gets the list of hosting units
            GuestRequest temp = new GuestRequest() { EntryDate = startDate, ReleaseDate = end };
            var request = from unit in hostingUnits //creates a list of all available units
                          where AvailabilityCheck(unit, temp)
                          select unit;//selects if available in given dates
            return request;
        }
        public int NumOfSent_HU_Orders(HostingUnit hu, Predicate<Order> conditions)//returns the number of orders that were sent email or booked for this hosting unit
        {
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders

            var result = from ord in orders //creates a list of all orders that fit the condition
                         let closed = ord.Status == Status.Booked
                         where conditions(ord) && ord.HostingUnitKey == hu.HostingUnitKey
                         select ord;

            return result.Count();
        }
        public void UpdateDiary(Order o)//after the status changes to closed, mark the days in the units diary
        {
            HostingUnit hu = dal_bl.SearchHUbyID(o.HostingUnitKey);
            GuestRequest gr = dal_bl.searchGRbyID(o.GuestRequestKey);
            DateTime start = gr.EntryDate;
            DateTime end = gr.ReleaseDate;

            while (start != end)//updates the dates in the diary
            {
                hu.Diary[start.Month - 1, start.Day - 1] = true;
                start = start.AddDays(1);
            }
            dal_bl.UpdateHostingUnit(hu);
        }

        #endregion

        //----------------------------------------------------------------------------------------------

        #region guest requests
        public IEnumerable<GuestRequest> GetAllReq(string searchString, StarRating? star, VacationArea? area, VacationType? type)
        {
            return dal_bl.GetAllRequests(t => ((t.FamilyName.Contains(searchString) || (t.PrivateName.Contains(searchString))
            || ((t.FamilyName + t.PrivateName).Contains(searchString)) || ((t.PrivateName + t.FamilyName).Contains(searchString)))
                && (star == null || star == t.Stars)

                && (area == null || area == t.Area)

                && (type == null || t.Type == type)));
        }
        public void addreq(GuestRequest gr)
        {
            try
            {
                if (!DateOK(gr.EntryDate, gr.ReleaseDate))
                {
                    throw new ArgumentException("Dates not ok");
                }
                dal_bl.AddGuestRequest(gr);
            }
            catch (ArgumentException a)
            {
                throw a;
            }
        }
        public void Updategr(GuestRequest gr)
        {
            try
            {
                GuestRequest g = dal_bl.searchGRbyID(gr.GuestRequestKey);
                if (g.Status == Status.Closed)
                    throw new InvalidOperationException("cannot change status of closed request");
                dal_bl.UpdateGuestRequest(gr);
            }
            catch (InvalidOperationException a)
            {
                throw a;
            }
        }
        public void ChangeRequestStatus(Order o)//to change order status to booked, other orders connected are closed, also the request status is changed to booked
        {
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders
            GuestRequest gr = dal_bl.searchGRbyID(o.GuestRequestKey).Clone();

            o.Status = Status.Booked;
            UpdateOrder(o.Clone());
            gr.Status = Status.Booked;
            Updategr(gr.Clone());
            CalculateComission(gr.Clone());


            foreach (Order item in orders)
            {
                if (item.GuestRequestKey == gr.GuestRequestKey && item.OrderKey != o.OrderKey)
                {
                    item.Status = Status.Closed;
                    UpdateOrder(item);
                }
            }

            HostingUnit hu = dal_bl.SearchHUbyID(o.HostingUnitKey);
            Console.WriteLine(hu);


        }
        public IEnumerable<GuestRequest> AllRequestsThatMatch(Predicate<GuestRequest> conditions)//returns all requests that fullfill the conditions 
        {
            IEnumerable<GuestRequest> guestRequests = dal_bl.ListOfCustomers();//gets the list of requests

            List<GuestRequest> result = new List<GuestRequest>();

            bool temp = true;
            foreach (GuestRequest req in guestRequests)
            {
                foreach (Predicate<GuestRequest> item in conditions.GetInvocationList())
                {
                    if (!item(req))
                        temp = false;
                }
                if (temp)
                    result.Add(req);
                temp = true;

            }
            return (IEnumerable<GuestRequest>)result;
        }
        public IEnumerable<GuestRequest> listGR()
        {
            return dal_bl.ListOfCustomers();//returns the list of orders
        }
        public int NumOfSent_GR_Orders(GuestRequest gr)//returns the num of orders that were sent for that guest request
        {
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders

            var result = from ord in orders //creates a list of all orders that fit the condition
                         where ord.GuestRequestKey == gr.GuestRequestKey
                         select ord;

            return result.Count();
        }
        #endregion

        //----------------------------------------------------------------------------------------------

        #region order
        public IEnumerable<Order> GetAllOrders(BE.Status? status)
        {
            return dal_bl.GetAllOrders(t => (status == null || status == t.Status));
        }

        public void AddOrder(Order o)
        {
            try
            {
                Host h= dal_bl.SearchHUbyID(o.HostingUnitKey).Owner;
                if (!AvailabilityCheck(dal_bl.SearchHUbyID(o.HostingUnitKey), dal_bl.searchGRbyID(o.GuestRequestKey)))
                    throw new InvalidOperationException("unit not available for this request");
                if (!dal_bl.HUexist(o.HostingUnitKey))
                    throw new KeyNotFoundException("unit doesnt exist");
                if (!dal_bl.GRexist(o.GuestRequestKey))
                    throw new KeyNotFoundException("request doesnt exist");
                dal_bl.AddOrder(o.Clone());
                //o.Status = Status.SentEmail;
                //UpdateOrder(o.Clone());
                if (h.CollectionClearance)
                {
                    o.SentEmail = Configuration.today;
                    SendEmail(o.Clone());

                }
                else throw new InvalidOperationException("cannot send email without permission to charge");

            }
            catch (Exception a)
            {
                throw a;
            }
        }
        public void UpdateOrder(Order newO) //status update
        {
            try
            {
                Order oldO = dal_bl.SearchOrbyID(newO.OrderKey);
                if (oldO.Status == Status.Closed)
                    throw new InvalidOperationException("cannot change status that is already closed");
                Host h = dal_bl.SearchHUbyID(newO.HostingUnitKey).Owner;
                if (newO.Status == Status.Booked)
                {
                    GuestRequest g = dal_bl.searchGRbyID(newO.GuestRequestKey).Clone();
                    g.Status = Status.Closed;
                    Updategr(g.Clone());
                    UpdateDiary(newO.Clone());
                }
                //if (newO.Status == Status.SentEmail)
                //{
                //    if (h.CollectionClearance)
                //    {
                //        newO.SentEmail = Configuration.today;
                //        SendEmail(newO.Clone());

                //    }
                //    else throw new InvalidOperationException("cannot send email without permission to charge");
                //}
                dal_bl.UpdateOrder(newO.Clone());

            }
            catch (Exception a)
            {
                throw a;
            }
        }
        public Order CreateOrder(int HUkey, int GRkey)//in case of gr and hu matching creates an order for them
        {
            Order ord=new Order();
            try
            {
                if (!AvailabilityCheck(dal_bl.SearchHUbyID(HUkey), dal_bl.searchGRbyID(GRkey)))
                    throw new InvalidOperationException("unit not available for this request");
                ord = new Order
                {
                    GuestRequestKey = GRkey,
                    HostingUnitKey = HUkey,
                    CreateDate = Configuration.today,
                    Status = Status.SentEmail
                };
                return ord;
            }
            catch (InvalidProgramException e)
            {
                throw e;
            }
        }
        public IEnumerable<GuestRequest> DaysPassedOnReq(int numOfDays)//returns all gr that were sent a email/ created "numOfDays" ago
        {
            IEnumerable<GuestRequest> gr = dal_bl.ListOfCustomers();//gets the list of gr

            var createResult = from ger in gr //creates a list of all gr that need to be closed
                               where (ger.RegistrationDate.AddDays(numOfDays) < Configuration.today && ger.Status == Status.Active)
                               select ger;

            return createResult;

        }
        
        public IEnumerable<Order> DaysPassedOnOrders(int numOfDays)//returns all orders that were sent a email/ created "numOfDays" ago
        {
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders

            var createResult = from ord in orders //creates a list of all orders that need to be closed
                               where (ord.SentEmail.AddDays(numOfDays)<Configuration.today && ord.Status!=Status.Closed)
                               select ord;

            return createResult;

        }
        public IEnumerable<Order> GetsOpenOrders()
        {
            return dal_bl.ListOfOrders();//returns the list of orders
        }


        #endregion

        //----------------------------------------------------------------------------------------------

        #region calculation
        public bool DateOK(DateTime start, DateTime end)
        {
            try
            {
                DateTime today = Configuration.today;
                today = today.AddMonths(11);
                if (today < start)
                {
                    throw new ArgumentException("wrong date");
                }
                if (!DateLengthPermission(start, end))
                {
                    throw new ArgumentException("wrong date");
                }
                if (Configuration.today > start)
                {
                    throw new ArgumentException("wrong date");
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Predicate<GuestRequest> BuildPredicate(HostingUnit hu)//based on a hosting unit builds a predicate to filter all guest requests
        {
            IEnumerable<GuestRequest> guestRequests = dal_bl.ListOfCustomers();//gets the list of requests
            Predicate<GuestRequest> pred = default(Predicate<GuestRequest>);
            bool HasPool(GuestRequest gr) { return gr.Pool == Choices.Yes || gr.Pool == Choices.DontCare; }
            bool NoPool(GuestRequest gr) { return gr.Pool == Choices.No || gr.Pool == Choices.DontCare; }
            bool HasJacuzzi(GuestRequest gr) { return gr.Jacuzzi == Choices.Yes || gr.Jacuzzi == Choices.DontCare; }
            bool NoJacuzzi(GuestRequest gr) { return gr.Jacuzzi == Choices.No || gr.Jacuzzi == Choices.DontCare; }
            bool HasGarden(GuestRequest gr) { return gr.Garden == Choices.Yes || gr.Garden == Choices.DontCare; }
            bool NoGarden(GuestRequest gr) { return gr.Garden == Choices.No || gr.Garden == Choices.DontCare; }
            bool HasParking(GuestRequest gr) { return gr.Parking == Choices.Yes || gr.Parking == Choices.DontCare; }
            bool NoParking(GuestRequest gr) { return gr.Parking == Choices.No || gr.Parking == Choices.DontCare; }
            bool HasPet(GuestRequest gr) { return gr.Pet; }
            bool NoPet(GuestRequest gr) { return !gr.Pet; }
            bool HasWiFi(GuestRequest gr) { return gr.WiFi == Choices.Yes || gr.WiFi == Choices.DontCare; }
            bool NoWiFi(GuestRequest gr) { return gr.WiFi == Choices.No || gr.WiFi == Choices.DontCare; }
            bool HasChildrensAttractions(GuestRequest gr) { return gr.ChildrensAttractions == Choices.Yes || gr.ChildrensAttractions == Choices.DontCare; }
            bool NoChildrensAttractions(GuestRequest gr) { return gr.ChildrensAttractions == Choices.No || gr.ChildrensAttractions == Choices.DontCare; }
            bool HasFitnessCenter(GuestRequest gr) { return gr.FitnessCenter == Choices.Yes || gr.FitnessCenter == Choices.DontCare; }
            bool NoFitnessCenter(GuestRequest gr) { return gr.FitnessCenter == Choices.No || gr.FitnessCenter == Choices.DontCare; }

            bool VacaArea(GuestRequest gr) { return gr.Area == hu.Area; }
            bool VacaSubArea(GuestRequest gr) { return gr.SubArea == hu.SubArea; }
            bool VacaType(GuestRequest gr) { return gr.Pool == Choices.Yes || gr.Pool == Choices.DontCare; }
            bool NumBeds(GuestRequest gr) { return hu.Beds >= (gr.Children + gr.Adults); }
            bool StarRating(GuestRequest gr) { return gr.Stars <= hu.Stars; }
            bool Status(GuestRequest gr) { return gr.Status != BE.Status.Closed; }


            if (hu.Pool)
                pred += HasPool;
            else pred += NoPool;
            if (hu.Jacuzzi)
                pred += HasJacuzzi;
            else pred += NoJacuzzi;
            if (hu.Garden)
                pred += HasGarden;
            else pred += NoGarden;
            if (hu.Parking)
                pred += HasParking;
            else pred += NoParking;
            if (hu.Pet)
                pred += HasPet;
            else pred += NoPet;
            if (hu.WiFi)
                pred += HasWiFi;
            else pred += NoWiFi;
            if (hu.ChildrensAttractions)
                pred += HasChildrensAttractions;
            else pred += NoChildrensAttractions;
            if (hu.FitnessCenter)
                pred += HasFitnessCenter;
            else pred += NoFitnessCenter;

            pred += VacaArea;
            pred += VacaSubArea;
            pred += VacaType;
            pred += NumBeds;
            pred += StarRating;
            pred += Status;

            return pred;
        }
        public bool DateLengthPermission(DateTime start, DateTime end)//checks if stay is at least one full day long
        {
            if (end>start)//if theres at least one day difference between the start and end dates
                return true;
            return false;
        }
        public int TotalCommissionCalculator()//calculates total commission from all the bookings on the website (each time its called it calculates for now)
        {
            int TotalCommission = 0;
            IEnumerable<GuestRequest> requests = dal_bl.ListOfCustomers();//gets the list of requests
            foreach (GuestRequest req in requests)
            {
                if(req.Status==Status.Closed)
                    TotalCommission += CalculateComission(req);
            }
            return TotalCommission;
        }
        public bool AvailabilityCheck(HostingUnit hu, GuestRequest gr)//checks if requested dates are available
        {
            DateTime start = gr.EntryDate;
            DateTime end = gr.ReleaseDate.AddDays(-1);

            while (start != end)//checks availability for duration of the visit
            {
                if (hu.Diary[start.Month - 1, start.Day - 1])//returns false when any day isnt available
                    return false;
                start = start.AddDays(1);//next day
            }
            return true;
        }
        public int CalculateDurationOfStay(GuestRequest gr)//returns duration of stay
        {
            return (gr.ReleaseDate - gr.EntryDate).Days;
        }
        public int CalculateComission(GuestRequest gr)//calculates comission
        {
            int duration = CalculateDurationOfStay(gr);//calculates the duration of stay
            return duration * Configuration.commission;//returns TOTAL commission
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                throw new InvalidOperationException("invalid email address");
            }
        }
        public int NumOfDaysInBetweeen(DateTime startDate, DateTime endDate = default(DateTime))//remember if the end date is null change it to Configuration.today
        {
            if (endDate == default(DateTime))//if there was no end date given, use today as an end date
                endDate = Configuration.today;

            return (endDate - startDate).Days;
        }
        public void SendEmail(Order o)//sends email when order status changes to "sent mail"
        {
            GuestRequest gr = dal_bl.searchGRbyID(o.GuestRequestKey);
                Host h = dal_bl.SearchHUbyID(o.HostingUnitKey).Owner;

                try
                {
                    IsValidEmail(gr.MailAddress);
                    IsValidEmail(h.MailAddress);
                }
                catch (InvalidOperationException a)
                {
                    throw a;
                }
            new Thread(() =>
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(gr.MailAddress);
                mail.From = new MailAddress("stburack@gmail.com");
                mail.Subject = "vacation home offer";
                mail.Body = "Hello, I am a Host at 'Keep Calm, Vacation On'. My vacation home suits your request. Are you interested in cont" +
                "inuing the process? if so please contact me at " + h.MailAddress;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";

                smtp.Credentials = new System.Net.NetworkCredential("stminiproject@gmail.com", "stmini123");
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }).Start();
        }
        #endregion

        #region bank
        public IEnumerable<BankBranch> ListOfBanks(string searchString)
        {
            return dal_bl.GetAllbanks(t => ((t.BankName.Contains(searchString)||t.BranchAddress.Contains(searchString)||t.BranchCity.Contains(searchString))));
        }
            public IEnumerable<BankBranch> ListOfBanks()
        {
            return dal_bl.ListOfBanks();//returns the list of orders

        }
        #endregion

    }
}
