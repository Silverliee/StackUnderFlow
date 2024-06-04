using StackUnderFlow.Application.DataTransferObject;

namespace StackUnderFlow.Domains.Services;

public interface IReactionService
{
    public Task DeleteComment(int commentId);
    public Task<CommentDto?> GetCommentById(int commentId);
    public  Task<IEnumerable<CommentDto?>> GetCommentListByScriptId(int scriptId);
    public Task<CommentDto> PostComment(PostCommentDto commentDto);

    public Task<CommentDto?> UpdateComment(PatchCommentDto patchComment);
}