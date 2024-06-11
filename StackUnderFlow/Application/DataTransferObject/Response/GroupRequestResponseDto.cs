using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Application.DataTransferObject.Response;

public class GroupRequestResponseDto
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public string Status { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
}
