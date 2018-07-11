using System;
using System.Collections.Generic;

using Idea.Entity;

namespace Idea.Cookbook.Entities
{
    public class Unit : Entity<Guid>
    {
        public string Symbol { get; set; }

        public string Name { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}