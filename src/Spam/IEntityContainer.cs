using System.Collections.Generic;

namespace Spam
{
    public interface IEntityContainer
    {
        IEnumerable<IEntity> ChangedEntities { get; }
        IEnumerable<IEntity> NewEntities { get; }
        IEnumerable<IEntity> DeletededEntities { get; }

        void ClearChanges();
        void LoadRelationships();
    }

    public interface IEntityContainer<K> : IEntityContainer
    {
        void SetChanged(Entity<K> entity);

        bool IsChanged(Entity<K> entity);

        bool IsNew(Entity<K> entity);

        bool IsDeleted(Entity<K> entity);
    }

}