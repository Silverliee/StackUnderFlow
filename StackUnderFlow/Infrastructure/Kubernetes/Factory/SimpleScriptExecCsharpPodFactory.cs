using k8s.Models;

namespace StackUnderFlow.Infrastructure.Kubernetes.Factory;

public static class SimpleScriptExecCsharpPodFactory
{
    public static V1Pod CreatePod(string script)
    {
        var pod = new V1Pod
        {
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
                            "bash",
                            "-c",
                            $"echo \"{script}\" > script.csx && dotnet tool install -g dotnet-script > install.txt && export PATH=\"$PATH:/root/.dotnet/tools\" && dotnet script script.csx"
                        }
                    }
                },
                RestartPolicy = "Never"
            },
            Metadata = new V1ObjectMeta
            {
                Name = "script-runner-" + Guid.NewGuid(),
                Labels = new Dictionary<string, string> { { "app", "script-runner" } }
            }
        };
        return pod;
    }
}