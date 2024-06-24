using k8s;
using k8s.KubeConfigModels;
using k8s.Models;

namespace StackUnderFlow.Infrastructure.Kubernetes;

public class KubernetesService
{
    private static readonly HttpClientHandler Handler = new()
    {
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    };

    private readonly HttpClient _httpClient = new(Handler);
    
    private readonly IKubernetes _client = new k8s.Kubernetes(
        KubernetesClientConfiguration.BuildConfigFromConfigObject(new K8SConfiguration
        {
            ApiVersion = "v1",
            Clusters = new[]
            {
                new Cluster
                {
                    Name = "HERMES",
                    ClusterEndpoint = new ClusterEndpoint
                    {
                        Server = "https://hermes-dns-ju5kbh8j.hcp.francecentral.azmk8s.io:443",
                        CertificateAuthorityData =
                            "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSUU2RENDQXRDZ0F3SUJBZ0lRVWtndXFPZzh6L3YrQlp2YkdtZXMzekFOQmdrcWhraUc5dzBCQVFzRkFEQU4KTVFzd0NRWURWUVFERXdKallUQWdGdzB5TkRBMk1qUXhNVE13TlRCYUdBOHlNRFUwTURZeU5ERXhOREExTUZvdwpEVEVMTUFrR0ExVUVBeE1DWTJFd2dnSWlNQTBHQ1NxR1NJYjNEUUVCQVFVQUE0SUNEd0F3Z2dJS0FvSUNBUUN3CkJNKzhKM2UvLzZZOUR0UjVEWmV4MGJmQVpGc1YvcmVMUU5MRENWcXRrb3ZFaUtqMWJ6bkU4cncxWUh6Vk10WkEKRm5neER3MjZpKy9kZm01a1dWS0ZheTFsSHJkcVJvNDNjTDV3cDJYQnpDVHp2cmxiVzQ4dE0vZUEwYlFSekJpWgpHdERjUTRRVzlvOWdwcmFSTzk0S1M1MXZyUjMzNHdLTHJIQUFKWnBlK0RQSXNVc203eU12SzFDQTAraXJNQ3ROCmREK2dhaFhOSEJkODFjdUk3RUNad21TSEdyV2N0Q2wrQWQ5bUJFbWZHQzU0VW1oT1V2eE9SVVhBUGJreitvdVAKWUI0M25pRGt4SENRY3ozZEZjV05HWVlaQ0RQZ2tyZUNzMjg2UitMY3FRMVVGQWxUZXkzb2VrNHNJallib2dZTgpETlpjSUhDK0d3U1VyYWRKckRScm1xbmlMTlFNUWwzM1ZzZkluTHZRKzh0dVNqUFZOWWlOcHY0ZXU5SDM1NmJOClZ6TDc3bzdnbmJNOUhQL1dnQW5TSGVhTmtrdVJNa1ArbUh4Q2Z5NkpUdjdGeXpoeXd2bmwzZy9zTjUrd3VveGQKaUlsdjlvKzBLOGFhSExuamlmeFpBSjlhb0lnWXltdGVuNFZBOUhmOU8yb0JRT2IwT0ZuWWRzb3hxTXRtR1E3LwpsbDUyQWVJazdEbW80YU9JTzRpaGtaS0Rpd2cySlN1UEVaL1hZUlk2RVk3anNheGxLbFl3RTlKTElzUHNzUGJyCmRRVWNSQ2I0RzMzWWxBOU9wRzlRNzJDU3dTbDNwMjF0M1JiMXI1dkN1NE9nRGVrdXRtVGNhN3R6Y0M4MWZYMm4KeTY5VUl2WE5ReXlFaHY5NXNhUDVKb21YcThCaG45TUlwdGViWG9zeFJ3SURBUUFCbzBJd1FEQU9CZ05WSFE4QgpBZjhFQkFNQ0FxUXdEd1lEVlIwVEFRSC9CQVV3QXdFQi96QWRCZ05WSFE0RUZnUVVuS2xnUnA0cWtlTjBCN3EvCmV4aExhZEF4VnBnd0RRWUpLb1pJaHZjTkFRRUxCUUFEZ2dJQkFGakY5dkJ3QnpHZUp4OUlTNWk4ZG1KeW5LeUUKREFVc3Fhb3FXSGxBVVlxS2Z5c3BHN2NXK1diOVBRZVRnZWZkbGtySng4SXZlNGxCWHYxUFY4S2F0WWpZeCtxUApDUy9zR1JWNmtqd01mR29xTDA0dVp1Ti9UTXMwYk5EeGtuOFExNHB5NjhkMmE5cFhtNytnK3hzMmtPdld1emxrCmVVOXpSSGlqM3RrZy82bGp6MGNIREhSbjB6dmtIWlZRbDJ3eXplQmFrZWpydDdTMjlYZTNNMi94bWJSL2c1eC8KaEF6VlpaWkFLOGt3UTkyMnJBb1NTdkRsaUM5WjMzRzJ5QXZtUnVabFc1WGM2Qmh5WDJUaVM1ZTM5a29PYSsrLwo0TEFRNHUvOGlZYURVaVVmS3FnZjdLVURpbkRIMWlldWJkeG1sZEx5a05GalY5M2hlSFhnRW1sODQzbEU0dkdOCk5YRzgyVTFKNWlPa29YckM5NmpyU3Vma2RlcVUwTjdodTI2V0x1QVAyWDJCcjgzSVp1SWUzNVlEZG1GcWtaeUgKMWkwSzZGY1NXbmhPOCtnM3Y0ZGdoL2lUMUZwZWhPTjRsaU4zZmlUdlB4d2ZCb3RFeU9qSHFWZXRkNlRPaE1VSgpOR1RLaWJ2enlTWmxmR0tIS2ZoTFI5a3hwZHhJdThwejV2ZkRrVm9KN2R3RUx5ckg0Ujc1TW93ekV2TndxTnVpCjdmSWlOZHIvMzV1UGEraHRYcThIWDNpZjIzTm9XR2VIUjE5STdudC8wRm9oUndNK2Q1QzZ3TFcvS2hjR2tPRkwKYU5OSDVkYjZDb2xnZ0V5dWg5dkUvNnh1VkYxTngyMG9SdkpkVkU4a1gxN1VhTlpra09zN2xXK0JxdVhXb3B6SAowMHQyRStoR0tLaTFyNkdDCi0tLS0tRU5EIENFUlRJRklDQVRFLS0tLS0tCg=="
                    }
                }
            },
            Contexts = new[]
            {
                new Context
                {
                    Name = "HERMES-admin",
                    ContextDetails = new ContextDetails
                    {
                        Cluster = "HERMES",
                        User = "clusterAdmin_StackUnderFlow_HERMES"
                    }
                }
            },
            CurrentContext = "HERMES-admin",
            Kind = "Config",
            Users = new[]
            {
                new User
                {
                    Name = "clusterAdmin_StackUnderFlow_HERMES",
                    UserCredentials = new UserCredentials
                    { Token = "pnjz0mr8mb3o7h3iv596zh3n0vrs91j7msrr3818jridlri6bd0u4yxbgzr3m74erabyahaqu30nddxhgacaw2iqcwfheq6exp3wougrw9n8qrypr1ivb6l2mmk7v5fm" }
                }
            }
        })
    );

