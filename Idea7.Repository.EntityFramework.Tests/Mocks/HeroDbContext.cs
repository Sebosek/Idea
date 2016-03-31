using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace Idea7.Repository.EntityFramework.Tests.Mocks
{
    public class HeroDbContext : DbContext
    {
        public HeroDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Hero> Heroes { get; set; }
    }
}
