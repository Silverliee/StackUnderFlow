using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IGroupRepository
{
    public Task<IEnumerable<Group?>> GetAllGroups();

    public Task<Group?> GetGroupById(int id);

    public Task<Group?> GetGroupByName(string name);

    public Task<IEnumerable<Group?>> GetGroupsByUserId(int userId);

    public Task<Group?> AddUserToGroup(int userId, int groupId);

    public Task<Group?> RemoveUserFromGroup(int userId, int groupId);

    public Task<IEnumerable<int>> GetGroupMembers(int groupId);
}