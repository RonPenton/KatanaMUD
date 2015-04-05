using System;
using System.Data.SqlClient;
using Spam;
using System.Collections.Generic;

namespace KatanaMUD.Models.Test
{
	public partial class ClassTemplate : Entity<int>
	{
		public override int Key
		{
			get { return Id; }
			set { Id = value; }
		}

		private int _id;
        private string _name;
        private string _description;

        public ClassTemplate()
        {
            Stats = new JsonContainer(this);
            Actors = new ParentChildRelationshipContainer<ClassTemplate, Actor, Guid>(this, child => child.ClassTemplate, (child, parent) => child.ClassTemplate = parent);
            Actors = new ManyToManyRelationshipContainer<ClassTemplate, RaceTemplate, int, int>(this, child => child.ClassTemplate, (child, parent) => child.ClassTemplate = parent);
        }

        public int Id { get { return _id; } set { _id = value; this.Changed(); } }
        public string Name { get { return _name; } set { _name = value; this.Changed(); } }
        public string Description { get { return _description; } set { _description = value; this.Changed(); } }
        public dynamic Stats { get; private set; }
        public ICollection<Actor> Actors { get; private set; }
        public ICollection<RaceTemplate> RaceTemplates { get; private set; }



        public static ClassTemplate Load(SqlDataReader reader)
        {
            var entity = new ClassTemplate();
            entity.Id = reader.GetInt32(0);
            entity.Name = reader.GetSafeString(1);
            entity.Description = reader.GetSafeString(2);
            entity.Stats = new JsonContainer(entity);
            entity.Stats.FromJson(reader.GetSafeString(3));
            return entity;
        }

        private static void AddSqlParameters(SqlCommand c, RaceTemplate e)
        {
            c.Parameters.AddWithValue("@Id", e.Id);
            c.Parameters.AddWithValue("@Name", e.Name);
            c.Parameters.AddWithValue("@Description", e.Description);
            c.Parameters.AddWithValue("@JSONStats", e.Stats.ToJson());
        }

        public static void GenerateInsertCommand(SqlCommand c, RaceTemplate e)
        {
            c.CommandText = @"INSERT INTO [RaceTemplate] ([Id], [Name], [Description], [JSONStats])
                              VALUES (@Id, @Name, @Description, @JSONStats)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, RaceTemplate e)
        {
            c.CommandText = @"UPDATE [RaceTemplate] SET [Name] = @Name, [Description] = @Description, [JSONStats] = @JSONStats
                              WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, RaceTemplate e)
        {
            c.CommandText = @"DELETE FROM [RaceTemplate] WHERE [Id] = @Id";
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}