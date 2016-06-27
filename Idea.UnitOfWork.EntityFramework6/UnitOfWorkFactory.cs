using System;

namespace Idea.UnitOfWork.EntityFramework6
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory _factory;
        private readonly IUnitOfWorkManager _manager;

        public UnitOfWorkFactory(IDbContextFactory factory, IUnitOfWorkManager manager)
        {
            if (factory == null)
            {
                throw new NullReferenceException("IDbContextFactory is null in UnitOfWorkFactory!");
            }
            if (manager == null)
            {
                throw new NullReferenceException("IUnitOfWorkManager is null in UnitOfWorkFactory!");
            }

            _factory = factory;
            _manager = manager;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_factory, _manager);
        }
    }
}
