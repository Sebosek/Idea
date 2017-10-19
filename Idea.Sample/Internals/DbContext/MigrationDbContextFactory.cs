using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Idea.Sample.Internals.DbContext
{
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<SampleDbContext>
    {
        public SampleDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SampleDbContext>();
            builder.UseNpgsql("Server=localhost;Port=5432;Database=idea;User Id=postgres;Password=postgres;");
            var context = new SampleDbContext(builder.Options);
            return context;
        }
    }
}