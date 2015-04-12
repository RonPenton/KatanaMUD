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
        private Int32 _RegionId;
        private Region _Region;
        private Int32 _TextBlockId;
        private TextBlock _TextBlock;

        public Room()
        {
            Actors = new ParentChildRelationshipContainer<Room, Actor, Guid>(this, child => child.Room, (child, parent) => child.Room= parent);
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
        public static Room Load(SqlDataReader reader)
        {
            var entity = new Room();
            entity._Id = reader.GetInt32(0);
            entity._RegionId = reader.GetInt32(1);
            entity._Name = reader.GetString(2);
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
            return entity;
        }

        public override void LoadRelationships()
        {
            Region = Context.Regions.Single(x => x.Id == _RegionId);
            TextBlock = Context.TextBlocks.Single(x => x.Id == _TextBlockId);
        }

        private static void AddSqlParameters(SqlCommand c, Room e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
            c.Parameters.AddWithValue("@RegionId", e.Region?.Id);
            c.Parameters.AddWithValue("@Name", e.Name);
            c.Parameters.AddWithValue("@TextBlockId", e.TextBlock?.Id);
            c.Parameters.AddWithValue("@NorthExit", e.NorthExit);
            c.Parameters.AddWithValue("@SouthExit", e.SouthExit);
            c.Parameters.AddWithValue("@EastExit", e.EastExit);
            c.Parameters.AddWithValue("@WestExit", e.WestExit);
            c.Parameters.AddWithValue("@NorthEastExit", e.NorthEastExit);
            c.Parameters.AddWithValue("@NorthWestExit", e.NorthWestExit);
            c.Parameters.AddWithValue("@SouthEastExit", e.SouthEastExit);
            c.Parameters.AddWithValue("@SouthWestExit", e.SouthWestExit);
            c.Parameters.AddWithValue("@UpExit", e.UpExit);
            c.Parameters.AddWithValue("@DownExit", e.DownExit);
        }

        public static void GenerateInsertCommand(SqlCommand c, Room e)
        {
            c.CommandText = @"INSERT INTO [Room]([Id], [RegionId], [Name], [TextBlockId], [NorthExit], [SouthExit], [EastExit], [WestExit], [NorthEastExit], [NorthWestExit], [SouthEastExit], [SouthWestExit], [UpExit], [DownExit]
                              VALUES (@Id, @RegionId, @Name, @TextBlockId, @NorthExit, @SouthExit, @EastExit, @WestExit, @NorthEastExit, @NorthWestExit, @SouthEastExit, @SouthWestExit, @UpExit, @DownExit)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, Room e)
        {
            c.CommandText = @"UPDATE [Room] SET [Id] = @Id, [RegionId] = @RegionId, [Name] = @Name, [TextBlockId] = @TextBlockId, [NorthExit] = @NorthExit, [SouthExit] = @SouthExit, [EastExit] = @EastExit, [WestExit] = @WestExit, [NorthEastExit] = @NorthEastExit, [NorthWestExit] = @NorthWestExit, [SouthEastExit] = @SouthEastExit, [SouthWestExit] = @SouthWestExit, [UpExit] = @UpExit, [DownExit] = @DownExit WHERE [Id] = @Id";
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