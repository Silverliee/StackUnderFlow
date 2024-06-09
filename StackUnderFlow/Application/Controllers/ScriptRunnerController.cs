using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Infrastructure.Kubernetes;
using Microsoft.AspNetCore.Cors;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
[EnableCors("AllowAll")]
public class ScriptRunnerController : ControllerBase
{
    private readonly KubernetesService _kubernetesService = new();
    
    [HttpPost]
    public async Task<IActionResult> ExecuteScript(
        IFormFile? script)
    {
        if (script == null || script.Length == 0)
            return BadRequest("No script file uploaded");

        var rootPath = Directory.GetCurrentDirectory();

        string scriptDirectory;
        if (Path.GetExtension(script.FileName).Equals(".py", StringComparison.OrdinalIgnoreCase))
        {
            scriptDirectory = Path.Combine(rootPath, "Infrastructure", "Kubernetes", "python-scripts");
        }
        else if (Path.GetExtension(script.FileName).Equals(".cs", StringComparison.OrdinalIgnoreCase))
        {
            scriptDirectory = Path.Combine(rootPath, "Infrastructure", "Kubernetes", "csharp-scripts");
        }
        else
        {
            return BadRequest("Unsupported file extension");
        }
        
        if (!Directory.Exists(scriptDirectory))
        {
            Directory.CreateDirectory(scriptDirectory);
        }

        var scriptPath = Path.Combine(scriptDirectory, script.FileName);
        await using (var stream = System.IO.File.Create(scriptPath))
        {
            await script.CopyToAsync(stream);
        }

        _kubernetesService.CreatePythonJob("default", script.FileName);
        return Ok();
    }


}