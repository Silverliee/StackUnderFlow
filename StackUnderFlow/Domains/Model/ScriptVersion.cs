using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderFlow.Domains.Model;

public class ScriptVersion
{
    [Key]
    public int ScriptVersionId { get; set; }
    public int ScriptId { get; set; }
    public string VersionNumber { get; set; }
    public DateTime CreationDate { get; set; }
    public int CreatorUserId { get; set; }
    public string SourceScriptLink { get; set; }
    public byte[] SourceScriptBinary { get; set; }
    
    public string Comment { get; set; }
    
    [ForeignKey("ScriptId")]
    public Script Script { get; set; }
    [ForeignKey("CreatorUserId")]
    public User Creator { get; set; }
}