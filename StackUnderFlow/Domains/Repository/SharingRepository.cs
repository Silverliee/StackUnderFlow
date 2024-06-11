using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class SharingRepository(MySqlDbContext context) : ISharingRepository
{
    //get all sharing
    public async Task<List<Sharing>> GetAllSharing()
    {
        return await context.Sharings.ToListAsync();
    }

    //get sharing by id
    public async Task<Sharing?> GetSharingById(int id)
    {
        return await context.Sharings.FindAsync(id);
    }

    //get sharing by script id
    public async Task<List<Sharing>> GetSharingByScriptId(int scriptId)
    {
        return await context.Sharings.Where(x => x.ScritpId == scriptId).ToListAsync();
    }

    //add sharing
    public async Task AddSharing(Sharing? sharing)
    {
        await context.Sharings.AddAsync(sharing);
        await context.SaveChangesAsync();
    }

    //update sharing
    public async Task UpdateSharing(Sharing? sharing)
    {
        context.Sharings.Update(sharing);
        await context.SaveChangesAsync();
    }

    //delete sharing
    public async Task DeleteSharing(int id)
    {
        var sharing = await context.Sharings.FindAsync(id);
        context.Sharings.Remove(sharing);
        await context.SaveChangesAsync();
    }
}
