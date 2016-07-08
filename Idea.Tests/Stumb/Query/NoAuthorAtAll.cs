using System.Linq;
using Idea.Query;
using Idea.Tests.Entity;

namespace Idea.Tests.Stumb.Query
{
    public class NoAuthorAtAll : Query<Author>
    {
        protected override IQueryable<Author> CreateQuery(IQueryable<Author> repository)
        {
            return repository.Where(w => false);
        }
    }
}
