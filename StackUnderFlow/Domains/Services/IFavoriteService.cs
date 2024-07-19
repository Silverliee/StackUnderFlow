using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Services;

public interface IFavoriteService
{
    public Task<List<ScriptResponseDto>> GetFavorites(int userId);
    public Task<FavoriteResponseDto?> CreateFavorite(int scriptId, int userId);
    public Task DeleteFavorite(int scriptId, int userId);
}