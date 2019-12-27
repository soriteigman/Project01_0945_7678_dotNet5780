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
        int beds;
        bool pet;
        bool wiFi;
        bool parking;
        bool pool;
        bool jacuzzi;
        bool garden;
        bool childrensAttractions;
        bool fitnessCenter;
        StarRating stars;


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
        public bool Pet { get => pet; set => pet = value; }
        public bool WiFi { get => wiFi; set => wiFi = value; }
        public bool Parking { get => parking; set => parking = value; }
        public bool Pool { get => pool; set => pool = value; }
        public bool Jacuzzi { get => jacuzzi; set => jacuzzi = value; }
        public bool Garden { get => garden; set => garden = value; }
        public bool ChildrensAttractions { get => childrensAttractions; set => childrensAttractions = value; }
        public bool FitnessCenter { get => fitnessCenter; set => fitnessCenter = value; }
        public StarRating Stars { get => stars; set => stars = value; }
        public int Beds { get => beds; set => beds = value; }
    }
}
