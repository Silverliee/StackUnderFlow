using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class StatusRepository(MySqlDbContext context) : IStatusRepository
{
    //get all status
    public async Task<List<Status?>> GetAllStatus()
    {
        return await context.Statuses.ToListAsync();
    }
    
    //get status by id
    public async Task<Status?> GetStatusById(int id)
    {
        return await context.Statuses.FirstOrDefaultAsync(x => x.StatusId == id);
    }
    
    //add status
    public async Task AddStatus(Status? status)
    {
        await context.Statuses.AddAsync(status);
        await context.SaveChangesAsync();
    }
    
    //update status
    public async Task UpdateStatus(Status? status)
    {
        context.Statuses.Update(status);
        await context.SaveChangesAsync();
    }
    
    //delete status
    public async Task DeleteStatus(int id)
    {
        var status = await context.Statuses.FirstOrDefaultAsync(x => x.StatusId == id);
        context.Statuses.Remove(status);
        await context.SaveChangesAsync();
    }
    
}