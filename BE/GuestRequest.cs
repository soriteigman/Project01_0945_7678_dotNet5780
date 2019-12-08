using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    class GuestRequest
    {
        int guestRequestKey;
        string privateName;
        string familyName;
        string mailAddress;
        string status;
        DateTime registrationDate;
        DateTime entryDate;
        DateTime releaseDate;
        Enums.VacationArea area;
        Enums.VacationSubArea subArea;
        Enums.VacationType type;
        int adults;
        int children;
        bool pool;
        bool jacuzzi;
        bool garden;
        bool childrensAttractions;

        public override string ToString()
        {
            return "Hiiiiiiiiiii";
        }

        public int GuestRequestKey { get => guestRequestKey; set => guestRequestKey = value; }
        public string PrivateName { get => privateName; set => privateName = value; }
        public string FamilyName { get => familyName; set => familyName = value; }
        public string MailAddress { get => mailAddress; set => mailAddress = value; }
        public string Status { get => status; set => status = value; }
        public DateTime RegistrationDate { get => registrationDate; set => registrationDate = value; }
        public DateTime EntryDate { get => entryDate; set => entryDate = value; }
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public Enums.VacationArea Area { get => area; set => area = value; }
        public Enums.VacationSubArea SubArea { get => subArea; set => subArea = value; }
        public Enums.VacationType Type { get => type; set => type = value; }
        public int Adults { get => adults; set => adults = value; }
        public int Children { get => children; set => children = value; }
        public bool Pool { get => pool; set => pool = value; }
        public bool Jacuzzi { get => jacuzzi; set => jacuzzi = value; }
        public bool Garden { get => garden; set => garden = value; }
        public bool ChildrensAttractions { get => childrensAttractions; set => childrensAttractions = value; }

    }
}
