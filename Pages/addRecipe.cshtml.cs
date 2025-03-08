using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;
using ReceptHemsida.Services;
using System.Security.Claims;

namespace ReceptHemsida.Pages
{
    public class AddRecipeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly RecipeService _recipeService;
        public AddRecipeModel(ApplicationDbContext context, RecipeService recipeService)
        {
            _recipeService = recipeService;
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = new Recipe();

        [BindProperty]
        public List<RecipeIngredientViewModel> RecipeIngredients { get; set; } = new List<RecipeIngredientViewModel>();

        [BindProperty]
        public List<RecipeInstructionViewModel> Instructions { get; set; } = new List<RecipeInstructionViewModel>();

        public SelectList Categories { get; set; }

        [BindProperty]
        public string TagsInput { get; set; } = string.Empty;

        [BindProperty]
        public bool IsEditMode { get; set; }

        public SelectList DifficultyLevels { get; set; }

        public List<Ingredient> AvailableIngredients { get; set; }

        public async Task OnGetAsync(string id = null)
        {
            Categories = new SelectList(Enum.GetValues(typeof(RecipeCategory)));
            DifficultyLevels = new SelectList(new[] { "Easy", "Medium", "Hard", "Expert" });
            AvailableIngredients = await _context.Ingredients.ToListAsync();

            if (!string.IsNullOrEmpty(id))
            {
                IsEditMode = true;

                Recipe = await _recipeService.GetRecipeByIdAsync(id);

                if (Recipe != null)
                {
                    RecipeIngredients = Recipe.RecipeIngredients.Select(ri => new RecipeIngredientViewModel
                    {
                        IngredientName = ri.Ingredient.Name,
                        Quantity = ri.Quantity,
                        Unit = ri.Unit
                    }).ToList();

                    Instructions = Recipe.Instructions
                        .OrderBy(i => i.StepNumber)
                        .Select(i => new RecipeInstructionViewModel
                        {
                            StepNumber = i.StepNumber,
                            InstructionText = i.InstructionText
                        }).ToList();

                    TagsInput = string.Join(", ", Recipe.Tags);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    RecipeIngredients.Add(new RecipeIngredientViewModel());
                }

                for (int i = 0; i < 3; i++)
                {
                    Instructions.Add(new RecipeInstructionViewModel { StepNumber = i + 1 });
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IsEditMode = !string.IsNullOrEmpty(Recipe.Id);

            ModelState.Remove("Recipe.Id");
            ModelState.Remove("Recipe.UserId");
            ModelState.Remove("Recipe.User");
            ModelState.Remove("Recipe.ImageUrl");
            ModelState.Remove("Recipe.Instructions");
            ModelState.Remove("Recipe.RecipeIngredients");

            if (!ModelState.IsValid)
            {
                Categories = new SelectList(Enum.GetValues(typeof(RecipeCategory)));
                DifficultyLevels = new SelectList(new[] { "Easy", "Medium", "Hard", "Expert" });
                AvailableIngredients = await _context.Ingredients.ToListAsync();
                return Page();
            }

            if (IsEditMode)
            {
                // Update an existing recipe
                var existingRecipe = await _recipeService.GetRecipeByIdAsync(Recipe.Id);
                if (existingRecipe == null)
                {
                    return NotFound();
                }

                // Update the properties of the existing recipe
                existingRecipe.Title = Recipe.Title;
                existingRecipe.Description = Recipe.Description;
                existingRecipe.CookTime = Recipe.CookTime;
                existingRecipe.Difficulty = Recipe.Difficulty;
                existingRecipe.Category = Recipe.Category;

                // Handle tags
                existingRecipe.Tags = !string.IsNullOrWhiteSpace(TagsInput)
                    ? TagsInput.Split(',')
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrEmpty(t))
                        .ToList()
                    : new List<string>();

                // Remove old ingredients and instructions
                _context.RecipeIngredients.RemoveRange(_context.RecipeIngredients.Where(ri => ri.RecipeId == existingRecipe.Id));
                _context.RecipeInstructions.RemoveRange(_context.RecipeInstructions.Where(ri => ri.RecipeId == existingRecipe.Id));

                // Add new ingredients
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
                        RecipeId = existingRecipe.Id,
                        IngredientId = ingredient.Id,
                        Quantity = item.Quantity,
                        Unit = item.Unit ?? string.Empty
                    };

                    _context.RecipeIngredients.Add(recipeIngredient);
                }

                // Add new instructions
                foreach (var instruction in Instructions.Where(i => !string.IsNullOrEmpty(i.InstructionText)))
                {
                    var recipeInstruction = new RecipeInstruction
                    {
                        Id = Guid.NewGuid().ToString(),
                        RecipeId = existingRecipe.Id,
                        StepNumber = instruction.StepNumber,
                        InstructionText = instruction.InstructionText
                    };

                    _context.RecipeInstructions.Add(recipeInstruction);
                }

                _context.Recipes.Update(existingRecipe);
            }
            else
            {
                // Create new recipe
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

                // Add ingredients
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

                // Add instructions
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
            }

            try
            {
                // Save the changes to the database
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