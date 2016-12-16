using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory
        where TDbContext : DbContext
    {
        private readonly IDbContextFactory<TDbContext> _factory;
        private readonly IUnitOfWorkManager _manager;

        public UnitOfWorkFactory(IDbContextFactory<TDbContext> factory, IUnitOfWorkManager manager)
        {
            _factory = factory;
            _manager = manager;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork<TDbContext>(_factory, _manager);
        }
    }
}
