using KatanaMUD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Spam
{
    /// <summary>
    /// Entity Context represents a Simple Persistent Access Manager (or SPAM). It is meant to be a lightweight 
    /// and baremetal context designed to work with SQL to retrieve and persist objects in memory for game-like 
    /// implementations. These objects should be periodically written back out to disk as the game progresses, 
    /// to handle events like a server crash or power outage. 
    /// </summary>
    public abstract class EntityContext : IDisposable
    {
        private string _connectionString;

        protected List<EntityMetadata> EntityTypes { get; } = new List<EntityMetadata>();

        public EntityContext(string connectionString)
        {
            _connectionString = connectionString;

            using (var connection = new SqlConnection(_connectionString))
            {
                LoadMetaData();
                LoadAllData(connection);
                AttachRelationships();
            }
        }

        protected abstract void LoadMetaData();

        protected void AttachRelationships()
        {
        }

        protected void LoadData<T, K>(SqlConnection connection, EntityContainer<T, K> container, string tableName, Func<SqlDataReader, T> creator) where T : Entity<K>
        {
            // I'm ok with not doing SQL injection protection here. TableName shouldn't ever come from user input, but a code generator.
            // God help me if this assumption ever changes.
            var command = new SqlCommand("SELECT * from " + tableName, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    container.Add(creator(reader), true);
                }
            }
        }

        protected abstract void LoadAllData(SqlConnection connection);

        public void SaveChanges()
        {
            // A set of entities which have been visited by the iteration operation, but may not
            // have been added to the SQL command. This is primarily to prevent infinite-recursion scenarios.
            HashSet<IEntity> visited = new HashSet<IEntity>();

            // A set of entities which have been added to the SQL command.
            HashSet<IEntity> processed = new HashSet<IEntity>();

            using (var connection = new SqlConnection(_connectionString))
            {
                
                var transaction = connection.BeginTransaction();
                var command = connection.CreateCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                foreach (var type in EntityTypes)
                {
                    foreach (var entity in type.Container.NewEntities)
                        InsertEntity(command, entity, type, visited, processed);
                    foreach (var entity in type.Container.ChangedEntities)
                        UpdateEntity(command, entity, type, visited, processed);
                    foreach (var entity in type.Container.DeletededEntities)
                        DeleteEntity(command, entity, type, visited, processed);
                }

                transaction.Commit();
            }
        }

        private void InsertEntity(SqlCommand command, IEntity entity, EntityMetadata type, HashSet<IEntity> visited, HashSet<IEntity> processed)
        {
            visited.Add(entity);

            // Already added, just return.
            if (processed.Contains(entity))
                return;

            if (type == null)
            {
                // Look up the entity type in case it isn't specified. This is good for recursive news. 
                // It really shouldn't happen all that often, but if it becomes a bottleneck, maybe we can cache it 
                // somehow with a dictionary. Though I suspect the biggest hit will be the reflection of the type.
                type = this.EntityTypes.Single(x => x.EntityType == entity.GetType());
            }

            foreach (var relationship in type.Relationships)
            {
                var parent = relationship.GetParent(entity);
                if (parent != null && parent.IsNew)
                {
                    // The parent is also a new entity. Therefore, we must create it before the current entity. 
                    // But make sure it's not going to cause an infinitely-recursing loop first.
                    // In that case, there's really nothing we can do to fix the problem. Well,
                    // I SUPPOSE I could commit the entity with no parent defined, then commit the parent, then
                    // update the child with the parents ID. However, that assumes that the relationship is nullable,
                    // and quite frankly once we've reached that level of complexity, I think we've passed
                    // the bounds of a "Simple" ORM. I don't anticipate a data model complex enough that this
                    // will happen often or ever.
                    if (visited.Contains(parent) && !processed.Contains(parent))
                    {
                        throw new InvalidOperationException("Self-Referential Loop detected. Cannot commit SQL without manual intervention.");
                    }

                    InsertEntity(command, parent, null, visited, processed);
                }
            }

            type.GenerateInsertCommand(command, entity);
            command.ExecuteNonQuery();

            processed.Add(entity);
        }

        private void UpdateEntity(SqlCommand command, IEntity entity, EntityMetadata type, HashSet<IEntity> visited, HashSet<IEntity> processed)
        {
            // Already processed, just return.
            if (processed.Contains(entity))
                return;

            type.GenerateUpdateCommand(command, entity);
            command.ExecuteNonQuery();

            processed.Add(entity);
        }

        private void DeleteEntity(SqlCommand command, IEntity entity, EntityMetadata type, HashSet<IEntity> visited, HashSet<IEntity> processed)
        {
            // Already processed, just return.
            if (processed.Contains(entity))
                return;

            type.GenerateDeleteCommand(command, entity);
            command.ExecuteNonQuery();

            processed.Add(entity);
        }
        
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_connection != null)
                    {
                        _connection.Close();
                        _connection = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
