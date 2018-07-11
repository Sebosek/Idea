using System;
using System.Collections.Generic;

namespace Idea.Cookbook.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Directions { get; set; }

        public IEnumerable<IdModel> Ingredients { get; set; }
    }
}