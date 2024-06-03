using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Application.DataTransferObject.Response;

public class LoginUserResponseDto
{
    public User? User { get; set; }
    public string? Token { get; set; }
}