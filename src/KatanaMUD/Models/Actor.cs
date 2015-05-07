using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Actor : Entity<Guid>
    {
        public override Guid Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private Guid _Id;
        private String _Name;
        private String _Surname;
        private Int32 _ActorType;
        private Int32 _CharacterPoints;
        private String _UserId;
        private User _User;
        private Int32 _RoomId;
        private Room _Room;
        private Int32? _ClassTemplateId;
        private ClassTemplate _ClassTemplate;
        private Int32? _RaceTemplateId;
        private RaceTemplate _RaceTemplate;

        public Actor()
        {
            Stats = new JsonContainer(this);
            Cash = new JsonContainer(this);
            Abilities = new JsonContainer(this);
            Items = new ParentChildRelationshipContainer<Actor, Item, Guid>(this, child => child.Actor, (child, parent) => child.Actor= parent);
        }

        public Guid Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public String Surname { get { return _Surname; } set { _Surname = value; this.Changed(); } }
        public Int32 ActorType { get { return _ActorType; } set { _ActorType = value; this.Changed(); } }
        public Int32 CharacterPoints { get { return _CharacterPoints; } set { _CharacterPoints = value; this.Changed(); } }
        public dynamic Stats { get; private set; }
        public dynamic Cash { get; private set; }
        public dynamic Abilities { get; private set; }
        partial void OnUserChanging(User oldValue, User newValue);
        public User User {
            get { return _User; }
            set
            {
                    OnUserChanging(_User, value);
                    ChangeParent(value, ref _User, 
                    (User parent, Actor child) => parent.Actors.Remove(child), 
                    (User parent, Actor child) => parent.Actors.Add(child));
            }
        }

        partial void OnRoomChanging(Room oldValue, Room newValue);
        public Room Room {
            get { return _Room; }
            set
            {
                    OnRoomChanging(_Room, value);
                    ChangeParent(value, ref _Room, 
                    (Room parent, Actor child) => parent.Actors.Remove(child), 
                    (Room parent, Actor child) => parent.Actors.Add(child));
            }
        }

        partial void OnClassTemplateChanging(ClassTemplate oldValue, ClassTemplate newValue);
        public ClassTemplate ClassTemplate {
            get { return _ClassTemplate; }
            set
            {
                    OnClassTemplateChanging(_ClassTemplate, value);
                    ChangeParent(value, ref _ClassTemplate, 
                    (ClassTemplate parent, Actor child) => parent.Actors.Remove(child), 
                    (ClassTemplate parent, Actor child) => parent.Actors.Add(child));
            }
        }

        partial void OnRaceTemplateChanging(RaceTemplate oldValue, RaceTemplate newValue);
        public RaceTemplate RaceTemplate {
            get { return _RaceTemplate; }
            set
            {
                    OnRaceTemplateChanging(_RaceTemplate, value);
                    ChangeParent(value, ref _RaceTemplate, 
                    (RaceTemplate parent, Actor child) => parent.Actors.Remove(child), 
                    (RaceTemplate parent, Actor child) => parent.Actors.Add(child));
            }
        }

        public ICollection<Item> Items { get; private set; }
        public static Actor Load(SqlDataReader reader)
        {
            var entity = new Actor();
            entity._Id = reader.GetGuid(0);
            entity._Name = reader.GetString(1);
            entity._Surname = reader.GetSafeString(2);
            entity._ActorType = reader.GetInt32(3);
            entity._UserId = reader.GetSafeString(4);
            entity._RoomId = reader.GetInt32(5);
            entity._ClassTemplateId = reader.GetSafeInt32(6);
            entity._RaceTemplateId = reader.GetSafeInt32(7);
            entity._CharacterPoints = reader.GetInt32(8);
            entity.Stats = new JsonContainer(entity);
            entity.Stats.FromJson(reader.GetSafeString(9));
            entity.Cash = new JsonContainer(entity);
            entity.Cash.FromJson(reader.GetSafeString(10));
            entity.Abilities = new JsonContainer(entity);
            entity.Abilities.FromJson(reader.GetSafeString(11));
            return entity;
        }

        public override void LoadRelationships()
        {
            User = Context.Users.SingleOrDefault(x => x.Id == _UserId);
            Room = Context.Rooms.Single(x => x.Id == _RoomId);
            ClassTemplate = Context.ClassTemplates.SingleOrDefault(x => x.Id == _ClassTemplateId);
            RaceTemplate = Context.RaceTemplates.SingleOrDefault(x => x.Id == _RaceTemplateId);
        }

        private static void AddSqlParameters(SqlCommand c, Actor e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@Name", (object)e.Name ?? DBNull.Value);
            c.Parameters.AddWithValue("@Surname", (object)e.Surname ?? DBNull.Value);
            c.Parameters.AddWithValue("@ActorType", (object)e.ActorType ?? DBNull.Value);
            c.Parameters.AddWithValue("@UserId", (object)e.User?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@RoomId", (object)e.Room?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@ClassTemplateId", (object)e.ClassTemplate?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@RaceTemplateId", (object)e.RaceTemplate?.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@CharacterPoints", (object)e.CharacterPoints ?? DBNull.Value);
            c.Parameters.AddWithValue("@JSONStats", e.Stats.ToJson());
            c.Parameters.AddWithValue("@JSONCash", e.Cash.ToJson());
            c.Parameters.AddWithValue("@JSONAbilities", e.Abilities.ToJson());
        }

        public static void GenerateInsertCommand(SqlCommand c, Actor e)
        {
            c.CommandText = @"INSERT INTO [Actor]([Id], [Name], [Surname], [ActorType], [UserId], [RoomId], [ClassTemplateId], [RaceTemplateId], [CharacterPoints], [JSONStats], [JSONCash], [JSONAbilities])
                              VALUES (@Id, @Name, @Surname, @ActorType, @UserId, @RoomId, @ClassTemplateId, @RaceTemplateId, @CharacterPoints, @JSONStats, @JSONCash, @JSONAbilities)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, Actor e)
        {
            c.CommandText = @"UPDATE [Actor] SET [Id] = @Id, [Name] = @Name, [Surname] = @Surname, [ActorType] = @ActorType, [UserId] = @UserId, [RoomId] = @RoomId, [ClassTemplateId] = @ClassTemplateId, [RaceTemplateId] = @RaceTemplateId, [CharacterPoints] = @CharacterPoints, [JSONStats] = @JSONStats, [JSONCash] = @JSONCash, [JSONAbilities] = @JSONAbilities WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, Actor e)
        {
            c.CommandText = @"DELETE FROM[Actor] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}