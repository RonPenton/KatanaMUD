using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace KatanaMUD.Models
{
    public class ClassTemplate : Entity<Int32>
    {
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;
        private Int32 _HpMin;
        private Int32 _HpMax;
        private String _Description;

        public ClassTemplate()
        {
            Actors = new ParentChildRelationshipContainer<ClassTemplate, Actor, Guid>(this, child => child.ClassTemplate, (child, parent) => child.ClassTemplate= parent);
            ArmorTypes = new ObservableHashSet<ArmorType>();
			ArmorTypes.ItemsAdded += ArmorTypes_ItemsAdded;
			ArmorTypes.ItemsRemoved += ArmorTypes_ItemsRemoved;
            WeaponTypes = new ObservableHashSet<WeaponType>();
			WeaponTypes.ItemsAdded += WeaponTypes_ItemsAdded;
			WeaponTypes.ItemsRemoved += WeaponTypes_ItemsRemoved;
            RaceTemplates = new ObservableHashSet<RaceTemplate>();
			RaceTemplates.ItemsAdded += RaceTemplates_ItemsAdded;
			RaceTemplates.ItemsRemoved += RaceTemplates_ItemsRemoved;
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public Int32 HpMin { get { return _HpMin; } set { _HpMin = value; this.Changed(); } }
        public Int32 HpMax { get { return _HpMax; } set { _HpMax = value; this.Changed(); } }
        public String Description { get { return _Description; } set { _Description = value; this.Changed(); } }
        public ICollection<Actor> Actors { get; private set; }
        public ObservableHashSet<ArmorType> ArmorTypes { get; private set; }
        public ObservableHashSet<WeaponType> WeaponTypes { get; private set; }
        public ObservableHashSet<RaceTemplate> RaceTemplates { get; private set; }
        public static ClassTemplate Load(SqlDataReader reader)
        {
            var entity = new ClassTemplate();
            entity._Id = reader.GetInt32(0);
            entity._Name = reader.GetString(1);
            entity._HpMin = reader.GetInt32(2);
            entity._HpMax = reader.GetInt32(3);
            entity._Description = reader.GetSafeString(4);
            return entity;
        }

        private static void AddSqlParameters(SqlCommand c, ClassTemplate e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
            c.Parameters.AddWithValue("@Name", e.Name);
            c.Parameters.AddWithValue("@HpMin", e.HpMin);
            c.Parameters.AddWithValue("@HpMax", e.HpMax);
            c.Parameters.AddWithValue("@Description", e.Description);
        }

        public static void GenerateInsertCommand(SqlCommand c, ClassTemplate e)
        {
            c.CommandText = @"INSERT INTO [ClassTemplate]([Id], [Name], [HpMin], [HpMax], [Description]
                              VALUES (@Id, @Name, @HpMin, @HpMax, @Description)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, ClassTemplate e)
        {
            c.CommandText = @"UPDATE [ClassTemplate] [KatanaMUD.EntityGenerator.ColumnMetadata] @KatanaMUD.EntityGenerator.ColumnMetadata, [KatanaMUD.EntityGenerator.ColumnMetadata] @KatanaMUD.EntityGenerator.ColumnMetadata, [KatanaMUD.EntityGenerator.ColumnMetadata] @KatanaMUD.EntityGenerator.ColumnMetadata, [KatanaMUD.EntityGenerator.ColumnMetadata] @KatanaMUD.EntityGenerator.ColumnMetadata, [KatanaMUD.EntityGenerator.ColumnMetadata] @KatanaMUD.EntityGenerator.ColumnMetadata                              WHERE [Id] = @Id";             AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, ClassTemplate e)
        {
            c.CommandText = @"DELETE FROM[ClassTemplate] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
		private void ArmorTypes_ItemsAdded(object sender, CollectionChangedEventArgs<ArmorType> e)
        {
            foreach (var item in e.Items)
            {
                item.ClassTemplates.Add(this, true);
                Context.ClassTemplateArmorTypes.Link(this.Key, item.Key, false);
            }
        }
		private void ArmorTypes_ItemsRemoved(object sender, CollectionChangedEventArgs<ArmorType> e)
		{
			foreach (var item in e.Items)
			{
				item.ClassTemplates.Remove(this, true);
				Context.ClassTemplateArmorTypes.Unlink(this.Key, item.Key);
			}
		}

		private void WeaponTypes_ItemsAdded(object sender, CollectionChangedEventArgs<WeaponType> e)
        {
            foreach (var item in e.Items)
            {
                item.ClassTemplates.Add(this, true);
                Context.ClassTemplateWeaponTypes.Link(this.Key, item.Key, false);
            }
        }
		private void WeaponTypes_ItemsRemoved(object sender, CollectionChangedEventArgs<WeaponType> e)
		{
			foreach (var item in e.Items)
			{
				item.ClassTemplates.Remove(this, true);
				Context.ClassTemplateWeaponTypes.Unlink(this.Key, item.Key);
			}
		}

		private void RaceTemplates_ItemsAdded(object sender, CollectionChangedEventArgs<RaceTemplate> e)
        {
            foreach (var item in e.Items)
            {
                item.ClassTemplates.Add(this, true);
                Context.RaceClassRestrictions.Link(item.Key, this.Key, false);
            }
        }
		private void RaceTemplates_ItemsRemoved(object sender, CollectionChangedEventArgs<RaceTemplate> e)
		{
			foreach (var item in e.Items)
			{
				item.ClassTemplates.Remove(this, true);
				Context.RaceClassRestrictions.Unlink(item.Key, this.Key);
			}
		}

    }
}