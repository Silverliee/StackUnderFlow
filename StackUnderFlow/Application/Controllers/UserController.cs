using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
public class UserController(ILoginService loginService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto user)
    {
        var result = await loginService.Register(user);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto login)
    {
        var result = await loginService.Login(login);
        return Ok(result);
    }
    
}