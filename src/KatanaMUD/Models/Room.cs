using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Room : Entity<Int32>
    {
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;
        private Int32? _NorthExit;
        private Int32? _SouthExit;
        private Int32? _EastExit;
        private Int32? _WestExit;
        private Int32? _NorthEastExit;
        private Int32? _NorthWestExit;
        private Int32? _SouthEastExit;
        private Int32? _SouthWestExit;
        private Int32? _UpExit;
        private Int32? _DownExit;
        private Int32? _RegionId;
        private Region _Region;
        private Int32 _TextBlockId;
        private TextBlock _TextBlock;

        public Room()
        {
            Cash = new JsonContainer(this);
            HiddenCash = new JsonContainer(this);
            Actors = new ParentChildRelationshipContainer<Room, Actor, Guid>(this, child => child.Room, (child, parent) => child.Room= parent);
            Items = new ParentChildRelationshipContainer<Room, Item, Guid>(this, child => child.Room, (child, parent) => child.Room= parent);
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public Int32? NorthExit { get { return _NorthExit; } set { _NorthExit = value; this.Changed(); } }
        public Int32? SouthExit { get { return _SouthExit; } set { _SouthExit = value; this.Changed(); } }
        public Int32? EastExit { get { return _EastExit; } set { _EastExit = value; this.Changed(); } }
        public Int32? WestExit { get { return _WestExit; } set { _WestExit = value; this.Changed(); } }
        public Int32? NorthEastExit { get { return _NorthEastExit; } set { _NorthEastExit = value; this.Changed(); } }
        public Int32? NorthWestExit { get { return _NorthWestExit; } set { _NorthWestExit = value; this.Changed(); } }
        public Int32? SouthEastExit { get { return _SouthEastExit; } set { _SouthEastExit = value; this.Changed(); } }
        public Int32? SouthWestExit { get { return _SouthWestExit; } set { _SouthWestExit = value; this.Changed(); } }
        public Int32? UpExit { get { return _UpExit; } set { _UpExit = value; this.Changed(); } }
        public Int32? DownExit { get { return _DownExit; } set { _DownExit = value; this.Changed(); } }
        public dynamic Cash { get; private set; }
        public dynamic HiddenCash { get; private set; }
        public Region Region {
            get { return _Region; }
            set
            {
                ChangeParent(value, ref _Region, 
                    (Region parent, Room child) => parent.Rooms.Remove(child), 
                    (Region parent, Room child) => parent.Rooms.Add(child));
            }
        }

        public TextBlock TextBlock {
            get { return _TextBlock; }
            set
            {
                ChangeParent(value, ref _TextBlock, 
                    (TextBlock parent, Room child) => parent.Rooms.Remove(child), 
                    (TextBlock parent, Room child) => parent.Rooms.Add(child));
            }
        }

        public ICollection<Actor> Actors { get; private set; }
        public ICollection<Item> Items { get; private set; }
        public static Room Load(SqlDataReader reader)
        {
            var entity = new Room();
            entity._Id = reader.GetInt32(0);
            entity._RegionId = reader.GetSafeInt32(1);
            entity._Name = reader.GetSafeString(2);
            entity._TextBlockId = reader.GetInt32(3);
            entity._NorthExit = reader.GetSafeInt32(4);
            entity._SouthExit = reader.GetSafeInt32(5);
            entity._EastExit = reader.GetSafeInt32(6);
            entity._WestExit = reader.GetSafeInt32(7);
            entity._NorthEastExit = reader.GetSafeInt32(8);
            entity._NorthWestExit = reader.GetSafeInt32(9);
            entity._SouthEastExit = reader.GetSafeInt32(10);
            entity._SouthWestExit = reader.GetSafeInt32(11);
            entity._UpExit = reader.GetSafeInt32(12);
            entity._DownExit = reader.GetSafeInt32(13);
            entity.Cash = new JsonContainer(entity);
            entity.Cash.FromJson(reader.GetSafeString(14));
            entity.HiddenCash = new JsonContainer(entity);
            entity.HiddenCash.FromJson(reader.GetSafeString(15));
            return entity;
        }

        public override void LoadRelationships()
        {
            Region = Context.Regions.SingleOrDefault(x => x.Id == _RegionId);
            TextBlock = Context.TextBlocks.Single(x => x.Id == _TextBlockId);
        }

        private static void AddSqlParameters(SqlCommand c, Room e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@RegionId", (object)e.Region?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Name", (object)e.Name ?? DBNull.Value);
            c.Parameters.AddWithValue("@TextBlockId", (object)e.TextBlock?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@NorthExit", (object)e.NorthExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@SouthExit", (object)e.SouthExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@EastExit", (object)e.EastExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@WestExit", (object)e.WestExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@NorthEastExit", (object)e.NorthEastExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@NorthWestExit", (object)e.NorthWestExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@SouthEastExit", (object)e.SouthEastExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@SouthWestExit", (object)e.SouthWestExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@UpExit", (object)e.UpExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@DownExit", (object)e.DownExit ?? DBNull.Value);
            c.Parameters.AddWithValue("@JSONCash", e.Cash.ToJson());
            c.Parameters.AddWithValue("@JSONHiddenCash", e.HiddenCash.ToJson());
        }

        public static void GenerateInsertCommand(SqlCommand c, Room e)
        {
            c.CommandText = @"INSERT INTO [Room]([Id], [RegionId], [Name], [TextBlockId], [NorthExit], [SouthExit], [EastExit], [WestExit], [NorthEastExit], [NorthWestExit], [SouthEastExit], [SouthWestExit], [UpExit], [DownExit], [JSONCash], [JSONHiddenCash])
                              VALUES (@Id, @RegionId, @Name, @TextBlockId, @NorthExit, @SouthExit, @EastExit, @WestExit, @NorthEastExit, @NorthWestExit, @SouthEastExit, @SouthWestExit, @UpExit, @DownExit, @JSONCash, @JSONHiddenCash)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, Room e)
        {
            c.CommandText = @"UPDATE [Room] SET [Id] = @Id, [RegionId] = @RegionId, [Name] = @Name, [TextBlockId] = @TextBlockId, [NorthExit] = @NorthExit, [SouthExit] = @SouthExit, [EastExit] = @EastExit, [WestExit] = @WestExit, [NorthEastExit] = @NorthEastExit, [NorthWestExit] = @NorthWestExit, [SouthEastExit] = @SouthEastExit, [SouthWestExit] = @SouthWestExit, [UpExit] = @UpExit, [DownExit] = @DownExit, [JSONCash] = @JSONCash, [JSONHiddenCash] = @JSONHiddenCash WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, Room e)
        {
            c.CommandText = @"DELETE FROM[Room] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}