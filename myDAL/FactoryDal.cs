using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FactoryDal
    {
        public static IDal getDal()
        {
            return DAL_XML_imp.Instance;

        }
    }
}