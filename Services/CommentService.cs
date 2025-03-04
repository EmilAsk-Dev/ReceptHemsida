using ReceptHemsida.Data;
using ReceptHemsida.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace ReceptHemsida.Services
{
    public class CommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CommentService> _logger;

        public CommentService(ApplicationDbContext context, ILogger<CommentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        public async Task<List<Comment>> GetCommentsForRecipeAsync(string recipeId)
        {
            try
            {
                return await _context.Comments
                    .Where(c => c.RecipeId == recipeId)
                    .Include(c => c.User)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{recipeId}", recipeId);
                return new List<Comment>(); 
            }
        }

       
        public async Task<Comment?> GetCommentByIdAsync(string id)
        {
            try
            {
                return await _context.Comments
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching comment with ID: {id}", id);
                return null;
            }
        }

        
        public async Task<bool> AddCommentAsync(Comment comment)
        {
            try
            {
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding comment: {comment}", comment.Content);
                return false;
            }
        }

        
        public async Task<bool> DeleteCommentAsync(string id)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment != null)
                {
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting comment with ID:" + id);
                return false;
            }
        }
    }
}
