using StackUnderFlow.Application.DataTransferObject;

namespace StackUnderFlow.Domains.Services;

public interface ILoginService
{
    public Task<RegisterUserDto> Register(RegisterUserDto user);
}