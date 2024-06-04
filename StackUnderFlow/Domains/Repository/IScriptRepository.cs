using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IScriptRepository
{
    Task<List<Script?>> GetAllScripts();
    Task<Script?> GetScriptById(int id);
    Task<List<Script>> GetScriptsByUserId(int userId);
    Task<Script?> AddScript(Script script);
    Task<Script?> UpdateScript(Script script);
    Task DeleteScript(int id);
}