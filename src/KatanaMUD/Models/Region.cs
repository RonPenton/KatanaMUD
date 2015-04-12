using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Region : Entity<Int32>
    {
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;

        public Region()
        {
            Rooms = new ParentChildRelationshipContainer<Region, Room, Int32>(this, child => child.Region, (child, parent) => child.Region= parent);
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public ICollection<Room> Rooms { get; private set; }
        public static Region Load(SqlDataReader reader)
        {
            var entity = new Region();
            entity._Id = reader.GetInt32(0);
            entity._Name = reader.GetString(1);
            return entity;
        }

        public override void LoadRelationships()
        {
        }

        private static void AddSqlParameters(SqlCommand c, Region e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Name", (object)e.Name ?? DBNull.Value);
        }

        public static void GenerateInsertCommand(SqlCommand c, Region e)
        {
            c.CommandText = @"INSERT INTO [Region]([Id], [Name])
                              VALUES (@Id, @Name)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, Region e)
        {
            c.CommandText = @"UPDATE [Region] SET [Id] = @Id, [Name] = @Name WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, Region e)
        {
            c.CommandText = @"DELETE FROM[Region] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}