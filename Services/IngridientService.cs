using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;

namespace ReceptHemsida.Services
{
    public class IngridientService
    {
        // Property
        private readonly ApplicationDbContext _context;

        // Constructor
        public IngridientService(ApplicationDbContext context)
        {
            _context = context;
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
                Console.WriteLine($"Error fetching ingredients: {ex.Message}");
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
                Console.WriteLine($"Error fetching ingredient by ID: {ex.Message}");
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
                Console.WriteLine($"Error adding ingredient: {ex.Message}");
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
                Console.WriteLine($"Error updating ingredient: {ex.Message}");
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
                Console.WriteLine($"Error deleting ingredient: {ex.Message}");
            }
        }
    }
}
