using System;
using Idea.Repository.EntityFrameworkCore.Tests.Mocks;
using Xunit;

namespace Idea.Repository.EntityFrameworkCore.Tests
{
    public class RepositoryTests : IClassFixture<RepositoryFixture>
    {
        public const string SupermanKey = "a1943a62-34e7-4cec-8617-192a431ef689";
        public const string ZodKey = "44b20652-2f1b-44cb-961d-c2e254d240a6";
        public const string BatmanKey = "8c1f0e85-a665-4d2c-bfb3-0258c9b16b55";
        public const string IvyKey = "5b41516a-f2cb-4256-88a7-3b91535639d1";
        public const string HarleyKey = "1635bbb8-4168-4b42-a717-5b7813d913d7";
        public const string CyborgKey = "0d615564-9ceb-4c19-aff2-95882c38ec5e";
        public const string WonderWomanKey = "8adbe0b8-c597-408f-a9cd-d50619264886";
        public const string AquamanKey = "44338cf7-8592-4bc3-a46e-926e3b1bff28";
        public const string FlashKey = "aa742dae-1086-4401-a875-4d4b2024bf56";
        public const string GreenLanternKey = "08500149-bb1c-4a2d-95ac-87c7ba059ca3";

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
            using (new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
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
            using (new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                cyborg = repository.Find(FlashKey);
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
            using (var uow = new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                repository.Create(cyborg);

                uow.Commit();
            }

            cyborg = null;

            using (new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                cyborg = repository.Find(CyborgKey);
            }

            Assert.NotNull(cyborg);
            Assert.Equal(cyborg.Name, "Cyborg");
        }

        [Fact]
        public void Create_CreateHeroInCommitedUow_InsideNotCommitedUow_ShouldNotStoreData()
        {
            var aquaman = new Hero
            {
                Id = AquamanKey,
                Name = "Aquaman",
                RealName = "Arthur Curry",
                Origin = "Atlantis"
            };

            using (var uow = new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                using (var uow2 = new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
                {
                    var repository = new HeroRepository(_fixture.UowManager);
                    repository.Create(aquaman);

                    uow2.Commit();
                }
            }

            aquaman = null;
            using (new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                aquaman = repository.Find(AquamanKey);
            }

            Assert.Null(aquaman);
        }

        [Fact]
        public void Create_CreateHeroInNotCommitedUow_InsideCommitedUow_ShouldNotStoreData()
        {
            var greenLantern = new Hero
            {
                Id = GreenLanternKey,
                Name = "Green lantern",
                RealName = "Kyle Rayner",
                Origin = "Unknown"
            };

            using (var uow = new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                using (var uow2 = new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
                {
                    var repository = new HeroRepository(_fixture.UowManager);
                    repository.Create(greenLantern);
                }

                uow.Commit();
            }

            greenLantern = null;
            using (new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                greenLantern = repository.Find(WonderWomanKey);
            }

            Assert.Null(greenLantern);
        }

        [Fact]
        public void Create_CreateHeroInCommitedUow_InsideCommitedUow_ShouldNotStoreData()
        {
            var ww = new Hero
            {
                Id = WonderWomanKey,
                Name = "Wonder woman",
                RealName = "Diana Prince",
                Origin = "Themyscira"
            };

            using (var uow = new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                using (var uow2 = new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
                {
                    var repository = new HeroRepository(_fixture.UowManager);
                    repository.Create(ww);

                    uow2.Commit();
                }

                uow.Commit();
            }

            ww = null;
            using (new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                ww = repository.Find(WonderWomanKey);
            }

            Assert.NotNull(ww);
            Assert.Equal("Wonder woman", ww.Name);
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
            using (var uow = new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                repository.Create(cyborg);
            }

            cyborg = null;

            using (new UnitOfWork.EntityFrameworkCore.UnitOfWork(_fixture.DbContextFactory, _fixture.UowManager))
            {
                var repository = new HeroRepository(_fixture.UowManager);
                cyborg = repository.Find(CyborgKey);
            }

            Assert.Null(cyborg);
        }
    }
}