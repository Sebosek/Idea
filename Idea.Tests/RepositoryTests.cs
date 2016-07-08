using System;
using System.Linq;
using System.Threading.Tasks;
using Idea.Tests.Fixture;
using Idea.Tests.Fixture.Seed;
using Idea.Tests.Repository;
using Idea.Tests.Stumb.Query;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;
using Xunit;

namespace Idea.Tests
{
    [Collection("Database collection")]
    public class RepositoryTests
    {
        private readonly DbFixture _fixture;
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWorkFactory _factory;

        public RepositoryTests(DbFixture fixture)
        {
            _fixture = fixture;
            _authorRepository = new AuthorRepository(_fixture.UowManager);
            _factory = new UnitOfWorkFactory(_fixture.DbContextFactory, _fixture.UowManager);
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
        public async Task CreateAsync_ValidDataCommited_ShouldSuccess()
        {
            using (var uow = _factory.Create())
            {
                await _authorRepository.CreateAsync(AuthorSeed.MITCHELL);
                await uow.CommitAsync();
            }

            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_MITCHELL);

                Assert.NotNull(author);
            }
        }

        [Fact]
        public async Task CreateAsync_ValidDataNotCommited_ShouldNotSaveData()
        {
            using (var uow = _factory.Create())
            {
                await _authorRepository.CreateAsync(AuthorSeed.KING);
            }

            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.NEW_ID_KING);

                Assert.Null(author);
            }
        }

        [Fact]
        public async Task CreateAsync_NullArgument_ShouldThrowException()
        {
            using (var uow = _factory.Create())
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => _authorRepository.CreateAsync(null));
            }
        }

        [Fact]
        public async Task Create_CreateInCommitedUow_InsideNotCommitedUow_ShouldNotStoreData()
        {
            using (var uow = _factory.Create())
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
                using (var uow2 = _factory.Create())
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
                using (var uow2 = _factory.Create())
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

            using (var uow = _factory.Create())
            {
                var author = await _authorRepository.FindAsync(AuthorSeed.ID_COELHO);

                Assert.Equal(lastname, author.Lastname);
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

        [Fact]
        public async Task FetchAsync_WithoutIncludes_ShouldSuccess()
        {
            using (_factory.Create())
            {
                var data = await _authorRepository.FetchAsync(new AllAuthors());

                Assert.NotEmpty(data);
            }
        }

        [Fact]
        public async Task FetchAsync_WithIncludes_ShouldSuccess()
        {
            using (_factory.Create())
            {
                var data = await _authorRepository.FetchAsync(new AllAuthorsWithIncludes());

                Assert.NotEmpty(data.SelectMany(s => s.AuthorBooks));
            }
        }

        [Fact]
        public async Task FetchOneAsync_WithValidFilter_ShouldSuccess()
        {
            using (_factory.Create())
            {
                var author = await _authorRepository.FetchOneAsync(new AuthorByMurakamiId());

                Assert.NotNull(author);
            }
        }

        [Fact]
        public async Task GetData_WithInvalidFilter_ShouldSuccess()
        {
            using (_factory.Create())
            {
                var author = await _authorRepository.FetchOneAsync(new NoAuthorAtAll());
                var data = await _authorRepository.FetchAsync(new NoAuthorAtAll());

                Assert.Null(author);
                Assert.Empty(data);
            }
        }

        [Fact]
        public async Task GetAsync_WithValidFilterAndIncludes_ShouldSuccess()
        {
            using (_factory.Create())
            {
                var data = await _authorRepository.FetchOneAsync(new AuthorByMurakamiIdWithIncludes());

                Assert.NotNull(data);
                Assert.Equal(2, data.AuthorBooks.Count);
            }
        }

        [Fact]
        public async Task GetAsync_WithValidFilterAndPaging_ShouldSuccess()
        {
            using (_factory.Create())
            {
                var data = await _authorRepository.FetchAsync(new AllAuthorsOrderedByLastnameWithPagingAndSorting());

                Assert.Equal(AuthorSeed.ID_TOLKIEN, data.Last().Id);
            }
        }

        [Fact]
        public async Task GetAsync_WithValidFilterAndPagingAndIncludes_ShouldSuccess()
        {
            using (_factory.Create())
            {
                var data = await _authorRepository.FetchAsync(new AllAuthorsOrderedByLastnameWithPagingAndSorting());

                Assert.Equal(AuthorSeed.ID_TOLKIEN, data.Last().Id);
                Assert.NotEmpty(data.Last().AuthorBooks);
            }
        }
    }
}
