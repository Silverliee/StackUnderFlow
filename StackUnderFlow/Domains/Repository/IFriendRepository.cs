using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IFriendRepository
{
    public Task<List<Friend>> GetAllFriends();

    public Task<List<Friend>> GetFriendsByUserId(int id);
    
    public Task<Friend?> AddFriend(int userId, int friendId);

    public Task RemoveFriend(int userId, int friendId);
}