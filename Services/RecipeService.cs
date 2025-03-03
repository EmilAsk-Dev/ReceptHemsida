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

        public async Task<Recipe> GetRecipeByIdAsync(Guid id)
        {
            try
            {
                // Söker databasen efter receptet med angivet ID
                return await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipe by ID: {RecipeId}", id);
                return null;
            }
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            try
            {

                // Lägger till receptet i databasen
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
                // Uppdaterar receptet i databasen
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating recipe: {RecipeTitle}", recipe.Title);
            }
        }

        public async Task DeleteRecipeAsync(Guid id)
        {
            try
            {
                var recipe = await GetRecipeByIdAsync(id);
                // Tar bort receptet från databasen
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe with ID: {RecipeId}", id);
            }
        }
       
       
    }
}
