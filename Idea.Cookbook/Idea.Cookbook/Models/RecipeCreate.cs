using System.Collections.Generic;

namespace Idea.Cookbook.Models
{
    public class RecipeCreate
    {
        public string Name { get; set; }

        public string Directions { get; set; }

        public IEnumerable<IdModel> Ingredients { get; set; }
    }
}