using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Currency : Entity<Int32>
    {
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;
        private String _ShortName;
        private Int64 _Value;
        private float _Weight;

        public Currency()
        {
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public String ShortName { get { return _ShortName; } set { _ShortName = value; this.Changed(); } }
        public Int64 Value { get { return _Value; } set { _Value = value; this.Changed(); } }
        public float Weight { get { return _Weight; } set { _Weight = value; this.Changed(); } }
        public static Currency Load(SqlDataReader reader)
        {
            var entity = new Currency();
            entity._Id = reader.GetInt32(0);
            entity._Name = reader.GetString(1);
            entity._ShortName = reader.GetString(2);
            entity._Value = reader.GetInt64(3);
            entity._Weight = reader.GetFloat(4);
            return entity;
        }

        public override void LoadRelationships()
        {
        }

        private static void AddSqlParameters(SqlCommand c, Currency e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Name", (object)e.Name ?? DBNull.Value);
            c.Parameters.AddWithValue("@ShortName", (object)e.ShortName ?? DBNull.Value);
            c.Parameters.AddWithValue("@Value", (object)e.Value ?? DBNull.Value);
            c.Parameters.AddWithValue("@Weight", (object)e.Weight ?? DBNull.Value);
        }

        public static void GenerateInsertCommand(SqlCommand c, Currency e)
        {
            c.CommandText = @"INSERT INTO [Currency]([Id], [Name], [ShortName], [Value], [Weight])
                              VALUES (@Id, @Name, @ShortName, @Value, @Weight)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, Currency e)
        {
            c.CommandText = @"UPDATE [Currency] SET [Id] = @Id, [Name] = @Name, [ShortName] = @ShortName, [Value] = @Value, [Weight] = @Weight WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, Currency e)
        {
            c.CommandText = @"DELETE FROM[Currency] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}