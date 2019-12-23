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
        bool DateLengthPermission(GuestRequest gr)//checks if stay is at least one full day long
        {
            DateTime temp = gr.EntryDate.AddDays(1);
            if (gr.ReleaseDate >= temp)//if theres at least one day difference between the start and end dates
                return true;
            return false;
        }

        void PermissionToCharge(Host h, Order o)//checks if client gave permission for payment
        {
            if(h.CollectionClearance)//checks if there is permission to collect the money
            {
                o.Status = Status.SentEmail;//changes the status to sent mail
                SendEmail(o);//calls the function to send an email
            }
        }

        bool AvailabilityCheck(HostingUnit hu, GuestRequest gr)//checks if requested dates are available
        {
            DateTime start = gr.EntryDate;
            DateTime end = gr.ReleaseDate.AddDays(-1);

            while(start!=end)//checks availability for duration of the visit
            {
                if (hu.Diary[start.Month - 1, start.Day - 1])//returns false when any day isnt available
                    return false;
                start = start.AddDays(1);//next day
            }
            return true;  
        }

        void FinalStatusChange(Order o)//after order status changes to closed cannot make further changes to the status
        {

        }

        GuestRequest FindRequest(int requestKey)//finds the guest request based on the key
        {
            Idal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<GuestRequest> requests = dal_bl.ListOfCustomers();//gets the list of guest requests

            var request = from guest in requests
                          where guest.GuestRequestKey == requestKey
                          select guest;//finds the request with the matching key of the order
            GuestRequest req = request.FirstOrDefault();
            return req;
        }
        int CalculateDurationOfStay(GuestRequest gr)//returns duration of stay
        {
            return (gr.ReleaseDate - gr.EntryDate).Days;
        }
        int CalculateComission(Order o)//calculates comission
        {
            GuestRequest my_req = FindRequest(o.GuestRequestKey);//finds the correct guest request
            int duration = CalculateDurationOfStay(my_req);//calculates the duration of stay

            return duration * Configuration.commission;//returns TOTAL commission
        }

        void UpdateDiary(HostingUnit hu, Order o)//after the status changes to closed, mark the days in the units diary
        {
            GuestRequest gr = FindRequest(o.GuestRequestKey);
            DateTime start = gr.EntryDate;
            DateTime end = gr.ReleaseDate;
            while (start != end)//updates the dates in the diary
            {
                hu.Diary[start.Month - 1, start.Day - 1] = true;
                start = start.AddDays(1);
            }
        }

        void ChangeRequestStatus(Order o, GuestRequest gr)//after order status changes to closed, also close to request status
        {
            Idal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of orders

            gr.Status = Status.Closed;
            foreach (Order item in orders)
            {
                if (item.GuestRequestKey == gr.GuestRequestKey)
                    item.Status = Status.Closed;
            }

        }
        bool RemoveUnitCheck(HostingUnit hu)//checks to see if there are any active reservations for that unit before removing it
        {
            Idal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<Order> orders = dal_bl.ListOfOrders();//gets the list of guest requests

            if (orders.Any(x => x.HostingUnitKey == hu.HostingUnitKey && x.Status != Status.Closed))//checks if any orders connected to the hosting unit are not closed
                return false;
            return true;
        }
        bool ChangeCollectionClearance(Host h)//?????
        {

        }
        void SendEmail(Order o)//sends email when order status changes to "sent mail"
        {

            MailMessage mail = new MailMessage();
            mail.To.Add("toEmailAddress");
            mail.From = new MailAddress("fromEmailAddress");
            mail.Subject = "mailSubject";
            mail.Body = "mailBody";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";

            smtp.Credentials = new System.Net.NetworkCredential("myGmailEmailAddress@gmail.com", "myGmailPassword");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
               // txtMessage.Text = ex.ToString();
            }
        }
        IEnumerable<HostingUnit> AvailableUnits(DateTime startDate, int numOfDays)//returns all available hosting units for the dates requested
        {
            DateTime end = startDate.AddDays(numOfDays);
            Idal dal_bl = DAL.FactoryDal.getDal();//creates an instance of dal
            IEnumerable<HostingUnit> hostingUnits = dal_bl.ListOfHostingUnits();//gets the list of hosting units
            GuestRequest temp = new GuestRequest() { EntryDate = startDate, ReleaseDate = end };
            var request = from unit in hostingUnits //creates a list of all available units
                          where AvailabilityCheck(unit,temp)
                          select unit;//selects if available in given dates
            return request;
        }
        int NumOfDaysInBetweeen(DateTime startDate, DateTime endDate = default(DateTime))//remember if the end date is null change it to Configuration.today
        {
            if (endDate == default(DateTime))//if there was no end date given, use today as an end date
                endDate = Configuration.today;

            return (endDate - startDate).Days;
        }
        IEnumerable<Order> DaysPassedOnOrders(int numOfDays)//returns all orders that were sent a email/ created "numOfDays" ago
        {

        }
        IEnumerable<GuestRequest> AllRequestsThatMatch(Predicate<GuestRequest> conditions)//returns all requests that fullfill the conditions 
        {

        }
        int NumOfSent_GR_Orders(GuestRequest gr)//returns the num of orders that were sent for that guest request
        {

        }
        int NumOfSent_HU_Orders(HostingUnit hu)//returns the number of orders that were sent or booked for this hosting unit
        {

        }
        IEnumerable<GuestRequest> Group_GR_ByArea()//groups the requests by area of choice
        {

        }
        IEnumerable<GuestRequest> GroupByNumOfGuests()//groups by number of guests
        {

        }
        IEnumerable<Host> GroupByNumOfUnits()//groups by number of hosting units the hosts own
        {

        }

        IEnumerable<HostingUnit> Group_HU_ByArea()//groups the units by area of choice
        {

        }

        private class Predicate
        {
        }
    }
}
