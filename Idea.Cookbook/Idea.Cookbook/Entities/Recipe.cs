using System;
using System.Collections.Generic;

using Idea.Entity;

namespace Idea.Cookbook.Entities
{
    public class Recipe : Record<Guid>
    {
        public string Name { get; set; }

        public string Directions { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}