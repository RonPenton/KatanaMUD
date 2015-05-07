using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class ItemTemplate : Entity<Int32>
    {
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;
        private String _Description;
        private Int32 _Type;
        private Int32? _EquipType;
        private Int32? _WeaponType;
        private Int32? _Limit;
        private Boolean _Fixed;
        private Boolean _NotDroppable;
        private Boolean _DestroyOnDeath;
        private Boolean _NotRobable;
        private Int64 _Cost;
        private Int32 _Level;

        public ItemTemplate()
        {
            Stats = new JsonContainer(this);
            Requirements = new JsonContainer(this);
            Items = new ParentChildRelationshipContainer<ItemTemplate, Item, Guid>(this, child => child.ItemTemplate, (child, parent) => child.ItemTemplate= parent);
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public String Description { get { return _Description; } set { _Description = value; this.Changed(); } }
        public Int32 Type { get { return _Type; } set { _Type = value; this.Changed(); } }
        public EquipmentSlot? EquipType { get { return (EquipmentSlot?)_EquipType; } set { _EquipType = (Int32?)value; this.Changed(); } }
        public WeaponType? WeaponType { get { return (WeaponType?)_WeaponType; } set { _WeaponType = (Int32?)value; this.Changed(); } }
        public Int32? Limit { get { return _Limit; } set { _Limit = value; this.Changed(); } }
        public Boolean Fixed { get { return _Fixed; } set { _Fixed = value; this.Changed(); } }
        public Boolean NotDroppable { get { return _NotDroppable; } set { _NotDroppable = value; this.Changed(); } }
        public Boolean DestroyOnDeath { get { return _DestroyOnDeath; } set { _DestroyOnDeath = value; this.Changed(); } }
        public Boolean NotRobable { get { return _NotRobable; } set { _NotRobable = value; this.Changed(); } }
        public Int64 Cost { get { return _Cost; } set { _Cost = value; this.Changed(); } }
        public Int32 Level { get { return _Level; } set { _Level = value; this.Changed(); } }
        public dynamic Stats { get; private set; }
        public dynamic Requirements { get; private set; }
        public ICollection<Item> Items { get; private set; }
        public static ItemTemplate Load(SqlDataReader reader)
        {
            var entity = new ItemTemplate();
            entity._Id = reader.GetInt32(0);
            entity._Name = reader.GetString(1);
            entity._Description = reader.GetSafeString(2);
            entity._Type = reader.GetInt32(3);
            entity._EquipType = reader.GetSafeInt32(4);
            entity._WeaponType = reader.GetSafeInt32(5);
            entity._Limit = reader.GetSafeInt32(6);
            entity._Fixed = reader.GetBoolean(7);
            entity._NotDroppable = reader.GetBoolean(8);
            entity._DestroyOnDeath = reader.GetBoolean(9);
            entity._NotRobable = reader.GetBoolean(10);
            entity._Cost = reader.GetInt64(11);
            entity._Level = reader.GetInt32(12);
            entity.Stats = new JsonContainer(entity);
            entity.Stats.FromJson(reader.GetSafeString(13));
            entity.Requirements = new JsonContainer(entity);
            entity.Requirements.FromJson(reader.GetSafeString(14));
            return entity;
        }

        public override void LoadRelationships()
        {
        }

        private static void AddSqlParameters(SqlCommand c, ItemTemplate e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Name", (object)e.Name ?? DBNull.Value);
            c.Parameters.AddWithValue("@Description", (object)e.Description ?? DBNull.Value);
            c.Parameters.AddWithValue("@Type", (object)e.Type ?? DBNull.Value);
            c.Parameters.AddWithValue("@EquipType", (object)e.EquipType ?? DBNull.Value);
            c.Parameters.AddWithValue("@WeaponType", (object)e.WeaponType ?? DBNull.Value);
            c.Parameters.AddWithValue("@Limit", (object)e.Limit ?? DBNull.Value);
            c.Parameters.AddWithValue("@Fixed", (object)e.Fixed ?? DBNull.Value);
            c.Parameters.AddWithValue("@NotDroppable", (object)e.NotDroppable ?? DBNull.Value);
            c.Parameters.AddWithValue("@DestroyOnDeath", (object)e.DestroyOnDeath ?? DBNull.Value);
            c.Parameters.AddWithValue("@NotRobable", (object)e.NotRobable ?? DBNull.Value);
            c.Parameters.AddWithValue("@Cost", (object)e.Cost ?? DBNull.Value);
            c.Parameters.AddWithValue("@Level", (object)e.Level ?? DBNull.Value);
            c.Parameters.AddWithValue("@JSONStats", e.Stats.ToJson());
            c.Parameters.AddWithValue("@JSONRequirements", e.Requirements.ToJson());
        }

        public static void GenerateInsertCommand(SqlCommand c, ItemTemplate e)
        {
            c.CommandText = @"INSERT INTO [ItemTemplate]([Id], [Name], [Description], [Type], [EquipType], [WeaponType], [Limit], [Fixed], [NotDroppable], [DestroyOnDeath], [NotRobable], [Cost], [Level], [JSONStats], [JSONRequirements])
                              VALUES (@Id, @Name, @Description, @Type, @EquipType, @WeaponType, @Limit, @Fixed, @NotDroppable, @DestroyOnDeath, @NotRobable, @Cost, @Level, @JSONStats, @JSONRequirements)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, ItemTemplate e)
        {
            c.CommandText = @"UPDATE [ItemTemplate] SET [Id] = @Id, [Name] = @Name, [Description] = @Description, [Type] = @Type, [EquipType] = @EquipType, [WeaponType] = @WeaponType, [Limit] = @Limit, [Fixed] = @Fixed, [NotDroppable] = @NotDroppable, [DestroyOnDeath] = @DestroyOnDeath, [NotRobable] = @NotRobable, [Cost] = @Cost, [Level] = @Level, [JSONStats] = @JSONStats, [JSONRequirements] = @JSONRequirements WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, ItemTemplate e)
        {
            c.CommandText = @"DELETE FROM[ItemTemplate] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}