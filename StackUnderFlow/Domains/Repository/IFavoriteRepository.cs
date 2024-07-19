using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IFavoriteRepository
{
    Task<List<Favorite>> GetAllFavorite();
    Task<Favorite?> GetFavoriteById(int id);
    Task<List<Favorite>> GetFavoriteByScriptId(int scriptId);
    Task<Favorite?> GetFavoriteByScriptIdAndUserId(int scriptId, int userId);
    Task<List<Favorite>> GetFavoriteByUserId(int userId);
    Task AddFavorite(Favorite? favorite);
    Task UpdateFavorite(Favorite? favorite);
    Task DeleteFavorite(int scriptId, int userId);
}