using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Idea.Tests.Factory
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            var o = new DbContextOptionsBuilder();
            o.UseInMemoryDatabase("UnitTest");

            return new TestDbContext(o.Options);
        }
    }
}
