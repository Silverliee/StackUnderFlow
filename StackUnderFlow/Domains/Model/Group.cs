namespace StackUnderFlow.Domains.Model;

public class Group
{
    public int GroupID { get; set; }
    public string GroupName { get; set; }
    public string Description { get; set; }
    public int CreatorUserID { get; set; }

    public User Creator { get; set; }
}