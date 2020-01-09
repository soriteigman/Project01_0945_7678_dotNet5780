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
        string subArea;
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

        private static string PrintDiary(bool[,] Diary)//returns a string of all the booked days in current diary
        {
            string booked = null;
            DateTime start = new DateTime(2019, 1, 1);//beginning of the year
            DateTime end = new DateTime(2019, 12, 31);//end of the year
            while (start != end)//going ove rthe whole year
            {
                if (Diary[start.Month - 1, start.Day - 1])//when we reach the first day of booked days
                {
                    DateTime saved_start = start;//save the date to print
                    while (Diary[start.Month - 1, start.Day - 1])//until we find the next unbooked day
                    {
                        start = start.AddDays(1);//add a day
                    }
                    DateTime saved_end = start.AddDays(1);//save end date
                    booked += ("start day:  " + saved_start.ToString("dd-MM-yyyy") + "    end day:    " + saved_end.ToString("dd-MM-yyyy") + "\n");//catonates the dates to one string
                }
                if (start != end)// in case it already left the last loop at the end of the year 
                    start = start.AddDays(1);
            }
            return booked;
        }
        public override string ToString()
        {
            return ("Hosting Unit Key: " + HostingUnitKey + "\nOwner: "+Owner+"\nHosting Unit Name: "+HostingUnitName+
                "\nArea: "+Area+"    SubArea: "+SubArea+"\nType: "+Type+"\ndiary: "+PrintDiary(diary));
        }

        public int HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public string HostingUnitName { get => hostingUnitName; set => hostingUnitName = value; }
        public bool[,] Diary { get => diary; set => diary = value; }
        public Host Owner { get => owner; set => owner = value; }
        public VacationArea Area { get => area; set => area = value; }
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
        public string SubArea { get => subArea; set => subArea = value; }
    }
}
