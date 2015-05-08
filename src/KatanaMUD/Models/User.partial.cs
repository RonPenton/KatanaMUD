using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Models
{
    public partial class User
    {
    }


    public enum AccessLevel
    {
        Regular = 0,
        Mudop = 100,
        Sysop = 200
    }
}
