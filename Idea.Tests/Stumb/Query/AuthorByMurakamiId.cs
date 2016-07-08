using System.Linq;
using Idea.Query;
using Idea.Tests.Entity;
using Idea.Tests.Fixture.Seed;

namespace Idea.Tests.Stumb.Query
{
    public class AuthorByMurakamiId : Query<Author>
    {
        protected override IQueryable<Author> CreateQuery(IQueryable<Author> repository)
        {
            return repository.Where(w => w.Id == AuthorSeed.ID_MURAKAMI);
        }
    }
}
