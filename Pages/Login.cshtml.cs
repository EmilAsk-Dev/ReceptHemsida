using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using ReceptHemsida.Data; 

namespace ReceptHemsida.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public class LoginInputModel
        {
            public string UsernameOrEmail { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(Input.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(Input.UsernameOrEmail);
            }

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
