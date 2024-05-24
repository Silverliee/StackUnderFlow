using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IScriptVersionRepository
{
    Task<List<ScriptVersion>> GetAllScriptVersions();
    Task<ScriptVersion?> GetScriptVersionById(int id);
    Task<List<ScriptVersion>> GetScriptVersionsByScriptId(int scriptId);
    Task AddScriptVersion(ScriptVersion? scriptVersion);
    Task UpdateScriptVersion(ScriptVersion? scriptVersion);
    Task DeleteScriptVersion(int id);
}