using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvioRepair.AppData
{
    internal class Class1
    {
        public static RepairEntities c;
        public static RepairEntities context
        {
            get
            {
                if (c == null)
                    c = new RepairEntities();
                return c;
            }
        }
    }
}
