using Idea.UnitOfWork.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Idea.Repository.EntityFrameworkCore.Tests.Mocks
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
