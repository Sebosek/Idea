using System;

using Idea.Entity;
using Idea.SmartQuery.Interfaces;

namespace Idea.SmartQuery
{
    public class QueryFactory : IQueryFactory
    {
        public TQuery CreateQuery<TQuery, TEntity, TKey>(IQueryReader<IQueryData> reader)
            where TQuery : class, IQuery<TEntity, TKey>
            where TEntity : IEntity<TKey>
        {
            return (TQuery)Activator.CreateInstance(typeof(TQuery), reader);
        }
    }
}