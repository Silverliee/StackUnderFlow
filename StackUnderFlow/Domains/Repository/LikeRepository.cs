using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;

namespace StackUnderFlow.Domains.Repository;

public class LikeRepository(MySqlDbContext context) : ILikeRepository
{
    public async Task<List<int>> GetAllLikes()
    {
        var likes = new List<Like>();
        try
        {
            likes = await context.Likes.ToListAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return likes.Count > 0 ? likes.Select(l => l.LikeId).ToList() : new List<int>();
    }

    public async Task<Like?> GetLikeById(int id)
    {
        return await context.Likes.FirstOrDefaultAsync(l => l.LikeId == id);
    }

    public async Task<IEnumerable<Like?>> GetLikesByUserId(int userId)
    {
        return await context.Likes.Where(l => l.UserId == userId).ToListAsync();
    }

    public async Task<List<Like>> GetLikesByScriptId(int scriptId)
    {
        return await context.Likes.Where(l => l.ScriptId == scriptId).ToListAsync();
        // return likes.Count;
    }
    
    public async Task<Like?> GetLikesByUserIdAndScriptId(int userId, int scriptId)
    {
       return await context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.ScriptId == scriptId);
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
