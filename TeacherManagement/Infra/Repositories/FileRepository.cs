using Core.Entities.Files;
using Infra.Contexts;

namespace Infra.Repositories
{
    public class FileRepository: AuditGenericRepository<MyFile>, IMyFileRepository
    {
        public FileRepository(TMContext context): base(context)
        {
                
        }

        public async Task<List<MyFile>> GetUserFiles(Guid userId)
        {
            var userFiles = All().Where(a => a.OwnerId == userId).ToList();

            return await Task.FromResult(userFiles);
        }
    }
}
