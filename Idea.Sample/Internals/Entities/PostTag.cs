using System;
using Idea.Entity;

namespace Idea.Sample.Internals.Entities
{
    public class PostTag : Entity<Guid>
    {
        public Guid PostId { get; set; }

        public Post Post { get; set; }

        public Guid TagId { get; set; }

        public Tag Tag { get; set; }
    }
}