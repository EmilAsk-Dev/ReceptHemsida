using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;

namespace ReceptHemsida.Services
{
    public class IngredientService
    {
        // Property
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IngredientService> _logger;

        // Constructor
        public IngredientService(ApplicationDbContext context, ILogger<IngredientService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Methods
        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            try
            {
                return await _context.Ingredients
                    .Include(i => i.RecipeIngredients)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ingredients.");
                return new List<Ingredient>();
            }
        }

        public async Task<Ingredient> GetIngredientByIdAsync(Guid id)
        {
            try
            {
                return await _context.Ingredients
                    .Include(i => i.RecipeIngredients)
                    .FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ingredient: {id}", id);
                return null;
            }
        }

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            try
            {
                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ingredient.");
            }
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            try
            {
                _context.Ingredients.Update(ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ingredient: {ingredientId}", ingredient.Id);
            }
        }

        public async Task DeleteIngredientAsync(Guid id)
        {
            try
            {
                var ingredient = await GetIngredientByIdAsync(id);
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ingredient: {ingredientId}", id);
            }
        }
    }
}