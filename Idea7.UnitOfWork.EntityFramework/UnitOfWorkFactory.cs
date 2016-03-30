namespace Idea7.UnitOfWork.EntityFramework
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory _factory;
        private readonly IUnitOfWorkManager _manager;

        public UnitOfWorkFactory(IDbContextFactory factory, IUnitOfWorkManager manager)
        {
            _factory = factory;
            _manager = manager;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_factory, _manager);
        }
    }
}
