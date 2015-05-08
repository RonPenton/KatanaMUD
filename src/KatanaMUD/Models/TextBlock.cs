using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class TextBlock : Entity<Int32>
    {
        partial void OnConstruct();
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Text;

        public TextBlock()
        {
            OnConstruct();
            Rooms = new ParentChildRelationshipContainer<TextBlock, Room, Int32>(this, child => child.TextBlock, (child, parent) => child.TextBlock= parent);
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Text { get { return _Text; } set { _Text = value; this.Changed(); } }
        public ICollection<Room> Rooms { get; private set; }
        public static TextBlock Load(SqlDataReader reader)
        {
            var entity = new TextBlock();
            entity._Id = reader.GetInt32(0);
            entity._Text = reader.GetString(1);
            return entity;
        }

        public override void LoadRelationships()
        {
        }

        private static void AddSqlParameters(SqlCommand c, TextBlock e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Text", (object)e.Text ?? DBNull.Value);
        }

        public static void GenerateInsertCommand(SqlCommand c, TextBlock e)
        {
            c.CommandText = @"INSERT INTO [TextBlock]([Id], [Text])
                              VALUES (@Id, @Text)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, TextBlock e)
        {
            c.CommandText = @"UPDATE [TextBlock] SET [Id] = @Id, [Text] = @Text WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, TextBlock e)
        {
            c.CommandText = @"DELETE FROM[TextBlock] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}