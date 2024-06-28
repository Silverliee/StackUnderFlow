namespace StackUnderFlow.Domains.Services;

using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;

public interface IScriptService
{
    public Task<List<ScriptResponseDto>> GetScripts(int offset, int records, string visibility, int requesterId);
    public Task<ScriptResponseDto?> GetScriptById(int scriptId, int requesterId);
    public Task<List<ScriptResponseDto>> GetScriptsByUserId(int userId, int requesterId);
    public Task<ScriptResponseDto?> AddScript(ScriptUploadRequestDto? scriptUploadRequestDto);
    public Task<ScriptResponseDto?> UpdateScript(ScriptUpdateRequestDto? scriptUpdateRequestDto);
    public Task<ScriptFileResponseDto?> GetScriptFileByScriptId(int scriptId);
    public Task DeleteScriptAndVersions(int scriptId);
    public Task DeleteScriptVersionById(int scriptVersionId);
    public Task<List<ScriptVersionResponseDto>> GetScriptVersionsByScriptId(int scriptId);
    public Task<ScriptVersionFileResponseDto?> GetScriptVersionFileById(int scriptVersionId);
    public Task<ScriptVersionResponseDto?> AddScriptVersion(
        ScriptVersionUploadRequestDto? scriptVersionUploadRequestDto
    );
    public Task<List<ScriptResponseDto>> GetScriptsByKeyWord(string keyword, int requesterId);
    public Task<List<ScriptResponseDto>> GetScriptsByUserIdAndVisibility(int userId,ScriptRequestForOtherUserDto scriptRequest);
}
