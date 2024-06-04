using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Application.Middleware;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class LoginService(IUserRepository userRepository, AuthentificationMiddleware authentificationMiddleware) : ILoginService
{
    public async Task<RegisterUserDto> Register(RegisterUserDto user)
    {
        var myUser = new User
        {
            Username = user.UserName,
            Email = user.Email,
            Password = user.Password
        };
        var result = await userRepository.CreateUser(myUser);
        return user;
    }
    
    public async Task<LoginUserResponseDto> Login(LoginUserDto loginUserDto)
    {
        var user = await userRepository.GetUserByEmail(loginUserDto.Email);
        if (user == null || user.Password != loginUserDto.Password)
        {
            return new LoginUserResponseDto();
        }
        var token = authentificationMiddleware.GenerateJwtToken(user.Username);
        var result = new LoginUserResponseDto
        {
            User = user,
            Token = token
        };
        return result;
    }
}