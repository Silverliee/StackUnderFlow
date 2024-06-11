using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderFlow.Domains.Model;

public class FriendRequest
{
    public int UserId1 { get; set; }
    public int UserId2 { get; set; }

    // Status can be "Pending", "Accepted". "Declined" = removed from table
    public string Status { get; set; }
    public string Message { get; set; }

    // Navigation properties to User entities
    [ForeignKey("UserId1")]
    public User User1 { get; set; }

    [ForeignKey("UserId2")]
    public User User2 { get; set; }
}
