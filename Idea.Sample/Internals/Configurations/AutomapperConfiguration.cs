using System;
using System.Linq;

using AutoMapper;

using Idea.Sample.Internals.Models;

using PostEntity = Idea.Sample.Internals.Entities.Post;

namespace Idea.Sample.Internals.Configurations
{
    internal static class AutomapperConfiguration
    {
        private static Action<IMapperConfigurationExpression> CreateConfigurationExpression()
        {
            return configuration =>
            {
                configuration
                    .CreateMap<PostEntity, PostRead>()
                    .ForMember(d => d.Tags, o => o.ResolveUsing(s =>
                        s.Tags.Select(e => e.TagId)));

                configuration.CreateMap<Post, PostEntity>().ReverseMap();
            };
        }

        public static IMapper Configuration()
        {
            return new MapperConfiguration(CreateConfigurationExpression()).CreateMapper();
        }
    }
}