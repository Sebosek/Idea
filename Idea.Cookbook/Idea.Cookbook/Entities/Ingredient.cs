using System;

using Idea.Entity;

namespace Idea.Cookbook.Entities
{
    public class Ingredient : Entity<Guid>
    {
        public string Name { get; set; }

        public float? Amount { get; set; }

        public Guid UnitId { get; set; }

        public Unit Unit { get; set; }

        public Guid? RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}