using System.Collections.Generic;

namespace Spam
{
    public interface IEntityContainer
    {
        ICollection<IEntity> ChangedEntities { get; }
        ICollection<IEntity> NewEntities { get; }
        ICollection<IEntity> DeletededEntities { get; }
    }

    public interface IEntityContainer<K> : IEntityContainer
    {
        void SetChanged(Entity<K> entity);

        bool IsChanged(Entity<K> entity);

        bool IsNew(Entity<K> entity);

        bool IsDeleted(Entity<K> entity);
    }

}