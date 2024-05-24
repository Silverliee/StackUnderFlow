using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Domains.Model;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow.Domains.Repository;

public class PipelineRepository(MySqlDbContext context) : IPipelineRepository
{
    //get all pipelines
    public async Task<IEnumerable<Pipeline?>> GetAllPipelines()
    {
        return await context.Pipelines.ToListAsync();
    }
    
    //get pipeline by id
    public async Task<Pipeline?> GetPipelineById(int id)
    {
        return await context.Pipelines.FirstOrDefaultAsync(p => p.PipelineId == id);
    }
    
    //get pipelines by user id
    public async Task<IEnumerable<Pipeline?>> GetPipelinesByUserId(int userId)
    {
        return await context.Pipelines.Where(p => p.CreatorUserId == userId).ToListAsync();
    }
    
    //create pipeline
    public async Task<Pipeline?> CreatePipeline(Pipeline pipeline)
    {
        await context.Pipelines.AddAsync(pipeline);
        await context.SaveChangesAsync();
        return pipeline;
    }
    
    //update pipeline
    public async Task<Pipeline?> UpdatePipeline(Pipeline pipeline)
    {
        context.Pipelines.Update(pipeline);
        await context.SaveChangesAsync();
        return pipeline;
    }
    
    //delete pipeline
    public async Task<Pipeline?> DeletePipeline(int id)
    {
        var pipeline = await context.Pipelines.FirstOrDefaultAsync(p => p.PipelineId == id);
        if (pipeline == null)
        {
            return null;
        }
        
        context.Pipelines.Remove(pipeline);
        await context.SaveChangesAsync();
        return pipeline;
    }
}