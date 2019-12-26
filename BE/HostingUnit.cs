using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit
    {
        int hostingUnitKey;
        Host owner;
        string hostingUnitName;
        private bool[,] diary = new bool[12, 31];
        VacationArea area;
        VacationSubArea subArea;
        VacationType type;
        int adults;
        int children;
        Choices pool;
        Choices jacuzzi;
        Choices garden;
        Choices childrensAttractions;

        public override string ToString()
        {
            return "Hiiiiiiiiiii";
        }

        public int HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public string HostingUnitName { get => hostingUnitName; set => hostingUnitName = value; }
        public bool[,] Diary { get => diary; set => diary = value; }
        public Host Owner { get => owner; set => owner = value; }
        public VacationArea Area { get => area; set => area = value; }
        public VacationSubArea SubArea { get => subArea; set => subArea = value; }
        public VacationType Type { get => type; set => type = value; }
        public int Adults { get => adults; set => adults = value; }
        public int Children { get => children; set => children = value; }
        public Choices Pool { get => pool; set => pool = value; }
        public Choices Jacuzzi { get => jacuzzi; set => jacuzzi = value; }
        public Choices Garden { get => garden; set => garden = value; }
        public Choices ChildrensAttractions { get => childrensAttractions; set => childrensAttractions = value; }

    }
}
