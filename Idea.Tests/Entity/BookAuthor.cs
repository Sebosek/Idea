namespace Idea.Tests.Entity
{
    public class BookAuthor : Idea.Entity.Entity<int>
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
