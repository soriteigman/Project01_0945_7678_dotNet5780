using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        int hostingUnitKey;
        int guestRequestKey;
        int orderKey;
        Status status;
        DateTime createDate;
        DateTime sentEmail;

        public override string ToString()
        {
            return ("Hosting Unit Key: "+HostingUnitKey+"\nGuest Request Key: "+GuestRequestKey+"\nOrder Key: "+OrderKey+
                "\nStatus: "+Status+"\nCreate Date: "+CreateDate+"  Order Email Date: "+SentEmail);
        }


        public int HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public int GuestRequestKey { get => guestRequestKey; set => guestRequestKey = value; }
        public int OrderKey { get => orderKey; set => orderKey = value; }
        public DateTime CreateDate { get => createDate; set => createDate = value; }
        public Status Status { get => status; set => status = value; }
        public DateTime SentEmail { get => sentEmail; set => sentEmail = value; }
    }
}
