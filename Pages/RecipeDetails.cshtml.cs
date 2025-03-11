using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Data;
using ReceptHemsida.Models;
using ReceptHemsida.Services;

namespace ReceptHemsida.Pages
{
    public class RecipeDetailsModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly FavoriteService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public Recipe Recipe { get; set; }
        public bool IsFavorited { get; set; }

        public RecipeDetailsModel(RecipeService recipeService, FavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            _recipeService = recipeService;
            _favoriteService = favoriteService;
            _userManager = userManager;
        }


        public async Task<IActionResult> OnGetAsync(string id)
        {

            Recipe = await _recipeService.GetRecipeByIdAsync(id);

            if (Recipe == null)
            {
                return BadRequest();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                IsFavorited = await _favoriteService.IsRecipeFavoritedAsync(userId, id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string recipeId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = _userManager.GetUserId(User);

            var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);

            if (recipe == null)
            {
                return NotFound();
            }

            if (recipe.UserId != userId)
            {
                return Unauthorized();
            }

            try
            {
                await _recipeService.DeleteRecipeAsync(recipeId, userId);
                return RedirectToPage("/Recipe");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the recipe.");
            }
        }

        public async Task<IActionResult> OnPostFavoriteAsync(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = _userManager.GetUserId(User);

            if (await _favoriteService.IsRecipeFavoritedAsync(userId, id))
            {
                await _favoriteService.RemoveFavoriteAsync(userId, id);
            }
            else
            {
                await _favoriteService.AddFavoriteAsync(new Favorite { UserId = userId, RecipeId = id });
            }

            return RedirectToPage();
        }

    }
}