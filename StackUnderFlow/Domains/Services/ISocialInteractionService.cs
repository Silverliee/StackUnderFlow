using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;

namespace StackUnderFlow.Domains.Services;

public interface ISocialInteractionService
{
    public Task<List<UserResponseDto>> GetFriendsByUserId(int userId);
    public Task RemoveFriend(int userId, int friendId);
    public Task<FriendRequestResponseDto?> GetFriendRequest(int userId, int friendId);
    public Task<List<FriendRequestResponseDto>> GetFriendRequestsByUserId(int userId);
    public Task<FriendRequestResponseDto> CreateFriendRequest(int userId, int friendId, string message);
    public Task<UserResponseDto?> AcceptFriendRequest(int userId, int friendId);
    public Task<List<UserResponseDto>> GetFollowsByUserId(int userId);
    public Task RemoveFollow(int userId, int followedId);
    public Task<UserResponseDto?> AddFollow(int userId, int followedId);
    public Task<List<GroupRequestResponseDto>> GetGroupRequestsByUserId(int userId);
    public Task<List<GroupRequestResponseDto>> GetGroupRequestsByGroupId(int groupId);
    public Task<List<GroupResponseDto>> GetGroupsByUserId(int userId);
    public Task<GroupResponseDto?> GetGroupById(int groupId);
    public Task<List<UserResponseDto>> GetGroupMembers(int groupId);
    public Task<GroupResponseDto?> CreateGroup(int userId, GroupRequestDto groupRequestDto);
    public Task<GroupResponseDto> UpdateGroup(int groupId, GroupRequestDto groupRequestDto);
    public Task<GroupRequestResponseDto?> CreateGroupRequest(int userId, int groupId);
    public Task<GroupRequestResponseDto> AcceptGroupRequest(int userId, int groupId);
    public Task RejectGroupRequest(int userId, int groupId);
    public Task RemoveGroup(int groupId);
    public Task<List<CommentResponseDto>> GetCommentsByScriptId(int scriptId);
    public Task<CommentResponseDto?> CreateComment(int userId, int scriptId, CommentRequestDto commentRequestDto);
    public Task<CommentResponseDto?> UpdateComment(int commentId, CommentPatchRequestDto commentRequestDto);
    public Task<CommentResponseDto?> GetCommentById(int commentId);
    public Task DeleteComment(int commentId);
    public Task<int?> CreateLike(int userId, int scriptId);
    public Task DeleteLike(int userId, int scriptId);
    public Task<int?> GetLikeById(int userId,int likeId);
    public Task<List<int>> GetLikes();
}
