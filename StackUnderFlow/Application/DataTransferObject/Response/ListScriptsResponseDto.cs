namespace StackUnderFlow.Application.DataTransferObject.Response;

public class ListScriptsResponseDto
{
    public List<ScriptResponseDto> Scripts { get; set; }
    public int TotalCount { get; set; }
}