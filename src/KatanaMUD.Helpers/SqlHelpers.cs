using System;
using System.Data;
using System.Data.SqlClient;

namespace KatanaMUD
{
    public static class SqlHelpers
    {
        public static string GetSafeString(this SqlDataReader reader, int index)
        {
            var val = reader.GetSqlString(index);
            if (val.IsNull)
                return null;
            return val.Value;
        }

        public static DateTime? GetSafeDateTime(this SqlDataReader reader, int index)
        {
            var val = reader.GetSqlDateTime(index);
            if (val.IsNull)
                return null;
            return val.Value;
        }

        public static int? GetSafeInt32(this SqlDataReader reader, int index)
        {
            var val = reader.GetSqlInt32(index);
            if (val.IsNull)
                return null;
            return val.Value;
        }

        public static Guid? GetSafeGuid(this SqlDataReader reader, int index)
        {
            var val = reader.GetSqlGuid(index);
            if (val.IsNull)
                return null;
            return val.Value;
        }
    }
}