using Spam;
using System;
using System.Data.SqlClient;

namespace KatanaMUD.Models
{
    public class GameEntities : Spam.EntityContext
    {
        public GameEntities(string connectionString) : base(connectionString)
        {
            Actors = new EntityContainer<Actor, Guid>(this);
            ArmorTypes = new EntityContainer<ArmorType, Int32>(this);
            ClassTemplates = new EntityContainer<ClassTemplate, Int32>(this);
            RaceTemplates = new EntityContainer<RaceTemplate, Int32>(this);
            Users = new EntityContainer<User, String>(this);
            WeaponTypes = new EntityContainer<WeaponType, String>(this);
        }

        public EntityContainer<Actor, Guid> Actors { get; private set; }
        public EntityContainer<ArmorType, Int32> ArmorTypes { get; private set; }
        public EntityContainer<ClassTemplate, Int32> ClassTemplates { get; private set; }
        public EntityContainer<RaceTemplate, Int32> RaceTemplates { get; private set; }
        public EntityContainer<User, String> Users { get; private set; }
        public EntityContainer<WeaponType, String> WeaponTypes { get; private set; }
        internal LinkEntityContainer<ClassTemplate, ArmorType, Int32, Int32> ClassTemplateArmorTypes = new LinkEntityContainer<ClassTemplate, ArmorType, Int32, Int32>("ClassTemplateArmorType", "ClassTemplateId", "ArmorTypeId");
        internal LinkEntityContainer<ClassTemplate, WeaponType, Int32, String> ClassTemplateWeaponTypes = new LinkEntityContainer<ClassTemplate, WeaponType, Int32, String>("ClassTemplateWeaponType", "ClassTemplateId", "WeaponTypeId");
        internal LinkEntityContainer<RaceTemplate, ClassTemplate, Int32, Int32> RaceClassRestrictions = new LinkEntityContainer<RaceTemplate, ClassTemplate, Int32, Int32>("RaceClassRestriction", "RaceTemplateId", "ClassTemplateId");
        protected override void LoadMetaData()
        {
            EntityMetadata meta;

            meta = new EntityMetadata() { EntityType = typeof(Actor), Container = Actors };
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Actor)e).User));
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Actor)e).ClassTemplate));
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Actor)e).RaceTemplate));
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => Actor.GenerateInsertCommand(c, (Actor)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => Actor.GenerateUpdateCommand(c, (Actor)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => Actor.GenerateDeleteCommand(c, (Actor)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(ArmorType), Container = ArmorTypes };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => ArmorType.GenerateInsertCommand(c, (ArmorType)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => ArmorType.GenerateUpdateCommand(c, (ArmorType)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => ArmorType.GenerateDeleteCommand(c, (ArmorType)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(ClassTemplate), Container = ClassTemplates };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => ClassTemplate.GenerateInsertCommand(c, (ClassTemplate)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => ClassTemplate.GenerateUpdateCommand(c, (ClassTemplate)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => ClassTemplate.GenerateDeleteCommand(c, (ClassTemplate)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(RaceTemplate), Container = RaceTemplates };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => RaceTemplate.GenerateInsertCommand(c, (RaceTemplate)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => RaceTemplate.GenerateUpdateCommand(c, (RaceTemplate)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => RaceTemplate.GenerateDeleteCommand(c, (RaceTemplate)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(User), Container = Users };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => User.GenerateInsertCommand(c, (User)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => User.GenerateUpdateCommand(c, (User)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => User.GenerateDeleteCommand(c, (User)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(WeaponType), Container = WeaponTypes };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => WeaponType.GenerateInsertCommand(c, (WeaponType)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => WeaponType.GenerateUpdateCommand(c, (WeaponType)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => WeaponType.GenerateDeleteCommand(c, (WeaponType)e);
            EntityTypes.Add(meta);

            this.LinkTypes.Add(ClassTemplateArmorTypes);
            this.LinkTypes.Add(ClassTemplateWeaponTypes);
            this.LinkTypes.Add(RaceClassRestrictions);
        }

        protected override void LoadAllData(SqlConnection connection)
        {
            LoadData(connection, Actors, "Actor", Actor.Load);
            LoadData(connection, ArmorTypes, "ArmorType", ArmorType.Load);
            LoadData(connection, ClassTemplates, "ClassTemplate", ClassTemplate.Load);
            LoadData(connection, RaceTemplates, "RaceTemplate", RaceTemplate.Load);
            LoadData(connection, Users, "User", User.Load);
            LoadData(connection, WeaponTypes, "WeaponType", WeaponType.Load);
            ClassTemplateArmorTypes.Load(connection);
            ClassTemplateWeaponTypes.Load(connection);
            RaceClassRestrictions.Load(connection);
        }
    }
}