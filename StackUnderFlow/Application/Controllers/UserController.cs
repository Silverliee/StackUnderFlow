using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Application.Middleware;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAll")]
public class UserController(ILoginService loginService,
    Bugsnag.IClient _bugsnag) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto? user)
    {
        try
        {
            var result = await loginService.Register(user);
            if (result == null)
            {
                return BadRequest("Username or Email already exists.");
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            _bugsnag.Notify(e);
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
            var result = await loginService.Login(login);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            _bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserByToken()
    {
        try
        {
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
            _bugsnag.Notify(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
