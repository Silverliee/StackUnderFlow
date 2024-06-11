using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderFlow.Domains.Model;

public class Sharing
{
    [Key]
    public int ScritpId { get; set; }
    public int UserId { get; set; }

    [ForeignKey("ProgramId")]
    public Script Script { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}
