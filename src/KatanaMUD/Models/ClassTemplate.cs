using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class ClassTemplate : Entity<Int32>
    {
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;
        private String _Description;

        public ClassTemplate()
        {
            Stats = new JsonContainer(this);
            Actors = new ParentChildRelationshipContainer<ClassTemplate, Actor, Guid>(this, child => child.ClassTemplate, (child, parent) => child.ClassTemplate= parent);
            RaceTemplates = new ObservableHashSet<RaceTemplate>();
            RaceTemplates.ItemsAdded += RaceTemplates_ItemsAdded;
            RaceTemplates.ItemsRemoved += RaceTemplates_ItemsRemoved;
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public String Description { get { return _Description; } set { _Description = value; this.Changed(); } }
        public dynamic Stats { get; private set; }
        public ICollection<Actor> Actors { get; private set; }
        public ObservableHashSet<RaceTemplate> RaceTemplates { get; private set; }
        public static ClassTemplate Load(SqlDataReader reader)
        {
            var entity = new ClassTemplate();
            entity._Id = reader.GetInt32(0);
            entity._Name = reader.GetString(1);
            entity._Description = reader.GetSafeString(2);
            entity.Stats = new JsonContainer(entity);
            entity.Stats.FromJson(reader.GetSafeString(3));
            return entity;
        }

        public override void LoadRelationships()
        {
            RaceTemplates.AddRange(Context.RaceClassRestrictions.Where(x => x.Item2 == this.Id).Select(x => Context.RaceTemplates.Single(y => y.Id == x.Item1)), true);
        }

        private static void AddSqlParameters(SqlCommand c, ClassTemplate e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Name", (object)e.Name ?? DBNull.Value);
            c.Parameters.AddWithValue("@Description", (object)e.Description ?? DBNull.Value);
            c.Parameters.AddWithValue("@JSONStats", e.Stats.ToJson());
        }

        public static void GenerateInsertCommand(SqlCommand c, ClassTemplate e)
        {
            c.CommandText = @"INSERT INTO [ClassTemplate]([Id], [Name], [Description], [JSONStats])
                              VALUES (@Id, @Name, @Description, @JSONStats)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, ClassTemplate e)
        {
            c.CommandText = @"UPDATE [ClassTemplate] SET [Id] = @Id, [Name] = @Name, [Description] = @Description, [JSONStats] = @JSONStats WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, ClassTemplate e)
        {
            c.CommandText = @"DELETE FROM[ClassTemplate] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
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