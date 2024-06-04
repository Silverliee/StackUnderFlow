using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
public class ScriptController(IScriptService scriptService) : ControllerBase
{
    [HttpGet("script/{scriptId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScriptById(int scriptId)
    {
        var script = await scriptService.GetScriptById(scriptId);
        if (script == null)
        {
            return NotFound();
        }
        return Ok(script);
    }
    
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScriptsByUserId(int userId)
    {
        var scripts = await scriptService.GetScriptsByUserId(userId);
       //ScriptService always send back a list, empty if no scripts or if user not found
        return Ok(scripts);
    }
    
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddScript([FromBody] ScriptUploadRequestDto scriptUploadRequestDto)
    {
        var response = await scriptService.AddScript(scriptUploadRequestDto);
        if (response == null)
        {
            return NotFound();
        }
        return Created($"{response.ScriptId}",response);
    }
    
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateScript(int scriptId, [FromBody] ScriptUpdateRequestDto scriptUpdateRequestDto)
    {
        await scriptService.UpdateScript(scriptUpdateRequestDto);
        return Ok();
    }
    
}