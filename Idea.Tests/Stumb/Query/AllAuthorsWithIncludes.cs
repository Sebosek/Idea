using System.Linq;
using Idea.Query;
using Idea.Tests.Entity;
using Microsoft.EntityFrameworkCore;

namespace Idea.Tests.Stumb.Query
{
    public class AllAuthorsWithIncludes : Query<Author>
    {
        protected override IQueryable<Author> CreateQuery(IQueryable<Author> repository)
        {
            return repository.Where(w => true).Include(i => i.AuthorBooks);
        }
    }
}