using k8s;
using k8s.Models;

namespace StackUnderFlow.Infrastructure.Kubernetes;

public class KubernetesService
{
    private readonly IKubernetes _client = new k8s.Kubernetes(
        KubernetesClientConfiguration.BuildConfigFromConfigFile("Infrastructure/Kubernetes/kubeconfig/config"));

    public async Task<string> CreatePythonJob(string namespaceName, string scriptContent)
    {
        var jobName = Guid.NewGuid().ToString();
        var job = new V1Job
        {
            ApiVersion = "batch/v1",
            Kind = "Job",
            Metadata = new V1ObjectMeta { Name = jobName },
            Spec = new V1JobSpec
            {
                Template = new V1PodTemplateSpec
                {
                    Spec = new V1PodSpec
                    {
                        Containers = new List<V1Container>
                        {
                            new()
                            {
                                Name = "python-container" + Guid.NewGuid(),
                                Image = "python:3.9",
                                Command = new List<string>
                                {
                                    "bash",
                                    "-c",
                                    $"echo '{scriptContent}' | python"
                                }
                            }
                        },
                        RestartPolicy = "Never"
                    }
                }
            }
        };

        await _client.BatchV1.CreateNamespacedJobAsync(job, namespaceName);
        
        //wait for job to complete here
        await Task.Delay(5000);
        
        var podName = await GetJobPodNameAsync(namespaceName, jobName);
        var logs = await GetPodLogsAsync(namespaceName, podName!);
        DeleteJob(namespaceName, jobName);
        return logs;
    }

    public async Task<string> CreateCSharpJob(string namespaceName, string scriptContent)
    {
        var jobName = Guid.NewGuid().ToString();
        var job = new V1Job
        {
            ApiVersion = "batch/v1",
            Kind = "Job",
            Metadata = new V1ObjectMeta { Name = jobName },
            Spec = new V1JobSpec
            {
                Template = new V1PodTemplateSpec
                {
                    Spec = new V1PodSpec
                    {
                        Containers = new List<V1Container>
                        {
                            new()
                            {
                                Name = "csharp-container" + Guid.NewGuid(),
                                Image = "mcr.microsoft.com/dotnet/sdk:8.0",
                                Command = new List<string>
                                {
                                    "bash", "-c",
                                    $"echo '{scriptContent}' > script.cs && mcs script.cs && mono script.exe"
                                },
                            }
                        },
                        RestartPolicy = "Never",
                    }
                }
            }
        };

        await _client.BatchV1.CreateNamespacedJobAsync(job, namespaceName);
        await Task.Delay(5000);
        var podName = await GetJobPodNameAsync(namespaceName, jobName);
        var logs = await GetPodLogsAsync(namespaceName, podName!);
        DeleteJob(namespaceName, jobName);
        return logs;
    }

    private async void DeleteJob(string namespaceName, string jobName)
    {
        await _client.BatchV1.DeleteNamespacedJobAsync(jobName, namespaceName, new V1DeleteOptions
        {
            PropagationPolicy = "Foreground"
        });
    }

    private async Task<string?> GetJobPodNameAsync(string namespaceName, string jobName)
    {
        var pods = await _client.CoreV1.ListNamespacedPodAsync(namespaceName, labelSelector: $"job-name={jobName}");
        return pods.Items.FirstOrDefault()?.Metadata.Name;
    }

    private async Task<string> GetPodLogsAsync(string namespaceName, string podName)
    {
        await using var logs = await _client.CoreV1.ReadNamespacedPodLogAsync(podName, namespaceName);
        using var reader = new StreamReader(logs);
        var result = await reader.ReadToEndAsync();
        return result;
    }
}