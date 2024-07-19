using Core.Entities.Common;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class AuditGenericRepository<T>: IAuditableRepository<T>
        where T: AuditEntity
    {
        protected TMContext Context;

        protected AuditGenericRepository(TMContext context)
        {
            Context = context;
        }

        protected DbSet<T> Repository => Context.Set<T>();

        protected IQueryable<T> All(bool includeDeleted = false)
        {
            Func<T, bool> baseEntries = (entity) => includeDeleted || !entity.DeletedAt.HasValue;

            return Repository.Where(baseEntries).AsQueryable();
        }

        public virtual async Task<IQueryable<T>> GetAsync(bool includeDeleted = false)
        {
            return await Task.FromResult(All(includeDeleted).AsQueryable());
        }

        public async Task CreateAsync(T entity)
        {
            await Repository.AddAsync(entity);

            await Task.CompletedTask;
        }

        public virtual async Task<T> GetByIdAsync(Guid entityId) =>
            await Task.FromResult(All()?.FirstOrDefault(a => a.Id == entityId));

        public async Task RemoveAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;

            await Task.CompletedTask;
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;

            await Task.CompletedTask;
        }

        public async Task<List<T>> GetAll(bool includeDeleted = false)
        {
            return await Task.FromResult(All(includeDeleted).ToList());
        }

    }
}
