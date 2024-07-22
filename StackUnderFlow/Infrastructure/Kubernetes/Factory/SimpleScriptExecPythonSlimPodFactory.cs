using k8s.Models;

namespace StackUnderFlow.Infrastructure.Kubernetes.Factory;

public static class SimpleScriptExecPythonSlimPodFactory
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
                        Image = "python:3.9-slim",
                        Command = new List<string>
                        {
                            "bash",
                            "-c",
                            $"echo \"{script}\" | python"
                            
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