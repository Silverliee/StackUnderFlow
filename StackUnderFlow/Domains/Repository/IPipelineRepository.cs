using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IPipelineRepository
{
    public Task<IEnumerable<Pipeline?>> GetAllPipelines();

    public Task<Pipeline?> GetPipelineById(int id);

    public Task<IEnumerable<Pipeline?>> GetPipelinesByUserId(int userId);

    public Task<Pipeline?> CreatePipeline(Pipeline pipeline);

    public Task<Pipeline?> UpdatePipeline(Pipeline pipeline);

    public Task<Pipeline?> DeletePipeline(int id);
}