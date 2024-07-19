
namespace Core.Entities.Common
{
    public interface IAuditableRepository<T>
        where T : AuditEntity
    {
        Task<T> GetByIdAsync(Guid entityId);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<IQueryable<T>> GetAsync(bool includeDeleted = false);
        Task<List<T>> GetAll(bool includeDeleted = false);
    }
}
