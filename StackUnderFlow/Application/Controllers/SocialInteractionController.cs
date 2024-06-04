using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
public class SocialInteractionController(IReactionService reactionService) : ControllerBase
{

    [HttpPost("postComment")]
    public async Task<IActionResult> PostComment(PostCommentDto comment)
    {
        var result = await reactionService.PostComment(comment);
        return Ok(result);
    }

    [HttpDelete("deleteComment")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        await reactionService.DeleteComment(commentId);
        return Ok();
    }


    [HttpPatch("updateComment")]
    public async Task<IActionResult> UpdateComment(PatchCommentDto patchComment)
    {
        var result = await reactionService.UpdateComment(patchComment);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("getComment")]
    public async Task<IActionResult> GetCommentById(int commentId)
    {
        var result = await reactionService.GetCommentById(commentId);
        if (result == null) 
            return NotFound();
        return Ok(result);
    }

    [HttpGet("getCommentList")]
    public async Task<IActionResult> GetCommentListByScriptId(int scriptId)
    {
        var result = await reactionService.GetCommentListByScriptId(scriptId);
        if (result == null)
            return NotFound();
        return Ok(result);
    }
}