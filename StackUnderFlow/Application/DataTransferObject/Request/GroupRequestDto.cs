using System.ComponentModel.DataAnnotations;

namespace StackUnderFlow.Application.DataTransferObject.Request;

public class GroupRequestDto
{
    [Required]
    public string GroupName { get; set; }
    public string Description { get; set; }
}
