using System.Linq;
using Idea.Query;
using Idea.Tests.Entity;
using Idea.Tests.Fixture.Seed;
using Microsoft.EntityFrameworkCore;

namespace Idea.Tests.Stumb.Query
{
    public class AuthorByMurakamiIdWithIncludes : Query<Author>
    {
        protected override IQueryable<Author> CreateQuery(IQueryable<Author> repository)
        {
            return repository.Where(w => w.Id == AuthorSeed.ID_MURAKAMI).Include(i => i.AuthorBooks);
        }
    }
}
