using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderFlow.Domains.Model;

public class Group
{
    [Key]
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public string Description { get; set; }
    public int CreatorUserID { get; set; }
    public List<int> Members { get; set; }

    [ForeignKey("CreatorUserID")]
    public User Creator { get; set; }
}