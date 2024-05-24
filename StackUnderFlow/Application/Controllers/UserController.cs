using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject;
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
    
}