using Core.Entities.Files;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infra.Repositories
{
    public class FileRepository : AuditGenericRepository<MyFile>, IMyFileRepository
    {
        public FileRepository(TMContext context) : base(context)
        {

        }

        public async Task<List<MyFile>> GetUserFiles(Guid userId)
        {
            var userFiles = All(false)
                .Where(a => a.OwnerId == userId)
                .ToList();

            return await Task.FromResult(userFiles);
        }
        protected override IQueryable<MyFile> All(bool includeDeleted = false)
        {
            Func<MyFile, bool> baseEntries = (entity) => includeDeleted || !entity.DeletedAt.HasValue;
            
            return Repository.Include(a => a.Data).Where(baseEntries).AsQueryable();
        }
    }
}
