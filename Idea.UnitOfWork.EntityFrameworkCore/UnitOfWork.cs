using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWork<TDbContext> : Idea.UnitOfWork.UnitOfWork
        where TDbContext : DbContext
    {
        private readonly DbContext _context;

        public UnitOfWork(IDbContextFactory<TDbContext> factory, IUnitOfWorkManager manager)
            : base(manager)
        {
            var builder = new DbContextFactoryOptions();
            _context = factory.Create(builder);
        }

        public DbContext DbContext => _context;

        protected override void DoCommit()
        {
            _context.SaveChanges();
        }

        protected override async Task DoCommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Release()
        {
            _context.Dispose();
        }
    }
}
