using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWork<TDbContext> : UnitOfWork
        where TDbContext : DbContext
    {
        public UnitOfWork(TDbContext context, IUnitOfWorkManager manager)
            : base(manager)
        {
            DbContext = context;
        }

        public DbContext DbContext { get; }

        public void Release() => DbContext.Dispose();

        protected override void DoCommit() => DbContext.SaveChanges();

        protected override async Task DoCommitAsync() => await DbContext.SaveChangesAsync();
    }
}
