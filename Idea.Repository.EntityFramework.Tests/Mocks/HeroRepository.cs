using Idea.UnitOfWork;

namespace Idea.Repository.EntityFrameworkCore.Tests.Mocks
{
    public class HeroRepository : Repository<Hero, string>
    {
        public HeroRepository(IUnitOfWorkManager manager) : base(manager)
        { }
    }
}
