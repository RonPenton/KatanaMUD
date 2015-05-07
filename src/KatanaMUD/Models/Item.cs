using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Item : Entity<Guid>
    {
        public override Guid Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Guid _Id;
        private String _CustomName;
        private Boolean _Modified;
        private Int32? _EquippedSlot;
        private Int64? _HiddenTime;
        private Int32 _ItemTemplateId;
        private ItemTemplate _ItemTemplate;
        private Guid? _ActorId;
        private Actor _Actor;
        private Int32? _RoomId;
        private Room _Room;

        public Item()
        {
            Stats = new JsonContainer(this);
        }

        public Guid Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String CustomName { get { return _CustomName; } set { _CustomName = value; this.Changed(); } }
        public Boolean Modified { get { return _Modified; } set { _Modified = value; this.Changed(); } }
        public EquipmentSlot? EquippedSlot { get { return (EquipmentSlot?)_EquippedSlot; } set { _EquippedSlot = (Int32?)value; this.Changed(); } }
        public Int64? HiddenTime { get { return _HiddenTime; } set { _HiddenTime = value; this.Changed(); } }
        public dynamic Stats { get; private set; }
        partial void OnItemTemplateChanging(ItemTemplate oldValue, ItemTemplate newValue);
        public ItemTemplate ItemTemplate {
            get { return _ItemTemplate; }
            set
            {
                    OnItemTemplateChanging(_ItemTemplate, value);
                    ChangeParent(value, ref _ItemTemplate, 
                    (ItemTemplate parent, Item child) => parent.Items.Remove(child), 
                    (ItemTemplate parent, Item child) => parent.Items.Add(child));
            }
        }

        partial void OnActorChanging(Actor oldValue, Actor newValue);
        public Actor Actor {
            get { return _Actor; }
            set
            {
                    OnActorChanging(_Actor, value);
                    ChangeParent(value, ref _Actor, 
                    (Actor parent, Item child) => parent.Items.Remove(child), 
                    (Actor parent, Item child) => parent.Items.Add(child));
            }
        }

        partial void OnRoomChanging(Room oldValue, Room newValue);
        public Room Room {
            get { return _Room; }
            set
            {
                    OnRoomChanging(_Room, value);
                    ChangeParent(value, ref _Room, 
                    (Room parent, Item child) => parent.Items.Remove(child), 
                    (Room parent, Item child) => parent.Items.Add(child));
            }
        }

        public static Item Load(SqlDataReader reader)
        {
            var entity = new Item();
            entity._Id = reader.GetGuid(0);
            entity._ItemTemplateId = reader.GetInt32(1);
            entity._CustomName = reader.GetSafeString(2);
            entity._ActorId = reader.GetSafeGuid(3);
            entity._RoomId = reader.GetSafeInt32(4);
            entity._Modified = reader.GetBoolean(5);
            entity.Stats = new JsonContainer(entity);
            entity.Stats.FromJson(reader.GetSafeString(6));
            entity._EquippedSlot = reader.GetSafeInt32(7);
            entity._HiddenTime = reader.GetSafeInt64(8);
            return entity;
        }

        public override void LoadRelationships()
        {
            ItemTemplate = Context.ItemTemplates.Single(x => x.Id == _ItemTemplateId);
            Actor = Context.Actors.SingleOrDefault(x => x.Id == _ActorId);
            Room = Context.Rooms.SingleOrDefault(x => x.Id == _RoomId);
        }

        private static void AddSqlParameters(SqlCommand c, Item e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@ItemTemplateId", (object)e.ItemTemplate?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@CustomName", (object)e.CustomName ?? DBNull.Value);
            c.Parameters.AddWithValue("@ActorId", (object)e.Actor?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@RoomId", (object)e.Room?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Modified", (object)e.Modified ?? DBNull.Value);
            c.Parameters.AddWithValue("@JSONStats", e.Stats.ToJson());
            c.Parameters.AddWithValue("@EquippedSlot", (object)e.EquippedSlot ?? DBNull.Value);
            c.Parameters.AddWithValue("@HiddenTime", (object)e.HiddenTime ?? DBNull.Value);
        }

        public static void GenerateInsertCommand(SqlCommand c, Item e)
        {
            c.CommandText = @"INSERT INTO [Item]([Id], [ItemTemplateId], [CustomName], [ActorId], [RoomId], [Modified], [JSONStats], [EquippedSlot], [HiddenTime])
                              VALUES (@Id, @ItemTemplateId, @CustomName, @ActorId, @RoomId, @Modified, @JSONStats, @EquippedSlot, @HiddenTime)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, Item e)
        {
            c.CommandText = @"UPDATE [Item] SET [Id] = @Id, [ItemTemplateId] = @ItemTemplateId, [CustomName] = @CustomName, [ActorId] = @ActorId, [RoomId] = @RoomId, [Modified] = @Modified, [JSONStats] = @JSONStats, [EquippedSlot] = @EquippedSlot, [HiddenTime] = @HiddenTime WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, Item e)
        {
            c.CommandText = @"DELETE FROM[Item] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}