using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class GroupRepository(MySqlDbContext context) : IGroupRepository
{
    public async Task<List<Group>> GetAllGroups()
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

    public async Task<List<Group>> GetGroupsByUserId(int userId)
    {
        return await context.Groups.Where(g => g.CreatorUserID == userId).ToListAsync();
    }

    // public async Task<Group?> AddUserToGroup(int userId, int groupId)
    // {
    //     var group = await context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
    //     if (group == null)
    //     {
    //         return null;
    //     }
    //
    //     group.Members.Add(userId);
    //     await context.SaveChangesAsync();
    //     return group;
    // }
    //
    // public async Task<Group?> RemoveUserFromGroup(int userId, int groupId)
    // {
    //     var group = await context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
    //     if (group == null)
    //     {
    //         return null;
    //     }
    //
    //     group.Members.Remove(userId);
    //     await context.SaveChangesAsync();
    //     return group;
    // }

    public async Task<List<User>> GetGroupMembers(int groupId)
    {
        var groupRequests = await context
            .GroupRequests.Where(g => g.GroupId == groupId && g.Status == "Accepted")
            .Include(gr => gr.User)
            .ToListAsync();
        return groupRequests.Count == 0 ? [] : groupRequests.Select(gr => gr.User).ToList();
    }

    public async Task<Group> CreateGroup(Group group)
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

    public async Task<List<GroupRequest>> GetGroupRequestsByGroupId(int groupId)
    {
        return await context.GroupRequests.Where(gr => gr.GroupId == groupId).ToListAsync();
    }

    public async Task<List<GroupRequest>> GetGroupRequestsByUserId(int userId)
    {
        return await context.GroupRequests.Where(gr => gr.UserId == userId).ToListAsync();
    }

    public async Task<GroupRequest> CreateGroupRequest(GroupRequest groupRequest)
    {
        await context.GroupRequests.AddAsync(groupRequest);
        await context.SaveChangesAsync();
        return groupRequest;
    }

    public async Task<GroupRequest?> GetGroupRequest(int userId, int groupId)
    {
        return await context.GroupRequests.FirstOrDefaultAsync(gr =>
            gr.UserId == userId && gr.GroupId == groupId
        );
    }

    public async Task<GroupRequest> AcceptGroupRequest(GroupRequest groupRequest)
    {
        context.GroupRequests.Update(groupRequest);
        await context.SaveChangesAsync();
        return groupRequest;
    }

    public async Task RemoveGroupRequest(GroupRequest groupRequest)
    {
        context.GroupRequests.Remove(groupRequest);
        await context.SaveChangesAsync();
    }

    public async Task DeleteGroup(Group group)
    {
        var groupRequests = await context
            .GroupRequests.Where(gr => gr.GroupId == group.GroupId)
            .ToListAsync();
        context.Groups.Remove(group);
        context.GroupRequests.RemoveRange(groupRequests);
        await context.SaveChangesAsync();
    }
}
