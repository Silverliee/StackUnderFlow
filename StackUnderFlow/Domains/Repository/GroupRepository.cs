using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class GroupRepository(MySqlDbContext context) : IGroupRepository
{
    public async Task<IEnumerable<Group?>> GetAllGroups()
    {
        return await context.Groups.ToListAsync();
    }

    public async Task<Group?> GetGroupById(int id)
    {
        return await context.Groups.FirstOrDefaultAsync(g => g.GroupId == id);
    }
    
    public async Task<Group?> GetGroupByName(string name)
    {
        return await context.Groups.FirstOrDefaultAsync(g => g.GroupName == name);
    }
    
    public async Task<IEnumerable<Group?>> GetGroupsByUserId(int userId)
    {
        return await context.Groups.Where(g => g.CreatorUserID == userId).ToListAsync();
    }
    
    public async Task<Group?> AddUserToGroup(int userId, int groupId)
    {
        var group = await context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (group == null)
        {
            return null;
        }

        group.Members.Add(userId);
        await context.SaveChangesAsync();
        return group;
    }
    
    public async Task<Group?> RemoveUserFromGroup(int userId, int groupId)
    {
        var group = await context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (group == null)
        {
            return null;
        }

        group.Members.Remove(userId);
        await context.SaveChangesAsync();
        return group;
    }
    
    public async Task<IEnumerable<int>> GetGroupMembers(int groupId)
    {
        var group = await context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (group == null)
        {
            return new List<int>();
        }

        return group.Members;
    }
    
    public async Task<Group?> CreateGroup(Group group)
    {
        await context.Groups.AddAsync(group);
        await context.SaveChangesAsync();
        return group;
    }
    
    public async Task<Group?> UpdateGroup(Group group)
    {
        context.Groups.Update(group);
        await context.SaveChangesAsync();
        return group;
    }
    
    public async Task<Group?> DeleteGroup(int id)
    {
        var group = await context.Groups.FirstOrDefaultAsync(g => g.GroupId == id);
        if (group == null)
        {
            return null;
        }
        
        context.Groups.Remove(group);
        await context.SaveChangesAsync();
        return group;
    }
}