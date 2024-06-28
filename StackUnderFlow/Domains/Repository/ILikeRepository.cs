using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface ILikeRepository
{
    public Task<List<int>> GetAllLikes();

    public Task<Like?> GetLikeById(int id);

    public Task<IEnumerable<Like?>> GetLikesByUserId(int userId);
    public Task<Like?> GetLikesByUserIdAndScriptId(int userId, int scriptId);


    public Task<List<Like>> GetLikesByScriptId(int commentId);

    public Task<Like?> CreateLike(Like like);

    public Task<Like?> DeleteLike(int id);
}
