using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Services;
using ReceptHemsida.Models;
using System.Threading.Tasks;


namespace ReceptHemsida.Pages
{
    public class RecipeDetailsModel : PageModel
    {
        private readonly RecipeService _recipeService;
        public RecipeDetailsModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }
        public Recipe Recipe { get; set; }

        public async Task OnGetAsync(string id)
        {
            Recipe = await _recipeService.GetRecipeByIdAsync(id);
            if(Recipe == null)
            {
                RedirectToPage("/Index");
            }
        }
        public void OnGet()
        {
        }
    }
}
