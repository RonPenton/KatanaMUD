using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class User : Entity<String>
    {
        partial void OnConstruct();
        partial void OnLoaded();
        public override String Key { get { return Id; } set { Id = value; } }
        private GameEntities Context => (GameEntities)__context;
        private String _Id;
        private Int32 _AccessFailedCount;
        private DateTime? _LockoutEnd;
        private String _PasswordHash;
        private Boolean _IsConfirmed;
        private Int32 _AccessLevel;

        public User()
        {
            Actors = new ParentChildRelationshipContainer<User, Actor, Guid>(this, child => child.User, (child, parent) => child.User= parent);
            OnConstruct();
        }

        public String Id { get { return _Id; } set { _Id = value; this.Changed(); } }
        public Int32 AccessFailedCount { get { return _AccessFailedCount; } set { _AccessFailedCount = value; this.Changed(); } }
        public DateTime? LockoutEnd { get { return _LockoutEnd; } set { _LockoutEnd = value; this.Changed(); } }
        public String PasswordHash { get { return _PasswordHash; } set { _PasswordHash = value; this.Changed(); } }
        public Boolean IsConfirmed { get { return _IsConfirmed; } set { _IsConfirmed = value; this.Changed(); } }
        public AccessLevel AccessLevel { get { return (AccessLevel)_AccessLevel; } set { _AccessLevel = (Int32)value; this.Changed(); } }
        public ICollection<Actor> Actors { get; private set; }
        public static User Load(SqlDataReader reader)
        {
            var entity = new User();
            entity._Id = reader.GetString(0);
            entity._AccessFailedCount = reader.GetInt32(1);
            entity._LockoutEnd = reader.GetSafeDateTime(2);
            entity._PasswordHash = reader.GetSafeString(3);
            entity._IsConfirmed = reader.GetBoolean(4);
            entity._AccessLevel = reader.GetInt32(5);
            entity.OnLoaded();
            return entity;
        }

        public override void LoadRelationships()
        {
        }

        private static void AddSqlParameters(SqlCommand c, User e)
        {
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", (object)e.Id ?? DBNull.Value);
            c.Parameters.AddWithValue("@AccessFailedCount", (object)e.AccessFailedCount ?? DBNull.Value);
            c.Parameters.AddWithValue("@LockoutEnd", (object)e.LockoutEnd ?? DBNull.Value);
            c.Parameters.AddWithValue("@PasswordHash", (object)e.PasswordHash ?? DBNull.Value);
            c.Parameters.AddWithValue("@IsConfirmed", (object)e.IsConfirmed ?? DBNull.Value);
            c.Parameters.AddWithValue("@AccessLevel", (object)e.AccessLevel ?? DBNull.Value);
        }

        public static void GenerateInsertCommand(SqlCommand c, User e)
        {
            c.CommandText = @"INSERT INTO [User]([Id], [AccessFailedCount], [LockoutEnd], [PasswordHash], [IsConfirmed], [AccessLevel])
                              VALUES (@Id, @AccessFailedCount, @LockoutEnd, @PasswordHash, @IsConfirmed, @AccessLevel)";
            AddSqlParameters(c, e);
        }

        public static void GenerateUpdateCommand(SqlCommand c, User e)
        {
            c.CommandText = @"UPDATE [User] SET [Id] = @Id, [AccessFailedCount] = @AccessFailedCount, [LockoutEnd] = @LockoutEnd, [PasswordHash] = @PasswordHash, [IsConfirmed] = @IsConfirmed, [AccessLevel] = @AccessLevel WHERE [Id] = @Id";
            AddSqlParameters(c, e);
        }

        public static void GenerateDeleteCommand(SqlCommand c, User e)
        {
            c.CommandText = @"DELETE FROM[User] WHERE[Id] = @Id";
            c.Parameters.Clear();
            c.Parameters.AddWithValue("@Id", e.Id);
        }
    }
}