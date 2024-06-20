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
                        Server = "https://hermes-dns-z7fio5j8.hcp.uksouth.azmk8s.io:443",
                        CertificateAuthorityData =
                            "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSUU2VENDQXRHZ0F3SUJBZ0lSQU1VUWVXZ2VxSHVUWWpJRUthTFBWdll3RFFZSktvWklodmNOQVFFTEJRQXcKRFRFTE1Ba0dBMVVFQXhNQ1kyRXdJQmNOTWpRd05qRTBNRGN6TnpRMVdoZ1BNakExTkRBMk1UUXdOelEzTkRWYQpNQTB4Q3pBSkJnTlZCQU1UQW1OaE1JSUNJakFOQmdrcWhraUc5dzBCQVFFRkFBT0NBZzhBTUlJQ0NnS0NBZ0VBCjF1TUJTbHRsSmdnUEtJKzYvNkhCZHVWdmVSc0VnZ0FyTG9JcVdpMnBKQk04N0ZRU1hzUWE1SWJENVJ1VTNBbUEKYTgxLzVOb2JYczZadk9tbXM5aXdTV0NKNlZqQXR6emtnbmRlOGd2bTJCZDk1L3JOSndUTlB3dE9OVmdDVW9vNApzN084OU1Da2dHZmxZaUZvSHVSR1FPTXNqcHI4Si9iSTA3T1dnZWp3YzlqUHJMYVJXZ2V2TXZyRU1kaHh4UlFQCkxzazdaSkpNOFpoY1VLQWYxOFdpd3FHbml3SGpWOS94RE5yUnFFUUlvUUVldFRBYnZNYmJOVld0UWpmQ0JadDQKU08rSllxbk5sZWw0YXFSSnNmcHI0UFRJa3FiUzdSNEZIMHJhN1J0dWZoYzFXd3RTWGs0bURLeUhzd1hFZk96WgpqZVVUbUxKOW8rakwyTGpGbVZ6SWtma0RjLytJZVVndWFRQTUrYTFlTm43WGpVVkxmNmRaQnNDeXBZcDhPK3B1CmhZYzhCRmMrK3NoUE54bnV5WlhiQ25KbTRXSTc5azBOYlJVMmRGWFJvR2p2czhFaisyRUIvSGhtVGxrMnJrek4Kemxic0NlS24zcHQwSlRrSEcyWnF5Zkc3VGEycHQ3T1NnSXYyQTVzZC9rd2ZuMkVGWVVsbUlTMFZwU3dNampLKwozMWUwZ00wR2hxNnVwUDdlT1ZyamRXaDF3RlFxTFJWUUkwd0g4VE4ybUR0aUZwWnZZdVM3LzlFei90VUJ4RzRNCm5laVp6QUN1RXIrTW1zNUE3ZVNOU3gvTTF0MjE2dlAveVZYSjFVdFE4ZDlzV2pGK0hEUmxBbTRzWHNISE0xSDIKUWR3SDNTNTFONDdMM09za1YyRHJtNVozQkRic1V2SlBkSG1NY0xxNWg4Y0NBd0VBQWFOQ01FQXdEZ1lEVlIwUApBUUgvQkFRREFnS2tNQThHQTFVZEV3RUIvd1FGTUFNQkFmOHdIUVlEVlIwT0JCWUVGSWZVbXpQcGdld0gvSXRpCmEwaXpRSlQrNFdTNU1BMEdDU3FHU0liM0RRRUJDd1VBQTRJQ0FRQmdpQUhKb2FMaU56WEVVRzByaFM2MUtPdTQKMlN4TEl6TTVTTmJoQW1ycVhPK3BKMGVHeUVyVkt0VjFkL3NRWHNoWDNsSXFQb0RnL3o2aGRiLzNJZFRybWxrUAovZVRyRGJkLzd0QjNxQk5RQk1la2x3eEpzZEdVUnBjQUd1VkV5TkRqOGFoaVFkOVB1Ykg3MDRkOVFiWCtQNzdiCk5SVkRLSDdmckYxTEQ1RXFGcjhMZXU3bExRRnBra0IxVzZvUmtUOXBKUGhGR1Y4NjhFNVQ2RWdKditPV2tYT08KNTFwWmNBblp6bWpoMkhVQ0xCdHBMUThpVlN2cjRlcDdzUy8zMUlBay90aWtsVGk4L1lsZjYrZzRaSThEV2VobwpyRnhiRm5ua0hWT2ZDUDdUVkYrMVVjVjNBNTQwWm5vN1g4ZjVTQllaWGppNWtseE5kUEZuazhWdlVMcGttYWl2CjJHT29RREJicmVodFRpaVRIdWVmYU95a3hyQTVmdWRBQzd3MllnTG1hV0JzYnViQy9GazdHZ1pOZWdnMzFYNTcKRVFTSExJT3FhWkUxNi8vUndOYllTdVlOaVdWTnFIOGFXK3NnZ1VPVGtFTHNhZzZRbzRIZUd3TnhMVkJRaERORQpVbWtUM2o4ak5NWmw3MTlGKzFVamhTTzlENXl6bnhWK3BnYUcwMUZneG43RkNySWVSVHhYclRUV2VETExrZGFSCjRiRzh4VWk5WjI0NFB0WFozcmVMczVNNUZTelRGYXF1WjNzYTI2WU4rWlA5dHhva3pNVXRxaGFVeXMrS1Y0MDEKb3g5MHIyaDF1Y3Jlcnl3RUJyNVdyUXREbTJkYlNmaDZKS1lXRFNHNVNnVmFWU0NValdmZnNaSXhuNHRMU2F2Ugp2b2orWDMvV2FIdXFNcElmTFE9PQotLS0tLUVORCBDRVJUSUZJQ0FURS0tLS0tCg=="
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
                        User = "clusterAdmin_StackUnderflow_HERMES"
                    }
                }
            },
            CurrentContext = "HERMES-admin",
            Kind = "Config",
            Users = new[]
            {
                new User
                {
                    Name = "clusterAdmin_StackUnderflow_HERMES",
                    UserCredentials = new UserCredentials
                    { Token = "7uy51kcjn7pxv35iodk4czk97ouc0yx06a8bx4r9b9jezpcafdpdvz2tasxrcj6ens314oerj798skw95zd357u9bh0icwhuxe6jby8gsxdbvonkrd92nyiq3f60cbfh" }
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
