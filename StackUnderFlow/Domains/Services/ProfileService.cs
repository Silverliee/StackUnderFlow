using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Application.Security;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;

namespace StackUnderFlow.Domains.Services;

public class ProfileService(IUserRepository userRepository, ICryptographer cryptographer) : IProfileService
{
    public async Task<bool> CheckEmailAvailability(string email)
    {
        var result = await userRepository.GetUserByEmail(email);
        return result == null;
    }
    public async Task<bool> CheckUsernameAvailability(string username)
    {
        var result = await userRepository.GetUserByUsername(username);
        return result == null;
    }
    
    public async Task<UserResponseDto?> UpdateUser(UpdateUserDto userDto)
    {
        var userFromDb = await userRepository.GetUserById(userDto.UserId);
        if (userFromDb == null)
        {
            return null;
        }
        if (!string.IsNullOrEmpty(userDto.Username))
        {
            userFromDb.Username = userDto.Username;
        }

        if (!string.IsNullOrEmpty(userDto.Email))
        {
            userFromDb.Email = userDto.Email;
        }

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            userFromDb.Password = cryptographer.Encrypt(userDto.Password);
        }

        if (!string.IsNullOrEmpty(userDto.Description))
        {
            userFromDb.Description = userDto.Description;
        }
        
        var result = await userRepository.UpdateUser(userFromDb);
        return new UserResponseDto
        {
            Username = result.Username,
            Email = result.Email,
            Description = result.Description ?? "",
            UserId = result.UserId
        };
    }
}
