using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWork<TDbContext> : UnitOfWork
        where TDbContext : DbContext
    {
        public UnitOfWork(IDesignTimeDbContextFactory<TDbContext> factory, IUnitOfWorkManager manager)
            : base(manager)
        {
            DbContext = factory.CreateDbContext(new string[0]);
        }

        public DbContext DbContext { get; }

        public void Release() => DbContext.Dispose();

        protected override void DoCommit() => DbContext.SaveChanges();

        protected override async Task DoCommitAsync() => await DbContext.SaveChangesAsync();
    }
}
