using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankAccount
    {
        int bankNumber;
        string bankName;
        int branchNumber;
        string branchAddress;
        string branchCity;
        int bankAccountNumber;

        public int BankNumber { get => bankNumber; set => bankNumber = value; }
        public string BankName { get => bankName; set => bankName = value; }
        public int BranchNumber { get => branchNumber; set => branchNumber = value; }
        public string BranchAddress { get => branchAddress; set => branchAddress = value; }
        public string BranchCity { get => branchCity; set => branchCity = value; }
        public int BankAccountNumber { get => bankAccountNumber; set => bankAccountNumber = value; }
    }
}
