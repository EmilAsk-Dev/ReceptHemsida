using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Services;
using ReceptHemsida.Models;
using ReceptHemsida.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ReceptHemsida.Pages
{
    public class addRecipeModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly RecipeIngredientService _recipeIngredientService;
        private readonly IngredientService _ingredientService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public addRecipeModel(
            RecipeService recipeService,
            RecipeIngredientService recipeIngredientService,
            IngredientService ingredientService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _recipeService = recipeService;
            _recipeIngredientService = recipeIngredientService;
            _ingredientService = ingredientService;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "Recipe title is required.")]
        public Recipe Recipe { get; set; } = new Recipe { Title = "Change Title" };

        [BindProperty]
        public List<string> SelectedIngredientIds { get; set; } = new List<string>();

        [BindProperty]
        public List<string> IngredientQuantities { get; set; } = new List<string>();

        [BindProperty]
        [Required(ErrorMessage = "Please add at least one instruction.")]
        public List<string> InstructionTexts { get; set; } = new List<string>();

        public List<Ingredient> IngredientsList { get; set; } = new List<Ingredient>();
        public List<RecipeInstruction> RecipeInstructionList { get; set; } = new List<RecipeInstruction>();

        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            IngredientsList = await _context.Ingredients.ToListAsync();
            Recipe.Title = "Change Title";
            Recipe = new Recipe
            {
                UserId = user.Id
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Validation checks here
            if (!ModelState.IsValid)
            {
                return Page(); // Return the same page to show validation errors
            }

            // Create and Save Recipe
            Recipe.UserId = user.Id;

            // Process Tags
            if (!string.IsNullOrWhiteSpace(Request.Form["Recipe.Tags"]))
            {
                Recipe.Tags = Request.Form["Recipe.Tags"].ToString()
                    .Split(',')
                    .Select(t => t.Trim())
                    .ToList();
            }

            _context.Recipes.Add(Recipe);
            await _context.SaveChangesAsync();

            // Add Ingredients to Recipe
            for (int i = 0; i < SelectedIngredientIds.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(SelectedIngredientIds[i]))
                {
                    var recipeIngredient = new RecipeIngredient
                    {
                        RecipeId = Recipe.Id,
                        IngredientId = SelectedIngredientIds[i],
                        Quantity = i < IngredientQuantities.Count
                            ? IngredientQuantities[i]
                            : "1 unit"
                    };
                    _context.RecipeIngredients.Add(recipeIngredient);
                }
            }

            // Add Instructions to Recipe
            for (int i = 0; i < InstructionTexts.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(InstructionTexts[i]))
                {
                    var instruction = new RecipeInstruction
                    {
                        RecipeId = Recipe.Id,
                        StepNumber = i + 1,
                        InstructionText = InstructionTexts[i]
                    };
                    _context.RecipeInstructions.Add(instruction);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Recipes");
        }
    }
}