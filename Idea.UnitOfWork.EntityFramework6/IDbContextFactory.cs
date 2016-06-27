using System.Data.Entity;

namespace Idea.UnitOfWork.EntityFramework6
{
    public interface IDbContextFactory
    {
        DbContext Create();
    }
}
