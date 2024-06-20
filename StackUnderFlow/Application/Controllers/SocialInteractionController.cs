using System.Security.Claims;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAll")]
public class SocialInteractionController(ISocialInteractionService socialInteractionService,
    Bugsnag.IClient bugsnag)
    : ControllerBase
{
    #region Friends


    [HttpGet("friends")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFriends()
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Getting friends"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var friends = await socialInteractionService.GetFriendsByUserId(userId);
            return Ok(friends);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("friends/requests")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFriendRequests()
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Getting friend requests"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var requests = await socialInteractionService.GetFriendRequestsByUserId(userId);
            return Ok(requests);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("friends/{friendId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFriendRequest(int friendId, [FromBody] FriendRequestCreationRequestDto friendRequestCreationRequestDto)
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Creating friend request"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var friend = await socialInteractionService.CreateFriendRequest(
                userId,
                friendId,
                friendRequestCreationRequestDto.Message
            );
            if (friend == null)
            {
                return BadRequest();
            }
            return Created("", friend);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPatch("friends/{friendId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AcceptFriendRequest(int friendId)
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Accepting friend request"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var friendRequest = await socialInteractionService.GetFriendRequest(userId, friendId);
            if (friendRequest == null)
            {
                return NotFound();
            }
            var friend = await socialInteractionService.AcceptFriendRequest(
                friendRequest.UserId,
                friendRequest.FriendId
            );
            return Ok(friend);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("friends/{friendId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFriend(int friendId)
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Removing friend"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            await socialInteractionService.RemoveFriend(userId, friendId);
            return NoContent();
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    #endregion
    #region Follows

    [HttpGet("follows")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFollows()
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Getting follows"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var follows = await socialInteractionService.GetFollowsByUserId(userId);
            return Ok(follows);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("follows/{followId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFollow(int followId)
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Removing follow"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            await socialInteractionService.RemoveFollow(userId, followId);
            return NoContent();
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("follows/{followId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddFollow(int followId)
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Adding follow"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var follow = await socialInteractionService.AddFollow(userId, followId);
            if (follow == null)
            {
                return BadRequest();
            }
            return Created("", follow);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    #endregion
    #region Group

    [HttpGet("groups")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGroupsByUserId()
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Getting groups"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var groups = await socialInteractionService.GetGroupsByUserId(userId);
            return Ok(groups);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("groups/{groupId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGroupById(int groupId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting group by id"));
            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("groups/{groupId:int}/members")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGroupMembers(int groupId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting group members"));
            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }
            var members = await socialInteractionService.GetGroupMembers(groupId);
            return Ok(members);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("groups/{groupId:int}/requests")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGroupRequestsByGroupId(int groupId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting group requests by group id"));
            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }
            var requests = await socialInteractionService.GetGroupRequestsByGroupId(groupId);
            return Ok(requests);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("groups/requests")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGroupRequestsByUserId()
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Getting group requests by user id"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var requests = await socialInteractionService.GetGroupRequestsByUserId(userId);
            return Ok(requests);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("groups")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroup(GroupRequestDto groupRequestDto)
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Creating group"));
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var group = await socialInteractionService.CreateGroup(userId, groupRequestDto);
            if (group == null)
            {
                return BadRequest();
            }
            return Created("", group);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPatch("groups/{groupId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateGroup(int groupId, GroupRequestDto groupRequestDto)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Updating group"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }

            if (userId != group.CreatorUserID)
            {
                return Unauthorized();
            }

            var updatedGroup = await socialInteractionService.UpdateGroup(groupId, groupRequestDto);
            return Ok(updatedGroup);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("groups/{groupId:int}/{memberId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateGroupRequest(int groupId, int memberId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Creating group request"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }
            if (group.CreatorUserID != userId)
            {
                return Unauthorized();
            }
            var groupRequest = await socialInteractionService.CreateGroupRequest(memberId, groupId);
            if (groupRequest == null)
            {
                return BadRequest();
            }
            return Ok(groupRequest);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPatch("groups/requests/{groupId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AcceptGroupRequest(int groupId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Accepting group request"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }
            var groupRequest = await socialInteractionService.AcceptGroupRequest(userId, groupId);
            if (groupRequest == null)
            {
                StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(groupRequest);
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("groups/requests/{groupId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RejectGroupRequest(int groupId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Rejecting group request"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null)
            {
                return NoContent();
            }

            await socialInteractionService.RejectGroupRequest(userId, groupId);
            return NoContent();
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("groups/{groupId:int}/{memberId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveGroupMember(int groupId, int memberId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Removing group member"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null || group.CreatorUserID != userId)
            {
                return NoContent();
            }
            await socialInteractionService.RejectGroupRequest(groupId, memberId);
            return NoContent();
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("groups/{groupId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveGroup(int groupId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Removing group"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var group = await socialInteractionService.GetGroupById(groupId);
            if (group == null || group.CreatorUserID != userId)
            {
                return NoContent();
            }
            await socialInteractionService.RemoveGroup(groupId);
            return NoContent();
        }
        catch(Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    #endregion
}
