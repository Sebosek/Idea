namespace Idea7.Repository
{
    public interface IRepository<TEntity, in TKey> : IQueryExecuter<TEntity>
    {
        TEntity Find(TKey id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}