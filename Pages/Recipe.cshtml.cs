using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Models;
using ReceptHemsida.Services;

namespace ReceptHemsida.Pages
{
    public class RecipeModel : PageModel
    {
        private readonly RecipeService _recipeService;


        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        public RecipeModel(RecipeService recipeService)
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
