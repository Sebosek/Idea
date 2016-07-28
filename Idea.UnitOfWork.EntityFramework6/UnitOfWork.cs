using System.Data.Entity;
using System.Threading.Tasks;

namespace Idea.UnitOfWork.EntityFramework6
{
    public class UnitOfWork : Idea.UnitOfWork.UnitOfWork
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

        public void Release()
        {
            _context.Dispose();
        }
    }
}
