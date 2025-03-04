using ReceptHemsida.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ReceptHemsida.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
       

        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user: {ex.Message}");
                return null;
            }
        }

        
        public async Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                return await _context.Users
                    .Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching users: {ex.Message}");
                return new List<ApplicationUser>();
            }
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by username: {ex.Message}");
                return null;
            }
        }


    }
}
