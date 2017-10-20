using System;
using System.Linq;

using Idea.Sample.Internals.DbContext;
using Idea.Sample.Internals.Entities;
using Idea.SmartQuery.EntityFrameworkCore;
using Idea.SmartQuery.Interfaces;
using Idea.SmartQuery.QueryData;

using Microsoft.EntityFrameworkCore;

namespace Idea.Sample.Internals.Queries
{
    public class PostById : Query<SampleDbContext, Post, Guid>
    {
        public PostById(IQueryReader<GetById<Guid>> reader) : base(reader)
        {
        }

        protected override IQueryable<Post> CreateQuery()
        {
            if (Reader.Read() is GetById<Guid> data)
            {
                return Map<Post>().Where(w => w.Id == data.Id).Include(i => i.PostTags).ThenInclude(i => i.Tag);
            }

            return new EnumerableQuery<Post>(new Post[0]);
        }
    }
}