using System.Collections.Concurrent;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using StackUnderFlow.Domains.Websocket;
using StackUnderFlow.Infrastructure.Kubernetes;

namespace StackUnderFlow.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    public class RunnerController(
        PipelineService pipelineService,
        KubernetesService kubernetesService
    ) : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, WebSocket> Sockets = new();

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> ExecuteSingleScript(IFormFile? script)
        {
            try
            {
                if (script == null || script.Length == 0)
                    return BadRequest("No script file uploaded");

                return Ok(await ExecuteSingleScriptInternal(script));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("execute-pipeline")]
        //[Authorize]
        public IActionResult ExecutePipeline(List<IFormFile> scripts)
        {
            var pipelineId = Guid.NewGuid().ToString();
            Task.Run(() => pipelineService.ExecutePipelineAsync(pipelineId, scripts));
            return Ok(pipelineId);
        }

        [HttpGet("subscribe/{pipelineId}")]
        [Produces("application/json")]
        public async Task<IActionResult> Subscribe(string pipelineId)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
                return BadRequest("WebSocket connection expected.");
            var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            Sockets.TryAdd(pipelineId, socket);
            await Receive(socket);
            return new EmptyResult();
        }

        private static async Task Receive(WebSocket socket)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None
                );
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closed by the client",
                        CancellationToken.None
                    );
                }
            }
        }

        private async Task<string> ExecuteSingleScriptInternal(IFormFile? script)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var output = "";

            string scriptDirectory;
            if (
                Path.GetExtension(script!.FileName)
                    .Equals(".py", StringComparison.OrdinalIgnoreCase)
            )
            {
                scriptDirectory = Path.Combine(
                    rootPath,
                    "Infrastructure",
                    "Kubernetes",
                    "python-scripts"
                );
            }
            else if (
                Path.GetExtension(script.FileName).Equals(".cs", StringComparison.OrdinalIgnoreCase)
            )
            {
                scriptDirectory = Path.Combine(
                    rootPath,
                    "Infrastructure",
                    "Kubernetes",
                    "csharp-scripts"
                );
            }
            else
            {
                return "Invalid script file extension";
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

            if (
                Path.GetExtension(script.FileName).Equals(".py", StringComparison.OrdinalIgnoreCase)
            )
            {
                var pythonScript = await System.IO.File.ReadAllTextAsync(
                    Path.Combine(scriptDirectory, uniqueFileName)
                );
                output = await kubernetesService.ExecutePythonScript(
                    "default",
                    pythonScript,
                    _ => { }
                );
            }
            else if (
                Path.GetExtension(script.FileName).Equals(".cs", StringComparison.OrdinalIgnoreCase)
            )
            {
                var csharpScript = await System.IO.File.ReadAllTextAsync(
                    Path.Combine(scriptDirectory, uniqueFileName)
                );
                output = await kubernetesService.ExecuteCsharpScript(
                    "default",
                    csharpScript,
                    _ => { }
                );
            }

            System.IO.File.Delete(scriptPath);
            return output;
        }
    }
}
