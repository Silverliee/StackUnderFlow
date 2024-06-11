namespace StackUnderFlow.Domains.Model;

public class Follow
{
    public int UserId1 { get; set; }
    public int UserId2 { get; set; }

    // Navigation properties to User entities
    public User User1 { get; set; }
    public User User2 { get; set; }
}
