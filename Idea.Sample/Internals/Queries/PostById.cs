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
    public class PostById : Query<SampleDbContext, ById<Guid>, Post, Guid>
    {
        public PostById(IQueryReader<ById<Guid>> reader) : base(reader)
        {
        }

        protected override IQueryable<Post> CreateQuery()
        {
            var data = Reader.Read();

            return Map().Where(w => w.Id == data.Id).Include(i => i.PostTags).ThenInclude(i => i.Tag);
        }
    }
}