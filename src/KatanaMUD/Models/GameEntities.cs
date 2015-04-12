using Spam;
using System;
using System.Data.SqlClient;

namespace KatanaMUD.Models
{
    public partial class GameEntities : Spam.EntityContext
    {
        public GameEntities(string connectionString) : base(connectionString)
        {
            Actors = new EntityContainer<Actor, Guid>(this);
            ArmorTypes = new EntityContainer<ArmorType, Int32>(this);
            ClassTemplates = new EntityContainer<ClassTemplate, Int32>(this);
            RaceTemplates = new EntityContainer<RaceTemplate, Int32>(this);
            Regions = new EntityContainer<Region, Int32>(this);
            Rooms = new EntityContainer<Room, Int32>(this);
            Settings = new EntityContainer<Setting, String>(this);
            TextBlocks = new EntityContainer<TextBlock, Int32>(this);
            Users = new EntityContainer<User, String>(this);
            WeaponTypes = new EntityContainer<WeaponType, Int32>(this);
        }

        public EntityContainer<Actor, Guid> Actors { get; private set; }
        public EntityContainer<ArmorType, Int32> ArmorTypes { get; private set; }
        public EntityContainer<ClassTemplate, Int32> ClassTemplates { get; private set; }
        public EntityContainer<RaceTemplate, Int32> RaceTemplates { get; private set; }
        public EntityContainer<Region, Int32> Regions { get; private set; }
        public EntityContainer<Room, Int32> Rooms { get; private set; }
        public EntityContainer<Setting, String> Settings { get; private set; }
        public EntityContainer<TextBlock, Int32> TextBlocks { get; private set; }
        public EntityContainer<User, String> Users { get; private set; }
        public EntityContainer<WeaponType, Int32> WeaponTypes { get; private set; }
        internal LinkEntityContainer<ClassTemplate, ArmorType, Int32, Int32> ClassTemplateArmorTypes = new LinkEntityContainer<ClassTemplate, ArmorType, Int32, Int32>("ClassTemplateArmorType", "ClassTemplateId", "ArmorTypeId");
        internal LinkEntityContainer<ClassTemplate, WeaponType, Int32, Int32> ClassTemplateWeaponTypes = new LinkEntityContainer<ClassTemplate, WeaponType, Int32, Int32>("ClassTemplateWeaponType", "ClassTemplateId", "WeaponTypeId");
        internal LinkEntityContainer<RaceTemplate, ClassTemplate, Int32, Int32> RaceClassRestrictions = new LinkEntityContainer<RaceTemplate, ClassTemplate, Int32, Int32>("RaceClassRestriction", "RaceTemplateId", "ClassTemplateId");
        protected override void LoadMetaData()
        {
            EntityMetadata meta;

            meta = new EntityMetadata() { EntityType = typeof(Actor), Container = Actors };
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Actor)e).User));
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Actor)e).Room));
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

            meta = new EntityMetadata() { EntityType = typeof(Region), Container = Regions };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => Region.GenerateInsertCommand(c, (Region)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => Region.GenerateUpdateCommand(c, (Region)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => Region.GenerateDeleteCommand(c, (Region)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(Room), Container = Rooms };
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Room)e).Region));
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Room)e).TextBlock));
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => Room.GenerateInsertCommand(c, (Room)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => Room.GenerateUpdateCommand(c, (Room)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => Room.GenerateDeleteCommand(c, (Room)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(Setting), Container = Settings };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => Setting.GenerateInsertCommand(c, (Setting)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => Setting.GenerateUpdateCommand(c, (Setting)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => Setting.GenerateDeleteCommand(c, (Setting)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(TextBlock), Container = TextBlocks };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => TextBlock.GenerateInsertCommand(c, (TextBlock)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => TextBlock.GenerateUpdateCommand(c, (TextBlock)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => TextBlock.GenerateDeleteCommand(c, (TextBlock)e);
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
            LoadData(connection, Regions, "Region", Region.Load);
            LoadData(connection, Rooms, "Room", Room.Load);
            LoadData(connection, Settings, "Setting", Setting.Load);
            LoadData(connection, TextBlocks, "TextBlock", TextBlock.Load);
            LoadData(connection, Users, "User", User.Load);
            LoadData(connection, WeaponTypes, "WeaponType", WeaponType.Load);
            ClassTemplateArmorTypes.Load(connection);
            ClassTemplateWeaponTypes.Load(connection);
            RaceClassRestrictions.Load(connection);
        }
    }
}