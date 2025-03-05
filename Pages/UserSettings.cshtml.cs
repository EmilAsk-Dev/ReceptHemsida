using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReceptHemsida.Data;

namespace ReceptHemsida.Pages
{
    public class UserSettingsModel : PageModel
    {
        public void OnGet()
        {
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserSettingsModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public bool PasswordChanged { get; set; }

        public bool PasswordUnmatched { get; set; }

        public bool CurrentPasswordWrong { get; set; }


        public async Task OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (NewPassword != ConfirmPassword)
            {
                PasswordUnmatched = true;
                return;
            }

            // Kontrollera om det nuvarande lösenordet är korrekt
            var result = await _userManager.CheckPasswordAsync(user, CurrentPassword);
            if (!result)
            {
                CurrentPasswordWrong = true;
            }

            // Om det nuvarande lösenordet är korrekt, byt lösenordet
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
           
            if (changePasswordResult.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                PasswordChanged = true;
            }
        }
    }
}
