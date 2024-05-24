using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface ICommentRepository
{
    public Task<IEnumerable<Comment?>> GetAllComments();

    public Task<Comment?> GetCommentById(int id);

    public Task<IEnumerable<Comment?>> GetCommentsByScriptId(int scriptId);

    public Task<IEnumerable<Comment?>> GetCommentsByUserId(int userId);

    public Task<Comment?> CreateComment(Comment comment);

    public Task<Comment?> UpdateComment(Comment comment);

    public Task<Comment?> DeleteComment(int id);
}