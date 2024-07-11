using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class ScriptRepository(MySqlDbContext context) : IScriptRepository
{
    //get all scripts
    public async Task<List<Script?>> GetAllScripts()
    {
        return await context.Scripts.ToListAsync();
    }
    
    public async Task<List<Script?>> GetScripts(int offset, int records, string visibility)
    {
        return await context.Scripts
            .Where(x => x.Visibility == visibility)
            .OrderByDescending(x => x.ScriptId)
            .Skip(offset)
            .Take(records)
            .ToListAsync();
    }

    //get script by id
    public async Task<Script?> GetScriptById(int id)
    {
        return await context.Scripts.FirstOrDefaultAsync(x => x.ScriptId == id);
    }

    //get script by user id
    public async Task<List<Script>> GetScriptsByUserId(int userId)
    {
        return await context.Scripts.Where(x => x.UserId == userId).ToListAsync();
    }

    //add script
    public async Task<Script?> AddScript(Script script)
    {
        var result = await context.Scripts.AddAsync(script);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    //update script
    public async Task<Script?> UpdateScript(Script script)
    {
        var result = context.Scripts.Update(script);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    //delete script
    public async Task DeleteScript(int id)
    {
        var script = await context.Scripts.FirstOrDefaultAsync(x => x.ScriptId == id);
        context.Scripts.Remove(script);
        await context.SaveChangesAsync();
    }

    public async Task<(List<Script?> scripts, int totalCount)> GetScriptsByKeyWord(string keyword, int offset, int records, string visibility)
    {
        var filteredScripts = await context
            .Scripts
            .Where(x => (x.ScriptName.Contains(keyword) || x.Description.Contains(keyword)) && x.Visibility == visibility)
            .OrderByDescending(x => x.ScriptId)
            .Skip(offset)
            .Take(records)
            .ToListAsync();

        var totalCount = await context
            .Scripts
            .Where(x => (x.ScriptName.Contains(keyword) || x.Description.Contains(keyword)) && x.Visibility == visibility)
            .CountAsync();

        return (filteredScripts, totalCount);
    }

}
