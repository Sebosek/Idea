using System;

using Idea7.Repository.EntityFramework.Tests.Mocks;
using Idea7.UnitOfWork;
using Idea7.UnitOfWork.EntityFramework;

using Microsoft.Data.Entity;
using Xunit;

namespace Idea7.Repository.EntityFramework.Tests
{
    public class RepositoryTests
    {
        private const string SupermanKey = "a1943a62-34e7-4cec-8617-192a431ef689";
        private const string ZodKey = "44b20652-2f1b-44cb-961d-c2e254d240a6";
        private const string BatmanKey = "8c1f0e85-a665-4d2c-bfb3-0258c9b16b55";
        private const string IvyKey = "5b41516a-f2cb-4256-88a7-3b91535639d1";
        private const string HarleyKey = "1635bbb8-4168-4b42-a717-5b7813d913d7";

        private readonly HeroDbContext _context;
        private readonly IUnitOfWorkManager _manager;
        private readonly IDbContextFactory _factory;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder();
            options.UseInMemoryDatabase();

            _manager = new UnitOfWorkManager();
            _context = new HeroDbContext(options.Options);
            _factory = new HeroDbContextFactory(_context);

            SeedHeroes();
        }

        [Fact]
        public void CreateNewInstance_ShouldSuccess()
        {
            var repository = new HeroRepository(_manager);
            Assert.NotNull(repository);
        }

        [Fact]
        public void FindBatman_ShouldSuccess()
        {
            Hero batman;
            using (new UnitOfWork.EntityFramework.UnitOfWork(_factory, _manager))
            {
                var repository = new HeroRepository(_manager);
                batman = repository.Find(BatmanKey);
            }

            Assert.NotNull(batman);
        }

        private void SeedHeroes()
        {
            var superman = new Hero
            {
                Id = SupermanKey,
                Name = "Superman",
                RealName = "Kal-El",
                Origin = "Krypton"
            };

            var batman = new Hero
            {
                Id = BatmanKey,
                Name = "Batman",
                RealName = "Bruce Wayne",
                Origin = "Gotham"
            };

            var zod = new Hero
            {
                Id = ZodKey,
                Name = "General Zod",
                RealName = "Dru-Zod",
                Origin = "Krypton"
            };

            var ivy = new Hero
            {
                Id = IvyKey,
                Name = "Poison Ivy",
                RealName = "Pamela Lilian Isley",
                Origin = "Seattle"
            };

            var harley = new Hero
            {
                Id = HarleyKey,
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