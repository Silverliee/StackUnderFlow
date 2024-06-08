using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
public class ScriptController(IScriptService scriptService) : ControllerBase
{
    [HttpGet("{scriptId:int}")]
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
    
    [HttpGet("{userId:int}/{scriptId:int}/blob")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScriptBlobByScriptId(int scriptId, int userId)
    {
        var blob = await scriptService.GetScriptBlobByScriptId(scriptId, userId);
        if (blob == null)
        {
            return NotFound();
        }
        HttpContext.Response.Headers.Append("Content-Disposition", "attachment; filename=" + blob.FileName);
        return File(blob.Blob, "application/octet-stream");
    }
    
    [HttpGet("{userId:int}/{scriptVersionId:int}/versionBlob")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScriptVersionBlobById(int scriptVersionId, int userId)
    {
        var blob = await scriptService.GetScriptVersionBlobById(scriptVersionId);
        if (blob == null)
        {
            return NotFound();
        }
        HttpContext.Response.Headers.Append("Content-Disposition", "attachment; filename=" + blob.FileName);
        return File(blob.Blob, "application/octet-stream");
    }
    
    [HttpGet("versions/{scriptId:int}")]
    public async Task<IActionResult> GetScriptVersionsByScriptId(int scriptId)
    {
        var scriptVersions = await scriptService.GetScriptVersionsByScriptId(scriptId);
        return Ok(scriptVersions);
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
    
    [HttpPost("upload/version")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddScriptVersion([FromBody] ScriptVersionUploadRequestDto scriptVersionUploadRequestDto)
    {
        await scriptService.AddScriptVersion(scriptVersionUploadRequestDto);
        return Created();
    }
    
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateScript([FromBody] ScriptUpdateRequestDto scriptUpdateRequestDto)
    {
        await scriptService.UpdateScript(scriptUpdateRequestDto);
        return Ok();
    }
    [HttpDelete("deleteFull/{scriptId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteScript(int scriptId)
    {
        await scriptService.DeleteScriptAndVersions(scriptId);
        return NoContent();
    }
    [HttpDelete("delete/version/{scriptVersionId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteScriptVersionById(int scriptVersionId)
    {
        await scriptService.DeleteScriptVersionById(scriptVersionId);
        return NoContent();
    }

    [HttpGet("search/{keyword}")]
    public async Task<IActionResult> GetScriptsByKeyWord(string keyword)
    {
        var scripts = await scriptService.GetScriptsByKeyWord(keyword);
        return Ok(scripts);
    }
}