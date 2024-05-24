using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IUserRepository
{
    public Task<IEnumerable<User?>> GetAllUsers();

    public Task<User?> GetUserById(int id);

    public Task<User?> GetUserByEmail(string email);

    public Task<User?> CreateUser(User user);

    public Task<User?> UpdateUser(User user);

    public Task<User?> DeleteUser(int id);
}