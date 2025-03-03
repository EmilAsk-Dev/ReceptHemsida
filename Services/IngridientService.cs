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
            return await _context.Ingredients
                .Include(i => i.RecipeIngredients)
                .ToListAsync();
        }

        public async Task<Ingredient> GetIngredientByIdAsync(Guid id)
        {
            return await _context.Ingredients
                .Include(i => i.RecipeIngredients)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIngredientAsync(Guid id)
        {
            var ingredient = await GetIngredientByIdAsync(id);
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }
    }
}
