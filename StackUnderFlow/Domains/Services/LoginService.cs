using System.Net.Mail;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Application.Middleware;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class LoginService(
    IUserRepository userRepository,
    AuthenticationMiddleware authenticationMiddleware
) : ILoginService
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
        var result = new LoginUserResponseDto { Username = user.Username, Token = token };
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
            Email = user.Email,
            Description = user.Description ?? ""
        };
    }
    
    public async Task<List<UserResponseDto>> SearchUsersByKeyword(string keyword)
    {
        var user = await userRepository.GetUsersByKeyword(keyword);
        if (user.Count == 0)
        {
            return [];
        }
        return user.Select(u => new UserResponseDto
        {
            UserId = u.UserId,
            Username = u.Username,
            Email = u.Email,
            Description = u.Description ?? ""
        }).ToList();
    }

    public async Task<bool> ForgotPassword(string email)
    {
        var user = await userRepository.GetUserByEmail(email);
        if (user == null)
        {
            return false;
        }
        var client = new SmtpClient("smtp-relay.brevo.com", 587);
        client.Credentials = new System.Net.NetworkCredential("78a135001@smtp-brevo.com", "3gYFsx0ZGQJKDnp2");
        var from = new MailAddress("emailservice@stackunderflow.software", "StackUnderFlow", System.Text.Encoding.UTF8);
        var to = new MailAddress(user.Email);
        var message = new MailMessage(from, to);
        message.Body = "You forgot your password. Here is your password: " + user.Password;
        message.BodyEncoding = System.Text.Encoding.UTF8;
        message.Subject = "Password Reset";
        message.SubjectEncoding = System.Text.Encoding.UTF8;
        client.Send(message);
        return true;
    }
    
    public async Task<bool> CheckPassword(int userId, string password)
    {
        var user = await userRepository.GetUserById(userId);
        if (user == null)
        {
            return false;
        }
        return user.Password == password;
    }
}
