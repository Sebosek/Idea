using System.Collections.Generic;
using System.Threading.Tasks;

using Idea.Query;

namespace Idea.Repository
{
    public interface IQueryExecuter<T>
    {
        long Count(IQueryObject<T> query);
        IEnumerable<T> Fetch(IQueryObject<T> query);
        T FetchOne(IQueryObject<T> query);

        Task<long> CountAsync(IQueryObject<T> query);
        Task<IEnumerable<T>> FetchAsync(IQueryObject<T> query);
        Task<T> FetchOneAsync(IQueryObject<T> query);
    }
}