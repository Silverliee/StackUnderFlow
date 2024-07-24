using System.Globalization;
using System.Security.Cryptography;
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
    IGroupRepository groupRepository,
    ILikeRepository likeRepository,
    IFavoriteRepository favoriteRepository
) : IScriptService
{
    public async Task<List<ScriptResponseDto>> GetScripts(int offset, int records, string visibility, int requesterId)
    {
        var scripts = await scriptRepository.GetScripts(offset, records,visibility);
        if (scripts.Count == 0)
        {
            return [];
        }

        var scriptResponseDtos = new List<ScriptResponseDto>();

        foreach (var script in scripts)
        {
            var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);
            var favorite = await favoriteRepository.GetFavoriteByScriptId(script.ScriptId);
            var isFavorite = favorite.Any(x => x.UserId == requesterId);

            var scriptResponseDto = new ScriptResponseDto
            {
                ScriptId = script.ScriptId,
                ScriptName = script.ScriptName,
                Description = script.Description,
                InputScriptType = script.InputScriptType,
                UserId = script.UserId,
                OutputScriptType = script.OutputScriptType,
                ProgrammingLanguage = script.ProgrammingLanguage,
                Visibility = script.Visibility,
                CreatorName = script.CreatorName,
                NumberOfLikes = numberOfLikes.Count,
                CreationDate = script.CreationDate.ToString(),
                IsLiked = numberOfLikes.Any(x => x.UserId == requesterId),
                IsFavorite = isFavorite
            };

            scriptResponseDtos.Add(scriptResponseDto);
        }

        return scriptResponseDtos;
    }
    
    public async Task<List<ScriptResponseDto>> GetScriptsAndFavoriteScripts(int requesterId)
    {
        var scripts = await scriptRepository.GetScriptsByUserId(requesterId);
        var favorites = await favoriteRepository.GetFavoriteByUserId(requesterId);
        var favoriteScripts = new List<Script?>();
        foreach (var favorite in favorites)
        {
            var script = await scriptRepository.GetScriptById(favorite.ScritpId);
            if (script != null)
            {
                favoriteScripts.Add(script);
            }
        }
        List<Script> mergedList = scripts
            .Concat(favoriteScripts)
            .OrderByDescending(script => script.CreationDate)
            .ToList();
        
        if (mergedList.Count == 0)
        {
            return [];
        }

        var scriptResponseDtos = new List<ScriptResponseDto>();

        foreach (var script in mergedList)
        {
            var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);
            var favorite = await favoriteRepository.GetFavoriteByScriptId(script.ScriptId);
            var isFavorite = favorite.Any(x => x.UserId == requesterId);

            var scriptResponseDto = new ScriptResponseDto
            {
                ScriptId = script.ScriptId,
                ScriptName = script.ScriptName,
                Description = script.Description,
                InputScriptType = script.InputScriptType,
                UserId = script.UserId,
                OutputScriptType = script.OutputScriptType,
                ProgrammingLanguage = script.ProgrammingLanguage,
                Visibility = script.Visibility,
                CreatorName = script.CreatorName,
                NumberOfLikes = numberOfLikes.Count,
                CreationDate = script.CreationDate.ToString(),
                IsLiked = numberOfLikes.Any(x => x.UserId == requesterId),
                IsFavorite = isFavorite
            };

            scriptResponseDtos.Add(scriptResponseDto);
        }

        return scriptResponseDtos;
    }
    
    public async Task<ScriptResponseDto?> GetScriptById(int scriptId, int requesterId)
    {
        var script = await scriptRepository.GetScriptById(scriptId);
        if (script == null)
        {
            return null;
        }
        var numberOfLikes = await likeRepository.GetLikesByScriptId(scriptId);
        var favorite = await favoriteRepository.GetFavoriteByScriptId(scriptId);
        var isFavorite = favorite.Any(x => x.UserId == requesterId);
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
            CreatorName = script.CreatorName,
            NumberOfLikes = numberOfLikes.Count,
            CreationDate = script.CreationDate.ToString(),
            IsLiked = numberOfLikes.Any(x => x.UserId == requesterId),
            IsFavorite = isFavorite
        };
    }

    public async Task<List<ScriptResponseDto>> GetScriptsByUserId(int userId, int requesterId)
    {
        var scripts = await scriptRepository.GetScriptsByUserId(userId);
        if (scripts.Count == 0)
        {
            return new List<ScriptResponseDto>();
        }
        var scriptResponseDtos = new List<ScriptResponseDto>();

        foreach (var script in scripts)
        {
            var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);
            var favorite = await favoriteRepository.GetFavoriteByScriptId(script.ScriptId);
            var isFavorite = favorite.Any(x => x.UserId == requesterId);

            var scriptResponseDto = new ScriptResponseDto
            {
                ScriptId = script.ScriptId,
                ScriptName = script.ScriptName,
                Description = script.Description,
                InputScriptType = script.InputScriptType,
                UserId = script.UserId,
                OutputScriptType = script.OutputScriptType,
                ProgrammingLanguage = script.ProgrammingLanguage,
                Visibility = script.Visibility,
                CreatorName = script.CreatorName,
                CreationDate = script.CreationDate.ToString(CultureInfo.CurrentCulture),
                NumberOfLikes = numberOfLikes.Count,
                IsLiked = numberOfLikes.Any(x => x.UserId == requesterId),
                IsFavorite = isFavorite
            };

            scriptResponseDtos.Add(scriptResponseDto);
        }

        return scriptResponseDtos;
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
            CreatorName = user.Username,
            CreationDate = DateTime.Now
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
        await likeRepository.CreateLike(new Like
        {
            UserId = script.UserId,
            ScriptId = script.ScriptId
        });
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
            CreatorName = user.Username,
            NumberOfLikes = 1
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
        var numberOfLikes = await likeRepository.GetLikesByScriptId(scriptUpdated.ScriptId);
        var favorite = await favoriteRepository.GetFavoriteByScriptId(scriptUpdated.ScriptId);
        var isFavorite = favorite.Any(x => x.UserId == scriptUpdated.UserId);
        
        return new ScriptResponseDto
        {
            ScriptId = scriptUpdated.ScriptId,
            ScriptName = scriptUpdated.ScriptName,
            Description = scriptUpdated.Description,
            InputScriptType = scriptUpdated.InputScriptType,
            UserId = scriptUpdated.UserId,
            OutputScriptType = scriptUpdated.OutputScriptType,
            ProgrammingLanguage = scriptUpdated.ProgrammingLanguage,
            CreatorName = scriptUpdated.CreatorName,
            NumberOfLikes = numberOfLikes.Count,
            IsLiked = numberOfLikes.Any(x => x.UserId == scriptUpdated.UserId),
            IsFavorite = isFavorite
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
            script.ScriptName + (script.ProgrammingLanguage == "Python" ? ".py" : ".csx");

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
                + (script.ProgrammingLanguage == "Python" ? ".py" : ".csx")
        };
    }

    public async Task<ListScriptsResponseDto> GetScriptsByKeyWord(string[] keywords, int requesterId, int offset, int records, string visibility, string language)
    {
        var result = await scriptRepository.GetScriptsByKeyWord(keywords, offset,records,visibility, language);
        if (result.totalCount == 0)
        {
            return new ListScriptsResponseDto
            {
                Scripts = new List<ScriptResponseDto>(),
                TotalCount = 0
            };
        }
        var listOfScripts = new List<ScriptResponseDto>();
        foreach (var script in result.scripts)
        {
            var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);
            var favorite = await favoriteRepository.GetFavoriteByScriptId(script.ScriptId);
            var isFavorite = favorite.Any(x => x.UserId == requesterId);
            listOfScripts.Add(new ScriptResponseDto
            {
                ScriptId = script.ScriptId,
                ScriptName = script.ScriptName,
                Description = script.Description,
                InputScriptType = script.InputScriptType,
                UserId = script.UserId,
                OutputScriptType = script.OutputScriptType,
                ProgrammingLanguage = script.ProgrammingLanguage,
                Visibility = script.Visibility,
                CreatorName = script.CreatorName,
                NumberOfLikes = numberOfLikes.Count,
                IsLiked = numberOfLikes.Any(x => x.UserId == requesterId),
                IsFavorite = isFavorite
            });
        }
        return new ListScriptsResponseDto
        {
            Scripts = listOfScripts,
            TotalCount = result.totalCount
        };
    }

    public async Task<List<ScriptResponseDto>> GetScriptsByUserIdAndVisibility(int userId, ScriptRequestForOtherUserDto scriptRequest)
    {
        var visibility = scriptRequest.Visibility;
        var creatorId = scriptRequest.UserId;
        var scripts = new List<ScriptResponseDto>();
        if (userId == scriptRequest.UserId)
        {
            visibility = "All";
        }
        try
        {
            
            switch (visibility)
            {
                case "Public":
                    var userScripts = await scriptRepository.GetScriptsByUserId(creatorId);
                    foreach (var script in userScripts)
                    {
                        if (script.Visibility != "Public") continue;
                        var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);
                        var favorite = await favoriteRepository.GetFavoriteByScriptId(script.ScriptId);
                        var isFavorite = favorite.Any(x => x.UserId == userId);
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
                            CreatorName = script.CreatorName,
                            CreationDate = script.CreationDate.ToString(),
                            NumberOfLikes = numberOfLikes.Count,
                            IsLiked = numberOfLikes.Any(x => x.UserId == userId),
                            IsFavorite = isFavorite
                        });
                    };
                    break;
                case "Private":
                    //never provide private scripts
                    break;
                case "Friend":
                    //check if userId is friend with creatorId
                    var friendRequest = await friendRepository.GetFriendRequest(userId, creatorId);
                    if (friendRequest is { Status: "Accepted" })
                    {
                        var friendScripts = await scriptRepository.GetScriptsByUserId(creatorId);
                        foreach (var script in friendScripts)
                        {
                            if (script.Visibility is not ("Friend" or "Public")) continue;
                            var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);
                            var favorite = await favoriteRepository.GetFavoriteByScriptId(script.ScriptId);
                            var isFavorite = favorite.Any(x => x.UserId == userId);
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
                                CreatorName = script.CreatorName,
                                CreationDate = script.CreationDate.ToString(),
                                NumberOfLikes = numberOfLikes.Count,
                                IsLiked = numberOfLikes.Any(x => x.UserId == userId),
                                IsFavorite = isFavorite
                            });
                        };
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
                    if (groupRequest.Any(x => x.UserId == userId) && groupRequest.Any(x => x.UserId == creatorId))
                    {
                        var groupScripts = await scriptRepository.GetScriptsByUserId(creatorId);
                        foreach (var script in groupScripts)
                        {
                            if (script.Visibility is not ("Group" or "Public")) continue;
                            var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);
                            var favorite = await favoriteRepository.GetFavoriteByScriptId(script.ScriptId);
                            var isFavorite = favorite.Any(x => x.UserId == userId);
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
                                CreatorName = script.CreatorName,
                                CreationDate = script.CreationDate.ToString(),
                                NumberOfLikes = numberOfLikes.Count,
                                IsLiked = numberOfLikes.Any(x => x.UserId == userId),
                                IsFavorite = isFavorite
                            });
                        };
                    }
                    break;
                default:
                    //Case when you are looking at your own scripts through the user profile (as for friends and groups)
                    var userScripts2 = await scriptRepository.GetScriptsByUserId(creatorId);
                    foreach (var script in userScripts2)
                    {
                        var numberOfLikes = await likeRepository.GetLikesByScriptId(script.ScriptId);
                        var favorite = await favoriteRepository.GetFavoriteByScriptId(script.ScriptId);
                        var isFavorite = favorite.Any(x => x.UserId == userId);
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
                            CreatorName = script.CreatorName,
                            NumberOfLikes = numberOfLikes.Count,
                            CreationDate = script.CreationDate.ToString(),
                            IsLiked = numberOfLikes.Any(x => x.UserId == userId),
                            IsFavorite = isFavorite
                        });
                    };
                    break;
            }
        } catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return scripts;
    }
}
