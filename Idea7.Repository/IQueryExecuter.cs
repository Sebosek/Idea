using System.Collections.Generic;

using Idea7.Query;

namespace Idea7.Repository
{
    public interface IQueryExecuter<T>
    {
        long Count(IQueryObject<T> query);
        IEnumerable<T> Fetch(IQueryObject<T> query);
        T FetchOne(IQueryObject<T> query);
    }
}