using System.Collections.Generic;
using System.Linq;

namespace Idea.Query
{
    public interface IQueryObject<T>
    {
        long Count(IQueryable<T> repository);
        IEnumerable<T> Fetch(IQueryable<T> repository);
        T FetchOne(IQueryable<T> repository);
    }
}