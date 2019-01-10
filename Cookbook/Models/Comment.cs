using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Models
{
    public class Comment
    {
        public string CommentId { get; set; }
        public string RecipeId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ApplicationUser Author { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
