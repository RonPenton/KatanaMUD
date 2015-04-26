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
            ClassTemplates = new EntityContainer<ClassTemplate, Int32>(this);
            Currencys = new EntityContainer<Currency, Int32>(this);
            Items = new EntityContainer<Item, Guid>(this);
            ItemTemplates = new EntityContainer<ItemTemplate, Int32>(this);
            RaceTemplates = new EntityContainer<RaceTemplate, Int32>(this);
            Regions = new EntityContainer<Region, Int32>(this);
            Rooms = new EntityContainer<Room, Int32>(this);
            Settings = new EntityContainer<Setting, String>(this);
            TextBlocks = new EntityContainer<TextBlock, Int32>(this);
            Users = new EntityContainer<User, String>(this);
        }

        public EntityContainer<Actor, Guid> Actors { get; private set; }
        public EntityContainer<ClassTemplate, Int32> ClassTemplates { get; private set; }
        public EntityContainer<Currency, Int32> Currencys { get; private set; }
        public EntityContainer<Item, Guid> Items { get; private set; }
        public EntityContainer<ItemTemplate, Int32> ItemTemplates { get; private set; }
        public EntityContainer<RaceTemplate, Int32> RaceTemplates { get; private set; }
        public EntityContainer<Region, Int32> Regions { get; private set; }
        public EntityContainer<Room, Int32> Rooms { get; private set; }
        public EntityContainer<Setting, String> Settings { get; private set; }
        public EntityContainer<TextBlock, Int32> TextBlocks { get; private set; }
        public EntityContainer<User, String> Users { get; private set; }
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

            meta = new EntityMetadata() { EntityType = typeof(ClassTemplate), Container = ClassTemplates };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => ClassTemplate.GenerateInsertCommand(c, (ClassTemplate)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => ClassTemplate.GenerateUpdateCommand(c, (ClassTemplate)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => ClassTemplate.GenerateDeleteCommand(c, (ClassTemplate)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(Currency), Container = Currencys };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => Currency.GenerateInsertCommand(c, (Currency)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => Currency.GenerateUpdateCommand(c, (Currency)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => Currency.GenerateDeleteCommand(c, (Currency)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(Item), Container = Items };
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Item)e).ItemTemplate));
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Item)e).Actor));
            meta.Relationships.Add(new EntityRelationship((IEntity e) => ((Item)e).Room));
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => Item.GenerateInsertCommand(c, (Item)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => Item.GenerateUpdateCommand(c, (Item)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => Item.GenerateDeleteCommand(c, (Item)e);
            EntityTypes.Add(meta);

            meta = new EntityMetadata() { EntityType = typeof(ItemTemplate), Container = ItemTemplates };
            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => ItemTemplate.GenerateInsertCommand(c, (ItemTemplate)e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => ItemTemplate.GenerateUpdateCommand(c, (ItemTemplate)e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => ItemTemplate.GenerateDeleteCommand(c, (ItemTemplate)e);
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

            this.LinkTypes.Add(RaceClassRestrictions);
        }

        protected override void LoadAllData(SqlConnection connection)
        {
            LoadData(connection, Actors, "Actor", Actor.Load);
            LoadData(connection, ClassTemplates, "ClassTemplate", ClassTemplate.Load);
            LoadData(connection, Currencys, "Currency", Currency.Load);
            LoadData(connection, Items, "Item", Item.Load);
            LoadData(connection, ItemTemplates, "ItemTemplate", ItemTemplate.Load);
            LoadData(connection, RaceTemplates, "RaceTemplate", RaceTemplate.Load);
            LoadData(connection, Regions, "Region", Region.Load);
            LoadData(connection, Rooms, "Room", Room.Load);
            LoadData(connection, Settings, "Setting", Setting.Load);
            LoadData(connection, TextBlocks, "TextBlock", TextBlock.Load);
            LoadData(connection, Users, "User", User.Load);
            RaceClassRestrictions.Load(connection);
        }
    }
}