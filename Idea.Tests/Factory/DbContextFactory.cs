using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Idea.Tests.Factory
{
    public class DbContextFactory : IDbContextFactory<TestDbContext>
    {
        public TestDbContext Create(DbContextFactoryOptions options)
        {
            var o = new DbContextOptionsBuilder();
            o.UseInMemoryDatabase();

            return new TestDbContext(o.Options);
        }
    }
}
