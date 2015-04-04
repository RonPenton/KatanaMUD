using System;
using System.Reflection;

namespace Spam
{
    public class EntityRelationship
    {
        public EntityRelationship(Func<IEntity, IEntity> getParent)
        {
            GetParent = getParent;
        }

        public Func<IEntity, IEntity> GetParent { get; set; }
    }
}