using Idea.Entity;

namespace Idea.SmartQuery.Interfaces
{
    public interface IQueryFactory
    {
        TQuery CreateQuery<TQuery, TEntity, TKey>(IQueryReader<IQueryData> reader)
            where TQuery : class, IQuery<TEntity, TKey>
            where TEntity : IEntity<TKey>;

        TQuery CreateQuery<TQuery, TEntity, TKey>()
            where TQuery : class, IQuery<TEntity, TKey>
            where TEntity : IEntity<TKey>;
    }
}