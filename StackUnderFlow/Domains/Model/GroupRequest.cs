namespace StackUnderFlow.Domains.Model;

public class GroupRequest
{
    public int GroupId { get; set; }
    public int UserId { get; set; }
    // Status can be "Pending", "Accepted". "Declined" = removed from table
    public string Status { get; set; } = "Pending";
    
    // Navigation properties to User and Group entities
    public User User { get; set; }
    public Group Group { get; set; }
}