using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
namespace StackUnderFlow.Domains.Services;

public interface ILoginService
{
    public Task<RegisterUserDto?> Register(RegisterUserDto? user);
    public Task<LoginUserResponseDto?> Login(LoginUserDto loginUserDto);
}