using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Application.Middleware;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class LoginService(IUserRepository userRepository, AuthenticationMiddleware authenticationMiddleware) : ILoginService
{
    public async Task<RegisterUserDto?> Register(RegisterUserDto? user)
    {
        var myUser = new User
        {
            Username = user.UserName,
            Email = user.Email,
            Password = user.Password
        };
        var result = await userRepository.CreateUser(myUser);
        return result == null ? null : user;
    }
    
    public async Task<LoginUserResponseDto?> Login(LoginUserDto loginUserDto)
    {
        var user = await userRepository.GetUserByEmail(loginUserDto.Email);
        if (user == null || user.Password != loginUserDto.Password)
        {
            return null;
        }
        var token = authenticationMiddleware.GenerateJwtToken(user.UserId);
        var result = new LoginUserResponseDto
        {
            Username = user.Username,
            Token = token
        };
        return result;
    }
    
    public async Task<UserResponseDto?> GetUserById(int userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user == null)
        {
            return null;
        }
        return new UserResponseDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email
        };
    }
}