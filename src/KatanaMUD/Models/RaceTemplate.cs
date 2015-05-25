using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class RaceTemplate : Entity<Int32>
    {
        partial void OnConstruct();
        partial void OnLoaded();
        public override Int32 Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Int32 _Id;
        private String _Name;
        private String _Description;

        public RaceTemplate()
        {
            Stats = new Spam.JsonContainer(this, null);
            Actors = new ParentChildRelationshipContainer<RaceTemplate, Actor, Guid>(this, child => child.RaceTemplate, (child, parent) => child.RaceTemplate= parent);
            ClassTemplates = new ObservableHashSet<ClassTemplate>();
            ClassTemplates.ItemsAdded += ClassTemplates_ItemsAdded;
            ClassTemplates.ItemsRemoved += ClassTemplates_ItemsRemoved;
            OnConstruct();
        }

        public Int32 Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public String Description { get { return _Description; } set { _Description = value; this.Changed(); } }
        public Spam.JsonContainer Stats { get; private set; }
        public ICollection<Actor> Actors { get; private set; }
        public ObservableHashSet<ClassTemplate> ClassTemplates { get; private set; }
        public static RaceTemplate Load(SqlDataReader reader)
        {
            var entity = new RaceTemplate();
            entity._Id = reader.GetInt32(0);
            entity._Name = reader.GetString(1);
            entity._Description = reader.GetSafeString(2);
            entity.Stats = new Spam.JsonContainer(entity, reader.GetSafeString(3));
            entity.OnLoaded();
            return entity;
        }

        public override void LoadRelationships()
        {
            ClassTemplates.AddRange(Context.RaceClassRestrictions.Where(x => x.Item1 == this.Id).Select(x => Context.ClassTemplates.Single(y => y.Id == x.Item2)), true);
        }

        private static void AddSqlParameters(SqlCommand c, RaceTemplate e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Name", (object)e.Name ?? DBNull.Value);
            c.Parameters.AddWithValue("@Description", (object)e.Description ?? DBNull.Value);
            c.Parameters.AddWithValue("@Stats", e.Stats.Serialize());
        }

        public static void GenerateInsertCommand(SqlCommand c, RaceTemplate e)
        {
            c.CommandText = @"INSERT INTO [RaceTemplate]([Id], [Name], [Description], [Stats])
                              VALUES (@Id, @Name, @Description, @Stats)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, RaceTemplate e)
        {
            c.CommandText = @"UPDATE [RaceTemplate] SET [Id] = @Id, [Name] = @Name, [Description] = @Description, [Stats] = @Stats WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, RaceTemplate e)
        {
            c.CommandText = @"DELETE FROM[RaceTemplate] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
        private void ClassTemplates_ItemsAdded(object sender, CollectionChangedEventArgs<ClassTemplate> e)
        {
            foreach (var item in e.Items)
            {
                item.RaceTemplates.Add(this, true);
                Context.RaceClassRestrictions.Link(this.Key, item.Key, false);
            }
        }
        private void ClassTemplates_ItemsRemoved(object sender, CollectionChangedEventArgs<ClassTemplate> e)
        {
            foreach (var item in e.Items)
            {
                item.RaceTemplates.Remove(this, true);
                Context.RaceClassRestrictions.Unlink(this.Key, item.Key);
            }
        }

    }
}