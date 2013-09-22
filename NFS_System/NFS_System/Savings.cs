using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NFS_System
{
    class Savings : Account
    {
        public Savings(int accNum) : base(accNum, "Savings") { }

        public Savings(int accNumNew, double balanceNew, string custIDNew, int daysSinceInterestPaidNew, int limitNew)
                : base(accNumNew, balanceNew, custIDNew, daysSinceInterestPaidNew, limitNew) { }        

        
    }
}
