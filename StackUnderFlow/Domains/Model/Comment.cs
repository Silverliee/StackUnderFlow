namespace StackUnderFlow.Domains.Model;

public class Comment
{
    public int RelationID { get; set; }
    public int UserID { get; set; }
    public int FileID { get; set; }
    public string Description { get; set; }

    public User User { get; set; }
    public Script File { get; set; }
}