using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class FollowRepository(MySqlDbContext context) : IFollowRepository
{
    public Task<List<Follow>> GetAllFollows()
    {
        return context.Follows.ToListAsync();
    }

    public async Task<List<Follow>> GetFollowsByUserId(int userId)
    {
        return await context.Follows.Where(f => f.UserId1 == userId).ToListAsync();
    }

    public async Task RemoveFollow(int userId, int followedId)
    {
        var followed = await context.Follows.FirstOrDefaultAsync(f =>
            f.UserId1 == userId && f.UserId2 == followedId
        );
        if (followed == null)
        {
            return;
        }

        context.Follows.Remove(followed);
        await context.SaveChangesAsync();
    }

    public async Task<Follow?> AddFollow(int userId, int followedId)
    {
        var followed = await context.Follows.FirstOrDefaultAsync(f =>
            f.UserId1 == userId && f.UserId2 == followedId
        );
        if (followed != null)
        {
            return null;
        }

        followed = new Follow { UserId1 = userId, UserId2 = followedId };
        await context.Follows.AddAsync(followed);
        await context.SaveChangesAsync();
        return followed;
    }
}
