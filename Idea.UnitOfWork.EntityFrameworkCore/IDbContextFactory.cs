using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public interface __IDbContextFactory
    {
        DbContext Create();
    }
}