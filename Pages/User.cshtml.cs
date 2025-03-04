using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ReceptHemsida.Data;
using ReceptHemsida.Models;
using ReceptHemsida.Services;

namespace ReceptHemsida.Pages
{
    public class UserModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FavoriteService _favoriteSerive;

        public ApplicationUser ProfileUser { get; set; }
        public List<Recipe> CreatedRecipes { get; set; }
        public List<Recipe> SavedRecipes { get; set; }

        public bool IsCurrentUserProfile { get; set; }

        public UserModel(UserService userService, RecipeService recipeService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _recipeService = recipeService;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string username)
        {
            ProfileUser = await _userService.GetUserByUsernameAsync(username);

            if (ProfileUser == null)
            {
                return NotFound("User not found");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            IsCurrentUserProfile = currentUser?.Id == ProfileUser?.Id;

            // Use RecipeService to get created recipes
            CreatedRecipes = await _recipeService.RecipesCreatedByUser(ProfileUser.Id);

            // Use RecipeService to get saved recipes
            SavedRecipes = await _favoriteSerive.GetFavoriteRecipesAsync(ProfileUser.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteRecipe(string recipeId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Use RecipeService to delete the recipe
            await _recipeService.DeleteRecipeAsync(currentUser.Id, recipeId);

            return RedirectToPage("/User", new { username = currentUser.UserName });
        }

        public async Task<IActionResult> OnPostRemoveFavorite(string recipeId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Use RecipeService to remove the saved recipe
            await _favoriteSerive.RemoveFavoriteAsync(currentUser.Id, recipeId);

            return RedirectToPage("/User", new { username = currentUser.UserName });
        }
    }
}

