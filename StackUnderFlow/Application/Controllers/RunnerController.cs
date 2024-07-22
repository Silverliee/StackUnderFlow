using Hangfire;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    public class RunnerController(
        IRunnerService runnerService,
        NotificationService notificationService,
        Bugsnag.IClient bugsnag
    ) : ControllerBase
    {
        
        [HttpPost("script")]
        //[Authorize]
        public async Task<IActionResult> RunScript(int scriptId)
        {
            if (scriptId <= 0)
            {
                return BadRequest("Invalid script ID.");
            }

            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine($"Executing script by id: {scriptId}"));
                return Ok(await runnerService.ExecuteScript(scriptId));
            }
            catch (Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to execute script.");
            }
        }
        
        [HttpPost("script/file")]
        //[Authorize]
        public async Task<IActionResult> RunScriptFile(IFormFile? script)
        {
            if (script == null || script.Length == 0)
            {
                return BadRequest("No script file uploaded or file is empty.");
            }

            var fileName = script.FileName;

            try
            {
                BackgroundJob.Enqueue(() =>  Console.WriteLine($"Executing script file: {fileName}"));
                return Ok(await runnerService.ExecuteScriptFile(script));
            }
            catch (Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to execute script file.");
            }
        }

        [HttpPost("pipeline")]
        //[Authorize]
        public async Task<IActionResult> RunPipeline([FromForm]PipelineRequestDto pipelineRequest)
        {
            if (pipelineRequest.ScriptIds.Any(scriptId => scriptId <= 0))
            {
                return BadRequest("Invalid script ID.");
            }
            
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine($"Executing pipeline with scripts: {string.Join(", ", pipelineRequest.ScriptIds)}"));
                await notificationService.SendMessageAsync(pipelineRequest.PipelineId, "Pipeline started.");
                await notificationService.SendMessageAsync(pipelineRequest.PipelineId, $"Executing pipeline with scripts: {string.Join(", ", pipelineRequest.ScriptIds)}");
                var files = await runnerService.ExecutePipelineWithIds(pipelineRequest.ScriptIds, pipelineRequest.Input,pipelineRequest.PipelineId);
                if (files.Count == 1)
                {
                    var file = files.First();
                    const string contentType = "APPLICATION/octet-stream";
                    HttpContext.Response.ContentType = contentType;
                    HttpContext.Response.Headers.Append("Content-Disposition", $"attachment; filename={file.FileName}");
                    await file.CopyToAsync(HttpContext.Response.Body);
                    return new EmptyResult();
                }
                using var memoryStream = new MemoryStream();
                using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipEntry = archive.CreateEntry(file.FileName, System.IO.Compression.CompressionLevel.Fastest);
                        await using var zipStream = zipEntry.Open();
                        await file.CopyToAsync(zipStream);
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                var zipName = $"output-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.zip";
                await notificationService.SendMessageAsync(pipelineRequest.PipelineId, "Pipeline completed.");
                return File(memoryStream.ToArray(), "APPLICATION/octet-stream", zipName);
            }
            catch (Exception e)
            {
                bugsnag.Notify(e);
                await notificationService.SendMessageAsync(pipelineRequest.PipelineId, "Pipeline failed.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to execute pipeline.");
            }
        }
        
    }
}
