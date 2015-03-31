using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.EntityGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(local);Initial Catalog=KatanaMUD;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var columns = GetColumns(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static List<ColumnMetadata> GetColumns(SqlConnection connection)
        {
            string query = @"use [KatanaMUD]
                                select o.name as [Table], 
		                                c.name as [Column], 
		                                c.column_id as [Order],
		                                st.name as [DataType], 
		                                c.max_length as [MaxLength], 
		                                c.precision as [Precision],  
		                                c.scale as [Scale],
		                                c.is_nullable as [Nullable],
		                                c.is_identity as [Identity],
		                                c.is_computed as [Computed]
                                from sys.objects o inner
                                join sys.columns c on o.object_id = c.object_id
                                inner
                                join sys.types st on c.user_type_id = st.user_type_id
                                where o.type_desc = 'USER_TABLE'
                                order by o.name, c.column_id";

            SqlCommand command = new SqlCommand(query, connection);
            List<ColumnMetadata> list = new List<ColumnMetadata>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new ColumnMetadata(reader));
                }
            }
            return list;
        }
    }
}
