using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IFriendRepository
{
    public Task<List<FriendRequest>> GetAllFriends();
    
    public Task<FriendRequest?> GetFriendRequest(int userId, int friendId);

    public Task<List<FriendRequest>> GetFriendsByUserId(int id);
    
    public Task<FriendRequest?> CreateFriendRequest(int userId, int friendId, string message);

    public Task RemoveFriend(int userId, int friendId);
    
    public Task<FriendRequest> AcceptFriendRequest(FriendRequest friendRequest);
}