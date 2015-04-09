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
        }

        public string Name { get; set; }
        public List<ColumnMetadata> Columns { get; set; }

        public IEnumerable<ColumnMetadata> NormalColumns => Columns.Where(x => !x.Column.StartsWith("JSON", StringComparison.InvariantCultureIgnoreCase) && x.ForeignKeyTable == null);
        public IEnumerable<ColumnMetadata> JsonColumns => Columns.Where(x => x.Column.StartsWith("JSON", StringComparison.InvariantCultureIgnoreCase));

        public string PluralName
        {
            get
            {
                return Name + "s";
            }
        }

        public ColumnMetadata PrimaryKey => this.Columns.Single(x => x.PrimaryKey == true);

        public Table FirstLink { get; set; }
        public Table SecondLink { get; set; }

        public List<Table> Children { get; set; }
        public List<Table> LinkEntities { get; set; }
        public IEnumerable<ColumnMetadata> Relationships => Columns.Where(x => x.ForeignKeyTable != null);
    }
}
