using System.ComponentModel.DataAnnotations;

namespace Idea.Cookbook.Models
{
    public class UnitUpdate
    {
        [Required]
        public string Symbol { get; set; }

        [Required]
        public string Name { get; set; }
    }
}