using Microsoft.EntityFrameworkCore;

namespace Idea7.UnitOfWork.EntityFramework
{
    public interface IDbContextFactory
    {
        DbContext Create();
    }
}