namespace StackUnderFlow.Domains.Model;

public class Sharing
{
    public int ProgramID { get; set; }
    public int UserID { get; set; }

    public Script Script { get; set; }
    public User User { get; set; }
}