using System;

using Idea7.Repository.EntityFramework.Tests.Mocks;

using Xunit;

namespace Idea7.Repository.EntityFramework.Tests
{
    public class RepositoryTests : IClassFixture<RepositoryFixture>
    {
        public const string SupermanKey = "a1943a62-34e7-4cec-8617-192a431ef689";
        public const string ZodKey = "44b20652-2f1b-44cb-961d-c2e254d240a6";
        public const string BatmanKey = "8c1f0e85-a665-4d2c-bfb3-0258c9b16b55";
        public const string IvyKey = "5b41516a-f2cb-4256-88a7-3b91535639d1";
        public const string HarleyKey = "1635bbb8-4168-4b42-a717-5b7813d913d7";
        public const string CyborgKey = "0d615564-9ceb-4c19-aff2-95882c38ec5e";

        private readonly RepositoryFixture _fixture;

        public RepositoryTests(RepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CreateNewInstance_ShouldSuccess()
        {
            var repository = new HeroRepository(_fixture.UowManager);
            Assert.NotNull(repository);
        }

        [Fact]
        public void FindBatman_ShouldSuccess()
        {
            Hero batman;
            using (new UnitOfWork.EntityFramework.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                batman = repository.Find(BatmanKey);
            }

            Assert.NotNull(batman);
            Assert.Equal(batman.Name, "Batman");
        }

        [Fact]
        public void FindCyborg_ShouldFoundNothing()
        {
            Hero cyborg;
            using (new UnitOfWork.EntityFramework.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                cyborg = repository.Find(CyborgKey);
            }

            Assert.Null(cyborg);
        }

        [Fact]
        public void CreateHero_ShouldSuccess()
        {
            Hero cyborg = new Hero
            {
                Id = CyborgKey,
                Name = "Cyborg",
                RealName = "Victor Stone",
                Origin = "Detroit"
            };
            using (var uow = new UnitOfWork.EntityFramework.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                repository.Create(cyborg);

                uow.Commit();
            }

            cyborg = null;

            using (new UnitOfWork.EntityFramework.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                cyborg = repository.Find(CyborgKey);
            }

            Assert.NotNull(cyborg);
            Assert.Equal(cyborg.Name, "Cyborg");
        }

        [Fact]
        public void CreateHero_NotCommitedShouldFoundNothing()
        {
            Hero cyborg = new Hero
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Cyborg",
                RealName = "Victor Stone",
                Origin = "Detroit"
            };
            using (var uow = new UnitOfWork.EntityFramework.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                repository.Create(cyborg);
            }

            cyborg = null;

            using (new UnitOfWork.EntityFramework.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                cyborg = repository.Find(CyborgKey);
            }

            Assert.Null(cyborg);
        }
    }
}