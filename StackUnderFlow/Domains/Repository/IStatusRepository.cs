using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface IStatusRepository
{
    Task<List<Status?>> GetAllStatus();
    Task<Status?> GetStatusById(int id);
    Task AddStatus(Status? status);
    Task UpdateStatus(Status? status);
    Task DeleteStatus(int id);
}