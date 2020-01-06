using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Host
    {
        int hostKey;
        string privateName;
        string familyName;
        int phoneNumber;
        string mailAddress;
        BankBranch bankBranchDetails;
        int bankAccountNumber;
        bool collectionClearance;
        //int toPayOwner;

        public override string ToString()
        {
            return ("Private Name: "+PrivateName+"  Family Name: "+FamilyName+
                "\nPhone Number: "+PhoneNumber+"\nEmail Address: "+MailAddress);
        }


        public int HostKey { get => hostKey; set => hostKey = value; }
        public string PrivateName { get => privateName; set => privateName = value; }
        public string FamilyName { get => familyName; set => familyName = value; }
        public int PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string MailAddress { get => mailAddress; set => mailAddress = value; }
        public bool CollectionClearance { get => collectionClearance; set => collectionClearance = value; }
        public BankBranch BankBranchDetails { get => bankBranchDetails; set => bankBranchDetails = value; }
        public int BankAccountNumber { get => bankAccountNumber; set => bankAccountNumber = value; }
        //public int ToPayOwner { get => toPayOwner; set => toPayOwner = value; }
    }
}
