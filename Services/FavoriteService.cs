using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceptHemsida.Services
{
    public class FavoriteService
    {
        private readonly ApplicationDbContext _context;

        public FavoriteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetFavoriteRecipesAsync(string userId)
        {
            try
            {
                return await _context.Favorites
                    .Where(f => f.UserId == userId)
                    .Select(f => f.Recipe)
                    .Include(r => r.User) // Include the user who created the recipe
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting favorite recipes: {ex.Message}");
                return new List<Recipe>();
            }
        }

        public async Task AddFavoriteAsync(Favorite favorite)
        {
            try
            {
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding favorite: {ex.Message}");
            }
        }

        public async Task RemoveFavoriteAsync(string userId, Guid recipeId)
        {
            try
            {
                var favorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

                if (favorite != null)
                {
                    _context.Favorites.Remove(favorite);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing favorite: {ex.Message}");
            }
        }

        public async Task<bool> IsRecipeFavoritedAsync(string userId, Guid recipeId)
        {
            try
            {
                return await _context.Favorites
                    .AnyAsync(f => f.UserId == userId && f.RecipeId == recipeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if recipe is favorited: {ex.Message}");
                return false;
            }
        }
        public async Task<Dictionary<string,List<Recipe>>> GetAllUserFavoritesAsync()
        {
            try
            {
                var allFavorites= await _context.Favorites
                    .Include(f => f.Recipe)
                    .Include(f => f.Recipe.User)
                    .ToListAsync();
                return allFavorites
                    .GroupBy(f => f.User.UserName)
                    .ToDictionary(g => g.Key,
                     g => g.Select(f => f.Recipe).ToList()
                    );

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user favorites:{ex.Message} ");
                return new Dictionary<string, List<Recipe>>();
            }
        }
    }
}
