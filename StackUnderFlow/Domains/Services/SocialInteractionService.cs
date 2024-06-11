using Microsoft.Identity.Client;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class SocialInteractionService(
    IFriendRepository friendRepository,
    IFollowRepository followRepository,
    IGroupRepository groupRepository
) : ISocialInteractionService
{
    public async Task<List<UserResponseDto>> GetFriendsByUserId(int userId)
    {
        var friends = await friendRepository.GetFriendsByUserId(userId);
        if (friends.Count == 0)
        {
            return [];
        }
        return friends
            .Select(f => new UserResponseDto
            {
                UserId = f.UserId2,
                Username = f.User2.Username,
                Email = f.User2.Email
            })
            .ToList();
    }

    public async Task<List<FriendRequestResponseDto>> GetFriendRequestsByUserId(int userId)
    {
        var friendRequests = await friendRepository.GetFriendRequestsByUserId(userId);
        if (friendRequests.Count == 0)
        {
            return [];
        }
        return friendRequests
            .Select(f => new FriendRequestResponseDto
            {
                UserId = f.UserId1,
                FriendId = f.UserId2,
                Status = f.Status,
                Message = f.Message
            })
            .ToList();
    }

    public async Task RemoveFriend(int userId, int friendId)
    {
        await friendRepository.RemoveFriend(userId, friendId);
    }

    public async Task<UserResponseDto?> CreateFriendRequest(
        int userId,
        int friendId,
        string message
    )
    {
        var user = await friendRepository.CreateFriendRequest(userId, friendId, message);
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

    public async Task<FriendRequestResponseDto?> GetFriendRequest(int userId, int friendId)
    {
        var friendRequest = await friendRepository.GetFriendRequest(userId, friendId);
        if (friendRequest == null)
        {
            return null;
        }
        return new FriendRequestResponseDto
        {
            UserId = friendRequest.UserId1,
            FriendId = friendRequest.UserId2,
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
        return new UserResponseDto
        {
            UserId = friendRequest.UserId2,
            Username = friendRequest.User2.Username,
            Email = friendRequest.User2.Email
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
                Email = f.User1.Email
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
            Email = user.User1.Email
        };
    }

    public async Task<List<GroupRequestResponseDto>> GetGroupRequestsByUserId(int userId)
    {
        var result = await groupRepository.GetGroupRequestsByUserId(userId);
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
                Email = user.Email
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

        await groupRepository.CreateGroupRequest(groupRequest);
        return new GroupRequestResponseDto
        {
            GroupId = groupRequest.GroupId,
            GroupName = groupRequest.Group.GroupName,
            UserId = groupRequest.UserId,
            Username = groupRequest.User.Username,
            Status = groupRequest.Status
        };
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
        return new GroupRequestResponseDto
        {
            GroupId = groupRequest.GroupId,
            GroupName = groupRequest.Group.GroupName,
            UserId = groupRequest.UserId,
            Username = groupRequest.User.Username,
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
}
