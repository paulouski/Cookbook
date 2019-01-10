using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int RecipeId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ApplicationUser Author { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
