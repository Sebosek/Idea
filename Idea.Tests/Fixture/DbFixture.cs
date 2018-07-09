using System;

using Idea.Tests.Entity;
using Idea.Tests.Fixture.Seed;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using UnitOfWorkGenerationFactory = Idea.UnitOfWork.UnitOfWorkGenerationFactory;

namespace Idea.Tests.Fixture
{
    public class DbFixture : IDisposable
    {
        private readonly TestDbContext _context;

        private IUnitOfWorkManager _manager;

        private IDbContextFactory<TestDbContext> _dbContextFactory;

        public TestDbContext Context => _context;

        public IUnitOfWorkManager UowManager => _manager;

        public IDbContextFactory<TestDbContext> DbContextFactory => _dbContextFactory;

        public DbFixture()
        {
            var options = new DbContextOptionsBuilder();
            options.UseInMemoryDatabase("UnitTest");

            _context = new TestDbContext(options.Options);
            _dbContextFactory = new DbContextFactory<TestDbContext>(o => o.UseInMemoryDatabase("UnitTest"));
            _manager = new UnitOfWorkManager(new UnitOfWorkGenerationFactory());

            SeedData();
            Context.SaveChanges();
        }

        private void SeedData()
        {
            Context.Authors.AddRange(
                  AuthorSeed.ROWLING
                , AuthorSeed.TOLKIEN
                , AuthorSeed.MURAKAMI
                , AuthorSeed.COELHO
                , AuthorSeed.PALAHNIUK
                , AuthorSeed.NESBO
                , AuthorSeed.CLARK
                , AuthorSeed.HAMINGWAY
                , AuthorSeed.STEINBECK
                , AuthorSeed.SAINT_EXUPERI
                , AuthorSeed.WILDE
                , AuthorSeed.ADAMS);

            Context.Categories.AddRange(
                  CategorySeed.NOVEL
                , CategorySeed.FANTASY
                , CategorySeed.CRIME
                , CategorySeed.HOROR
                , CategorySeed.MAGICAL_REALISM
                , CategorySeed.NATURALISM);

            Context.Books.AddRange(BookSeed.BOOKS_DATA);

            Context.BooksAuthors.AddRange(
                  new BookAuthor { AuthorId = AuthorSeed.ID_COELHO, BookId = BookSeed.ID_ALCHEMIST }
                , new BookAuthor { AuthorId = AuthorSeed.ID_COELHO, BookId = BookSeed.ID_ELEVEN_MINUTES }
                , new BookAuthor { AuthorId = AuthorSeed.ID_MURAKAMI, BookId = BookSeed.ID_LIBRARY }
                , new BookAuthor { AuthorId = AuthorSeed.ID_MURAKAMI, BookId = BookSeed.ID_KAFKA_ON_THE_STORE }
                , new BookAuthor { AuthorId = AuthorSeed.ID_PALAHNIUK, BookId = BookSeed.ID_HAUNTED }
                , new BookAuthor { AuthorId = AuthorSeed.ID_PALAHNIUK, BookId = BookSeed.ID_FIGHT_CLUB }
                , new BookAuthor { AuthorId = AuthorSeed.ID_ROWLING, BookId = BookSeed.ID_HARRY_POTTER_I }
                , new BookAuthor { AuthorId = AuthorSeed.ID_ROWLING, BookId = BookSeed.ID_HARRY_POTTER_II }
                , new BookAuthor { AuthorId = AuthorSeed.ID_ROWLING, BookId = BookSeed.ID_HARRY_POTTER_III }
                , new BookAuthor { AuthorId = AuthorSeed.ID_TOLKIEN, BookId = BookSeed.ID_LORD_OF_THE_RINGS_I }
                , new BookAuthor { AuthorId = AuthorSeed.ID_TOLKIEN, BookId = BookSeed.ID_LORD_OF_THE_RINGS_II }
                , new BookAuthor { AuthorId = AuthorSeed.ID_TOLKIEN, BookId = BookSeed.ID_LORD_OF_THE_RINGS_III });

            Context.BooksCategories.AddRange(
                  new BookCategory { CategoryId = CategorySeed.ID_CRIME, BookId = BookSeed.ID_FIGHT_CLUB }
                , new BookCategory { CategoryId = CategorySeed.ID_FANTASY, BookId = BookSeed.ID_HARRY_POTTER_I }
                , new BookCategory { CategoryId = CategorySeed.ID_FANTASY, BookId = BookSeed.ID_HARRY_POTTER_II }
                , new BookCategory { CategoryId = CategorySeed.ID_FANTASY, BookId = BookSeed.ID_HARRY_POTTER_III }
                , new BookCategory { CategoryId = CategorySeed.ID_FANTASY, BookId = BookSeed.ID_LORD_OF_THE_RINGS_I }
                , new BookCategory { CategoryId = CategorySeed.ID_FANTASY, BookId = BookSeed.ID_LORD_OF_THE_RINGS_II }
                , new BookCategory { CategoryId = CategorySeed.ID_FANTASY, BookId = BookSeed.ID_LORD_OF_THE_RINGS_III }
                , new BookCategory { CategoryId = CategorySeed.ID_HOROR, BookId = BookSeed.ID_HAUNTED }
                , new BookCategory { CategoryId = CategorySeed.ID_MAGICALREALISM, BookId = BookSeed.ID_ALCHEMIST }
                , new BookCategory { CategoryId = CategorySeed.ID_MAGICALREALISM, BookId = BookSeed.ID_LIBRARY }
                , new BookCategory { CategoryId = CategorySeed.ID_MAGICALREALISM, BookId = BookSeed.ID_KAFKA_ON_THE_STORE }
                , new BookCategory { CategoryId = CategorySeed.ID_NATURALISM, BookId = BookSeed.ID_ELEVEN_MINUTES }
                , new BookCategory { CategoryId = CategorySeed.ID_NATURALISM, BookId = BookSeed.ID_FIGHT_CLUB }
                , new BookCategory { CategoryId = CategorySeed.ID_NATURALISM, BookId = BookSeed.ID_HAUNTED }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_ALCHEMIST }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_ELEVEN_MINUTES }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_FIGHT_CLUB }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_HARRY_POTTER_I }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_HARRY_POTTER_II }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_HARRY_POTTER_III }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_HAUNTED }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_KAFKA_ON_THE_STORE }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_LIBRARY }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_LORD_OF_THE_RINGS_I }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_LORD_OF_THE_RINGS_II }
                , new BookCategory { CategoryId = CategorySeed.ID_NOVEL, BookId = BookSeed.ID_LORD_OF_THE_RINGS_III });
        }

        public void Dispose()
        {
            _manager = null;
            _context.Dispose();
        }
    }
}
