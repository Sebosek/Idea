using System.Data.Entity;

namespace Idea7.UnitOfWork.EntityFramework6
{
    public interface IDbContextFactory
    {
        DbContext Create();
    }
}
