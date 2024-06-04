namespace StackUnderFlow.Domains.Services;

using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Application.DataTransferObject.Request;

public interface IScriptService
{
    public Task<ScriptResponseDto?> GetScriptById(int scriptId);
    public Task<List<ScriptResponseDto>> GetScriptsByUserId(int userId);
    public Task<ScriptResponseDto?> AddScript(ScriptUploadRequestDto scriptUploadRequestDto);
    public Task<ScriptResponseDto?> UpdateScript(ScriptUpdateRequestDto scriptUpdateRequestDto);
}