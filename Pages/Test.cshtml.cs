using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Data;
using ReceptHemsida.Models;
using ReceptHemsida.Services;

namespace ReceptHemsida.Pages
{
    public class TestModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        // Constructor to inject dependencies
        public TestModel(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // BindProperty for UserId if you want it as input (e.g., in a form)
        [BindProperty]
        public string UserId { get; set; } = "c968b6bc-5f17-4041-914c-de91bfbbf1a7"; // Default value for testing

        // This method gets called when the form is submitted
        public async Task<IActionResult> OnPostCreateDummyRecipes()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                ModelState.AddModelError("", "Please provide a User ID.");
                return Page();
            }

            // Get the user
            var user = await _userService.GetUserByUsernameAsync("Emil");

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return Page();
            }

            // Create a dummy recipe
            var recipe = new Recipe
            {
                UserId = UserId,
                Title = "Pancakes",
                Description = "Delicious fluffy pancakes.",
                Category = RecipeCategory.Breakfast,
                CookTime = 20,
                Difficulty = "Easy",
                Instructions = new List<RecipeInstruction>
        {
            new RecipeInstruction
            {
                StepNumber = 1,
                InstructionText = "Mix ingredients."
            },
            new RecipeInstruction
            {
                StepNumber = 2,
                InstructionText = "Cook on a hot pan."
            }
        },
                Tags = new List<string> { "Breakfast", "Easy" }
            };

            // Save the recipe first, which will generate the Id
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();  // Save the recipe and get the generated RecipeId

            // Now create RecipeInstructions that reference the saved recipe's Id
            foreach (var instruction in recipe.Instructions)
            {
                instruction.RecipeId = recipe.Id;  // Set the RecipeId to the generated Id
                _context.RecipeInstructions.Add(instruction);
            }

            // Save the instructions
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");  // Redirect after success
        }

    }

}
