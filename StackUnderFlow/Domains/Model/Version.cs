namespace StackUnderFlow.Domains.Model;

public class Version
{
    public int VersionID { get; set; }
    public int ProgramID { get; set; }
    public int VersionNumber { get; set; }
    public DateTime CreationDate { get; set; }
    public int CreatorUserID { get; set; }
    public string SourceFileLink { get; set; }

    public Script Script { get; set; }
    public User Creator { get; set; }
}