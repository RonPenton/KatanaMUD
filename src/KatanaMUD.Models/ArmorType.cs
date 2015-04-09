using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace KatanaMUD.Models
{
    public class ArmorType : Entity<Int32>
    {
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;

        public ArmorType()
        {
            ClassTemplates = new ObservableHashSet<ClassTemplate>();
			ClassTemplates.ItemsAdded += ClassTemplates_ItemsAdded;
			ClassTemplates.ItemsRemoved += ClassTemplates_ItemsRemoved;
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public ObservableHashSet<ClassTemplate> ClassTemplates { get; private set; }
        public static ArmorType Load(SqlDataReader reader)
        {
            var entity = new ArmorType();
            entity._Id = reader.GetInt32(0);
            entity._Name = reader.GetString(1);
            return entity;
        }

        private static void AddSqlParameters(SqlCommand c, ArmorType e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
            c.Parameters.AddWithValue("@Name", e.Name);
        }

        public static void GenerateInsertCommand(SqlCommand c, ArmorType e)
        {
            c.CommandText = @"INSERT INTO [ArmorType]([Id], [Name]
                              VALUES (@Id, @Name)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, ArmorType e)
        {
            c.CommandText = @"UPDATE [ArmorType] [KatanaMUD.EntityGenerator.ColumnMetadata] @KatanaMUD.EntityGenerator.ColumnMetadata, [KatanaMUD.EntityGenerator.ColumnMetadata] @KatanaMUD.EntityGenerator.ColumnMetadata                              WHERE [Id] = @Id";             AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, ArmorType e)
        {
            c.CommandText = @"DELETE FROM[ArmorType] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
		private void ClassTemplates_ItemsAdded(object sender, CollectionChangedEventArgs<ClassTemplate> e)
        {
            foreach (var item in e.Items)
            {
                item.ArmorTypes.Add(this, true);
                Context.ClassTemplateArmorTypes.Link(item.Key, this.Key, false);
            }
        }
		private void ClassTemplates_ItemsRemoved(object sender, CollectionChangedEventArgs<ClassTemplate> e)
		{
			foreach (var item in e.Items)
			{
				item.ArmorTypes.Remove(this, true);
				Context.ClassTemplateArmorTypes.Unlink(item.Key, this.Key);
			}
		}

    }
}