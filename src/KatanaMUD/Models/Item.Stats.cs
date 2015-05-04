using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Models
{
    public partial class Item
    {
        public long Illumination
        {
            get
            {
                return GetStat<long>("Illumination", 0);
            }
        }
    }
}
