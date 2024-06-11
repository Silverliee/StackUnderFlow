using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IFollowRepository
{
    public Task<List<Follow>> GetAllFollows();

    public Task<List<Follow>> GetFollowsByUserId(int id);
    
    public Task<Follow?> AddFollow(int userId, int followedId);

    public Task RemoveFollow(int userId, int followedId);
}