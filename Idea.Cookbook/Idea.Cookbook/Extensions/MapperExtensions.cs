using System;
using System.Threading.Tasks;

using AutoMapper;

namespace Idea.Cookbook.Extensions
{
    public static class MapperExtensions
    {
        public static async Task<TSource> Bridge<TSource, TEntity>(this IMapper mapper, Func<Task<TEntity>> fnc) =>
            mapper.Map<TSource>(await fnc().ConfigureAwait(false));
    }
}