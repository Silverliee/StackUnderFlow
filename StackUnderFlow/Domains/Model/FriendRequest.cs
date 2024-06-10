namespace StackUnderFlow.Domains.Model;

public class FriendRequest
{
    public int UserId1 { get; set; }
    public int UserId2 { get; set; }
    // Status can be "Pending", "Accepted". "Declined" = removed from table
    public string Status { get; set; }

    // Navigation properties to User entities
    public User User1 { get; set; }
    public User User2 { get; set; }
}