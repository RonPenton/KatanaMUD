using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.EntityGenerator
{
    public class ColumnMetadata
    {
        string Table { get; set; }
        string Column { get; set; }
        int Order { get; set; }
        string DataType { get; set; }
        int Length { get; set; }
        int Precision { get; set; }
        int Scale { get; set; }
        bool Nullable { get; set; }
        bool Identity { get; set; }
        bool Computed { get; set; }

        public ColumnMetadata(SqlDataReader reader)
        {
            Table = reader.GetString(0);
            Column = reader.GetString(1);
            Order = reader.GetInt32(2);
            DataType = reader.GetString(3);
            Length = reader.GetInt16(4);
            Precision = reader.GetByte(5);
            Scale = reader.GetByte(6);
            Nullable = reader.GetBoolean(7);
            Identity = reader.GetBoolean(8);
            Computed = reader.GetBoolean(9);
        }
    }
}
