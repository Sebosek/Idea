using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory
        where TDbContext : DbContext
    {
        private readonly IDesignTimeDbContextFactory<TDbContext> _factory;
        private readonly IUnitOfWorkManager _manager;

        public UnitOfWorkFactory(IDesignTimeDbContextFactory<TDbContext> factory, IUnitOfWorkManager manager)
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
