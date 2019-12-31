using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   public class Creator
    {
        string username;
        string password;
        int totalCommission;

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public int TotalCommission { get => totalCommission; set => totalCommission = value; }
    }
}
