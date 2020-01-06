using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankBranch
    {
        int bankNumber;
        string bankName;
        int branchNumber;
        string branchAddress;
        string branchCity;
        public override string ToString()
        {
            return "Bank Name: "+BankName+" Bank Number: "+BankNumber+" Branch Number: "+BranchNumber+" Branch Address: "+BranchAddress+"   Branch City: "+BranchCity;
        }


        public int BankNumber { get => bankNumber; set => bankNumber = value; }
        public string BankName { get => bankName; set => bankName = value; }
        public int BranchNumber { get => branchNumber; set => branchNumber = value; }
        public string BranchAddress { get => branchAddress; set => branchAddress = value; }
        public string BranchCity { get => branchCity; set => branchCity = value; }
    }
}
