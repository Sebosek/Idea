using System.Collections.Generic;

namespace Idea.Tests.Entity
{
    public class Book : Idea.Entity.Entity<int>
    {
        public string Title { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<BookCategory> BookCategories { get; set; }
    }
}
