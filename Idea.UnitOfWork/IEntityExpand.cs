using Idea.Entity;

namespace Idea.UnitOfWork
{
    public interface IEntityExpand<in TKey>
    {
        void BeforeCreate(IEntity<TKey> entity);

        void BeforeUpdate(IEntity<TKey> entity);

        void BeforeRemove(IEntity<TKey> entity);
    }
}