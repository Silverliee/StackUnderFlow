using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderFlow.Domains.Model;

public class Comment
{
    [Key]
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int ScriptId { get; set; }
    public string Description { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
    [ForeignKey("ScriptId")]
    public Script Script { get; set; }
}