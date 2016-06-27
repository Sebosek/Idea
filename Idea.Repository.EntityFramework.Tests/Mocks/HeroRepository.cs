using Idea7.UnitOfWork;

namespace Idea7.Repository.EntityFramework.Tests.Mocks
{
    public class HeroRepository : Repository<Hero, string>
    {
        public HeroRepository(IUnitOfWorkManager manager) : base(manager)
        { }
    }
}
