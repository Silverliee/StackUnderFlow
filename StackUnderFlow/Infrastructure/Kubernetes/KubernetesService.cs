using k8s;
using k8s.Models;
using StackUnderFlow.Domains.Services;
using StackUnderFlow.Infrastructure.Kubernetes.Config;
using StackUnderFlow.Infrastructure.Kubernetes.Factory;

namespace StackUnderFlow.Infrastructure.Kubernetes;

public class KubernetesService(INotificationService notificationService)
{
    private readonly IKubernetes _client = KubernetesConfig.Client;

    public async Task<string> ExecutePythonScript(
        string namespaceName,
        string scriptContent
    )
    {
        var pod = await _client.CoreV1.CreateNamespacedPodAsync(
            SimpleScriptExecPythonSlimPodFactory.CreatePod(scriptContent), namespaceName);
        await Task.Delay(3000);
        await WatchPodCompletionAsync(namespaceName, pod.Metadata.Name);
        var logs = await GetPodLogsAsync(namespaceName, pod.Metadata.Name);
        if (logs == null)
        {
            throw new Exception(
                $"Failed to get logs for pod {pod.Metadata.Name} in namespace {namespaceName}"
            );
        }
        await _client.CoreV1.DeleteNamespacedPodAsync(pod.Metadata.Name, namespaceName, new V1DeleteOptions());
        return logs;
    }

    public async Task<string> ExecuteCsharpScript(
        string namespaceName,
        string scriptContent
    )
    {
        var pod = await _client.CoreV1.CreateNamespacedPodAsync(
            SimpleScriptExecCsharpPodFactory.CreatePod(scriptContent), namespaceName);
        await Task.Delay(3000);
        await WatchPodCompletionAsync(namespaceName, pod.Metadata.Name);
        var logs = await GetPodLogsAsync(namespaceName, pod.Metadata.Name);
        if (logs == null)
        {
            throw new Exception(
                $"Failed to get logs for pod {pod.Metadata.Name} in namespace {namespaceName}"
            );
        }

        await _client.CoreV1.DeleteNamespacedPodAsync(pod.Metadata.Name, namespaceName, new V1DeleteOptions());
        return logs;
    }

    public async Task<string> ExecutePythonScriptWithInput(string namespaceName, string script, string inputFileBinary,
        string inputType, string outputType, string pipelineRequestPipelineId)
    {
        await notificationService.SendMessageAsync(pipelineRequestPipelineId, "Python script detected...");
        var pod = PipelinePythonSlimPodFactory.CreatePod(script, inputFileBinary, inputType, outputType);
        await notificationService.SendMessageAsync(pipelineRequestPipelineId, "Finding a available worker...");
        await _client.CoreV1.CreateNamespacedPodAsync(pod, namespaceName);
        await notificationService.SendMessageAsync(pipelineRequestPipelineId, $"Worker {pod.Metadata.Name} ready, executing script...");
        await Task.Delay(3000);
        await WatchPodCompletionAsync(namespaceName, pod.Metadata.Name);
        var logs = await GetPodLogsAsync(namespaceName, pod.Metadata.Name);
        if (logs == null)
        {
            await notificationService.SendMessageAsync(pipelineRequestPipelineId, "Failed to get logs from worker.");
            throw new Exception(
                $"Failed to get logs for pod {pod.Metadata.Name} in namespace {namespaceName}"
            );
        }
        await _client.CoreV1.DeleteNamespacedPodAsync(pod.Metadata.Name, namespaceName, new V1DeleteOptions());
        return logs;
    }

    public async Task<string> ExecuteCsharpScriptWithInput(string namespaceName, string script, string inputFilePath,
        string inputType, string outputType, string pipelineRequestPipelineId)
    {
        var pod = PipelineCsharpPodFactory.CreatePod(script, inputFilePath, inputType, outputType);
        await _client.CoreV1.CreateNamespacedPodAsync(pod, namespaceName);
        await Task.Delay(3000);
        await WatchPodCompletionAsync(namespaceName, pod.Metadata.Name);
        var logs = await GetPodLogsAsync(namespaceName, pod.Metadata.Name);
        if (logs == null)
        {
            throw new Exception(
                $"Failed to get logs for pod {pod.Metadata.Name} in namespace {namespaceName}"
            );
        }
        await _client.CoreV1.DeleteNamespacedPodAsync(pod.Metadata.Name, namespaceName, new V1DeleteOptions());
        return logs;
    }

    private async Task<string> GetPodLogsAsync(string namespaceName, string podName)
    {
        await using var logs = await _client.CoreV1.ReadNamespacedPodLogAsync(
            podName,
            namespaceName
        );
        using var reader = new StreamReader(logs);
        var result = await reader.ReadToEndAsync();
        if (string.IsNullOrEmpty(result))
        {
            throw new Exception(
                $"Failed to get logs for pod {podName} in namespace {namespaceName}"
            );
        }

        return result;
    }

    private async Task WatchPodCompletionAsync(string namespaceName, string podName)
    {
        var podlistResp = _client.CoreV1.ListNamespacedPodWithHttpMessagesAsync(namespaceName, watch: true);
        await foreach (var (type, item) in podlistResp.WatchAsync<V1Pod, V1PodList>())
        {
            Console.WriteLine("==on watch event==");
            Console.WriteLine(item.Metadata.Name);
            Console.WriteLine(item.Status.Phase);
            Console.WriteLine("==on watch event==");
            if (item.Metadata.Name == podName && item.Status.Phase is "Succeeded" or "Failed")
            {
                break;
            }
        }
    }
}