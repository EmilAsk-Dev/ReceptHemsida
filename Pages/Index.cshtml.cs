using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Models;
using ReceptHemsida.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReceptHemsida.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly RecipeService _recipeService;

    public List<Recipe> RecentRecipes { get; set; } = new List<Recipe>();
    public List<string> Categories { get; set; } = new List<string>();

    
    
    public IndexModel(ILogger<IndexModel> logger, RecipeService recipeService)
    {
        _logger = logger;
        _recipeService = recipeService;
    }



    public async Task OnGetAsync()
    {
        // Get 3 most recent recipes
        RecentRecipes = await _recipeService.GetRecentRecipesAsync(3);
        
        // Get all categories (assuming RecipeCategory is an enum)
        Categories = Enum.GetNames(typeof(RecipeCategory)).ToList();
    }
}