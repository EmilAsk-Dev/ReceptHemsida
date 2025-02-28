using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ReceptHemsida.Data;
using ReceptHemsida.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReceptHemsida.Pages
{
    public class TestDevPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TestDevPageModel> _logger;

        public TestDevPageModel(ApplicationDbContext context, ILogger<TestDevPageModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAddTestUser()
        {
            await AddTestUser();
            return RedirectToPage(); // Reloads the page after submission
        }

        public async Task<IActionResult> OnPostRemoveTestUsers()
        {
            await RemoveTestUsers();
            return RedirectToPage();
        }

        private async Task AddTestUser()
        {
            _logger.LogInformation("Adding test user...");
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "TestUser" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var recipe = new Recipe { Id = Guid.NewGuid(), UserId = user.Id, Title = "Test Recipe", Description = "This is a test recipe." };
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            var ingredient = new Ingredient { Id = Guid.NewGuid(), Name = "Test Ingredient" };
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            var recipeIngredient = new RecipeIngredient { RecipeId = recipe.Id, IngredientId = ingredient.Id, Quantity = "1 cup" };
            _context.RecipeIngredients.Add(recipeIngredient);
            await _context.SaveChangesAsync();

            var comment = new Comment { Id = Guid.NewGuid(), UserId = user.Id, RecipeId = recipe.Id, Content = "Great recipe!" };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var favorite = new Favorite { UserId = user.Id, RecipeId = recipe.Id };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            var follower = new Follower { FollowerId = user.Id, FollowingId = user.Id };
            _context.Followers.Add(follower);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Test user and related data added successfully.");
        }

        private async Task RemoveTestUsers()
        {
            _logger.LogInformation("Removing test users...");
            var testUsers = _context.Users.Where(u => u.UserName == "TestUser").ToList();
            foreach (var user in testUsers)
            {
                var recipes = _context.Recipes.Where(r => r.UserId == user.Id);
                _context.Recipes.RemoveRange(recipes);

                var comments = _context.Comments.Where(c => c.UserId == user.Id);
                _context.Comments.RemoveRange(comments);

                var favorites = _context.Favorites.Where(f => f.UserId == user.Id);
                _context.Favorites.RemoveRange(favorites);

                var followers = _context.Followers.Where(f => f.FollowerId == user.Id || f.FollowingId == user.Id);
                _context.Followers.RemoveRange(followers);
            }
            _context.Users.RemoveRange(testUsers);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Test users and related data removed successfully.");
        }
    }
}
