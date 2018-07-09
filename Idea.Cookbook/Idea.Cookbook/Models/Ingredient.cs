using System;
using System.ComponentModel.DataAnnotations;

namespace Idea.Cookbook.Models
{
    public class Ingredient
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public IdModel Unit { get; set; }
    }
}