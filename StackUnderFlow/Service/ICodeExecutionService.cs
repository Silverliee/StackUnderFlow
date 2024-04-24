namespace StackUnderFlow.Service;

public interface ICodeExecutionService
{
    public bool ExecuteForCsharp(IFormFile file);
    public bool ExecuteForPython(IFormFile file);
}