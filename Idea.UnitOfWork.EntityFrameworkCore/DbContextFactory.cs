using System;

using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class DbContextFactory<TDbContext> : IDbContextFactory<TDbContext>
        where TDbContext : DbContext
    {
        private readonly DbContextOptions _options;

        public DbContextFactory(Action<DbContextOptionsBuilder> options)
        {
            var builder = new DbContextOptionsBuilder<TDbContext>();
            options(builder);

            _options = builder.Options;
        }

        public TDbContext CreateDbContext() => Activator.CreateInstance(typeof(TDbContext), _options) as TDbContext;
    }
}