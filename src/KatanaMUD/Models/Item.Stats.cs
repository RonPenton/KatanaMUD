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
                //TODO: See if item is equipped and return equipped illumination as well.
                return Stats.GetCalculatedValue<long>("Illumination");
            }
        }
    }
}
