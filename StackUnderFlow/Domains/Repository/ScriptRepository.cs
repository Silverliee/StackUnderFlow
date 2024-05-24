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
    
    //get script by id
    public async Task<Script?> GetScriptById(int id)
    {
        return await context.Scripts.FirstOrDefaultAsync(x => x.ScriptId == id);
    }
    
    //get script by user id
    public async Task<List<Script?>> GetScriptsByUserId(int userId)
    {
        return await context.Scripts.Where(x => x.UserId == userId).ToListAsync();
    }
    
    //add script
    public async Task AddScript(Script? script)
    {
        await context.Scripts.AddAsync(script);
        await context.SaveChangesAsync();
    }
    
    //update script
    public async Task UpdateScript(Script? script)
    {
        context.Scripts.Update(script);
        await context.SaveChangesAsync();
    }
    
    //delete script
    public async Task DeleteScript(int id)
    {
        var script = await context.Scripts.FirstOrDefaultAsync(x => x.ScriptId == id);
        context.Scripts.Remove(script);
        await context.SaveChangesAsync();
    }
}