using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public interface IDbContextFactory
    {
        DbContext Create();
    }
}