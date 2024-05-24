using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IScriptRepository
{
    Task<List<Script?>> GetAllScripts();
    Task<Script?> GetScriptById(int id);
    Task<List<Script?>> GetScriptsByUserId(int userId);
    Task AddScript(Script? script);
    Task UpdateScript(Script? script);
    Task DeleteScript(int id);
}