namespace Spam
{
    public interface IEntity
    {
        bool IsChanged { get; }

        bool IsDeleted { get; }

        bool IsNew { get; }

        void Changed();
    }

    public interface IEntity<T> : IEntity
    {

    }
}