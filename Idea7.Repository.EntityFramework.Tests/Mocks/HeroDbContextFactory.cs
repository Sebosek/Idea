using Idea7.UnitOfWork.EntityFramework;

using Microsoft.EntityFrameworkCore;

namespace Idea7.Repository.EntityFramework.Tests.Mocks
{
    public class HeroDbContextFactory : IDbContextFactory
    {
        private readonly DbContext _dbContext;

        public HeroDbContextFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext Create()
        {
            return _dbContext;
        }
    }
}
