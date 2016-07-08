using System.Collections.Generic;

namespace Idea.Tests.Entity
{
    public class Category : Idea.Entity.Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<BookCategory> CategoryBooks { get; set; }
    }
}
