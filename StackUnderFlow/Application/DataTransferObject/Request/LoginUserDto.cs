namespace StackUnderFlow.Application.DataTransferObject.Request;

using System.ComponentModel.DataAnnotations;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
