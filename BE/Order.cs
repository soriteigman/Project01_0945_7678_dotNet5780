﻿using System;
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
        DateTime orderDate;
        DateTime sentEmail;

        public override string ToString()
        {
            return "Hiiiiiiiiiii";
        }

        public int HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public int GuestRequestKey { get => guestRequestKey; set => guestRequestKey = value; }
        public int OrderKey { get => orderKey; set => orderKey = value; }
        public DateTime CreateDate { get => createDate; set => createDate = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
        public Status Status { get => status; set => status = value; }
        public DateTime SentEmail { get => sentEmail; set => sentEmail = value; }
    }
}
