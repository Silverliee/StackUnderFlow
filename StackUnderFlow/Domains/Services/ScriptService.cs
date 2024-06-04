using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class ScriptService(IScriptRepository scriptRepository, IUserRepository userRepository) : IScriptService
{
    public async Task<ScriptResponseDto?> GetScriptById(int scriptId)
    {
        var script = await scriptRepository.GetScriptById(scriptId);
        if (script == null)
        {
            return null;
        }
        return new ScriptResponseDto
        {
            ScriptId = script.ScriptId,
            ScriptName = script.ScriptName,
            Description = script.Description,
            InputScriptType = script.InputScriptType,
            UserId = script.UserId,
            OutputScriptType = script.OutputScriptType,
            ProgrammingLanguage = script.ProgrammingLanguage
        };
    }
    
    public async Task<List<ScriptResponseDto>> GetScriptsByUserId(int userId)
    {
        var scripts = await scriptRepository.GetScriptsByUserId(userId);
        if (scripts.Count == 0)
        {
            return new List<ScriptResponseDto>();
        }
        return scripts.Select(script => new ScriptResponseDto
        {
            ScriptId = script.ScriptId,
            ScriptName = script.ScriptName,
            Description = script.Description,
            InputScriptType = script.InputScriptType,
            UserId = script.UserId,
            OutputScriptType = script.OutputScriptType,
            ProgrammingLanguage = script.ProgrammingLanguage
        }).ToList();
    }
    
    public async Task<ScriptResponseDto?> AddScript(ScriptUploadRequestDto scriptUploadRequestDto)
    {
        //Voir pour que le check soit sur le token renvoyé
        var user = await userRepository.GetUserById(scriptUploadRequestDto.UserId);
        
        if (user == null)
        {
            return null;
        }
        var script = new Script
        {
            ScriptName = scriptUploadRequestDto.ScriptName,
            Description = scriptUploadRequestDto.Description,
            InputScriptType = scriptUploadRequestDto.InputScriptType,
            OutputScriptType = scriptUploadRequestDto.OutputScriptType,
            ProgrammingLanguage = scriptUploadRequestDto.ProgrammingLanguage,
            Visibility = scriptUploadRequestDto.Visibility,
            UserId = scriptUploadRequestDto.UserId
        };
        var scriptUploaded = await scriptRepository.AddScript(script);
        return new ScriptResponseDto
        {
            ScriptId = script.ScriptId,
            ScriptName = script.ScriptName,
            Description = script.Description,
            InputScriptType = script.InputScriptType,
            UserId = script.UserId,
            OutputScriptType = script.OutputScriptType,
            ProgrammingLanguage = script.ProgrammingLanguage
        };
    }
    
    public async Task<ScriptResponseDto?> UpdateScript(ScriptUpdateRequestDto scriptUpdateRequestDto)
    {   
        var scriptInBdd = await scriptRepository.GetScriptById(scriptUpdateRequestDto.ScriptId);
        
        if (scriptInBdd == null)
        {
            return null;
        }
        
        //Check with token if scriptInBdd.UserId == token.UserId
        // ....
        
        var scriptFromRequest = new Script
        {
            ScriptId = scriptUpdateRequestDto.ScriptId,
            ScriptName = scriptUpdateRequestDto.ScriptName,
            Description = scriptUpdateRequestDto.Description,
            InputScriptType = scriptUpdateRequestDto.InputScriptType,
            OutputScriptType = scriptUpdateRequestDto.OutputScriptType,
            ProgrammingLanguage = scriptUpdateRequestDto.ProgrammingLanguage,
            Visibility = scriptUpdateRequestDto.Visibility,
            UserId = scriptInBdd.UserId
        };
        var scriptUpdated = await scriptRepository.UpdateScript(scriptFromRequest);
        
        if (scriptUpdated == null)
        {
            return null;
        }
        return new ScriptResponseDto
        {
            ScriptId = scriptUpdated.ScriptId,
            ScriptName = scriptUpdated.ScriptName,
            Description = scriptUpdated.Description,
            InputScriptType = scriptUpdated.InputScriptType,
            UserId = scriptUpdated.UserId,
            OutputScriptType = scriptUpdated.OutputScriptType,
            ProgrammingLanguage = scriptUpdated.ProgrammingLanguage
        };
    }
}