using System;
using System.Linq;

using AutoMapper;

using Idea.Sample.Internals.Models;

using PostEntity = Idea.Sample.Internals.Entities.Post;
using TagEntity = Idea.Sample.Internals.Entities.Tag;

namespace Idea.Sample.Internals.Configurations
{
    internal static class AutomapperConfiguration
    {
        private static Action<IMapperConfigurationExpression> CreateConfigurationExpression()
        {
            return configuration =>
            {
                configuration.CreateMap<TagEntity, TagRead>();
                configuration.CreateMap<Tag, TagEntity>();
                configuration.CreateMap<Post, PostEntity>().ReverseMap();
                configuration
                    .CreateMap<PostEntity, PostRead>()
                    .ForMember(d => d.Tags, o => o.ResolveUsing(s =>
                        s.PostTags.Select(e => e.TagId)));
            };
        }

        public static IMapper Configuration()
        {
            return new MapperConfiguration(CreateConfigurationExpression()).CreateMapper();
        }
    }
}