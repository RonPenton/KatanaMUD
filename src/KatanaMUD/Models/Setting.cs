using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Setting : Entity<String>
    {
        partial void OnConstruct();
        partial void OnLoaded();
        public override String Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private String _Id;
        private String _Value;

        public Setting()
        {
            OnConstruct();
        }

        public String Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Value { get { return _Value; } set { _Value = value; this.Changed(); } }
        public static Setting Load(SqlDataReader reader)
        {
            var entity = new Setting();
            entity._Id = reader.GetString(0);
            entity._Value = reader.GetString(1);
            entity.OnLoaded();
            return entity;
        }

        public override void LoadRelationships()
        {
        }

        private static void AddSqlParameters(SqlCommand c, Setting e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Value", (object)e.Value ?? DBNull.Value);
        }

        public static void GenerateInsertCommand(SqlCommand c, Setting e)
        {
            c.CommandText = @"INSERT INTO [Setting]([Id], [Value])
                              VALUES (@Id, @Value)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, Setting e)
        {
            c.CommandText = @"UPDATE [Setting] SET [Id] = @Id, [Value] = @Value WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, Setting e)
        {
            c.CommandText = @"DELETE FROM[Setting] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}