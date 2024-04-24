using System.Net;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Adapters;
using StackUnderFlow.Model;
using StackUnderFlow.Service;

namespace StackUnderFlow.Controllers;

[ApiController]
[Route("[controller]")]
public class PocController(ICodeExecutionService codeExecutionService) : ControllerBase
{
    [HttpPost(Name = "execute")]
    public HttpStatusCode ExecuteCode(IFormFile file)
    {
        var extension = file.FileName.Split(".").Last();
        try
        {
            var fileType = FileExtensionAdapter.DetermineFileType(extension);
            switch (fileType)
            {
                case AcceptedLanguage.Python :
                    codeExecutionService.ExecuteForPython(file);
                    break;
               case AcceptedLanguage.Csharp :
                    codeExecutionService.ExecuteForCsharp(file);
                   break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch(Exception e)
        {
            return HttpStatusCode.BadRequest;
        }

        return HttpStatusCode.InternalServerError;
    }
}