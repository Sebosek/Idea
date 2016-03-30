using Microsoft.Data.Entity;

namespace Idea7.UnitOfWork.EntityFramework
{
    public interface IDbContextFactory
    {
        DbContext Create();
    }
}