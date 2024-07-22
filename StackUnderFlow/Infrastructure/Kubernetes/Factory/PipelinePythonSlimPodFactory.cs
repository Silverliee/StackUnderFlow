using k8s.Models;

namespace StackUnderFlow.Infrastructure.Kubernetes.Factory;

public static class PipelinePythonSlimPodFactory
{
    public static V1Pod CreatePod(string script, string inputFileBinary, string inputType, string outputType)
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
                        Image = "python:3.9-slim",
                        Command = new List<string>
                        {
                            "sh", "-c",
                            $"echo '{script}' > script.py && echo '{inputFileBinary}' > input-file{inputType} && python script.py input-file{inputType} > output-file{outputType} && base64 output-file{outputType}"
                        }
                    }
                },
                RestartPolicy = "Never"
            },
        };
    }
}