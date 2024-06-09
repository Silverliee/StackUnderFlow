using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class SocialInteractionService(IFriendRepository friendRepository, IFollowRepository followRepository) : ISocialInteractionService
{
    public async Task<List<UserResponseDto>> GetFriendsByUserId(int userId)
    {
        var friends = await friendRepository.GetFriendsByUserId(userId);
        if (friends.Count == 0)
        {
            return [];
        }
        return friends.Select(f => new UserResponseDto
        {
            UserId = f.UserId2,
            Username = f.User2.Username,
            Email = f.User2.Email
        }).ToList();
    }
    
    public async Task RemoveFriend(int userId, int friendId)
    {
        await friendRepository.RemoveFriend(userId,friendId);
    }
    
    public async Task<UserResponseDto?> AddFriend(int userId, int friendId)
    {
        var user = await friendRepository.AddFriend(userId,friendId);
        if (user == null)
        {
            return null;
        }
        return new UserResponseDto
        {
            UserId = user.UserId2,
            Username = user.User2.Username,
            Email = user.User2.Email
        };
    }
    
    public async Task<List<UserResponseDto>> GetFollowsByUserId(int userId)
    {
        var followers = await followRepository.GetFollowsByUserId(userId);
        if (followers.Count == 0)
        {
            return [];
        }
        return followers.Select(f => new UserResponseDto
        {
            UserId = f.UserId1,
            Username = f.User1.Username,
            Email = f.User1.Email
        }).ToList();
    }
    
    public async Task RemoveFollow(int userId, int followerId)
    {
        await followRepository.RemoveFollow(userId,followerId);
    }
    
    public async Task<UserResponseDto?> AddFollow(int userId, int followedId)
    {
        var user = await followRepository.AddFollow(userId,followedId);
        if (user == null)
        {
            return null;
        }
        return new UserResponseDto
        {
            UserId = user.UserId1,
            Username = user.User1.Username,
            Email = user.User1.Email
        };
    }
    
    
}