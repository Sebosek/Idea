using System;

using Idea.Cookbook.Entities;
using Idea.UnitOfWork.EntityFrameworkCore;
using Idea.UnitOfWork.EntityFrameworkCore.Enums;
using Idea.UnitOfWork.EntityFrameworkCore.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Idea.Cookbook
{
    public class CookbookModelContext : ModelContext<Guid>
    {
        public CookbookModelContext(DbContextOptions<CookbookModelContext> options) : base(options) { }

        public override RemoveStrategy AppliedRemoveStrategy() => RemoveStrategy.MarkAsRemoved;

        protected override void DbModel(ModelBuilder builder) =>
            builder
                .Record<Unit, Guid>(e => e.HasData(UnitData()))
                .Record<Ingredient, Guid>(e =>
                {
                    e.Property(p => p.UnitId).IsRequired();
                    e.HasOne<Unit>().WithMany().HasForeignKey(k => k.UnitId);
                })
                .Record<Recipe, Guid>(e => e.HasMany(m => m.Ingredients).WithOne(o => o.Recipe).HasForeignKey(k => k.RecipeId));

        private Unit[] UnitData() =>
            new[]
                {
                    new Unit { Id = Guid.NewGuid(), Name = "Kilogram", Symbol = "Kg" },
                    new Unit { Id = Guid.NewGuid(), Name = "Gram", Symbol = "g" },
                    new Unit { Id = Guid.NewGuid(), Name = "Liter", Symbol = "l" }
                };
    }
}