using Idea.UnitOfWork.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Idea.Tests.Factory
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly DbContextOptions _options;

        public DbContextFactory(DbContextOptions options)
        {
            _options = options;
        }

        public DbContext Create()
        {
            return new TestDbContext(_options);
        }
    }
}
