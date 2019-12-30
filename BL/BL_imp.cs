using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using System.Net.Mail;

namespace BL
{
  
    public class BL_imp:IBL
    {

        #region done
        public void addreq(GuestRequest gr)
        {
            try
            {
                IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal

                DateTime today = Configuration.today;
                today = today.AddMonths(11);
                if (today < gr.EntryDate)
                {
                    throw new ArgumentException("wrong date");
                }
                if (!DateLengthPermission(gr))
                {
                    throw new ArgumentException("wrong date");
                }
                if (Configuration.today > gr.EntryDate)
                {
                    throw new ArgumentException("wrong date");
                }

                dal_bl.AddGuestRequest(gr);
            }
            catch (ArgumentException a)
            {
                throw a;
            }
        }
        public Predicate<GuestRequest> BuildPredicate(HostingUnit hu)//based on a hosting unit builds a predicate to filter all guest requests
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
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
            bool StarRating(GuestRequest gr) { return gr.Stars == hu.Stars; }



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

            return pred;
        }
        public void Updategr(GuestRequest gr)
        {
            try
            {
                IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal

                if (gr.Status == Status.Closed)
                    throw new InvalidOperationException("cannot change status of closed order");
                dal_bl.UpdateGuestRequest(gr);
            }
            catch (InvalidOperationException a)
            {
                throw a;
            }
        }
        public Order CreateOrder(int HUkey, int GRkey)//in case of gr and hu matching creates an order for them
        {
            Order ord = new Order
            {
                GuestRequestKey = GRkey,
                HostingUnitKey = HUkey,
                CreateDate = Configuration.today,
                Status = Status.Active
            };
            return ord;
        }

        #endregion



        public void AddHostingUnit(HostingUnit hu)
        {
            try
            {
                IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
                dal_bl.AddHostingUnit(hu);
            }
            catch (Exception a)
            {
                throw a;
            }

        }

        public void RemoveHostingUnit(HostingUnit hu)
        {
            try
            {
                IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
                if (!RemoveUnitCheck(hu))
                    throw new InvalidOperationException("cannot delete this hosting unit, there are active orders connected to it");

                dal_bl.RemoveHostingUnit(hu);
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            try
            {
                IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
                dal_bl.UpdateHostingUnit(hu);
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        public void AddOrder(Order o)
        {
            try
            {
                o.OrderKey = Configuration.OrderKey_s++;
                IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
                if (!AvailabilityCheck(dal_bl.SearchHUbyID(o.HostingUnitKey), dal_bl.searchGRbyID(o.GuestRequestKey)))
                { } //throw
                if (!dal_bl.HUexist(o.HostingUnitKey))
                { }//throw hu doesnt exist
                if (!dal_bl.GRexist(o.GuestRequestKey))
                { } //throw gr doesnt exist
                 dal_bl.AddOrder(o.Clone());
                //SendEmail(o.Clone());
                o.Status = Status.SentEmail;
                UpdateOrder(o.Clone());
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
                IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
                Order oldO = dal_bl.SearchOrbyID(newO.OrderKey);
                if (oldO.Status == Status.Closed)
                     { }//throw cannot update
                int commission = CalculateComission(newO);
                if (newO.Status == Status.Closed)
                {
                    ChangeRequestStatus(newO);
                    UpdateDiary(newO);
                }
                if (newO.Status == Status.SentEmail)
                    SendEmail(newO);

                dal_bl.UpdateOrder(newO);
            }
            catch (Exception a)
            {
                throw a;
            }
        }

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
        #region i think were done 
        public bool DateLengthPermission(GuestRequest gr)//checks if stay is at least one full day long
        {
            DateTime temp = gr.EntryDate.AddDays(1);
            if (gr.ReleaseDate >= temp)//if theres at least one day difference between the start and end dates
                return true;
            return false;
        }

        public void PermissionToCharge(Host h, Order o)//checks if client gave permission for payment
        {
            if (h.CollectionClearance)//checks if there is permission to collect the money
            {
                o.Status = Status.SentEmail;//changes the status to sent mail
                o.SentEmail = DateTime.Now;
                SendEmail(o);//calls the function to send an email
            }
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

        public int CalculateComission(Order o)//calculates comission
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal

            GuestRequest my_req = dal_bl.searchGRbyID(o.GuestRequestKey);//finds the correct guest request
            int duration = CalculateDurationOfStay(my_req);//calculates the duration of stay

            return duration * Configuration.commission;//returns TOTAL commission
        }

        public void UpdateDiary( Order o)//after the status changes to closed, mark the days in the units diary
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            HostingUnit hu = dal_bl.SearchHUbyID(o.HostingUnitKey);
            GuestRequest gr = dal_bl.searchGRbyID(o.GuestRequestKey);
            DateTime start = gr.EntryDate;
            DateTime end = gr.ReleaseDate;

            while (start != end)//updates the dates in the diary
            {
                hu.Diary[start.Month - 1, start.Day - 1] = true;
                start = start.AddDays(1);
            }
        }

        public void ChangeRequestStatus(Order o)//after order status changes to closed, also close the request status
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders
            GuestRequest gr = dal_bl.searchGRbyID(o.GuestRequestKey).Clone();

            gr.Status = Status.Closed;
            foreach (Order item in orders)
            {
                if (item.GuestRequestKey == gr.GuestRequestKey)
                    item.Status = Status.Closed;
            }

        }

        public bool RemoveUnitCheck(HostingUnit hu)//checks to see if there are any active reservations for that unit before removing it
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of guest requests

            if (orders.Any(x => x.HostingUnitKey == hu.HostingUnitKey && x.Status != Status.Closed))//checks if any orders connected to the hosting unit are not closed
                return false;
            return true;
        }

