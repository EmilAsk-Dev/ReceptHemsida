using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Models;
using ReceptHemsida.Services;

namespace ReceptHemsida.Pages
{
    public class AllRecipeModel : PageModel
    {
        private readonly RecipeService _recipeService;

        public AllRecipeModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }
        public List<Recipe> Recipes { get; set; } = new();
       
        public async Task OnGetAsync()
        {
            Recipes = await _recipeService.GetAllRecipesAsync();
        }
    }
}
