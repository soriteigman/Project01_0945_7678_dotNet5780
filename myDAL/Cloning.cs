using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace myDAL
{
    public static class Cloning
    {
        public static GuestRequest Clone(this GuestRequest original)
        {
            GuestRequest target = new GuestRequest();
            target.GuestRequestKey = original.GuestRequestKey;
            target.PrivateName = original.PrivateName;
            target.FamilyName = original.FamilyName;
            target.MailAddress = original.MailAddress;
            target.Status = original.Status;
            target.RegistrationDate = original.RegistrationDate;
            target.EntryDate = original.EntryDate;
            target.ReleaseDate = original.ReleaseDate;
            target.Area = original.Area;
            target.SubArea = original.SubArea;
            target.Type = original.Type;
            target.Adults = original.Adults;
            target.Children = original.Children;
            target.Pool = original.Pool;
            target.Jacuzzi = original.Jacuzzi;
            target.Garden = original.Garden;
            target.ChildrensAttractions = original.ChildrensAttractions;

            return target;
        }
 
        public static HostingUnit Clone(this HostingUnit original)
        {
            HostingUnit target = new HostingUnit();
            target.HostingUnitKey = original.HostingUnitKey;
            target.Owner = original.Owner;
            target.HostingUnitName = original.HostingUnitName;
            target.Diary = original.Diary;

            return target;
        }

        public static Order Clone(this Order original)
        {
            Order target = new Order();
            target.HostingUnitKey = original.HostingUnitKey;
            target.GuestRequestKey = original.GuestRequestKey;
            target.OrderKey = original.OrderKey;
            target.Status = original.Status;
            target.CreateDate = original.CreateDate;
            target.OrderDate = original.OrderDate;

            return target;
        }

        public static Host Clone(this Host original)
        {
            Host target = new Host();
            target.HostKey = original.HostKey;
            target.PrivateName = original.PrivateName;
            target.FamilyName = original.FamilyName;
            target.PhoneNumber = original.PhoneNumber;
            target.MailAddress = original.MailAddress;
            target.BankAccount = original.BankAccount;
            target.CollectionClearance = original.CollectionClearance;


            return target;
        }

        public static BankAccount Clone(this BankAccount original)
        {
            BankAccount target = new BankAccount();
            target.BankAccountNumber = original.BankAccountNumber;
            target.BankName = original.BankName;
            target.BranchNumber = original.BranchNumber;
            target.BranchAddress = original.BranchAddress;
            target.BranchCity = original.BranchCity;

            return target;
        }

    }
}
