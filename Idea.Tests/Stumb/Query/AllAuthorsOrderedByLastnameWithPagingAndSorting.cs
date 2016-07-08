using System.Linq;
using Idea.Query;
using Idea.Tests.Entity;
using Microsoft.EntityFrameworkCore;

namespace Idea.Tests.Stumb.Query
{
    public class AllAuthorsOrderedByLastnameWithPagingAndSorting : Query<Author>
    {
        protected override IQueryable<Author> CreateQuery(IQueryable<Author> repository)
        {
            return repository.Where(w => w.Middlename != null).OrderBy(o => o.Lastname).Skip(1).Take(1).Include(i => i.AuthorBooks);
        }
    }
}