using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Configuration
    {
        public static int HostingUnitKey_s=10000000;

        public static int GuestRequest_s = 10000000;

        public static int OrderKey_s = 10000000;

        public static int ExpDay = 10;

        public static int commission = 10;

        public static DateTime today = DateTime.Today.AddDays(-35);
        public static DateTime _DateLastRun = default(DateTime);


    }
}
