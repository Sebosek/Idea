using System.Collections.Generic;

namespace Idea.Cookbook.Models
{
    public class RecipeUpdate
    {
        public string Name { get; set; }

        public string Directions { get; set; }

        public IEnumerable<IdModel> Ingredients { get; set; }
    }
}