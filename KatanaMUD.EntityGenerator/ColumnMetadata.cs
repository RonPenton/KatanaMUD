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
        public string Table { get; set; }
        public string Column { get; set; }
        public int Order { get; set; }
        public string DataType { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool Nullable { get; set; }
        public bool Identity { get; set; }
        public bool Computed { get; set; }
        public bool PrimaryKey { get; set; }

        public string ForeignKeyTable { get; set; }
        public string ForeignKeyColumn { get; set; }

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

            var primary = reader.GetSqlBoolean(10);
            if (primary.IsNull || primary.IsFalse)
                PrimaryKey = false;
            else
                PrimaryKey = true;

            ForeignKeyTable = reader.GetSafeString(11);
            ForeignKeyColumn = reader.GetSafeString(12);
        }

        public string TypeName
        {
            get
            {
                if (this.Nullable)
                {
                    switch (this.DataType)
                    {
                        case "uniqueidentifier": return "Guid?";
                        case "nvarchar": return "String";
                        case "int": return "Int32?";
                        case "datetimeoffset": case "datetime": return "DateTime?";
                        case "bit": return "Boolean?";
                        case "bigint": return "Int64?";
                        case "float": return "double?";
                    }
                }

                switch (this.DataType)
                {
                    case "uniqueidentifier": return "Guid";
                    case "nvarchar": return "String";
                    case "int": return "Int32";
                    case "datetimeoffset": case "datetime": return "DateTime";
                    case "bit": return "Boolean";
                    case "bigint": return "Int64";
                    case "float": return "double";
                }

                throw new InvalidOperationException("Datatype not supported: " + this.DataType);
            }
        }
    }

    public static class SqlHelpers
    {
        public static string GetSafeString(this SqlDataReader reader, int index)
        {
            var val = reader.GetSqlString(index);
            if (val.IsNull)
                return null;
            return val.Value;
        }
    }

}
