namespace Idea.Tests.Entity
{
    public class BookCategory : Idea.Entity.Entity<int>
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
