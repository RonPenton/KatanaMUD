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
        private Int32 _RoomId;
        private Int32 _CharacterPoints;
        private String _UserId;
        private User _User;
        private Int32? _ClassTemplateId;
        private ClassTemplate _ClassTemplate;
        private Int32? _RaceTemplateId;
        private RaceTemplate _RaceTemplate;

        public Actor()
        {
            Stats = new JsonContainer(this);
        }

        public Guid Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public String Name { get { return _Name; } set { _Name = value; this.Changed(); } }
        public String Surname { get { return _Surname; } set { _Surname = value; this.Changed(); } }
        public Int32 ActorType { get { return _ActorType; } set { _ActorType = value; this.Changed(); } }
        public Int32 RoomId { get { return _RoomId; } set { _RoomId = value; this.Changed(); } }
        public Int32 CharacterPoints { get { return _CharacterPoints; } set { _CharacterPoints = value; this.Changed(); } }
        public dynamic Stats { get; private set; }
        public User User {
            get { return _User; }
            set
            {
                ChangeParent(value, ref _User, 
                    (User parent, Actor child) => parent.Actors.Remove(child), 
                    (User parent, Actor child) => parent.Actors.Add(child));
            }
        }

        public ClassTemplate ClassTemplate {
            get { return _ClassTemplate; }
            set
            {
                ChangeParent(value, ref _ClassTemplate, 
                    (ClassTemplate parent, Actor child) => parent.Actors.Remove(child), 
                    (ClassTemplate parent, Actor child) => parent.Actors.Add(child));
            }
        }

        public RaceTemplate RaceTemplate {
            get { return _RaceTemplate; }
            set
            {
                ChangeParent(value, ref _RaceTemplate, 
                    (RaceTemplate parent, Actor child) => parent.Actors.Remove(child), 
                    (RaceTemplate parent, Actor child) => parent.Actors.Add(child));
            }
        }

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
            return entity;
        }

        public override void LoadRelationships()
        {
            User = Context.Users.SingleOrDefault(x => x.Id == _UserId);
            ClassTemplate = Context.ClassTemplates.SingleOrDefault(x => x.Id == _ClassTemplateId);
            RaceTemplate = Context.RaceTemplates.SingleOrDefault(x => x.Id == _RaceTemplateId);
        }

        private static void AddSqlParameters(SqlCommand c, Actor e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
            c.Parameters.AddWithValue("@Name", e.Name);
            c.Parameters.AddWithValue("@Surname", e.Surname);
            c.Parameters.AddWithValue("@ActorType", e.ActorType);
            c.Parameters.AddWithValue("@UserId", e.User?.Id);
            c.Parameters.AddWithValue("@RoomId", e.RoomId);
            c.Parameters.AddWithValue("@ClassTemplateId", e.ClassTemplate?.Id);
            c.Parameters.AddWithValue("@RaceTemplateId", e.RaceTemplate?.Id);
            c.Parameters.AddWithValue("@CharacterPoints", e.CharacterPoints);
            c.Parameters.AddWithValue("@JSONStats", e.Stats.ToJson());
        }

        public static void GenerateInsertCommand(SqlCommand c, Actor e)
        {
            c.CommandText = @"INSERT INTO [Actor]([Id], [Name], [Surname], [ActorType], [UserId], [RoomId], [ClassTemplateId], [RaceTemplateId], [CharacterPoints], [JSONStats]
                              VALUES (@Id, @Name, @Surname, @ActorType, @UserId, @RoomId, @ClassTemplateId, @RaceTemplateId, @CharacterPoints, @JSONStats)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, Actor e)
        {
            c.CommandText = @"UPDATE [Actor] SET [Id] = @Id, [Name] = @Name, [Surname] = @Surname, [ActorType] = @ActorType, [UserId] = @UserId, [RoomId] = @RoomId, [ClassTemplateId] = @ClassTemplateId, [RaceTemplateId] = @RaceTemplateId, [CharacterPoints] = @CharacterPoints, [JSONStats] = @JSONStats WHERE [Id] = @Id";
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