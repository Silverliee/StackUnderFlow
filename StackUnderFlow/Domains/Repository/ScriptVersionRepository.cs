using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class ScriptVersionRepository(MySqlDbContext context) : IScriptVersionRepository
{
    //get all script versions
    public async Task<List<ScriptVersion>> GetAllScriptVersions()
    {
        return await context.ScriptVersions.ToListAsync();
    }

    //get script version by id
    public async Task<ScriptVersion?> GetScriptVersionById(int id)
    {
        return await context.ScriptVersions.FirstOrDefaultAsync(x => x.ScriptVersionId == id);
    }

    //get script version by script id
    public async Task<List<ScriptVersion>> GetScriptVersionsByScriptId(int scriptId)
    {
        return await context.ScriptVersions.Where(x => x.ScriptId == scriptId).ToListAsync();
    }

    //add script version
    public async Task AddScriptVersion(ScriptVersion? scriptVersion)
    {
        if (scriptVersion != null)
            await context.ScriptVersions.AddAsync(scriptVersion);
        await context.SaveChangesAsync();
    }

    //update script version
    public async Task UpdateScriptVersion(ScriptVersion? scriptVersion)
    {
        if (scriptVersion != null)
            context.ScriptVersions.Update(scriptVersion);
        await context.SaveChangesAsync();
    }

    //delete script version
    public async Task DeleteScriptVersion(int id)
    {
        var scriptVersion = await context.ScriptVersions.FirstOrDefaultAsync(x =>
            x.ScriptVersionId == id
        );
        if (scriptVersion != null)
            context.ScriptVersions.Remove(scriptVersion);
        await context.SaveChangesAsync();
    }

    //get latest script version by script id
    public async Task<ScriptVersion?> GetLatestScriptVersionByScriptId(int scriptId)
    {
        return await context
            .ScriptVersions.Where(x => x.ScriptId == scriptId)
            .OrderByDescending(x => x.CreationDate)
            .FirstOrDefaultAsync();
    }

    //delete all version of a script
    public async Task DeleteScriptVersionsByScriptId(int scriptId)
    {
        var scriptVersions = await context
            .ScriptVersions.Where(x => x.ScriptId == scriptId)
            .ToListAsync();
        context.ScriptVersions.RemoveRange(scriptVersions);
        await context.SaveChangesAsync();
    }
}
