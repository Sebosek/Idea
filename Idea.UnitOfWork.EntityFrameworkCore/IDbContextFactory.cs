using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public interface IDbContextFactory<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext CreateDbContext();
    }
}