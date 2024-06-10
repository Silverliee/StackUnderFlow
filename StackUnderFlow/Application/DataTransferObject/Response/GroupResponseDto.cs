namespace StackUnderFlow.Application.DataTransferObject.Response;

public class GroupResponseDto
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public string Description { get; set; }
    public int CreatorUserID { get; set; }
}