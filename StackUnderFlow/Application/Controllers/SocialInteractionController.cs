using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
[EnableCors("AllowAll")]
public class SocialInteractionController(ISocialInteractionService socialInteractionService) : ControllerBase
{
    [HttpGet("friends")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFriends()
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var friends = await socialInteractionService.GetFriendsByUserId(userId);
            return Ok(friends);
        }
        catch
        {
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
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            await socialInteractionService.RemoveFriend(userId, friendId);
            return NoContent();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPost("friends/{friendId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddFriend(int friendId)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var friend = await socialInteractionService.AddFriend(userId, friendId);
            if (friend == null)
            {
                return BadRequest();
            }
            return Created("", friend);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }   
    
    [HttpGet("follows")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFollows()
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var follows = await socialInteractionService.GetFollowsByUserId(userId);
            return Ok(follows);
        }
        catch
        {
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
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            await socialInteractionService.RemoveFollow(userId, followId);
            return NoContent();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPost("follows/{followId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddFollow(int followId)
    {
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
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }   
}