using System;
using System.ComponentModel.DataAnnotations;

namespace Idea.Cookbook.Models
{
    public class Unit
    {
        public Guid Id { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public string Name { get; set; }
    }
}