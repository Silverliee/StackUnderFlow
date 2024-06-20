using System.ComponentModel.DataAnnotations;

namespace StackUnderFlow.Application.DataTransferObject.Request;

public class ScriptRequestForOtherUserDto
{
    public string Visibility { get; set; } = "Public";
    public int GroupId { get; set; }
    [Required]
    public int UserId { get; set; }
    // To remove, only because swagger authorize was picky
    public int me { get; set; }
}