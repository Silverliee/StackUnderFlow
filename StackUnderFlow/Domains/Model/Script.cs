using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderFlow.Domains.Model;

public class Script
{
    [Key] public int ScriptId { get; set; }
    public string ScriptName { get; set; }
    public string Description { get; set; }
    public string InputScriptType { get; set; }
    public string OutputScriptType { get; set; }
    public string ProgrammingLanguage { get; set; }

    // Visibility can be "Public", "Friend", "Group", "Private"
    public string Visibility { get; set; }
    public int UserId { get; set; }
    public string CreatorName { get; set; }

    public DateTime CreationDate { get; set; }

    [ForeignKey("UserId")] public User User { get; set; }
    public ICollection<Sharing> Sharings { get; set; }

    public ICollection<Favorite> Favorite { get; set; }

    public ICollection<ScriptVersion> Versions { get; set; }
}