using System;

using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace Idea.SmartQuery.EntityFrameworkCore.Extensions
{
    public static class UnitOfWorkExtensions
    {
        private const string MESSAGE = "Given Unit of work can not be used in EntityFramework Core Query.";

        public static UnitOfWork<TDbContext> ToEntityFrameworkUnitOfWork<TDbContext>(this IUnitOfWork uow)
            where TDbContext : DbContext
        {
            if (!(uow is UnitOfWork<TDbContext> input))
            {
                throw new ArgumentException(MESSAGE);
            }

            return input;
        }

        public static void CheckEntityFrameworkUnitOfWork<TDbContext>(this IUnitOfWork uow)
            where TDbContext : DbContext
        {
            if (!(uow is UnitOfWork<TDbContext> input))
            {
                throw new ArgumentException(MESSAGE);
            }
        }
    }
}