    public async Task<string> ExecutePythonScript(
        string namespaceName,
        string scriptContent,
        Action<string> notifyCallback
    )
    {
        var jobName = Guid.NewGuid().ToString();
        try
        {
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

            notifyCallback("Creating job...");
            var createdJob = await _client.BatchV1.CreateNamespacedJobAsync(job, namespaceName);
            if (createdJob == null)
            {
                throw new Exception(
                    $"Failed to create job with name {jobName} in namespace {namespaceName}"
                );
            }

            await Task.Delay(5000);

            notifyCallback("Job created. Retrieving pod name...");
            var podName = await GetJobPodNameAsync(namespaceName, jobName);
            if (podName == null)
            {
                throw new Exception(
                    $"Failed to get pod name for job {jobName} in namespace {namespaceName}"
                );
            }

            notifyCallback("Retrieving logs...");
            var logs = await GetPodLogsAsync(namespaceName, podName);
            if (logs == null)
            {
                throw new Exception(
                    $"Failed to get logs for pod {podName} in namespace {namespaceName}"
                );
            }

            notifyCallback("Deleting job...");
            DeleteJob(namespaceName, jobName);
            return logs;
        }
        catch (Exception ex)
        {
            throw new Exception($"exception: {ex.Message}");
        }
    }

    public async Task<string> ExecuteCsharpScript(
        string namespaceName,
        string scriptContent,
        Action<string> notifyCallback
    )
    {
        var jobName = Guid.NewGuid().ToString();
        try
        {
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
                                        "bash",
                                        "-c",
                                        $"echo '{scriptContent}' > script.cs && mcs script.cs && mono script.exe"
                                    }
                                }
                            },
                            RestartPolicy = "Never",
                        }
                    }
                }
            };

            notifyCallback("Creating job...");
            var createdJob = await _client.BatchV1.CreateNamespacedJobAsync(job, namespaceName);
            if (createdJob == null)
            {
                throw new Exception(
                    $"Failed to create job with name {jobName} in namespace {namespaceName}"
                );
            }

            await Task.Delay(5000);

            notifyCallback("Job created. Retrieving pod name...");
            var podName = await GetJobPodNameAsync(namespaceName, jobName);
            if (podName == null)
            {
                throw new Exception(
                    $"Failed to get pod name for job {jobName} in namespace {namespaceName}"
                );
            }

            notifyCallback("Retrieving logs...");
            var logs = await GetPodLogsAsync(namespaceName, podName);
            if (logs == null)
            {
                throw new Exception(
                    $"Failed to get logs for pod {podName} in namespace {namespaceName}"
                );
            }

            notifyCallback("Deleting job...");
            DeleteJob(namespaceName, jobName);
            return logs;
        }
        catch (Exception ex)
        {
            throw new Exception($"exception: {ex.Message}");
        }
    }

    private async void DeleteJob(string namespaceName, string jobName)
    {
        var deletedJob = await _client.BatchV1.DeleteNamespacedJobAsync(
            jobName,
            namespaceName,
            new V1DeleteOptions { PropagationPolicy = "Foreground" }
        );
        if (deletedJob == null)
        {
            throw new Exception(
                $"Failed to delete job with name {jobName} in namespace {namespaceName}"
            );
        }
    }

    private async Task<string?> GetJobPodNameAsync(string namespaceName, string jobName)
    {
        var pods = await _client.CoreV1.ListNamespacedPodAsync(
            namespaceName,
            labelSelector: $"job-name={jobName}"
        );
        return pods.Items.FirstOrDefault()?.Metadata.Name;
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
}
