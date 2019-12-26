using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GuestRequest
    {
        int guestRequestKey;
        string privateName;
        string familyName;
        string mailAddress;
        Status status;
        DateTime registrationDate;
        DateTime entryDate;
        DateTime releaseDate;
        VacationArea area;
        VacationSubArea subArea;
        VacationType type;
        int adults;
        int children;
        bool pet;
        Choices wiFi;
        Choices parking;
        Choices pool;
        Choices jacuzzi;
        Choices garden;
        Choices childrensAttractions;
        Choices fitnessCenter;
        StarRating stars;

        public override string ToString()
        {
            return "Hiiiiiiiiiii";
        }

       


        public int GuestRequestKey { get => guestRequestKey; set => guestRequestKey = value; }
        public string PrivateName { get => privateName; set => privateName = value; }
        public string FamilyName { get => familyName; set => familyName = value; }
        public string MailAddress { get => mailAddress; set => mailAddress = value; }
        public Status Status { get => status; set => status = value; }
        public DateTime RegistrationDate { get => registrationDate; set => registrationDate = value; }
        public DateTime EntryDate { get => entryDate; set => entryDate = value; }
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public VacationArea Area { get => area; set => area = value; }
        public VacationSubArea SubArea { get => subArea; set => subArea = value; }
        public VacationType Type { get => type; set => type = value; }
        public int Adults { get => adults; set => adults = value; }
        public int Children { get => children; set => children = value; }
        public Choices Pool { get => pool; set => pool = value; }
        public Choices Jacuzzi { get => jacuzzi; set => jacuzzi = value; }
        public Choices Garden { get => garden; set => garden = value; }
        public Choices ChildrensAttractions { get => childrensAttractions; set => childrensAttractions = value; }
        public bool Pet { get => pet; set => pet = value; }
        public Choices FitnessCenter { get => fitnessCenter; set => fitnessCenter = value; }
        public StarRating Stars { get => stars; set => stars = value; }
        public Choices WiFi { get => wiFi; set => wiFi = value; }
        public Choices Parking { get => parking; set => parking = value; }
    }
}
