using System.Linq;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.UnitOfWork;

namespace Idea.Query.EntityFrameworkCore.Extensions
{
    public static class QueryExtensions
    {
        public static async Task<TEntity> FetchAsync<TEntity, TKey>(this Query<TEntity, TKey> query, IUnitOfWorkFactory factory)
            where TEntity : class, IEntity<TKey> =>
            (await query.ExecuteAsync(factory)).FirstOrDefault();
    }
}