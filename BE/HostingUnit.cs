using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    class HostingUnit
    {
        int hostingUnitKey;
        Host owner;
        string hostingUnitName;
        private bool[,] diary = new bool[12, 31];

        public override string ToString()
        {
            return "Hiiiiiiiiiii";
        }

        public int HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public string HostingUnitName { get => hostingUnitName; set => hostingUnitName = value; }
        public bool[,] Diary { get => diary; set => diary = value; }
        internal Host Owner { get => owner; set => owner = value; }
    }
}
