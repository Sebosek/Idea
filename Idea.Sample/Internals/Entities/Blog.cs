using System;
using System.Collections.Generic;

using Idea.Entity;

namespace Idea.Sample.Internals.Entities
{
    public class Blog : Entity<Guid>
    {
        public string Name { get; set; }

        public string Owner { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}