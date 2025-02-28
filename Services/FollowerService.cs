using Microsoft.EntityFrameworkCore;
using ReceptHemsida.Data;
using ReceptHemsida.Models;

namespace ReceptHemsida.Services
{
    public class FollowerService
    {
        private readonly ApplicationDbContext _context;

        public FollowerService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<List<ApplicationUser>> GetFollowersAsync(string userId)
        {
            return await _context.Followers
                .Where(f => f.FollowingId == userId)
                .Select(f => f.FollowerUser)
                .ToListAsync();
        }

        
        public async Task<List<ApplicationUser>> GetFollowingAsync(string userId)
        {
            return await _context.Followers
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingUser)
                .ToListAsync();
        }

        
        public async Task FollowUserAsync(Follower follower)
        {
            var exists = await _context.Followers
                .AnyAsync(f => f.FollowerId == follower.FollowerId && f.FollowingId == follower.FollowingId);

            if (!exists)
            {
                _context.Followers.Add(follower);
                await _context.SaveChangesAsync();
            }
        }

        
        public async Task UnfollowUserAsync(string followerId, string followingId)
        {
            var follower = await _context.Followers
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follower != null)
            {
                _context.Followers.Remove(follower);
                await _context.SaveChangesAsync();
            }
        }

        
        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            return await _context.Followers
                .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        }
    }
}
