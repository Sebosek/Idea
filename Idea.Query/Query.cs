using System.Collections.Generic;
using System.Linq;

namespace Idea.Query
{
    public abstract class Query<T> : IQueryObject<T>
    {
        protected abstract IQueryable<T> CreateQuery(IQueryable<T> repository);


        public long Count(IQueryable<T> repository)
        {
            return CreateQuery(repository).Count();
        }

        public IEnumerable<T> Fetch(IQueryable<T> repository)
        {
            return CreateQuery(repository);
        }

        public T FetchOne(IQueryable<T> repository)
        {
            return CreateQuery(repository).FirstOrDefault();
        }
    }
}
