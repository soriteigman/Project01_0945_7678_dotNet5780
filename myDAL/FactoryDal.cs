using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myDAL
{
    class FactoryDal
    {
        public static Idal getDal()
        {
            return Dal_imp.Instance;

        }
    }
}