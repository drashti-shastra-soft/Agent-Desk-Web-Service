using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.Utils
{
    public class Enums
    {
        public enum Role 
        {
            System_Owner = 1,
            Super_Admin = 2,
            Admin = 3,
            Employee = 4,
        }

        public enum PayMod
        {
            Monthly = 1,
            Quarterly = 2, 
            HalfYear = 3,
            Yearly = 4
        }
    }
}
