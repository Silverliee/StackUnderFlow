using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderFlow.Domains.Model;

public class Favorite
{
    public int ScritpId { get; set; }
    public int UserId { get; set; }

    [ForeignKey("ScritpId")]
    public Script Script { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}