using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Idea.Tests.Fixture;
using Idea.Tests.Fixture.Seed;
using Idea.Tests.Repository;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;

using Moq;

using Xunit;

namespace Idea.Tests
{
    [Collection("Database collection")]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Unit tests name convetion")]
    public class RepositoryTests
    {
        private readonly IAuthorRepository _authorRepository;

        private readonly IUnitOfWorkFactory _factory;
        
        public RepositoryTests(DbFixture fixture)
        {
            _authorRepository = new AuthorRepository(fixture.UowManager);

            _factory = new UnitOfWorkFactory<TestDbContext, int>(fixture.DbContextFactory, fixture.UowManager, null, null);
        }

        [Fact]
        public async Task FindAsync_ExistingKey_ShouldSuccess()
        {
            using (_factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_PALAHNIUK);

                Assert.NotNull(author);
            }
        }

        [Fact]
        public async Task FindAsync_NonExistingKey_ShouldFoundNothing()
        {
            using (_factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_NOBODY);

                Assert.Null(author);
            }
        }

        [Fact]
        public async Task CreateAsync_CreateAndRead_ShouldSuccess()
        {
            using (var uow = _factory.Create())
            {
                using (_factory.Create())
                {
                    var author = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_WYNDHAM);

                    Assert.Null(author);
                }

                await _authorRepository.CreateAsync(AuthorSeed.WYNDHAM);
                await uow.CommitAsync();
            }

            using (_factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_WYNDHAM);

                Assert.NotNull(author);
            }
        }

        [Fact]
        public async Task CreateAsync_ValidDataCommited_ShouldSuccess()
        {
            using (var uow = _factory.Create())
            {
                await _authorRepository.CreateAsync(AuthorSeed.MITCHELL);
                await uow.CommitAsync();
            }

            using (_factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_MITCHELL);

                Assert.NotNull(author);
            }
        }

        [Fact]
        public async Task CreateAsync_ValidDataNotCommited_ShouldNotSaveData()
        {
            using (_factory.Create())
            {
                await _authorRepository.CreateAsync(AuthorSeed.KING);
            }

            using (_factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_KING);

                Assert.Null(author);
            }
        }

        [Fact]
        public async Task CreateAsync_NullArgument_ShouldThrowException()
        {
            using (_factory.Create())
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => _authorRepository.CreateAsync(null));
            }
        }

        [Fact]
        public async Task Create_CreateInCommitedUow_InsideNotCommitedUow_ShouldNotStoreData()
        {
            using (_factory.Create())
            {
                using (var uow2 = _factory.Create())
                {
                    await _authorRepository.CreateAsync(AuthorSeed.TSUTSUI);
                    await uow2.CommitAsync();
                }
            }

            using (_factory.Create())
            {
                var tsutsui = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_TSUTSUI);

                Assert.Null(tsutsui);
            }
        }

        [Fact]
        public async Task Create_CreateInNotCommitedUow_InsideCommitedUow_ShouldNotStoreData()
        {
            using (var uow = _factory.Create())
            {
                using (_factory.Create())
                {
                    await _authorRepository.CreateAsync(AuthorSeed.TSUTSUI);
                }

                await uow.CommitAsync();
            }

            using (_factory.Create())
            {
                var tsutsui = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_TSUTSUI);

                Assert.Null(tsutsui);
            }
        }

        [Fact]
        public async Task Create_TriplePludgeCommitedOutsideUow_ShouldNotStoreData()
        {
            using (var uow = _factory.Create())
            {
                using (_factory.Create())
                {
                    using (var uow3 = _factory.Create())
                    {
                        await _authorRepository.CreateAsync(AuthorSeed.TSUTSUI);
                        await uow3.CommitAsync();
                    }
                }

                await uow.CommitAsync();
            }

            using (_factory.Create())
            {
                var tsutsui = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_TSUTSUI);

                Assert.Null(tsutsui);
            }
        }

        [Fact]
        public async Task Create_CreateInCommitedUow_InsideCommitedUow_ShouldSuccess()
        {
            using (var uow = _factory.Create())
            {
                using (var uow2 = _factory.Create())
                {
                    await _authorRepository.CreateAsync(AuthorSeed.KAHNEMAN);
                    await uow2.CommitAsync();
                }

                await uow.CommitAsync();
            }

            using (_factory.Create())
            {
                var kahneman = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_KAHNEMAN);

                Assert.NotNull(kahneman);
                Assert.Equal("Kahneman", kahneman.Lastname);
            }
        }

        [Fact]
        public async Task UpdateAsync_ValidDataCommited_ShouldSuccess()
        {
            const string lastname = "Coelho";

            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_COELHO);
                author.Lastname = lastname;

                await _authorRepository.UpdateAsync(author);
                await uow.CommitAsync();
            }

            using (_factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_COELHO);

                Assert.Equal(lastname, author.Lastname);
            }
        }

        [Fact]
        public async Task UpdateAsync_TrilevelSeparateUpdates_CommitedAll_ShouldSuccess()
        {
            const string exupery = "Saint-Exupéry";
            const string steinbect = "Steinbeck";

            using (var uow = _factory.Create())
            {
                using (var uow2 = _factory.Create())
                {
                    using (var uow3 = _factory.Create())
                    {
                        var author = await _authorRepository.FindAsync(AuthorSeed.ID_STEINBECK);
                        author.Lastname = steinbect;

                        await _authorRepository.UpdateAsync(author);
                        await uow3.CommitAsync();
                    }

                    using (var uow3 = _factory.Create())
                    {
                        var author = await _authorRepository.FindAsync(AuthorSeed.ID_SAINT_EXUPERI);
                        author.Lastname = exupery;

                        await _authorRepository.UpdateAsync(author);
                        await uow3.CommitAsync();
                    }

                    await uow2.CommitAsync();
                }

                await uow.CommitAsync();
            }

            using (_factory.Create())
            {
                var s = await _authorRepository.FindAsync(AuthorSeed.ID_STEINBECK);
                var e = await _authorRepository.FindAsync(AuthorSeed.ID_SAINT_EXUPERI);

                Assert.Equal(steinbect, s.Lastname);
                Assert.Equal(exupery, e.Lastname);
            }
        }

        [Fact]
        public async Task UpdateAsync_TrilevelSeparateUpdates_CommitedOutsidesOnly_ShouldNotSaveChanges()
        {
            const string clark = "Clark";
            const string bullshit = "X";

            using (var uow = _factory.Create())
            {
                using (var uow2 = _factory.Create())
                {
                    using (var uow3 = _factory.Create())
                    {
                        var author = await _authorRepository.FindAsync(AuthorSeed.ID_CLARK);
                        author.Lastname = clark;

                        await _authorRepository.UpdateAsync(author);
                    }

                    using (var uow3 = _factory.Create())
                    {
                        var author = await _authorRepository.FindAsync(AuthorSeed.ID_WILDE);
                        author.Lastname = bullshit;

                        await _authorRepository.UpdateAsync(author);
                        await uow3.CommitAsync();
                    }
                }

                await uow.CommitAsync();
            }

            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_WILDE);

                Assert.NotEqual(bullshit, author.Lastname);
            }
        }

        [Fact]
        public async Task UpdateAsync_DoublelevelSeparateUpdates_CommitedOnlyOneUow_ShouldNotStoreData()
        {
            const string bullshit = "X";
            const string adams = "Adams";

            using (var uow = _factory.Create())
            {
                using (var uow2 = _factory.Create())
                {
                    var author = await _authorRepository.FindAsync(AuthorSeed.ID_WILDE);
                    author.Lastname = bullshit;

                    await _authorRepository.UpdateAsync(author);
                }

                using (var uow2 = _factory.Create())
                {
                    var author = await _authorRepository.FindAsync(AuthorSeed.ID_ADAMS);
                    author.Lastname = adams;

                    await _authorRepository.UpdateAsync(author);
                    await uow2.CommitAsync();
                }

                await uow.CommitAsync();
            }

            using (var uow = _factory.Create())
            {
                var w = await _authorRepository.FindAsync(AuthorSeed.ID_WILDE);
                var a = await _authorRepository.FindAsync(AuthorSeed.ID_ADAMS);

                Assert.Equal(adams, a.Lastname);
                Assert.NotEqual(bullshit, w.Lastname);
            }
        }

        [Fact]
        public async Task UpdateAsync_DoublelevelSeparateUpdates_CommitAllUows_ShouldSuccess()
        {
            const string clark = "Clark";
            const string hamingway = "Hamingway";

            using (var uow = _factory.Create())
            {
                using (var uow2 = _factory.Create())
                {
                    var author = await _authorRepository.FindAsync(AuthorSeed.ID_CLARK);
                    author.Lastname = clark;

                    await _authorRepository.UpdateAsync(author);
                    await uow2.CommitAsync();
                }

                using (var uow2 = _factory.Create())
                {
                    var author = await _authorRepository.FindAsync(AuthorSeed.ID_HAMINGWAY);
                    author.Lastname = hamingway;

                    await _authorRepository.UpdateAsync(author);
                    await uow2.CommitAsync();
                }

                await uow.CommitAsync();
            }

            using (var uow = _factory.Create())
            {
                var c = await _authorRepository.FindAsync(AuthorSeed.ID_CLARK);
                var h = await _authorRepository.FindAsync(AuthorSeed.ID_HAMINGWAY);

                Assert.Equal(clark, c.Lastname);
                Assert.Equal(hamingway, h.Lastname);
            }
        }

        [Fact]
        public async Task UpdateAsync_ValidDataNotCommited_ShouldSuccess()
        {
            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_MURAKAMI);
                author.Lastname = "Japanesse";

                await _authorRepository.UpdateAsync(author);
            }

            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_MURAKAMI);

                Assert.Equal(AuthorSeed.MURAKAMI.Lastname, author.Lastname);
            }
        }

        [Fact]
        public async Task UpdateAsync_NullArgument_ShouldThrowException()
        {
            using (var uow = _factory.Create())
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => _authorRepository.UpdateAsync(null));
            }
        }

        [Fact]
        public async Task DeleteAsync_ValidDataCommited_ShouldSuccess()
        {
            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.REMOVE_ID_NESBO);

                await _authorRepository.DeleteAsync(author);
                await uow.CommitAsync();
            }

            using (_factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.REMOVE_ID_NESBO);

                Assert.Null(author);
            }
        }

        [Fact]
        public async Task DeleteAsync_ValidDataNotCommited_ShouldNotDelete()
        {
            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_TOLKIEN);
                await _authorRepository.DeleteAsync(author);
            }

            using (_factory.Create())
            {
                var author = _authorRepository.FindAsync(AuthorSeed.ID_TOLKIEN);

                Assert.NotNull(author);
            }
        }

        [Fact]
        public async Task DeleteAsync_NullArgument_ShouldThrowException()
        {
            using (_factory.Create())
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => _authorRepository.DeleteAsync(null));
            }
        }
    }
}
