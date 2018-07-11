using System;
using System.ComponentModel.DataAnnotations;

namespace Idea.Cookbook.Models
{
    public class IdModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}