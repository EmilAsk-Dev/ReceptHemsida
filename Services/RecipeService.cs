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

        public async Task<Recipe> GetRecipeByIdAsync(Guid id)
        {
            // Söker databasen efter receptet med angivet ID
            return await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateRecipeAsync(Recipe recipe)
        {
            // Lägger till receptet i databasen
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            // Uppdaterar receptet i databasen
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(Guid id)
        {
            var recipe = await GetRecipeByIdAsync(id);
            // Tar bort receptet från databasen
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
