using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class CommentRepository(MySqlDbContext context) : ICommentRepository
{
    public async Task<List<Comment?>> GetAllComments()
    {
        return await context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetCommentById(int id)
    {
        return await context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
    }

    public async Task<List<Comment?>> GetCommentsByScriptId(int scriptId)
    {
        return await context.Comments.Where(c => c.ScriptId == scriptId).ToListAsync();
    }

    public async Task<List<Comment?>> GetCommentsByUserId(int userId)
    {
        return await context.Comments.Where(c => c.UserId == userId).ToListAsync();
    }

    public async Task<Comment?> CreateComment(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> UpdateComment(Comment comment)
    {
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> DeleteComment(int id)
    {
        var comment = await context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
        if (comment == null)
        {
            return null;
        }

        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
        return comment;
    }
}
