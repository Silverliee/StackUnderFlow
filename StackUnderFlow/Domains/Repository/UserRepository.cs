using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class UserRepository(MySqlDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User?>> GetAllUsers()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUserById(int id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u != null && u.UserId == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u != null && u.Email == email);
    }

    public async Task<User?> CreateUser(User user)
    {
        var userExists = await context.Users.FirstOrDefaultAsync(u => u != null && (u.Email == user.Email || u.Username == user.Username));
        if (userExists != null)
        {
            return null;
        }
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUser(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }

    //delete user
    public async Task<User?> DeleteUser(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u != null && u.UserId == id);
        if (user == null)
        {
            return null;
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return user;
    }
}