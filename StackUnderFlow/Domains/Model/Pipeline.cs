using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderFlow.Domains.Model;

public class Pipeline
{
    [Key]
    public int PipelineId { get; set; }
    public int CreatorUserId { get; set; }
    public int StatusId { get; set; }
    public string ProgramIDsList { get; set; }
    public string Description { get; set; }

    [ForeignKey("CreatorUserId")]
    public User Creator { get; set; }

    [ForeignKey("StatusId")]
    public Status Status { get; set; }
}
