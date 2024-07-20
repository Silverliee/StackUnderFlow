
namespace StackUnderFlow.Domains.Services;

public interface IRunnerService
{
    public Task<string> ExecuteScript(int scriptId);
    public Task<string> ExecuteScriptFile(IFormFile? script);
    public Task<List<IFormFile>> ExecutePipelineWithIds(List<int> scriptIds, IFormFile input);
}
