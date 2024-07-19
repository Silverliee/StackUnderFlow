using System.Security.Claims;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAll")]
public class UserController(ILoginService loginService, IProfileService profileService,
    Bugsnag.IClient bugsnag) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto? user)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Registering user"));
            var result = await loginService.Register(user);
            if (result == null)
            {
                return BadRequest("Username or Email already exists.");
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(LoginUserDto login)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Logging in user"));
            var result = await loginService.Login(login);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserByToken()
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting user by token"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await loginService.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpGet("search/{keyword}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchUsersByKeyword(string keyword)
    {
        var users = await loginService.SearchUsersByKeyword(keyword);
        return Ok(users);
    }
    
    [HttpGet("{userId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting user by id"));
            var user = await loginService.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    // password reset
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Forgot password"));
            var result = await loginService.ForgotPassword(email);
            if (result)
            {
                return Ok("Password sent to email.");
            } else
            {
                return NotFound("Email not found.");
            }
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("checkEmailAvailability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckEmailAvailability([FromQuery] string email)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Check email availability"));
            var result = await profileService.CheckEmailAvailability(email);
            var response = new EmailAvailabilityResponse{ IsAvailable = result };
                return Ok(response);
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        } 
    }
    
    [HttpGet("checkUsernameAvailability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckUsernameAvailability([FromQuery] string username)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Check email availability"));
            var result = await profileService.CheckUsernameAvailability(username);
            var response = new UsernameAvailabilityResponse{ IsAvailable = result };
            return Ok(response);
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        } 
    }
    
    [HttpPatch("update")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(UpdateUserDto user)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Updating user"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            user.UserId = userId;
            var result = await profileService.UpdateUser(user);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPost("checkPassword")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckPassword([FromBody] CheckPasswordDto checkPasswordDto)
    {
        try
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Checking password"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await loginService.CheckPassword(userId, checkPasswordDto.Password);
            return Ok(new CheckPasswordResponseDto { IsValid = result });
        }
        catch (Exception e)
        {
            bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
}
