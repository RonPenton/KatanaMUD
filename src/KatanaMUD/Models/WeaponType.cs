using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class WeaponType : Entity<Int32>
    {
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;

        public WeaponType()
        {
            ClassTemplates = new ObservableHashSet<ClassTemplate>();
			ClassTemplates.ItemsAdded += ClassTemplates_ItemsAdded;
			ClassTemplates.ItemsRemoved += ClassTemplates_ItemsRemoved;
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public ObservableHashSet<ClassTemplate> ClassTemplates { get; private set; }
        public static WeaponType Load(SqlDataReader reader)
        {
            var entity = new WeaponType();
            entity._Id = reader.GetInt32(0);
            entity._Name = reader.GetString(1);
            return entity;
        }

        public override void LoadRelationships()
        {
            ClassTemplates.AddRange(Context.ClassTemplateWeaponTypes.Where(x => x.Item2 == this.Id).Select(x => Context.ClassTemplates.Single(y => y.Id == x.Item1)), true);
        }

        private static void AddSqlParameters(SqlCommand c, WeaponType e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Name", (object)e.Name ?? DBNull.Value);
        }

        public static void GenerateInsertCommand(SqlCommand c, WeaponType e)
        {
            c.CommandText = @"INSERT INTO [WeaponType]([Id], [Name])
                              VALUES (@Id, @Name)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, WeaponType e)
        {
            c.CommandText = @"UPDATE [WeaponType] SET [Id] = @Id, [Name] = @Name WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, WeaponType e)
        {
            c.CommandText = @"DELETE FROM[WeaponType] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
		private void ClassTemplates_ItemsAdded(object sender, CollectionChangedEventArgs<ClassTemplate> e)
        {
            foreach (var item in e.Items)
            {
                item.WeaponTypes.Add(this, true);
                Context.ClassTemplateWeaponTypes.Link(item.Key, this.Key, false);
            }
        }
		private void ClassTemplates_ItemsRemoved(object sender, CollectionChangedEventArgs<ClassTemplate> e)
		{
			foreach (var item in e.Items)
			{
				item.WeaponTypes.Remove(this, true);
				Context.ClassTemplateWeaponTypes.Unlink(item.Key, this.Key);
			}
		}

    }
}