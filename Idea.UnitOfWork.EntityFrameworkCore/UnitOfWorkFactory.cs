using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWorkFactory<TDbContext, TKey> : IUnitOfWorkFactory
        where TDbContext : DbContext, IModelContext
    {
        private readonly IDataProvider _provider;

        private readonly IEnumerable<IEntityExpand<TKey>> _expands;

        private readonly IDbContextFactory<TDbContext> _factory;

        private readonly IUnitOfWorkManager _manager;
        
        public UnitOfWorkFactory(IDbContextFactory<TDbContext> factory, IUnitOfWorkManager manager, IDataProvider provider, IEnumerable<IEntityExpand<TKey>> expands)
        {
            _factory = factory;
            _manager = manager;
            _provider = provider;
            _expands = expands;
        }

        public IUnitOfWork Create() => CreateUnitOfWork();

        public IDataProvider DataProvider() => _provider;

        private UnitOfWork<TDbContext, TKey> CreateUnitOfWork() =>
            new UnitOfWork<TDbContext, TKey>(_factory.CreateDbContext(), _manager, _expands ?? new IEntityExpand<TKey>[0]);
    }
}
