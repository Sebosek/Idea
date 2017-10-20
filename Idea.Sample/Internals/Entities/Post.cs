using System;
using System.Collections.Generic;

using Idea.Entity;

namespace Idea.Sample.Internals.Entities
{
    public class Post : Entity<Guid>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public List<PostTag> PostTags { get; set; }
    }
}