using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Application.DataTransferObject.Response;
using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Services;

public interface IProfileService
{
    public Task<bool> CheckEmailAvailability(string email);
    public Task<bool> CheckUsernameAvailability(string username);
    public Task<UserResponseDto?> UpdateUser(UpdateUserDto user);
}
