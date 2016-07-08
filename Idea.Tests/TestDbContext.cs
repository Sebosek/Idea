using Idea.Tests.Entity;
using Microsoft.EntityFrameworkCore;

namespace Idea.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext()
        { }

        public TestDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<BookCategory> BooksCategories { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(p => p.Title).IsRequired();
            modelBuilder.Entity<Book>().HasIndex(i => i.Title).IsUnique();
            modelBuilder.Entity<Book>().HasKey(k => k.Id);

            modelBuilder.Entity<Author>().Property(p => p.Firstname).IsRequired();
            modelBuilder.Entity<Author>().Property(p => p.Lastname).IsRequired();
            modelBuilder.Entity<Author>().HasKey(k => k.Id);

            modelBuilder.Entity<Category>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Category>().HasKey(k => k.Id);

            modelBuilder.Entity<BookAuthor>().HasKey(k => k.Id);
            modelBuilder.Entity<BookAuthor>().HasOne(o => o.Author).WithMany(m => m.AuthorBooks).HasForeignKey(k => k.AuthorId);
            modelBuilder.Entity<BookAuthor>().HasOne(o => o.Book).WithMany(m => m.BookAuthors).HasForeignKey(k => k.BookId);

            modelBuilder.Entity<BookCategory>().HasKey(k => k.Id);
            modelBuilder.Entity<BookCategory>().HasOne(o => o.Category).WithMany(m => m.CategoryBooks).HasForeignKey(k => k.CategoryId);
            modelBuilder.Entity<BookCategory>().HasOne(o => o.Book).WithMany(m => m.BookCategories).HasForeignKey(k => k.BookId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
