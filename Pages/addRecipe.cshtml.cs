using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Services;
using ReceptHemsida.Models;
using ReceptHemsida.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReceptHemsida.Pages
{
    public class AddRecipeModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly RecipeIngredientService _recipeIngredientService;
        private readonly IngredientService _ingredientService;

        public AddRecipeModel(RecipeService recipeService, RecipeIngredientService recipeIngredientService, IngredientService ingredientService)
        {
            _recipeService = recipeService;
            _recipeIngredientService = recipeIngredientService;
            _ingredientService = ingredientService;
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = new Recipe();
        [BindProperty]
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _recipeService.AddRecipeAsync(Recipe);
            foreach (var recipeIngredient in RecipeIngredients)
            {
                recipeIngredient.RecipeId = Recipe.Id;
                await _recipeIngredientService.AddRecipeIngredientAsync(recipeIngredient);
            }
            return RedirectToPage("Index");
        }

        public void OnGet()
        {
            Categories = new List<SelectListItem>();

            foreach (var category in Enum.GetValues(typeof(RecipeCategory)))
            {
                Categories.Add(new SelectListItem
                {
                    Value = category.ToString(),
                    Text = category.ToString()
                });
            }
        }
    }
}