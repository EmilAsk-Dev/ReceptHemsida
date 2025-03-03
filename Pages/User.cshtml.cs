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


namespace ReceptHemsida.Pages
{
    [Authorize]
    public class UserModel : PageModel
    {
        //Deklarerar variabler
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //Konstruktor
        public UserModel(ApplicationDbContext context, UserManager<ApplicationUser>userManager)
        {
            _context = context;
            _userManager = userManager; 
        }
        public List<Recipe> SavedRecipes { get; set; } = new List<Recipe>();

        public async Task OnGetAsync()
        {

            var user= await _userManager.GetUserAsync(User);
            if(user != null)
            {
               SavedRecipes =await _context.Favorites
                    .Where(f => f.UserId == user.Id)
                    .Include(f => f.Recipe)
                    .Select(f => new Recipe
                    {
                        Id = f.Recipe.Id,
                        Title = f.Recipe.Title,
                        Description = f.Recipe.Description,
                        CookTime = f.Recipe.CookTime,
                        Difficulty = f.Recipe.Difficulty,
                        Tags= new List<string> { "Quick", "Easy", "Healthy" }, //Dummy data
                    })
                    .ToListAsync();




            }
        }
    }
}
