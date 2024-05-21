namespace StackUnderFlow.Domains.Model;

public class Like
{
    public int RelationID { get; set; }
    public int UserID { get; set; }
    public int FileID { get; set; }

    public User User { get; set; }
    public Script File { get; set; }
}