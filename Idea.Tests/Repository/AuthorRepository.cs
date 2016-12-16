using Idea.Repository.EntityFrameworkCore;
using Idea.Tests.Entity;
using Idea.UnitOfWork;

namespace Idea.Tests.Repository
{
    public class AuthorRepository : Repository<TestDbContext, Author, int>, IAuthorRepository
    {
        public AuthorRepository(IUnitOfWorkManager manager) : base(manager)
        { }
    }
}
