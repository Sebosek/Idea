using Idea7.Repository.EntityFramework.Tests.Mocks;
using Idea7.UnitOfWork;

using Microsoft.Data.Entity;
using Xunit;

namespace Idea7.Repository.EntityFramework.Tests
{
    public class RepositoryTests
    {
        private readonly HeroDbContext _context;
        private readonly IUnitOfWorkManager _manager;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder();
            options.UseInMemoryDatabase();

            _manager = new UnitOfWorkManager();
            _context = new HeroDbContext(options.Options);
            SeedHeroes();
        }

        [Fact]
        public void CreateNewInstance_ShouldSuccess()
        {
            var repository = new HeroRepository(_manager);
            Assert.NotNull(repository);
        }

        private void SeedHeroes()
        {
            var superman = new Hero
            {
                Name = "Superman",
                RealName = "Kal El",
                Origin = "Krypton"
            };

            var batman = new Hero
            {
                Name = "Batman",
                RealName = "Bruce Wayne",
                Origin = "Gotham"
            };

            var zod = new Hero
            {
                Name = "General Zod",
                RealName = "Dru-Zod",
                Origin = "Krypton"
            };

            var ivy = new Hero
            {
                Name = "Poison Ivy",
                RealName = "Pamela Lilian Isley",
                Origin = "Seattle"
            };

            var harley = new Hero
            {
                Name = "Harley Quinn",
                RealName = "Harleen Frances Quinzel",
                Origin = "Gotham"
            };

            superman.Enemies = new[] {zod};
            zod.Enemies = new[] {superman};

            batman.Enemies = new[] {ivy, harley};
            ivy.Friends = new[] {harley};
            ivy.Enemies = new[] {batman};
            harley.Friends = new[] {ivy, harley};
            harley.Enemies = new[] {batman};

            _context.Heroes.Add(superman);
            _context.Heroes.Add(batman);
            _context.Heroes.Add(zod);
            _context.Heroes.Add(ivy);
            _context.Heroes.Add(harley);
        }
    }
}