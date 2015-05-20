using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.EntityGenerator
{
    public class Table
    {
        public Table(string name)
        {
            Name = name;
            LinkEntities = new List<Table>();
            Children = new List<Table>();
            Relationships = new List<Relationship>();
        }

        public string Name { get; set; }
        public List<ColumnMetadata> Columns { get; set; }

        public IEnumerable<ColumnMetadata> NormalColumns => Columns.Where(x => x.ForeignKeyTable == null);

        public string PluralName
        {
            get
            {
                if (Name.EndsWith("y"))
                    return Name.Substring(0, Name.Length - 1) + "ies";
                return Name + "s";
            }
        }

        public ColumnMetadata PrimaryKey => this.Columns.Single(x => x.PrimaryKey == true);

        public Table FirstLink { get; set; }
        public Table SecondLink { get; set; }

        public List<Table> Children { get; set; }
        public List<Table> LinkEntities { get; set; }
        public List<Relationship> Relationships { get; set; }
    }
}
