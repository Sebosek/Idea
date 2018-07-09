namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWorkGeneration<TDbContext, TKey> : UnitOfWorkGeneration
        where TDbContext : ModelContext<TKey>
    {
        protected override void Release(IUnitOfWork uow)
        {
            if (uow is UnitOfWork<TDbContext, TKey> current)
            {
                current.Release();
            }
        }
    }
}
