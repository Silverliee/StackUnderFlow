namespace StackUnderFlow.Application.DataTransferObject.Response;

public class ScriptResponseDto
{
    public int ScriptId { get; set; }
    public string ScriptName { get; set; }
    public string Description { get; set; }
    public string? InputScriptType { get; set; }
    public string? OutputScriptType { get; set; }
    public string ProgrammingLanguage { get; set; }
    public string Visibility { get; set; }
    public int UserId { get; set; }
    public string CreatorName { get; set; }
    public int NumberOfLikes { get; set; }
    public bool IsLiked { get; set; }

    // [ForeignKey("UserId")]
    // public User User { get; set; }
    // public ICollection<Sharing> Sharings { get; set; }
    // public ICollection<ScriptVersion> Versions { get; set; }
}
