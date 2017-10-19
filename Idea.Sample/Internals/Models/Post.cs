using System;
using System.Collections.Generic;

namespace Idea.Sample.Internals.Models
{
    public class Post
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public IEnumerable<Guid> Tags { get; set; }
    }
}