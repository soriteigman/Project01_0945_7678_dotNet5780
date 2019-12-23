using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FactoryDal
    {
        public static Idal getDal()
        {
            return Dal_imp.Instance;

        }
    }
}