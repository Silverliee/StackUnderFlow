using k8s.Models;

namespace StackUnderFlow.Infrastructure.Kubernetes.Factory;

public static class PipelineCsharpPodFactory
{
    public static V1Pod CreatePod(string scriptContent, string inputFileBinary, string inputType, string outputType)
    {
        return new V1Pod
        {
            Metadata = new V1ObjectMeta
            {
                Name = "script-runner-" + Guid.NewGuid(),
                Labels = new Dictionary<string, string> { { "app", "script-runner" } }
            },
            Spec = new V1PodSpec
            {
                Containers = new List<V1Container>
                {
                    new()
                    {
                        Name = "runner",
                        Image = "mcr.microsoft.com/dotnet/sdk:8.0",
                        Command = new List<string>
                        {
                            "sh", "-c",
                            $"echo \"{scriptContent}\" > script.csx && echo \"{inputFileBinary}\" > input-file{inputType} " + 
                            $"dotnet tool install -g dotnet-script > install.txt && export PATH=\"$PATH:/root/.dotnet/tools\" && dotnet script script.csx input-file{inputType} > output-file{outputType} && base64 output-file{outputType}"
                        }
                    }
                },
                RestartPolicy = "Never"
            }
        };
    }
}