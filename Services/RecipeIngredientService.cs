using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceptHemsida.Services
{
    public class RecipeIngredientService
    {
        private readonly ApplicationDbContext _context;

        public RecipeIngredientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecipeIngredient>> GetIngredientsForRecipeAsync(string recipeId)
        {
            try
            {
                return await _context.RecipeIngredients
                    .Where(ri => ri.RecipeId == recipeId)
                    .Include(ri => ri.Ingredient)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ingredients for recipe: {ex.Message}");
                return new List<RecipeIngredient>();
            }
        }

        public async Task AddRecipeIngredientAsync(RecipeIngredient recipeIngredient)
        {
            try
            {
                _context.RecipeIngredients.Add(recipeIngredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding recipe ingredient: {ex.Message}");
            }
        }

        public async Task RemoveRecipeIngredientAsync(string recipeId, string ingredientId)
        {
            try
            {
                var recipeIngredient = await _context.RecipeIngredients
                    .FirstOrDefaultAsync(ri => ri.RecipeId == recipeId && ri.IngredientId == ingredientId);

                if (recipeIngredient != null)
                {
                    _context.RecipeIngredients.Remove(recipeIngredient);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing recipe ingredient: {ex.Message}");
            }
        }
    }
}
