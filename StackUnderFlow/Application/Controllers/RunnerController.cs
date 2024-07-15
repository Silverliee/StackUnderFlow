using System.Collections.Concurrent;
using System.Net.WebSockets;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Domains.Websocket;
using StackUnderFlow.Infrastructure.Kubernetes;

namespace StackUnderFlow.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    public class RunnerController(
        PipelineService pipelineService,
        KubernetesService kubernetesService,
        Bugsnag.IClient bugsnag
    ) : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, WebSocket> Sockets = new();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ExecuteSingleScript(IFormFile? script)
        {
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("Executing script"));
                if (script == null || script.Length == 0)
                    return BadRequest("No script file uploaded");

                return Ok(await ExecuteSingleScriptInternal(script));
            }
            catch (Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("execute-pipeline")]
        public IActionResult ExecutePipeline(List<IFormFile> scripts)
        {
            var pipelineId = Guid.NewGuid().ToString();
            var task = Task.Run(() => pipelineService.ExecutePipelineAsync(pipelineId, scripts));
            return Ok(task.Result);
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
            string output;
            if (
                Path.GetExtension(script!.FileName)
                .Equals(".py", StringComparison.OrdinalIgnoreCase)
            )
            {
                using var reader = new StreamReader(script.OpenReadStream());
                var pythonScript = await reader.ReadToEndAsync();
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
                using var reader = new StreamReader(script.OpenReadStream());
                var csharpScript = await reader.ReadToEndAsync();
                output = await kubernetesService.ExecuteCsharpScript(
                    "default",
                    csharpScript,
                    _ => { }
                );
            }
            else
            {
                return "Invalid script file extension";
            }

            return output;
        }
    }
}
