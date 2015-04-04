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
    }
}