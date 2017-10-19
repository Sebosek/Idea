using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        private readonly IUnitOfWorkManager _manager;

        public UnitOfWorkFactory(TDbContext context, IUnitOfWorkManager manager)
        {
            _context = context;
            _manager = manager;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork<TDbContext>(_context, _manager);
        }
    }
}
