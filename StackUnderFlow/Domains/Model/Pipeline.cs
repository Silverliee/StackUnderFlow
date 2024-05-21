namespace StackUnderFlow.Domains.Model;

public class Pipeline
{
    public int PipelineID { get; set; }
    public int CreatorUserID { get; set; }
    public int StatusId { get; set; }
    public string ProgramIDsList { get; set; }
    public string Description { get; set; }

    public User Creator { get; set; }
    public Status Status { get; set; }
}