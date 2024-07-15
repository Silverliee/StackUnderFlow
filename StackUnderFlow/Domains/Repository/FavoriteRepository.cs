using System.Data.Entity;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class FavoriteRepository(MySqlDbContext context) : IFavoriteRepository
{
    public async Task<List<Favorite>> GetAllFavorite()
    {
        return await context.Favorites.ToListAsync();
    }

    public async Task<Favorite?> GetFavoriteById(int id)
    {
        return await context.Favorites.FindAsync(id);
    }

    public async Task<List<Favorite>> GetFavoriteByScriptId(int scriptId)
    {
        return await context.Favorites.Where(x => x.ScritpId == scriptId).ToListAsync();
    }

    public async Task AddFavorite(Favorite? favorite)
    {
        if (favorite != null) await context.Favorites.AddAsync(favorite);
        await context.SaveChangesAsync();
    }

    public async Task UpdateFavorite(Favorite? favorite)
    {
        if (favorite != null) context.Favorites.Update(favorite);
        await context.SaveChangesAsync();
    }

    public async Task DeleteFavorite(int id)
    {
        var favorite = await context.Favorites.FindAsync(id);
        if (favorite != null)
        {
            context.Favorites.Remove(favorite);
            await context.SaveChangesAsync();
        }
    }
}