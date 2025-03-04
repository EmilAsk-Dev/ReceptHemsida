using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;

namespace ReceptHemsida.Services
{
    public class RecipeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(ApplicationDbContext context, ILogger<RecipeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            try
            {
                return await _context.Recipes
                    .Include(r => r.User)
                    .Include(r => r.RecipeIngredients)
                    .Include(r => r.Comments)
                    .Include(r => r.Favorites)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipes");
                return new List<Recipe>();
            }
        }

        public async Task<List<Recipe>> RecipesCreatedByUser(string userId)
        {
            try
            {
                return await _context.Recipes
                    .Where(r => r.UserId == userId) // Filters by UserId to get recipes created by a specific user
                    .Include(r => r.User) // Optionally include user information (creator of the recipe)
                    .Include(r => r.RecipeIngredients) // Optionally include ingredients
                    .Include(r => r.Comments) // Optionally include comments
                    .Include(r => r.Favorites) // Optionally include favorites
                    .ToListAsync(); // Return the list of recipes
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipes created by user with ID: {UserId}", userId);
                return new List<Recipe>(); // Return an empty list if there is an error
            }
        }
        

        public async Task<Recipe> GetRecipeByIdAsync(string id)
        {
            try
            {
                return await _context.Recipes
                    .FirstOrDefaultAsync(r => r.Id == id); // Fetches a single recipe by its ID
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipe by ID: {RecipeId}", id);
                return null; // Return null if no recipe is found
            }
        }


        public async Task AddRecipeAsync(Recipe recipe)
        {
            try
            {
                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding recipe: {RecipeTitle}", recipe.Title);
            }
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            try
            {
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating recipe: {RecipeTitle}", recipe.Title);
            }
        }

        public async Task DeleteRecipeAsync(string id, string currentUserId)
        {
            try
            {
                // Fetch the recipe by its ID
                var recipe = await GetRecipeByIdAsync(id);

                // Check if the recipe exists and is created by the current user
                if (recipe != null && recipe.UserId == currentUserId)
                {
                    _context.Recipes.Remove(recipe);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (recipe == null)
                    {
                        _logger.LogWarning("Recipe with ID: {RecipeId} not found", id);
                    }
                    else
                    {
                        _logger.LogWarning("User with ID: {UserId} is not authorized to delete this recipe", currentUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe with ID: {RecipeId}", id);
            }
        }

    }
}