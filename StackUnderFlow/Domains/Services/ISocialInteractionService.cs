using StackUnderFlow.Application.DataTransferObject.Response;

namespace StackUnderFlow.Domains.Services;

public interface ISocialInteractionService
{
    public Task<List<UserResponseDto>> GetFriendsByUserId(int userId);
    public Task RemoveFriend(int userId, int friendId);
    public Task<UserResponseDto?> AddFriend(int userId, int friendId);
    public Task<List<UserResponseDto>> GetFollowsByUserId(int userId);
    public Task RemoveFollow(int userId, int followedId);
    public Task<UserResponseDto?> AddFollow(int userId, int followedId);
}
