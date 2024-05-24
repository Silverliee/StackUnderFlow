using System.Data.Entity;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class LikeRepository(MySqlDbContext context) : ILikeRepository
{
    public async Task<IEnumerable<Like?>> GetAllLikes()
    {
        return await context.Likes.ToListAsync();
    }
    
    public async Task<Like?> GetLikeById(int id)
    {
        return await context.Likes.FirstOrDefaultAsync(l => l.LikeId == id);
    }
    
    public async Task<IEnumerable<Like?>> GetLikesByUserId(int userId)
    {
        return await context.Likes.Where(l => l.UserId == userId).ToListAsync();
    }
    
    public async Task<IEnumerable<Like?>> GetLikesByScriptId(int scriptId)
    {
        return await context.Likes.Where(l => l.ScriptId == scriptId).ToListAsync();
    }
    
    public async Task<Like?> CreateLike(Like like)
    {
        await context.Likes.AddAsync(like);
        await context.SaveChangesAsync();
        return like;
    }
    
    public async Task<Like?> DeleteLike(int id)
    {
        var like = await context.Likes.FirstOrDefaultAsync(l => l.LikeId == id);
        if (like == null)
        {
            return null;
        }
        
        context.Likes.Remove(like);
        await context.SaveChangesAsync();
        return like;
    }
    
}