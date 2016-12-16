using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWorkGeneration<TDbContext> : Idea.UnitOfWork.UnitOfWorkGeneration
        where TDbContext : DbContext
    {
        protected override void Release(IUnitOfWork uow)
        {
            var current = uow as UnitOfWork<TDbContext>;
            if (current != null)
            {
                current.Release();
            }
        }
    }
}
