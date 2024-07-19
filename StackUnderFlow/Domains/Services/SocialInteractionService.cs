using Microsoft.Identity.Client;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class SocialInteractionService(
    IFriendRepository friendRepository,
    IFollowRepository followRepository,
    IGroupRepository groupRepository,
    IUserRepository userRepository,
    ICommentRepository commentRepository,
    IScriptRepository scriptRepository,
    ILikeRepository likeRepository
) : ISocialInteractionService
{
    public async Task<List<UserResponseDto>> GetFriendsByUserId(int userId)
    {
        var friends = await friendRepository.GetFriendsByUserId(userId);
        if (friends.Count == 0)
        {
            return [];
        }
        var friendIds = friends.Select(fr => fr.UserId1 == userId ? fr.UserId2 : fr.UserId1).ToList();
    
        // Récupérez tous les amis en une seule fois
        var friendsData = await userRepository.GetUsersByIds(friendIds);

        // Créez la liste de réponse
        var friendsListResponse = new List<UserResponseDto>();
        foreach (var friend in friendsData)
        {
            var friendRequestToAdd = new UserResponseDto
            {
                UserId = friend.UserId,
                Username = friend.Username,
                Email = friend.Email,
                Description = friend.Description ?? ""
            };
            friendsListResponse.Add(friendRequestToAdd);
        }

        return friendsListResponse;
    }

    public async Task<List<FriendRequestResponseDto>> GetFriendRequestsByUserId(int userId)
    {
        var friendRequests = await friendRepository.GetFriendRequestsByUserId(userId);
        if (friendRequests.Count == 0)
        {
            return new List<FriendRequestResponseDto>();
        }

        // Récupérez tous les userIds à l'avance
        var userIds = friendRequests.Select(fr => fr.UserId1).ToList();
    
        // Récupérez toutes les informations des utilisateurs en une seule fois
        var users = await userRepository.GetUsersByIds(userIds);

        // Créez la liste de réponse
        var response = friendRequests.Select(fr =>
        {
            var sender = users.FirstOrDefault(u => u.UserId == fr.UserId1);
            return new FriendRequestResponseDto
            {
                UserId = fr.UserId1,
                FriendId = fr.UserId2,
                FriendName = sender?.Username,
                Status = fr.Status,
                Message = fr.Message
            };
        }).ToList();

        return response;
    }


    public async Task RemoveFriend(int userId, int friendId)
    {
        await friendRepository.RemoveFriend(userId, friendId);
    }

    public async Task<FriendRequestResponseDto> CreateFriendRequest(
        int userId,
        int friendId,
        string message
    )
    {
        var friendRequest = await friendRepository.CreateFriendRequest(userId, friendId, message);
        if (friendRequest == null)
        {
            return null;
        }
        var friend = await userRepository.GetUserById(friendId);
        return new FriendRequestResponseDto
        {
            UserId = friendRequest.UserId1,
            FriendId = friendRequest.UserId2,
            FriendName = friend.Username,
            Message = friendRequest.Message,
            Status = friendRequest.Status
        };
    }

    public async Task<FriendRequestResponseDto?> GetFriendRequest(int userId, int friendId)
    {
        var friendRequest = await friendRepository.GetFriendRequest(userId, friendId);
        if (friendRequest == null)
        {
            return null;
        }

        var friend = await userRepository.GetUserById(friendRequest.UserId2);
        return new FriendRequestResponseDto
        {
            UserId = friendRequest.UserId1,
            FriendId = friendRequest.UserId2,
            FriendName = friend.Username,
            Status = friendRequest.Status,
            Message = friendRequest.Message
        };
    }

    public async Task<UserResponseDto?> AcceptFriendRequest(int userId, int friendId)
    {
        var friendRequest = await friendRepository.GetFriendRequest(userId, friendId);
        if (friendRequest == null)
        {
            return null;
        }
        var status = friendRequest.Status;
        if (status == "Pending")
        {
            friendRequest.Status = "Accepted";
            await friendRepository.AcceptFriendRequest(friendRequest);
        }
        var user = await userRepository.GetUserById(friendRequest.UserId1);
        //We return the id of the user that sent the friend request to the one that accepted it
        return new UserResponseDto
        {
            UserId = friendRequest.UserId1,
            Username = user.Username,
            Email = user.Email,
            Description = user.Description ?? ""
        };
    }

    public async Task<List<UserResponseDto>> GetFollowsByUserId(int userId)
    {
        var followers = await followRepository.GetFollowsByUserId(userId);
        if (followers.Count == 0)
        {
            return [];
        }
        return followers
            .Select(f => new UserResponseDto
            {
                UserId = f.UserId1,
                Username = f.User1.Username,
                Email = f.User1.Email,
                Description = f.User1.Description ?? ""
            })
            .ToList();
    }

    public async Task RemoveFollow(int userId, int followerId)
    {
        await followRepository.RemoveFollow(userId, followerId);
    }

    public async Task<UserResponseDto?> AddFollow(int userId, int followedId)
    {
        var user = await followRepository.AddFollow(userId, followedId);
        if (user == null)
        {
            return null;
        }
        return new UserResponseDto
        {
            UserId = user.UserId1,
            Username = user.User1.Username,
            Email = user.User1.Email,
            Description = user.User1.Description ?? ""
        };
    }

    public async Task<List<GroupRequestResponseDto>> GetGroupRequestsByUserId(int userId)
    {
        var groupRequests = new List<GroupRequest>();
        try
        {
            groupRequests = await groupRepository.GetGroupRequestsByUserId(userId);
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return [];
        }
        
        if (groupRequests.Count == 0 )
        {
            return [];
        }

        var result = new List<GroupRequestResponseDto>();
        foreach (var request in groupRequests)
        {
            var group = await groupRepository.GetGroupById(request.GroupId);
            var user = await userRepository.GetUserById(request.UserId);
            result.Add(
                new GroupRequestResponseDto
                {
                    GroupId = request.GroupId,
                    GroupName = group.GroupName,
                    UserId = request.UserId,
                    Username = user.Username,
                    Status = request.Status
                });
        }
        return result;
    }

    public async Task<List<GroupResponseDto>> GetGroupsByUserId(int userId)
    {
        var result = await groupRepository.GetGroupsByUserId(userId);
        if (result.Count == 0)
        {
            return [];
        }
        return result
            .Select(g => new GroupResponseDto
            {
                GroupId = g.GroupId,
                GroupName = g.GroupName,
                Description = g.Description,
                CreatorUserID = g.CreatorUserID
            })
            .ToList();
    }

    public async Task<GroupResponseDto?> GetGroupById(int groupId)
    {
        var group = await groupRepository.GetGroupById(groupId);
        if (group == null)
        {
            return null;
        }
        return new GroupResponseDto
        {
            GroupId = group.GroupId,
            GroupName = group.GroupName,
            Description = group.Description,
            CreatorUserID = group.CreatorUserID
        };
    }

    public async Task<List<UserResponseDto>> GetGroupMembers(int groupId)
    {
        var result = await groupRepository.GetGroupMembers(groupId);
        if (result.Count == 0)
        {
            return [];
        }

        return result
            .Select(user => new UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Description = user.Description ?? ""
            })
            .ToList();
    }

    public async Task<List<GroupRequestResponseDto>> GetGroupRequestsByGroupId(int groupId)
    {
        var result = await groupRepository.GetGroupRequestsByGroupId(groupId);
        if (result.Count == 0)
        {
            return [];
        }

        return result
            .Select(request => new GroupRequestResponseDto
            {
                GroupId = request.GroupId,
                GroupName = request.Group.GroupName,
                UserId = request.UserId,
                Username = request.User.Username,
                Status = request.Status
            })
            .ToList();
    }

    public async Task<GroupResponseDto?> CreateGroup(int userId, GroupRequestDto groupRequestDto)
    {
        var group = new Group
        {
            GroupName = groupRequestDto.GroupName,
            Description = groupRequestDto.Description,
            CreatorUserID = userId,
        };
        var result = await groupRepository.CreateGroup(group);
        if (result == null)
        {
            return null;
        }
        var groupRequest = new GroupRequest
        {
            GroupId = result.GroupId,
            UserId = userId,
            Status = "Accepted"
        };
        var groupRequestCreated = await groupRepository.CreateGroupRequest(groupRequest);
        if (groupRequestCreated == null)
        {
            await groupRepository.DeleteGroup(result);
            return null;
        }
        return new GroupResponseDto
        {
            GroupId = result.GroupId,
            GroupName = result.GroupName,
            Description = result.Description,
            CreatorUserID = result.CreatorUserID
        };
    }

    public async Task<GroupResponseDto> UpdateGroup(int groupId, GroupRequestDto groupRequestDto)
    {
        var group = await groupRepository.GetGroupById(groupId);
        if (groupRequestDto.GroupName != group.GroupName)
        {
            group.GroupName = groupRequestDto.GroupName;
        }
        if (groupRequestDto.Description != group.Description)
        {
            group.Description = groupRequestDto.Description;
        }
        var groupUpdated = await groupRepository.UpdateGroup(group);
        return new GroupResponseDto
        {
            GroupId = groupUpdated.GroupId,
            GroupName = groupUpdated.GroupName,
            Description = groupUpdated.Description,
            CreatorUserID = groupUpdated.CreatorUserID
        };
    }

    public async Task<GroupRequestResponseDto?> CreateGroupRequest(int userId, int groupId)
    {
        var check = await groupRepository.GetGroupRequest(userId, groupId);
        // Check if the user has already been requested to join the group or is already inside the group
        if (check != null)
        {
            return null;
        }
        var groupRequest = new GroupRequest
        {
            GroupId = groupId,
            UserId = userId,
            Status = "Pending"
        };

        var groupRequestDone = await groupRepository.CreateGroupRequest(groupRequest);
        var creator = await userRepository.GetUserById(groupRequest.Group.CreatorUserID);
        var grDto = new GroupRequestResponseDto
        {
            GroupId = groupRequestDone.GroupId,
            GroupName = groupRequest.Group.GroupName,
            UserId = groupRequest.UserId,
            Username = creator.Username,
            Status = groupRequest.Status
        };
        return grDto;
    }

    public async Task<GroupRequestResponseDto> AcceptGroupRequest(int userId, int groupId)
    {
        var groupRequest = await groupRepository.GetGroupRequest(userId, groupId);
        if (groupRequest == null)
        {
            return null;
        }
        var status = groupRequest.Status;
        if (status == "Pending")
        {
            groupRequest.Status = "Accepted";
            await groupRepository.AcceptGroupRequest(groupRequest);
        }
        var user = await userRepository.GetUserById(groupRequest.UserId);
        var group = await groupRepository.GetGroupById(groupRequest.GroupId);
        return new GroupRequestResponseDto
        {
            GroupId = groupRequest.GroupId,
            GroupName = group.GroupName,
            UserId = groupRequest.UserId,
            Username = user.Username,
            Status = groupRequest.Status
        };
    }

    public async Task RejectGroupRequest(int userId, int groupId)
    {
        var groupRequest = await groupRepository.GetGroupRequest(userId, groupId);
        if (groupRequest == null)
        {
            return;
        }
        await groupRepository.RemoveGroupRequest(groupRequest);
    }

    public async Task RemoveGroup(int groupId)
    {
        var group = await groupRepository.GetGroupById(groupId);
        if (group == null)
        {
            return;
        }
        await groupRepository.DeleteGroup(group);
    }

    public async Task<List<CommentResponseDto>> GetCommentsByScriptId(int scriptId)
    {
        var comments = await commentRepository.GetCommentsByScriptId(scriptId);
        if (comments.Count == 0)
        {
            return [];
        }
        var commentListResponse = new List<CommentResponseDto>();
        foreach (var comment in comments)
        {   
            var user = await userRepository.GetUserById(comment.UserId);
            if (user != null)
            {
                var commentRequestToAdd = new CommentResponseDto
                {
                    commentId = comment.CommentId,
                    userId = comment.UserId,
                    userName = user.Username,
                    scriptId = comment.ScriptId,
                    description = comment.Description,
                };
                commentListResponse.Add(commentRequestToAdd);
            }
        }

        return commentListResponse;
    }

    public async Task<CommentResponseDto?> CreateComment(int userId, int scriptId, CommentRequestDto commentRequestDto)
    {
        var script = await scriptRepository.GetScriptById(scriptId);
        if (script == null)
        {
            return null;
        }
        var comment = new Comment
        {
            UserId = userId,
            ScriptId = scriptId,
            Description = commentRequestDto.Description
        };
        var response = await commentRepository.CreateComment(comment);
        if (response == null)
        {
            return null;
        }
        var user = await userRepository.GetUserById(userId);
        return new CommentResponseDto
        {
            commentId = response.CommentId,
            userId = response.UserId,
            userName = user.Username,
            scriptId = response.ScriptId,
            description = response.Description
        };
        
    }

    public async Task<CommentResponseDto?> GetCommentById(int commentId)
    {
        var comment = await commentRepository.GetCommentById(commentId);
        if (comment == null)
        {
            return null;
        }
        var user = await userRepository.GetUserById(comment.UserId);
        return new CommentResponseDto
        {
            commentId = comment.CommentId,
            userId = comment.UserId,
            userName = user.Username,
            scriptId = comment.ScriptId,
            description = comment.Description
        };
    }

    public async Task<CommentResponseDto?> UpdateComment(int commentId, CommentPatchRequestDto commentRequestDto)
    {
        var initialComment = await commentRepository.GetCommentById(commentId);
        if (initialComment == null)
        {
            return null;
        }
        if (initialComment.Description != commentRequestDto.Description)
        {
            initialComment.Description = commentRequestDto.Description;
        }
        var response = await commentRepository.UpdateComment(initialComment);
        if (response == null)
        {
            return null;
        }
        var user = await userRepository.GetUserById(response.UserId);
        return new CommentResponseDto
                {
                    commentId = response.CommentId,
                    userId = response.UserId,
                    userName = user.Username,
                    scriptId = response.ScriptId,
                    description = response.Description
                };
    }
    
    public async Task DeleteComment(int commentId)
    {
        await commentRepository.DeleteComment(commentId);
    }
    
    public async Task<int?> CreateLike(int userId, int scriptId)
    {
        var likeExists = await likeRepository.GetLikesByUserIdAndScriptId(userId, scriptId);
        if (likeExists != null)
        {
            return null;
        }
        var likeToAdd = new Like
        {
            ScriptId = scriptId,
            UserId = userId
        };
        var like = await likeRepository.CreateLike(likeToAdd);
        return like?.LikeId;
    }

    public async Task<int?> GetLikeById(int userId, int likeId)
    {
        var like = await likeRepository.GetLikeById(likeId);
        if (like == null || like.UserId != userId)
        {
            return null;
        }
        return like.LikeId;
    }
    
    public async Task DeleteLike(int userId, int scriptId)
    {
        var like = await likeRepository.GetLikesByUserIdAndScriptId(userId, scriptId);
        if (like == null)
        {
            return;
        }
        await likeRepository.DeleteLike(like.LikeId);
    }

    public async Task<List<int>> GetLikes()
    {
        return await likeRepository.GetAllLikes();
    }
}
