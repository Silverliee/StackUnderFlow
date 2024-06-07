namespace StackUnderFlow.Application.DataTransferObject.Response;

public class ScriptVersionResponseDto
{
    public int ScriptVersionId { get; set; }
    public int ScriptId { get; set; }
    public string VersionNumber { get; set; }
    public DateTime CreationDate { get; set; }
    public int CreatorUserId { get; set; }
    public string? SourceScriptLink { get; set; }
    public string? Comment { get; set; }
}