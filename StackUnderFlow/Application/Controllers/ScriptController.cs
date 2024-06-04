using Microsoft.AspNetCore.Mvc;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
public class ScriptController : ControllerBase
{
    [HttpGet]
    public IActionResult GetScriptById(int id)
    {
        return Ok("Hello World");
    }
}