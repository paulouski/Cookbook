using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public ApplicationUser Author { get; set; }

        [Required]
        public string RecipeName { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Abstract { get; set; }
        [Required]
        public string Description { get; set; }

        public List<IngredientForRecipe> IngredientsForRecipe { get; set; } = new List<IngredientForRecipe>();

        public DateTime LastEditTime { get; set; }

        public int Rating { get; set; } = 0;
        public List<ApplicationUser> RatesUsers { get; set; } = new List<ApplicationUser>();

        public string CurrentComment { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
