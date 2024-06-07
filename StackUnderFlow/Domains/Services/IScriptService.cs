namespace StackUnderFlow.Domains.Services;

using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Application.DataTransferObject.Request;

public interface IScriptService
{
    public Task<ScriptResponseDto?> GetScriptById(int scriptId);
    public Task<List<ScriptResponseDto>> GetScriptsByUserId(int userId);
    public Task<ScriptResponseDto?> AddScript(ScriptUploadRequestDto scriptUploadRequestDto);
    public Task<ScriptResponseDto?> UpdateScript(ScriptUpdateRequestDto scriptUpdateRequestDto);
    public Task<ScriptBlobResponseDto?> GetScriptBlobByScriptId(int scriptId, int userId);
    public Task DeleteScriptAndVersions(int scriptId);
    public Task DeleteScriptVersionById(int scriptVersionId);
    public Task<List<ScriptVersionResponseDto>> GetScriptVersionsByScriptId(int scriptId);
    public Task<ScriptVersionBlobResponseDto?> GetScriptVersionBlobById(int scriptVersionId);
    public Task<ScriptVersionResponseDto?> AddScriptVersion(ScriptVersionUploadRequestDto scriptVersionUploadRequestDto);
    public Task<List<ScriptResponseDto>> GetScriptsByKeyWord(string keyword);
}