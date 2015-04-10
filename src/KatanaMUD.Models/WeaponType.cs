using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public class WeaponType : Entity<String>
    {
        public override String Key { get { return WepId; } set { WepId = value; } }
        private GameEntities Context => (GameEntities)__context;
        private String _WepId;
        private String _Name;

        public WeaponType()
        {
            ClassTemplates = new ObservableHashSet<ClassTemplate>();
			ClassTemplates.ItemsAdded += ClassTemplates_ItemsAdded;
			ClassTemplates.ItemsRemoved += ClassTemplates_ItemsRemoved;
        }

        public String WepId { get { return _WepId; } set { _WepId = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public ObservableHashSet<ClassTemplate> ClassTemplates { get; private set; }
        public static WeaponType Load(SqlDataReader reader)
        {
            var entity = new WeaponType();
            entity._WepId = reader.GetString(0);
            entity._Name = reader.GetString(1);
            return entity;
        }

        public override void LoadRelationships()
        {
            ClassTemplates.AddRange(Context.ClassTemplateWeaponTypes.Where(x => x.Item2 == this.WepId).Select(x => Context.ClassTemplates.Single(y => y.Id == x.Item1)), true);
        }

        private static void AddSqlParameters(SqlCommand c, WeaponType e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@WepId", e.WepId);
            c.Parameters.AddWithValue("@Name", e.Name);
        }

        public static void GenerateInsertCommand(SqlCommand c, WeaponType e)
        {
            c.CommandText = @"INSERT INTO [WeaponType]([WepId], [Name]
                              VALUES (@WepId, @Name)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, WeaponType e)
        {
            c.CommandText = @"UPDATE [WeaponType] SET [WepId] = @WepId, [Name] = @Name WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, WeaponType e)
        {
            c.CommandText = @"DELETE FROM[WeaponType] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.WepId);
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