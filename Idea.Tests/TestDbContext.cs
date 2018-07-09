using Idea.Tests.Entity;
using Idea.UnitOfWork.EntityFrameworkCore;
using Idea.UnitOfWork.EntityFrameworkCore.Enums;

using Microsoft.EntityFrameworkCore;

namespace Idea.Tests
{
    public class TestDbContext : ModelContext<int>
    {
        public TestDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<BookAuthor> BooksAuthors { get; set; }

        public DbSet<BookCategory> BooksCategories { get; set; }

        public override RemoveStrategy AppliedRemoveStrategy() => RemoveStrategy.Drop;

        protected override void DbModel(ModelBuilder builder)
        {
            builder.Entity<Book>().Property(p => p.Title).IsRequired();
            builder.Entity<Book>().HasIndex(i => i.Title).IsUnique();
            builder.Entity<Book>().HasKey(k => k.Id);

            builder.Entity<Author>().Property(p => p.Firstname).IsRequired();
            builder.Entity<Author>().Property(p => p.Lastname).IsRequired();
            builder.Entity<Author>().HasKey(k => k.Id);

            builder.Entity<Category>().Property(p => p.Name).IsRequired();
            builder.Entity<Category>().HasKey(k => k.Id);

            builder.Entity<BookAuthor>().HasKey(k => k.Id);
            builder.Entity<BookAuthor>().HasOne(o => o.Author).WithMany(m => m.AuthorBooks).HasForeignKey(k => k.AuthorId);
            builder.Entity<BookAuthor>().HasOne(o => o.Book).WithMany(m => m.BookAuthors).HasForeignKey(k => k.BookId);

            builder.Entity<BookCategory>().HasKey(k => k.Id);
            builder.Entity<BookCategory>().HasOne(o => o.Category).WithMany(m => m.CategoryBooks).HasForeignKey(k => k.CategoryId);
            builder.Entity<BookCategory>().HasOne(o => o.Book).WithMany(m => m.BookCategories).HasForeignKey(k => k.BookId);
        }
    }
}
