namespace StackUnderFlow.Application.DataTransferObject.Response;

public class CommentResponseDto
{
    public int commentId { get; set; }
    public int userId { get; set; }
    public string userName { get; set; }
    public int scriptId { get; set; }
    public string description { get; set; }
    
    public DateTime creationDate { get; set; }
}