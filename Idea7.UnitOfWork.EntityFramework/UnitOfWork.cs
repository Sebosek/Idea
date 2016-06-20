using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Idea7.UnitOfWork.EntityFramework
{
    public class UnitOfWork : Idea7.UnitOfWork.UnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(IDbContextFactory factory, IUnitOfWorkManager manager)
            : base(manager)
        {
            _context = factory.Create();
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
    }
}
