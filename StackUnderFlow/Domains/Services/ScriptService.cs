using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class ScriptService(
    IScriptRepository scriptRepository,
    IUserRepository userRepository,
    IScriptVersionRepository scriptVersionRepository,
    IFriendRepository friendRepository,
    IGroupRepository groupRepository
) : IScriptService
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
        return scripts
            .Select(script => new ScriptResponseDto
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
            })
            .ToList();
    }

    public async Task<ScriptVersionResponseDto?> AddScriptVersion(
        ScriptVersionUploadRequestDto? scriptVersionUploadRequestDto
    )
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

    public async Task<ScriptResponseDto?> AddScript(ScriptUploadRequestDto? scriptUploadRequestDto)
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

    public async Task<ScriptResponseDto?> UpdateScript(
        ScriptUpdateRequestDto? scriptUpdateRequestDto
    )
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

    public async Task<ScriptFileResponseDto?> GetScriptFileByScriptId(int scriptId)
    {
        //Check with token if scriptInBdd.UserId == token.UserId
        var script = await scriptRepository.GetScriptById(scriptId);
        if (script == null)
        {
            return null;
        }
        var latestScriptVersion = await scriptVersionRepository.GetLatestScriptVersionByScriptId(
            scriptId
        );
        if (latestScriptVersion == null)
        {
            return null;
        }

        var scriptNameWithExtension =
            script.ScriptName + (script.ProgrammingLanguage == "Python" ? ".py" : ".cs");

        return new ScriptFileResponseDto
        {
            File = latestScriptVersion.SourceScriptBinary,
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
        return scriptVersions
            .Select(scriptVersion => new ScriptVersionResponseDto
            {
                ScriptVersionId = scriptVersion.ScriptVersionId,
                ScriptId = scriptVersion.ScriptId,
                VersionNumber = scriptVersion.VersionNumber,
                CreationDate = scriptVersion.CreationDate,
                CreatorUserId = scriptVersion.CreatorUserId,
                SourceScriptLink = scriptVersion.SourceScriptLink
            })
            .ToList();
    }

    public async Task<ScriptVersionFileResponseDto?> GetScriptVersionFileById(int scriptVersionId)
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
        return new ScriptVersionFileResponseDto
        {
            File = scriptVersion.SourceScriptBinary,
            FileName =
                script.ScriptName
                + $"_v{scriptVersion.VersionNumber}_"
                + (scriptVersion.CreationDate.ToString("yyyyMMddHHmmss"))
                + (script.ProgrammingLanguage == "Python" ? ".py" : ".cs")
        };
    }

    public async Task<List<ScriptResponseDto>> GetScriptsByKeyWord(string keyword)
    {
        var scripts = await scriptRepository.GetScriptsByKeyWord(keyword);
        if (scripts.Count == 0)
        {
            return new List<ScriptResponseDto>();
        }
        return scripts
            .Select(script => new ScriptResponseDto
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
            })
            .ToList();
    }

    public async Task<List<ScriptResponseDto>> GetScriptsByUserIdAndVisibility(int userId, ScriptRequestForOtherUserDto scriptRequest)
    {
        var visibility = scriptRequest.Visibility;
        var creatorId = scriptRequest.UserId;
        var scripts = new List<ScriptResponseDto>();
        try
        {
            
        switch (visibility)
        {
            case "Public":
                var  r = await scriptRepository.GetScriptsByUserId(creatorId);
                r.ForEach((script) =>
                {
                    if (script.Visibility == "Public")
                    {
                        scripts.Add(new ScriptResponseDto
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
                        });
                    }
                });
                break;
            case "Private":
                //never provide private scripts
                break;
            case "Friend":
                //check if userId is friend with creatorId
                var friendRequest = await friendRepository.GetFriendRequest(userId, creatorId);
                if (friendRequest is { Status: "Accepted" })
                {
                    var r2 = await scriptRepository.GetScriptsByUserId(creatorId);
                    r2.ForEach((script) =>
                    {
                        if (script.Visibility is "Friend" or "Public")
                        {
                            scripts.Add(new ScriptResponseDto
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
                            });
                        }
                    });
                }
                break;
            case "Group":
                //check if userId is in the same group as creatorId
                var groupId = scriptRequest.GroupId;
                var group = await groupRepository.GetGroupById(groupId);
                if (group == null)
                {
                    break;
                }
                var groupRequest = await groupRepository.GetGroupMembers(groupId);
                if (groupRequest != null && groupRequest.Any(x => x.UserId == userId) && groupRequest.Any(x => x.UserId == creatorId))
                {
                    var r3 = await scriptRepository.GetScriptsByUserId(creatorId);
                    r3.ForEach((script) =>
                    {
                        if (script.Visibility is "Group" or "Public")
                        {
                            scripts.Add(new ScriptResponseDto
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
                            });
                        }
                    });
                }
                break;
        }
        } catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return scripts;
    }
}
