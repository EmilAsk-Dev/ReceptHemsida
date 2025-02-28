using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;

namespace ReceptHemsida.Services
{
    public class RecipeService
    {
        
        private readonly ApplicationDbContext _context;

        
        public RecipeService(ApplicationDbContext context)
        {
            _context = context;
        }

        
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
