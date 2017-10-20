using Idea.Entity;

using System;
using System.Collections.Generic;

namespace Idea.Sample.Internals.Entities
{
    public class Tag : Entity<Guid>
    {
        public string Value { get; set; }

        public List<PostTag> PostTags { get; set; }
    }
}