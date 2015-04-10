using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.EntityGenerator
{
    public class Relationship
    {
        public ColumnMetadata Column { get; set; }

        public Table ParentTable { get; set; }
    }
}