        public IEnumerable<HostingUnit> AvailableUnits(DateTime startDate, int numOfDays)//returns all available hosting units for the dates requested
        {
            DateTime end = startDate.AddDays(numOfDays);
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<HostingUnit> hostingUnits = dal_bl.ListOfHostingUnits();//gets the list of hosting units
            GuestRequest temp = new GuestRequest() { EntryDate = startDate, ReleaseDate = end };
            var request = from unit in hostingUnits //creates a list of all available units
                          where AvailabilityCheck(unit, temp)
                          select unit;//selects if available in given dates
            return request;
        }

        public int NumOfDaysInBetweeen(DateTime startDate, DateTime endDate = default(DateTime))//remember if the end date is null change it to Configuration.today
        {
            if (endDate == default(DateTime))//if there was no end date given, use today as an end date
                endDate = Configuration.today;

            return (endDate - startDate).Days;
        }

        public IEnumerable<Order> DaysPassedOnOrders(int numOfDays, Predicate<Order> conditions)//returns all orders that were sent a email/ created "numOfDays" ago
        {
            //(Configuration.today-ord.CreateDate).Days>=numOfDays
            //(Configuration.today - ord.SentEmail).Days >= numOfDays


            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders

            var createResult = from ord in orders //creates a list of all orders that fit the condition
                               where (conditions(ord))
                               select ord;

            return createResult;

        }

        public IEnumerable<GuestRequest> AllRequestsThatMatch(Predicate<GuestRequest> conditions)//returns all requests that fullfill the conditions 
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<GuestRequest> guestRequests = dal_bl.ListOfCustomers();//gets the list of requests

            List<GuestRequest> result = new List<GuestRequest>();

            bool temp=true;
            foreach(GuestRequest req in guestRequests)
            {
                foreach (Predicate<GuestRequest> item in conditions.GetInvocationList())
                {
                    if(!item(req))
                         temp=false;
                }
                if (temp)
                    result.Add(req);
                temp = true;
                    
            }
            return (IEnumerable<GuestRequest>)result;
        }
        
        public int NumOfSent_GR_Orders(GuestRequest gr)//returns the num of orders that were sent for that guest request
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders

            var result = from ord in orders //creates a list of all orders that fit the condition
                         where ord.GuestRequestKey == gr.GuestRequestKey
                         select ord;

            return result.Count();
        }

        public int NumOfSent_HU_Orders(HostingUnit hu, Predicate<Order> conditions)//returns the number of orders that were sent or booked for this hosting unit
        {
            //predicate condition is either booked or sent email
            //ord.Status==Status.SentEmail
            //ord.Status==Status.Booked


            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders

            var result = from ord in orders //creates a list of all orders that fit the condition
                         where conditions(ord) && ord.HostingUnitKey == hu.HostingUnitKey
                         select ord;

            return result.Count();
        }


        #endregion


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
        public IEnumerable<IGrouping<Host , HostingUnit>> GroupHUByHosts()
        {
            IDal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<HostingUnit> units = dal_bl.ListOfHostingUnits();//gets the list of units

            IEnumerable<IGrouping<Host, HostingUnit>> result = from unit in units
                                                               group unit by unit.Owner into g1
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

        #region need work

        //public bool ChangeCollectionClearance(Host h)//?????
        //{

        //}
        public void SendEmail(Order o)//sends email when order status changes to "sent mail"
        {
            Console.WriteLine("email was sent, catch it if u can!!!!");

            //MailMessage mail = new MailMessage();
            //mail.To.Add("toEmailAddress");
            //mail.From = new MailAddress("fromEmailAddress");
            //mail.Subject = "mailSubject";
            //mail.Body = "mailBody";
            //mail.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";

            //smtp.Credentials = new System.Net.NetworkCredential("myGmailEmailAddress@gmail.com", "myGmailPassword");
            //smtp.EnableSsl = true;
            //try
            //{
            //    smtp.Send(mail);
            //}
            //catch (Exception ex)
            //{
            //    // txtMessage.Text = ex.ToString();
            //}
        }


        #endregion



    }
}
