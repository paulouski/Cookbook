using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Models
{
    public class IngredientForRecipe
    {
        public int IngredientForRecipeId { get; set; }
        public int RecipeId { get; set; }

        public string IngredientName { get; set; }
        public string Amount { get; set; }
    }
}
