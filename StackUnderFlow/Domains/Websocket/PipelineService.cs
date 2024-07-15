using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using StackUnderFlow.Infrastructure.Kubernetes;

namespace StackUnderFlow.Domains.Websocket;

public class PipelineService(
    KubernetesService kubernetesService,
    ConcurrentDictionary<string, WebSocket> sockets
)
{
    public async Task<string> ExecutePipelineAsync(string pipelineId, List<IFormFile> scripts)
    {
        var result = "";
        foreach (var script in scripts)
        {
            var output = await ExecuteScriptAsync(script);
            result+= output;
            //NotifyClient(pipelineId, output);
        }

        return result;
        //NotifyClient(pipelineId, "Pipeline completed.");
    }

    private async Task<string> ExecuteScriptAsync(IFormFile script)
    {
        var rootPath = Directory.GetCurrentDirectory();
        var output = "";

        string scriptDirectory;
        if (Path.GetExtension(script.FileName).Equals(".py", StringComparison.OrdinalIgnoreCase))
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
        await using (var stream = File.Create(scriptPath))
        {
            await script.CopyToAsync(stream);
        }

        if (Path.GetExtension(script.FileName).Equals(".py", StringComparison.OrdinalIgnoreCase))
        {
            var pythonScript = await File.ReadAllTextAsync(
                Path.Combine(scriptDirectory, uniqueFileName)
            );
            output = await kubernetesService.ExecutePythonScript("default", pythonScript, _ => { });
        }
        else if (
            Path.GetExtension(script.FileName).Equals(".cs", StringComparison.OrdinalIgnoreCase)
        )
        {
            var csharpScript = await File.ReadAllTextAsync(
                Path.Combine(scriptDirectory, uniqueFileName)
            );
            output = await kubernetesService.ExecuteCsharpScript("default", csharpScript, _ => { });
        }

        File.Delete(scriptPath);

        return output;
    }

    private void NotifyClient(string pipelineId, string message)
    {
        if (!sockets.TryGetValue(pipelineId, out var socket))
            return;
        var buffer = Encoding.UTF8.GetBytes(message);
        socket.SendAsync(
            new ArraySegment<byte>(buffer),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None
        );
    }
}
