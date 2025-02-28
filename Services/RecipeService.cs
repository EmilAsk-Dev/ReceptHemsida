using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;

namespace ReceptHemsida.Services
{
    public class RecipeService
    {
        // Property
        private readonly ApplicationDbContext _context;

        // Constructor
        public RecipeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Methods
        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            return await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .Include(r => r.Favorites)
                .ToListAsync();
        }
    }
}
