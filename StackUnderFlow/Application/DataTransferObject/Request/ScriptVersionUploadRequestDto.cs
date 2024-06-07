namespace StackUnderFlow.Application.DataTransferObject.Request;

public class ScriptVersionUploadRequestDto
{
    public int ScriptId { get; set; }
    public string VersionNumber { get; set; }
    public int CreatorUserId { get; set; }
    public string SourceScriptBinary { get; set; }
    public string Comment { get; set; }
}