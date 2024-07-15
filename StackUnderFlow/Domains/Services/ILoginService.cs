using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Services;

public interface ILoginService
{
    public Task<RegisterUserDto?> Register(RegisterUserDto? user);
    public Task<LoginUserResponseDto?> Login(LoginUserDto loginUserDto);
    public Task<UserResponseDto?> GetUserById(int userId);
    public Task<List<UserResponseDto>> SearchUsersByKeyword(string keyword);
    
    public Task<bool> ForgotPassword(string email);
}
