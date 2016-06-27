using System;

using Idea7.Repository.EntityFramework.Tests.Mocks;
using Idea7.UnitOfWork;
using Idea7.UnitOfWork.EntityFramework;

using Microsoft.EntityFrameworkCore;

namespace Idea7.Repository.EntityFramework.Tests
{
    public class RepositoryFixture : IDisposable
    {
        private readonly HeroDbContext _context;
        private readonly IUnitOfWorkManager _manager;
        private readonly IDbContextFactory _factory;

        public HeroDbContext Context => _context;
        public IUnitOfWorkManager UowManager => _manager;
        public IDbContextFactory DbContextFactory => _factory;

        public RepositoryFixture()
        {
            var options = new DbContextOptionsBuilder();
            options.UseInMemoryDatabase();

            _manager = new UnitOfWorkManager();
            _context = new HeroDbContext(options.Options);
            _factory = new HeroDbContextFactory(_context);

            SeedHeroes();
            _context.SaveChanges();
        }

        public void Dispose()
        { }

        private void SeedHeroes()
        {
            var superman = new Hero
            {
                Id = RepositoryTests.SupermanKey,
                Name = "Superman",
                RealName = "Kal-El",
                Origin = "Krypton"
            };

            var batman = new Hero
            {
                Id = RepositoryTests.BatmanKey,
                Name = "Batman",
                RealName = "Bruce Wayne",
                Origin = "Gotham"
            };

            var zod = new Hero
            {
                Id = RepositoryTests.ZodKey,
                Name = "General Zod",
                RealName = "Dru-Zod",
                Origin = "Krypton"
            };

            var ivy = new Hero
            {
                Id = RepositoryTests.IvyKey,
                Name = "Poison Ivy",
                RealName = "Pamela Lilian Isley",
                Origin = "Seattle"
            };

            var harley = new Hero
            {
                Id = RepositoryTests.HarleyKey,
                Name = "Harley Quinn",
                RealName = "Harleen Frances Quinzel",
                Origin = "Gotham"
            };

            var zodVsSuperman = new HeroRelationship
            {
                Id = Guid.NewGuid().ToString(),
                Hero = superman,
                ToHero = zod,
                Relationship = Relationship.Enemy
            };
            var batmanVsIvy = new HeroRelationship
            {
                Id = Guid.NewGuid().ToString(),
                Hero = batman,
                ToHero = ivy,
                Relationship = Relationship.Enemy
            };
            var batmanVsHarley = new HeroRelationship
            {
                Id = Guid.NewGuid().ToString(),
                Hero = batman,
                ToHero = harley,
                Relationship = Relationship.Enemy
            };
            var ivyWithHarley = new HeroRelationship
            {
                Id = Guid.NewGuid().ToString(),
                Hero = ivy,
                ToHero = harley,
                Relationship = Relationship.Friend
            };

            superman.Relationships.Add(zodVsSuperman);
            zod.Relationships.Add(zodVsSuperman);

            batman.Relationships.Add(batmanVsIvy);
            batman.Relationships.Add(batmanVsHarley);
            ivy.Relationships.Add(batmanVsIvy);
            ivy.Relationships.Add(ivyWithHarley);
            harley.Relationships.Add(batmanVsHarley);
            harley.Relationships.Add(ivyWithHarley);

            _context.Heroes.Add(superman);
            _context.Heroes.Add(batman);
            _context.Heroes.Add(zod);
            _context.Heroes.Add(ivy);
            _context.Heroes.Add(harley);
        }
    }
}
