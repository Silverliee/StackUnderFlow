using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface ICommentRepository
{
    public Task<List<Comment?>> GetAllComments();

    public Task<Comment?> GetCommentById(int id);

    public Task<List<Comment?>> GetCommentsByScriptId(int scriptId);

    public Task<List<Comment?>> GetCommentsByUserId(int userId);

    public Task<Comment?> CreateComment(Comment comment);

    public Task<Comment?> UpdateComment(Comment comment);

    public Task<Comment?> DeleteComment(int id);
}
