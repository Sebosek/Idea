using System;
using System.Linq;

using Idea.Sample.Internals.DbContext;
using Idea.Sample.Internals.Entities;
using Idea.SmartQuery.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace Idea.Sample.Internals.Queries
{
    public class AllPosts : Query<SampleDbContext, Post, Guid>
    {
        protected override IQueryable<Post> CreateQuery()
        {
            return Map<Post>().Include(i => i.PostTags).ThenInclude(i => i.Tag);
        }
    }
}