using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class FriendRepository(MySqlDbContext context) : IFriendRepository
{
    public async Task<List<FriendRequest>> GetAllFriends()
    {
        return await context.Friends.ToListAsync();
    }
    
    public async Task<List<FriendRequest>> GetFriendsByUserId(int userId)
    {
        return await context.Friends.Where(f => f.UserId1 == userId || f.UserId2 == userId).ToListAsync();
    }
    
    public async Task<FriendRequest?> GetFriendRequest(int userId, int friendId)
    {
        return await context.Friends.FirstOrDefaultAsync(f => f.UserId1 == userId && f.UserId2 == friendId || f.UserId1 == friendId && f.UserId2 == userId);
    }

    public async Task<FriendRequest> AcceptFriendRequest(FriendRequest friendRequest)
    {
        var result = context.Friends.Update(friendRequest);
        await context.SaveChangesAsync();
        return result.Entity;
    }
    
    public async Task<FriendRequest?> CreateFriendRequest(int userId, int friendId)
    {
        var friend = await context.Friends.FirstOrDefaultAsync(f => f.UserId1 == userId && f.UserId2 == friendId || f.UserId1 == friendId && f.UserId2 == userId);
        if (friend != null)
        {
            return null;
        }
        
        friend = new FriendRequest
        {
            UserId1 = userId,
            UserId2 = friendId,
            Status = "Pending"
        };
        await context.Friends.AddAsync(friend);
        await context.SaveChangesAsync();
        return friend;
    }
    
    public async Task RemoveFriend(int userId, int friendId)
    {
        var friend = await context.Friends.FirstOrDefaultAsync(f => f.UserId1 == userId && f.UserId2 == friendId || f.UserId1 == friendId && f.UserId2 == userId);
        if (friend == null)
        {
            return;
        }
        
        context.Friends.Remove(friend);
        await context.SaveChangesAsync();
    }
}