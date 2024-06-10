using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IGroupRepository
{
    public Task<List<Group>> GetAllGroups();
    public Task<Group?> GetGroupById(int id);
    public Task<Group?> GetGroupByName(string name);
    public Task<List<Group>> GetGroupsByUserId(int userId);
    public Task<List<User>> GetGroupMembers(int groupId);
    public Task<List<GroupRequest>> GetGroupRequestsByGroupId(int groupId);
    public Task<List<GroupRequest>> GetGroupRequestsByUserId(int userId);
    public Task<Group> CreateGroup(Group group);
    public Task<Group> UpdateGroup(Group group);
    public Task<GroupRequest> CreateGroupRequest(GroupRequest groupRequest);
    public Task<GroupRequest?> GetGroupRequest(int userId, int groupId);
    public Task<GroupRequest> AcceptGroupRequest(GroupRequest groupRequest);
    public Task RemoveGroupRequest(GroupRequest groupRequest);
    public Task DeleteGroup(Group group);
}