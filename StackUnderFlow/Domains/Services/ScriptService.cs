using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class ScriptService(IScriptRepository scriptRepository, IUserRepository userRepository, IScriptVersionRepository scriptVersionRepository) : IScriptService
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
            ProgrammingLanguage = script.ProgrammingLanguage,
            Visibility = script.Visibility,
            CreatorName = script.CreatorName
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
            ProgrammingLanguage = script.ProgrammingLanguage,
            Visibility = script.Visibility,
            CreatorName = script.CreatorName
        }).ToList();
    }

    public async Task<ScriptVersionResponseDto?> AddScriptVersion(
        ScriptVersionUploadRequestDto scriptVersionUploadRequestDto)
    {
        var script = await scriptRepository.GetScriptById(scriptVersionUploadRequestDto.ScriptId);
        if (script == null)
        {
            return null;
        }
        var blob = Convert.FromBase64String(scriptVersionUploadRequestDto.SourceScriptBinary);

        var scriptVersion = new ScriptVersion
        {
            ScriptId = script.ScriptId,
            VersionNumber = scriptVersionUploadRequestDto.VersionNumber,
            Comment = scriptVersionUploadRequestDto.Comment,
            CreationDate = DateTime.Now,
            CreatorUserId = scriptVersionUploadRequestDto.CreatorUserId,
            SourceScriptLink = "None",
            SourceScriptBinary = blob
        };
        
        await scriptVersionRepository.AddScriptVersion(scriptVersion);
        var response = new ScriptVersionResponseDto
        {
            ScriptVersionId = scriptVersion.ScriptVersionId,
            ScriptId = scriptVersion.ScriptId,
            VersionNumber = scriptVersion.VersionNumber,
            CreationDate = scriptVersion.CreationDate,
            CreatorUserId = scriptVersion.CreatorUserId,
            SourceScriptLink = scriptVersion.SourceScriptLink,
            Comment = scriptVersion.Comment
        };
        return response;
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
            UserId = scriptUploadRequestDto.UserId,
            CreatorName = user.Username
        };
        var scriptUploaded = await scriptRepository.AddScript(script);
        if (scriptUploaded == null)
        {
            return null;
        }
        //Creating version script with data now
        var blob = Convert.FromBase64String(scriptUploadRequestDto.SourceScriptBinary);
        var scriptVersion = new ScriptVersion
        {
            ScriptId = scriptUploaded.ScriptId,
            VersionNumber = "1", //"AddVersionNumberFromDtoHere",
            Comment = "Initial version",
            CreationDate = DateTime.Now,
            CreatorUserId = scriptUploaded.UserId,
            SourceScriptLink = "",
            SourceScriptBinary = blob
        };
        await scriptVersionRepository.AddScriptVersion(scriptVersion);
        return new ScriptResponseDto
        {
            ScriptId = script.ScriptId,
            ScriptName = script.ScriptName,
            Description = script.Description,
            InputScriptType = script.InputScriptType,
            UserId = script.UserId,
            OutputScriptType = script.OutputScriptType,
            ProgrammingLanguage = script.ProgrammingLanguage,
            Visibility = script.Visibility,
            CreatorName = user.Username
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

        scriptInBdd.ScriptId = scriptUpdateRequestDto.ScriptId;
        scriptInBdd.ScriptName = scriptUpdateRequestDto.ScriptName;
        scriptInBdd.Description = scriptUpdateRequestDto.Description;
        scriptInBdd.InputScriptType = scriptUpdateRequestDto.InputScriptType;
        scriptInBdd.OutputScriptType = scriptUpdateRequestDto.OutputScriptType;
        scriptInBdd.ProgrammingLanguage = scriptUpdateRequestDto.ProgrammingLanguage;
        scriptInBdd.Visibility = scriptUpdateRequestDto.Visibility;
        scriptInBdd.UserId = scriptInBdd.UserId;
        scriptInBdd.CreatorName = scriptInBdd.CreatorName;
        
        var scriptUpdated = await scriptRepository.UpdateScript(scriptInBdd);
        
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
            ProgrammingLanguage = scriptUpdated.ProgrammingLanguage,
            CreatorName = scriptUpdated.CreatorName

        };
    }
    
    public async Task<ScriptBlobResponseDto?> GetScriptBlobByScriptId(int scriptId, int userId)
    {
        //Check with token if scriptInBdd.UserId == token.UserId
        var script = await scriptRepository.GetScriptById(scriptId);
        if (script == null)
        {
            return null;
        }
        var latestScriptVersion = await scriptVersionRepository.GetLatestScriptVersionByScriptId(scriptId);
        if (latestScriptVersion == null)
        {
            return null;
        }
        
        string scriptNameWithExtension = script.ScriptName + (script.ProgrammingLanguage == "Python" ? ".py" : ".cs");
        
        return new ScriptBlobResponseDto
        {
            Blob = latestScriptVersion.SourceScriptBinary,
            FileName = scriptNameWithExtension
        };
    }
    public async Task DeleteScriptAndVersions(int scriptId)
    {
        await scriptVersionRepository.DeleteScriptVersionsByScriptId(scriptId);
        await scriptRepository.DeleteScript(scriptId);
    }
    
    public async Task DeleteScriptVersionById(int scriptVersionId)
    {
        await scriptVersionRepository.DeleteScriptVersion(scriptVersionId);
    }
    
    public async Task<List<ScriptVersionResponseDto>> GetScriptVersionsByScriptId(int scriptId)
    {
        var scriptVersions = await scriptVersionRepository.GetScriptVersionsByScriptId(scriptId);
        if (scriptVersions.Count == 0)
        {
            return new List<ScriptVersionResponseDto>();
        }
        return scriptVersions.Select(scriptVersion => new ScriptVersionResponseDto
        {
            ScriptVersionId = scriptVersion.ScriptVersionId,
            ScriptId = scriptVersion.ScriptId,
            VersionNumber = scriptVersion.VersionNumber,
            CreationDate = scriptVersion.CreationDate,
            CreatorUserId = scriptVersion.CreatorUserId,
            SourceScriptLink = scriptVersion.SourceScriptLink
        }).ToList();
    }
    
    public async Task<ScriptVersionBlobResponseDto?> GetScriptVersionBlobById(int scriptVersionId)
    {
        var scriptVersion = await scriptVersionRepository.GetScriptVersionById(scriptVersionId);
        if (scriptVersion == null)
        {
            return null;
        }
        var script = await scriptRepository.GetScriptById(scriptVersion.ScriptId);
        if (script == null)
        {
            return null;
        }
        return new ScriptVersionBlobResponseDto
        {
            Blob = scriptVersion.SourceScriptBinary,
            FileName = script.ScriptName + $"_v{scriptVersion.VersionNumber}_" + (scriptVersion.CreationDate.ToString("yyyyMMddHHmmss")) + (script.ProgrammingLanguage == "Python" ? ".py" : ".cs")
        };
    }
    
    public async Task<List<ScriptResponseDto>> GetScriptsByKeyWord(string keyword)
    {
        var scripts = await scriptRepository.GetScriptsByKeyWord(keyword);
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
            ProgrammingLanguage = script.ProgrammingLanguage,
            Visibility = script.Visibility,
            CreatorName = script.CreatorName
        }).ToList();
    }
}