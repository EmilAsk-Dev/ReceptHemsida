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
    }
}
