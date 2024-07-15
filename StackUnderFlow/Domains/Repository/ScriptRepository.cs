using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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
    
    private Expression<Func<Script, bool>> BuildFilterExpression(string[] keywords, string visibility, string language)
    {
        var parameter = Expression.Parameter(typeof(Script), "x");
        var visibilityExpression = Expression.Equal(Expression.Property(parameter, "Visibility"), Expression.Constant(visibility));

        Expression combinedExpression = null;

        // Create the expression for each keyword
        foreach (var keyword in keywords)
        {
            var scriptNameContainsKeyword = Expression.Call(Expression.Property(parameter, "ScriptName"), "Contains", null, Expression.Constant(keyword));
            var descriptionContainsKeyword = Expression.Call(Expression.Property(parameter, "Description"), "Contains", null, Expression.Constant(keyword));

            var keywordExpression = Expression.OrElse(scriptNameContainsKeyword, descriptionContainsKeyword);
            
            combinedExpression = combinedExpression == null ? keywordExpression : Expression.OrElse(combinedExpression, keywordExpression);
        }
        
        combinedExpression = Expression.AndAlso(combinedExpression, visibilityExpression);
        if (language != "all")
        {
            combinedExpression = Expression.AndAlso(combinedExpression, Expression.Equal(Expression.Property(parameter, "ProgrammingLanguage"), Expression.Constant(language)));
        }
        return Expression.Lambda<Func<Script, bool>>(combinedExpression, parameter);
    }


    public async Task<(List<Script?> scripts, int totalCount)> GetScriptsByKeyWord(string[] keywords, int offset, int records, string visibility, string language)
    {
        var filterExpression = BuildFilterExpression(keywords, visibility,language);

        var filteredScriptsQuery = context
            .Scripts
            .Where(filterExpression)
            .Distinct();

        var totalCount = await filteredScriptsQuery.CountAsync();

        var filteredScripts = await filteredScriptsQuery
            .OrderByDescending(x => x.ScriptId)
            .Skip(offset)
            .Take(records)
            .ToListAsync();

        return (filteredScripts, totalCount);
    }
}
