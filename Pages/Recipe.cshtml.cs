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

        [BindProperty(SupportsGet = true)]
        public int? CookTime { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Difficulty { get; set; }

        public Recipe SingleRecipe { get; set; }
        public List<Recipe> Recipes { get; set; } = new();

        public RecipeModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var allRecipes = await _recipeService.GetAllRecipesAsync();

            if (!string.IsNullOrEmpty(Id))
            {
                SingleRecipe = await _recipeService.GetRecipeByIdAsync(Id);
                if (SingleRecipe == null)
                {
                    return NotFound();
                }
                return Page();
            }

            // Start with all recipes
            var filteredRecipes = allRecipes;

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(Search))
            {
                filteredRecipes = filteredRecipes
                    .Where(r => r.Title.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                                r.Description.Contains(Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply category filter if provided
            if (!string.IsNullOrEmpty(Category) && Enum.TryParse<RecipeCategory>(Category, true, out var categoryEnum))
            {
                filteredRecipes = filteredRecipes.Where(r => r.Category == categoryEnum).ToList();
            }

            // Apply cook time filter if provided
            if (CookTime.HasValue)
            {
                filteredRecipes = filteredRecipes.Where(r => r.CookTime <= CookTime.Value).ToList();
            }

            // Apply difficulty filter if provided
            if (!string.IsNullOrEmpty(Difficulty))
            {
                filteredRecipes = filteredRecipes
                    .Where(r => r.Difficulty.Equals(Difficulty, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            Recipes = filteredRecipes;
            return Page();
        }
    }
}