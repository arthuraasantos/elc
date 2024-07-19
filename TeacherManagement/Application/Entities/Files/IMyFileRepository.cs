using Core.Entities.Common;

namespace Core.Entities.Files
{
    public interface IMyFileRepository: IAuditableRepository<MyFile>
    {
        Task<List<MyFile>> GetUserFiles(Guid userId);
    }
}
