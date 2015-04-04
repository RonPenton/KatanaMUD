using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Spam
{
    public class EntityMetadata
    {
        public Type EntityType { get; set; }

        public IEntityContainer Container { get; set; }

        public List<EntityRelationship> Relationships { get; } = new List<EntityRelationship>();

        public Action<SqlCommand, IEntity> GenerateInsertCommand { get; set; }
        public Action<SqlCommand, IEntity> GenerateUpdateCommand { get; set; }
        public Action<SqlCommand, IEntity> GenerateDeleteCommand { get; set; }
    }
}