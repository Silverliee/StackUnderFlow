namespace StackUnderFlow.Domains.Model;

public class Script
{
    public int ProgramID { get; set; }
    public string ProgramName { get; set; }
    public string Description { get; set; }
    public string InputFileType { get; set; }
    public string OutputFileType { get; set; }
    public string ProgrammingLanguage { get; set; }
    public string Visibility { get; set; }
    public int UserID { get; set; }

    public User User { get; set; }
    public ICollection<Sharing> Sharings { get; set; }
    public ICollection<Version> Versions { get; set; }
}