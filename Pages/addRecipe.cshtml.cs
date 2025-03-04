using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Services;
using ReceptHemsida.Models;
using ReceptHemsida.Data;

namespace ReceptHemsida.Pages
{
    public class addRecipeModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly RecipeIngredientService _recipeIngredientService;
        private readonly IngredientService _ingredientService;

        public addRecipeModel(RecipeService recipeService, RecipeIngredientService recipeIngredientService, IngredientService ingredientService)
        {
            _recipeService = recipeService;
            _recipeIngredientService = recipeIngredientService;
            _ingredientService = ingredientService;
        }

        [BindProperty]
        public Recipe Recipe { get; set; }=new Recipe();
        [BindProperty]
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

      
        public void OnGet()
        {

        }
    }
}
