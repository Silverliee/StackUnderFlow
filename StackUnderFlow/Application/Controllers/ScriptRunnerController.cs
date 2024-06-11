using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Infrastructure.Kubernetes;
using Microsoft.AspNetCore.Cors;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
[EnableCors("AllowAll")]
public class ScriptRunnerController() : ControllerBase
{
    private readonly KubernetesService _kubernetesService = new();
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ExecuteScript(
        IFormFile? script)
    {
        try
        {
            if (script == null || script.Length == 0)
                return BadRequest("No script file uploaded");

            var rootPath = Directory.GetCurrentDirectory();
            var output = "";

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

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(script.FileName);
            var scriptPath = Path.Combine(scriptDirectory, uniqueFileName);
            await using (var stream = System.IO.File.Create(scriptPath))
            {
                await script.CopyToAsync(stream);
            }
        
            if (Path.GetExtension(script.FileName).Equals(".py", StringComparison.OrdinalIgnoreCase))
            {
                var pythonScript = await System.IO.File.ReadAllTextAsync(Path.Combine(scriptDirectory, uniqueFileName));
                output = await _kubernetesService.CreatePythonJob("default", pythonScript);
            }
            else if (Path.GetExtension(script.FileName).Equals(".cs", StringComparison.OrdinalIgnoreCase))
            {
                var csharpScript = await System.IO.File.ReadAllTextAsync(Path.Combine(scriptDirectory, uniqueFileName));
                output = await _kubernetesService.CreateCSharpJob("default", csharpScript);
            }
        
            return Ok(output);
        }
        catch (Exception e)
        {
           return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        
    }


}