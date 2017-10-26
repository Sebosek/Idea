using System.Linq;

using Idea.Entity;
using Idea.SmartQuery.Interfaces;
using Idea.SmartQuery.QueryData;

using Microsoft.EntityFrameworkCore;

namespace Idea.SmartQuery.EntityFrameworkCore
{
    public class GenericQuery<TDbContext, TOrderBy, TEntity, TKey> : Query<TDbContext, GenericConditions<TOrderBy, TEntity, TKey>, TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TKey>
    {
        public GenericQuery(IQueryReader<GenericConditions<TOrderBy, TEntity, TKey>> reader) : base(reader)
        {
        }


        protected override IQueryable<TEntity> CreateQuery()
        {
            var data = Reader.Read();

            return data.Includes.Aggregate(Map().Where(data.Filter).OrderBy(data.Order).Skip(data.Skip).Take(data.Take),
                (current, i) => current.Include(i));
        }
    }

    public class GenericQuery<TDbContext, TEntity, TKey> : Query<TDbContext, GenericConditions<TEntity, TKey>, TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TKey>
    {
        public GenericQuery(IQueryReader<GenericConditions<TEntity, TKey>> reader) : base(reader)
        {
        }


        protected override IQueryable<TEntity> CreateQuery()
        {
            var data = Reader.Read();
            
            return data.Includes.Aggregate(Map().Where(data.Filter), (current, i) => current.Include(i));
        }
    }
}