using System.ComponentModel.DataAnnotations;

namespace StackUnderFlow.Application.DataTransferObject.Request;

public class PipelineRequestDto
{
    [Required]
    public required List<int> ScriptIds { get; set; }
    [Required]
    public required IFormFile Input { get; set; }
    [Required]
    public required string PipelineId { get; set; }
}