using Microsoft.IdentityModel.Tokens;
using StackUnderFlow.Application.DataTransferObject;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class ReactionService(ICommentRepository commentRepository, ILikeRepository likeRepository, IScriptRepository scriptRepository) : IReactionService
{
    public async Task<CommentDto> PostComment(PostCommentDto commentDto)
    {
        var myComment = new Comment
        {
            UserId = commentDto.UserId,
            ScriptId = commentDto.ScriptId,
            Description = commentDto.Description
        };
        var result = await commentRepository.CreateComment(myComment);

        var myCommentDto = new CommentDto
        {
            CommentId = result.CommentId,
            UserId = result.UserId,
            ScriptId = result.ScriptId,
            Description = result.Description
        };

        return myCommentDto;
    }

    public async Task DeleteComment(int commentId)
    {
        await commentRepository.DeleteComment(commentId);
    }
     
    public async Task<CommentDto?> UpdateComment(PatchCommentDto patchComment)
    {
        var comment = await commentRepository.GetCommentById(patchComment.CommentId);

        if (comment == null)
            return null;

        comment.Description = patchComment.Description;
        var result = await commentRepository.UpdateComment(comment);

        return new CommentDto
        {
            CommentId = result.CommentId,
            UserId = result.UserId,
            ScriptId = result.ScriptId,
            Description = result.Description
        };
    }

    public async Task<CommentDto?> GetCommentById(int commentId)
    {
        var result = await commentRepository.GetCommentById(commentId);
        if(result == null)
            return null;
        var myCommentDto = new CommentDto
        {
            CommentId = result.CommentId,
            UserId = result.UserId,
            ScriptId = result.ScriptId,
            Description = result.Description
        };

        return myCommentDto;
    }

    public async Task<IEnumerable<CommentDto>?> GetCommentListByScriptId(int scriptId)
    {
        var script = await scriptRepository.GetScriptById(scriptId);
        if (script == null)
            return null;

        var result = await commentRepository.GetCommentsByScriptId(scriptId);
        if (result.IsNullOrEmpty())
            return new List<CommentDto>();

        var list = result.Select(x => new CommentDto
        {
            CommentId = x.CommentId,
            UserId = x.UserId,
            ScriptId = x.ScriptId,
            Description = x.Description
        });

        return list;
    }
}