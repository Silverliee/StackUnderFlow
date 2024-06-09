using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class FriendRepository(MySqlDbContext context) : IFriendRepository
{
    public async Task<List<Friend>> GetAllFriends()
    {
        return await context.Friends.ToListAsync();
    }
    
    public async Task<List<Friend>> GetFriendsByUserId(int userId)
    {
        return await context.Friends.Where(f => f.UserId1 == userId).ToListAsync();
    }
    
    public async Task<Friend?> AddFriend(int userId, int friendId)
    {
        var friend = await context.Friends.FirstOrDefaultAsync(f => f.UserId1 == userId && f.UserId2 == friendId || f.UserId1 == friendId && f.UserId2 == userId);
        if (friend != null)
        {
            return null;
        }
        
        friend = new Friend
        {
            UserId1 = userId,
            UserId2 = friendId
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