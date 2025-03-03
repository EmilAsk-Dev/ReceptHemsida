using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Models;
using ReceptHemsida.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using ReceptHemsida.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;


namespace ReceptHemsida.Pages
{
    [Authorize]
    public class UserModel : PageModel
    {
      
        private readonly FavoriteService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;
        //Konstruktor
        public UserModel(FavoriteService favoriteService,UserManager<ApplicationUser>userManager)
        {
            _favoriteService = favoriteService;
            _userManager = userManager; 
        }
        public string LoggdInUserId { get; set; }
        public Dictionary<string, List<Recipe>> UserFavorites { get; set; } = new();
        public List<Recipe> SavedRecipes { get; set; } = new ();
      

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user !=null)
            {
                LoggdInUserId = user.Id;

                //Hämtar alla användarens favoritrecept
                var allFavorites = await _favoriteService.GetAllUserFavoritesAsync();
                UserFavorites = allFavorites;

                //Hämtar den inloggade användarens egna recept
                SavedRecipes = await _favoriteService.GetFavoriteRecipesAsync(user.Id);
            }

        }
        public async Task<IActionResult> OnPostAddFavoriteAsync(Guid recipeId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            
            var favorite = new Favorite
            {
                UserId = user.Id,
                RecipeId = recipeId
            };
            await _favoriteService.AddFavoriteAsync(favorite);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostRemoveFavoriteAsync(Guid recipeId)
        {
            var user = await _userManager.GetUserAsync(User); 
            if (user == null) return Unauthorized(); // Om ingen användare är inloggad, neka åtkomst

            await _favoriteService.RemoveFavoriteAsync(user.Id, recipeId); // Använder FavoriteService för att ta bort
            return RedirectToPage();
        }
    }
}
