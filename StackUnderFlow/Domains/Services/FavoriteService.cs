using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class FavoriteService(IFavoriteRepository favoriteRepository, IScriptRepository scriptRepository, ILikeRepository likeRepository) : IFavoriteService 
{
    public async Task<List<ScriptResponseDto>> GetFavorites(int userId)
    {
        
        // var fav = new Favorite{ScritpId =  6, UserId = 1};
        // await favoriteRepository.AddFavorite(fav);
        // return [];
        var result = await favoriteRepository.GetFavoriteByUserId(userId);
        var favoriteList = new List<ScriptResponseDto>();
        foreach (var favorite in result)
        {
            var script = await scriptRepository.GetScriptById(favorite.ScritpId);
            var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);

            var scriptResponseDto = new ScriptResponseDto
                {
                    ScriptId = script.ScriptId,
                    ScriptName = script.ScriptName,
                    Description = script.Description,
                    InputScriptType = script.InputScriptType,
                    UserId = script.UserId,
                    OutputScriptType = script.OutputScriptType,
                    ProgrammingLanguage = script.ProgrammingLanguage,
                    Visibility = script.Visibility,
                    CreatorName = script.CreatorName,
                    NumberOfLikes = numberOfLikes.Count,
                    CreationDate = script.CreationDate.ToString(),
                    IsLiked = numberOfLikes.Any(x => x.UserId == favorite.UserId),
                    IsFavorite = true
                };

            favoriteList.Add(scriptResponseDto);
        }
        return favoriteList;
    }

    public async Task<FavoriteResponseDto?> CreateFavorite(int scriptId, int userId)
    {
        var checkFavorite = await favoriteRepository.GetFavoriteByScriptIdAndUserId(scriptId,userId);
        if (checkFavorite != null) return null;
        var favorite = new Favorite{ScritpId = scriptId, UserId = userId};
        await favoriteRepository.AddFavorite(favorite);
        return new FavoriteResponseDto{scriptId = favorite.ScritpId, userId = favorite.UserId};
    }
    
    public async Task DeleteFavorite(int scriptId, int userId)
    {
        await favoriteRepository.DeleteFavorite(scriptId, userId);
    }
}