using StackUnderFlow.Application.DataTransferObject;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class LoginService(IUserRepository userRepository) : ILoginService
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
}