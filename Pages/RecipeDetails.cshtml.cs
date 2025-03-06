using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Models;
using ReceptHemsida.Services;

namespace ReceptHemsida.Pages
{
    public class RecipeDetailsModel : PageModel
    {
        private readonly RecipeService _recipeService;
        
        public Recipe Recipe { get; set; }
        
        public RecipeDetailsModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }
        
        public async Task<IActionResult> OnGetAsync(string id)
        {
            
            Recipe = await _recipeService.GetRecipeByIdAsync(id);
            
            
            if (Recipe == null)
            {
                return Page();
            }
            
            return Page();
        }
    }
}