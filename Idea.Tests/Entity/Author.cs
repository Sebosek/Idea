using System.Collections.Generic;

namespace Idea.Tests.Entity
{
    public class Author : Idea.Entity.Entity<int>
    {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }

        public virtual ICollection<BookAuthor> AuthorBooks { get; set; }
    }
}
