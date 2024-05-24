using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Domains.Repository;

public interface ISharingRepository
{
    Task<List<Sharing>> GetAllSharing();
    Task<Sharing?> GetSharingById(int id);
    Task<List<Sharing>> GetSharingByScriptId(int scriptId);
    Task AddSharing(Sharing? sharing);
    Task UpdateSharing(Sharing? sharing);
    Task DeleteSharing(int id);
}