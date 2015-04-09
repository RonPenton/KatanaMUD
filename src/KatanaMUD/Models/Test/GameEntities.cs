using Spam;
using System;
using System.Data.SqlClient;

namespace KatanaMUD.Models.Test
{
    public class GameEntities : Spam.EntityContext
    {
        public GameEntities(string connectionString)
            : base(connectionString)
        {
            Races = new EntityContainer<RaceTemplate, int>(this);
            Actors = new EntityContainer<Actor, Guid>(this);
        }

        public EntityContainer<RaceTemplate, int> Races { get; private set; } 
        public EntityContainer<Actor, Guid> Actors { get; private set; }
        internal LinkEntityContainer<ClassTemplate, RaceTemplate, int, int> ClassTemplatesRaceTemplates = new LinkEntityContainer<ClassTemplate, RaceTemplate, int, int>("ClassTemplateRaceTemplate", "ClassTemplateId", "RaceTemplateId");

        protected override void LoadMetaData()
        {
            EntityMetadata meta;

            meta = new EntityMetadata() { EntityType = typeof(RaceTemplate), Container = Races };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => RaceTemplate.GenerateInsertCommand(c, (RaceTemplate)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => RaceTemplate.GenerateUpdateCommand(c, (RaceTemplate)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => RaceTemplate.GenerateDeleteCommand(c, (RaceTemplate)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(Actor), Container = Actors };
            meta.Relationships.Add(new EntityRelationship((IEntity x) => ((Actor)x).RaceTemplate));
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => Actor.GenerateInsertCommand(c, (Actor)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => Actor.GenerateUpdateCommand(c, (Actor)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => Actor.GenerateDeleteCommand(c, (Actor)e);
            EntityTypes.Add(meta);

            this.LinkTypes.Add(ClassTemplatesRaceTemplates);
        }

        protected override void LoadAllData(SqlConnection connection)
		{
            LoadData(connection, Races, "RaceTemplate", RaceTemplate.Load);
            LoadData(connection, Actors, "Actor", Actor.Load);
            ClassTemplatesRaceTemplates.Load(connection);
        }
    }
}