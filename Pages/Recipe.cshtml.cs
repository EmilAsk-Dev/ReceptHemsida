using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ReceptHemsida.Models;
using ReceptHemsida.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceptHemsida.Pages
{
    public class RecipeModel : PageModel
    {
        private readonly RecipeService _recipeService;

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        public Recipe SingleRecipe { get; set; }
        public List<Recipe> Recipes { get; set; } = new();

        public RecipeModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public async Task<IActionResult> OnGetAsync(string? id, string? category, string? search)
        {
            var allRecipes = await _recipeService.GetAllRecipesAsync();

            if (!string.IsNullOrEmpty(id))
            {
                SingleRecipe = await _recipeService.GetRecipeByIdAsync(id);
                if (SingleRecipe == null)
                {
                    return NotFound();
                }
                return Page();
            }

            if (!string.IsNullOrEmpty(search))
            {
                Recipes = allRecipes
                    .Where(r => r.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                r.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return Page();
            }

            if (!string.IsNullOrEmpty(category) && Enum.TryParse<RecipeCategory>(category, true, out var categoryEnum))
            {
                Recipes = allRecipes.Where(r => r.Category == categoryEnum).ToList();
                return Page();
            }

            // Default: Show all recipes
            Recipes = allRecipes;
            return Page();
        }
    }
}
