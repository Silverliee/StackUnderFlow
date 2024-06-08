using System.Reflection;
using k8s;
using k8s.Models;

namespace StackUnderFlow.Infrastructure.Kubernetes;

public class KubernetesService()
{
    private readonly IKubernetes _client =  new k8s.Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile("Infrastructure/Kubernetes/kubeconfig/config"));
    
    public void CreatePythonJob(string namespaceName, string scriptFileName)
    {
        var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var scriptDirectory = Path.Combine(rootPath!, "Infrastructure", "Kubernetes", "python-scripts");
        var job = new V1Job
        {
            ApiVersion = "batch/v1",
            Kind = "Job",
            Metadata = new V1ObjectMeta { Name = "python-job" },
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
                                Name = "python-container",
                                Image = "python:3.9",
                                Command = new List<string> { "python", $"{scriptFileName}" },
                                VolumeMounts = new List<V1VolumeMount>
                                {
                                    new()
                                    {
                                        Name = "python-script",
                                        MountPath = scriptDirectory
                                    }
                                }
                            }
                        },
                        RestartPolicy = "Never",
                        Volumes = new List<V1Volume>
                        {
                            new()
                            {
                                Name = "python-script",
                                PersistentVolumeClaim = new V1PersistentVolumeClaimVolumeSource
                                {
                                    ClaimName = "python-script-pvc"
                                }
                            }
                        }
                    }
                }
            }
        };
        
        _client.BatchV1.CreateNamespacedJob(job, namespaceName);
        //get output here
        DeleteJob(namespaceName,job.Name());
    }
    
    public void CreateCSharpJob(string namespaceName, string scriptFileName)
    {
        var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var scriptDirectory = Path.Combine(rootPath!, "Infrastructure", "Kubernetes", "csharp-scripts");
        var job = new V1Job
        {
            ApiVersion = "batch/v1",
            Kind = "Job",
            Metadata = new V1ObjectMeta { Name = "csharp-job" },
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
                                Name = "csharp-container",
                                Image = "mcr.microsoft.com/dotnet/sdk:8.0",
                                Command = new List<string> { "bash", "-c", $"mcs {scriptFileName} && mono {Path.GetFileNameWithoutExtension(scriptFileName)}.exe" },
                                VolumeMounts = new List<V1VolumeMount>
                                {
                                    new()
                                    {
                                        Name = "csharp-script",
                                        MountPath = scriptDirectory
                                    }
                                }
                            }
                        },
                        RestartPolicy = "Never",
                        Volumes = new List<V1Volume>
                        {
                            new()
                            {
                                Name = "csharp-script",
                                PersistentVolumeClaim = new V1PersistentVolumeClaimVolumeSource
                                {
                                    ClaimName = "csharp-script-pvc"
                                }
                            }
                        }
                    }
                }
            }
        };

        _client.BatchV1.CreateNamespacedJob(job, namespaceName);
    }
    
    public void DeleteJob(string namespaceName, string jobName)
    {
        _client.BatchV1.DeleteNamespacedJob(jobName, namespaceName);
    }
}