using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface ILikeRepository
{
    public Task<IEnumerable<Like?>> GetAllLikes();

    public Task<Like?> GetLikeById(int id);

    public Task<IEnumerable<Like?>> GetLikesByUserId(int userId);

    public Task<IEnumerable<Like?>> GetLikesByScriptId(int commentId);

    public Task<Like?> CreateLike(Like like);

    public Task<Like?> DeleteLike(int id);
}
