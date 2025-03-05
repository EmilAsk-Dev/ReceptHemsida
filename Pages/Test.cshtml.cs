using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;
using System.Security.Claims;

namespace ReceptHemsida.Pages
{
    public class TestModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TestModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = new Recipe();

        [BindProperty]
        public List<RecipeIngredientViewModel> RecipeIngredients { get; set; } = new List<RecipeIngredientViewModel>();

        [BindProperty]
        public List<RecipeInstructionViewModel> Instructions { get; set; } = new List<RecipeInstructionViewModel>();

        [BindProperty]
        public string TagsInput { get; set; } = string.Empty;

        public SelectList Categories { get; set; }
        public SelectList DifficultyLevels { get; set; }
        public List<Ingredient> AvailableIngredients { get; set; }

        public async Task OnGetAsync()
        {
            Categories = new SelectList(Enum.GetValues(typeof(RecipeCategory)));
            DifficultyLevels = new SelectList(new[] { "Easy", "Medium", "Hard", "Expert" });
            AvailableIngredients = await _context.Ingredients.ToListAsync();

            
            for (int i = 0; i < 3; i++)
            {
                RecipeIngredients.Add(new RecipeIngredientViewModel());
            }

            for (int i = 0; i < 3; i++)
            {
                Instructions.Add(new RecipeInstructionViewModel { StepNumber = i + 1 });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            ModelState.Remove("Recipe.Id");
            ModelState.Remove("Recipe.UserId");
            ModelState.Remove("Recipe.User");
            ModelState.Remove("Recipe.ImageUrl");
            ModelState.Remove("Recipe.Instructions");
            ModelState.Remove("Recipe.RecipeIngredients");

            if (!ModelState.IsValid)
            {
                
                foreach (var modelState in ModelState.Where(ms => ms.Value.Errors.Any()))
                {
                    var key = modelState.Key;
                    var errors = modelState.Value.Errors.Select(e => e.ErrorMessage);
                    
                }

                Categories = new SelectList(Enum.GetValues(typeof(RecipeCategory)));
                DifficultyLevels = new SelectList(new[] { "Easy", "Medium", "Hard", "Expert" });
                AvailableIngredients = await _context.Ingredients.ToListAsync();
                return Page();
            }

            
            Recipe.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Recipe.Id = Guid.NewGuid().ToString();
            Recipe.CreatedAt = DateTime.UtcNow;

            
            Recipe.Tags ??= new List<string>();

            
            if (!string.IsNullOrWhiteSpace(TagsInput))
            {
                Recipe.Tags = TagsInput.Split(',')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList();
            }

            
            _context.Recipes.Add(Recipe);

            
            foreach (var item in RecipeIngredients.Where(i => !string.IsNullOrEmpty(i.IngredientName) && !string.IsNullOrEmpty(i.Quantity)))
            {
                
                var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name.ToLower() == item.IngredientName.ToLower());

                
                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = item.IngredientName
                    };
                    _context.Ingredients.Add(ingredient);
                }

                
                var recipeIngredient = new RecipeIngredient
                {
                    RecipeId = Recipe.Id,
                    IngredientId = ingredient.Id,
                    Quantity = item.Quantity,
                    Unit = item.Unit ?? string.Empty
                };

                _context.RecipeIngredients.Add(recipeIngredient);
            }

            
            foreach (var instruction in Instructions.Where(i => !string.IsNullOrEmpty(i.InstructionText)))
            {
                var recipeInstruction = new RecipeInstruction
                {
                    Id = Guid.NewGuid().ToString(),
                    RecipeId = Recipe.Id,
                    StepNumber = instruction.StepNumber,
                    InstructionText = instruction.InstructionText
                };

                _context.RecipeInstructions.Add(recipeInstruction);
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError(string.Empty, "An error occurred while saving: " + ex.Message);

                Categories = new SelectList(Enum.GetValues(typeof(RecipeCategory)));
                DifficultyLevels = new SelectList(new[] { "Easy", "Medium", "Hard", "Expert" });
                AvailableIngredients = await _context.Ingredients.ToListAsync();
                return Page();
            }
        }

        public class RecipeIngredientViewModel
        {
            public string IngredientName { get; set; }
            public string Quantity { get; set; }
            public string Unit { get; set; }
        }

        public class RecipeInstructionViewModel
        {
            public int StepNumber { get; set; }
            public string InstructionText { get; set; }
        }
    }
}