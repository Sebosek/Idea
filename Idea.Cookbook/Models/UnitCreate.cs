using System.ComponentModel.DataAnnotations;

namespace Idea.Cookbook.Models
{
    public class UnitCreate
    {
        [Required]
        public string Symbol { get; set; }

        [Required]
        public string Name { get; set; }
    }
}