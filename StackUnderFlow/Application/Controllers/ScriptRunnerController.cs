using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
public class ScriptRunnerController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadFile(
        IFormFile? script,
        IFormFile? textFile = null,
        [FromForm] string arguments = "")
    {
        if (script == null || script.Length == 0)
            return BadRequest("No script file uploaded");
        
        using (var ms = new MemoryStream())
        {
            script.CopyTo(ms);
            var fileBytes = ms.ToArray();
            var s = Convert.ToBase64String(fileBytes);
            Console.Write(s);
            
            var fileBytesBack = Convert.FromBase64String(s);
            var ms2 = new MemoryStream(fileBytesBack);
            
        }

        var scriptPath = Path.Combine(Path.GetTempPath(), script.FileName);
        await using (var stream = System.IO.File.Create(scriptPath))
        {
            await script.CopyToAsync(stream);
        }

        string? textFilePath = null;
        if (textFile != null)
        {
            textFilePath = Path.Combine(Path.GetTempPath(), textFile.FileName);
            await using var stream = System.IO.File.Create(textFilePath);
            await textFile.CopyToAsync(stream);
        }

        if (textFilePath == null) return Ok();
        var output = await ExecutePythonScriptInDocker(scriptPath, textFilePath, arguments);

        return Ok(new { output });
    }

    private async Task<string> ExecutePythonScriptInDocker(string scriptPath, string? textFilePath, string arguments)
    {
        var scriptFileName = Path.GetFileName(scriptPath);
        var tempFolder = Path.GetTempPath();
        const string containerName = "python-runner-container";

        var dockerArgs =
            $"run --rm -v {tempFolder}:/usr/src/app --name {containerName} python-runner python {scriptFileName}";

        // Ajouter le chemin du fichier texte comme argument si fourni
        if (!string.IsNullOrEmpty(textFilePath))
        {
            var filename = Path.GetFileName(textFilePath);
            dockerArgs += $" {filename}";
        }

        // Ajouter les arguments supplÃ©mentaires du formulaire
        if (!string.IsNullOrEmpty(arguments))
        {
            dockerArgs += $" {arguments}";
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = dockerArgs,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Error: {error}");
        }

        return output.Trim();
    }
}